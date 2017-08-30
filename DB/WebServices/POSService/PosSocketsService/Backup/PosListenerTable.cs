using System;
using System.Collections;
using System.Data.SqlClient;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Fujitsu.eCrm.Generic.CrmService;
using Fujitsu.eCrm.Generic.SharedUtils;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace Fujitsu.eCrm.Seoul.PosSocketsService {

	internal class PosListenerTable : Hashtable, IDisposable {
		
		#region Attributes
		private int pendingBacklog;
		private Timer refreshListenersTime;
		private bool isDisposed;
		private long refreshFrequency;
		#endregion

		#region Constructor
		internal PosListenerTable(long refreshFrequency, int pendingBacklog) : base(107) {
			this.refreshFrequency = refreshFrequency;
			this.pendingBacklog = pendingBacklog;
			this.isDisposed = false;
			this.refreshListenersTime = new Timer(new TimerCallback(this.RefreshListeners),null,0,Timeout.Infinite);
		}
		#endregion

		#region Refresh Listeners
		/// <summary>
		/// Create, Update and Drop Listeners according to the contents of the store domain
		/// tables
		/// </summary>
		/// <param name="state"></param>
		private void RefreshListeners(Object state) {

			// If FinaliseServer is running, wait until it has finished
			lock (this) {

				// Disposed has been called, so don't start any new listeners
				if (this.isDisposed) {
					return;
				}

				ITraceState trState = PosApplication.Trace.StartProc("RefreshListeners");
				try {
					#region Identify Ports to Support
					string hostName = Dns.GetHostName();
					IPHostEntry lipa = Dns.Resolve(hostName);

					string findNgcPortStatement = 
						"SELECT DISTINCT ns.ngc_server_port "+
						"FROM ngc_server ns "+
						"WHERE ns.ngc_server_port IS NOT NULL ";

					string findStoreStatementTemplate = 
						"SELECT DISTINCT "+ 
						"sps.store_pos_server_ip_address, "+
						"s.store_code "+
						"FROM "+
						"ngc_server ns, "+
						"store s, "+
						"store_pos_server sps "+ 
						"WHERE "+
						"ns.ngc_server_port = '{0}' "+
						"AND ns.store_crmid = s.store_crmid "+
						"AND ns.store_crmid = sps.store_crmid ";

					string findMessageStatementTemplate = 
						"SELECT DISTINCT "+ 
						"nsm.ngc_server_message_code "+
						"FROM "+
						"ngc_server ns, "+
						"ngc_server_message nsm "+
						"WHERE "+
						"ns.ngc_server_port = '{0}' "+
						"AND ns.ngc_server_crmid = nsm.ngc_server_crmid ";

					string connectionString = (string)XmlApplication.ConfigurationTable["AdminConnectionString"];
					ArrayList ngcPortList = new ArrayList();
					AppSqlDataReader ngcPortReader = new AppSqlDataReader(findNgcPortStatement,connectionString);						
					try {
						#region Handle Supported Ports
						while (ngcPortReader.Read()) {
							object portNumber = ngcPortReader[0];
							ngcPortList.Add(portNumber);
							string findStoreStatement = String.Format(findStoreStatementTemplate,portNumber);
							string findMessageStatement = String.Format(findMessageStatementTemplate,portNumber);

							#region List of Store Codes and IP Address Supported by this Port
							Hashtable storeTable = new Hashtable();
							AppSqlDataReader storeReader = new AppSqlDataReader(findStoreStatement,connectionString);						
							try {
								while (storeReader.Read()) {
									object storeIPAddress = storeReader[0];
									object storeCode = storeReader[1];
									if (storeTable.Contains(storeIPAddress)) {
										ArrayList codeList = (ArrayList)storeTable[storeIPAddress];
										codeList.Add(storeCode);
									} else {
										ArrayList codeList = new ArrayList();
										codeList.Add(storeCode);
										storeTable.Add(storeIPAddress,codeList);
									}
								}
							} finally {
								storeReader.Close();
							}
							#endregion

							#region List of Messages Supported by this Port
							ArrayList messageList = new ArrayList();
							AppSqlDataReader messageReader = new AppSqlDataReader(findMessageStatement,connectionString);						
							try {
								while (messageReader.Read()) {
									messageList.Add(messageReader[0]);
								}
							} finally {
								messageReader.Close();
							}
							#endregion

							if (this.Contains(portNumber)) {
								// Update existing listener with new configurations
								PosListener posListener = (PosListener)this[portNumber];
								posListener.Change(storeTable,messageList);
							} else {
								// Create new listener
								IPEndPoint lep = new IPEndPoint(lipa.AddressList[0], (int)portNumber);
								PosListener posListener = new PosListener(lep,this.pendingBacklog,storeTable,messageList);
								this.Add(portNumber,posListener);
							}
						}
						#endregion
					} finally {
						ngcPortReader.Close();
					}
					#endregion

					#region Identify Ports no longer Supported
					foreach (object portNumber in this.Keys) {
						if (!ngcPortList.Contains(portNumber)) {
							PosListener posListener = (PosListener)this[portNumber];
							this.Remove(portNumber);
							posListener.Dispose();
						}
					}
					#endregion
				} catch (Exception e) {
					ExceptionManager.Publish(e);
				} finally {
					trState.EndProc();
					refreshListenersTime.Change(this.refreshFrequency,Timeout.Infinite);
				}
			}
		}
		#endregion

		#region Dispose
		public void Dispose() {
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		~PosListenerTable() {
			this.Dispose(false);
		}

		private void Dispose(bool disposing) {
			// Prevent Refresh Listener from running while this method is running
			lock (this) {

				// Disposed has already been called
				if (this.isDisposed) {
					return;
				}

				// Stop Timer for refreshing Port Data
				if (this.refreshListenersTime != null) {
					this.refreshListenersTime.Change(Timeout.Infinite,Timeout.Infinite);
				}

				// Stop Port's listeners and handlers
				foreach (PosListener posListener in this.Values) {
					posListener.Dispose();
				}

				this.isDisposed = true;
			}
		}
		#endregion

	}
}