#region Using
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
#endregion

namespace PollPartnerBatchService
{
	public class TxnBatchService : System.ServiceProcess.ServiceBase
	{
		private System.Timers.Timer batchTimer;
		private static Mutex xsdMutex=new Mutex();

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public TxnBatchService()
		{
			// This call is required by the Windows.Forms Component Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitComponent call
		}

		// The main entry point for the process
		static void Main()
		{
			System.ServiceProcess.ServiceBase[] ServicesToRun;
	
			// More than one user Service may run within the same process. To add
			// another service to this process, change the following line to
			// create a second service object. For example,
			//
			//   ServicesToRun = New System.ServiceProcess.ServiceBase[] {new Service1(), new MySecondUserService()};
			//
			ServicesToRun = new System.ServiceProcess.ServiceBase[] { new TxnBatchService() };

			System.ServiceProcess.ServiceBase.Run(ServicesToRun);
		}

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.batchTimer = new System.Timers.Timer();
			((System.ComponentModel.ISupportInitialize)(this.batchTimer)).BeginInit();
			// 
			// batchTimer
			// 
			this.batchTimer.Enabled = true;
			this.batchTimer.Interval = int.Parse(System.Configuration.ConfigurationSettings.AppSettings["TimerInterval"]);
			// 
			// TxnBatchService
			// 
			this.ServiceName = "NGCTxnBatchService";
			((System.ComponentModel.ISupportInitialize)(this.batchTimer)).EndInit();
			this.batchTimer.Elapsed+=new System.Timers.ElapsedEventHandler(this.batchTimer_Elapsed);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		/// <summary>
		/// Set things in motion so your service can do its work.
		/// </summary>
		protected override void OnStart(string[] args)
		{
			batchTimer.Start();
		}
 
		/// <summary>
		/// Stop this service.
		/// </summary>
		protected override void OnStop()
		{
			batchTimer.Stop();
		}


		#region batchTimer_Elapsed
		private void batchTimer_Elapsed(object sender,System.Timers.ElapsedEventArgs args)
		{
			PollFolder();
		}
		#endregion

		#region PollFolder
		private void PollFolder()
		{
			string folderPath=System.Configuration.ConfigurationSettings.AppSettings["TransactionFileFolderPath"];
			string[] txnFiles= System.IO.Directory.GetFiles(folderPath);
			if(txnFiles.Length>0)
			{
				MoveFilesToProcessFolder();
			}
		}

		#endregion

		#region StartProcessing
		private void StartProcessing()
		{
			string folderPath=System.Configuration.ConfigurationSettings.AppSettings["ProcessFolderPath"];
			try
			{
				string[] txnFiles=System.IO.Directory.GetFiles(folderPath);
				for(int i=0;i<txnFiles.Length;i++)
				{
					System.Threading.Thread txnThread=new System.Threading.Thread(new System.Threading.ThreadStart(ProcessTxnFile));
					txnThread.Name=txnFiles.GetValue(i).ToString();
					txnThread.Start();
				}
			}
			catch(Exception ex)
			{
				Logger.WriteToEventLog("NGCTxnBatch",ex.Message);
			}
		}

		#endregion

		#region MoveFilesToProcessFolder
		private void MoveFilesToProcessFolder()
		{
			string sourceFolder=System.Configuration.ConfigurationSettings.AppSettings["TransactionFileFolderPath"];
			string destFolder=System.Configuration.ConfigurationSettings.AppSettings["ProcessFolderPath"];
			string noofFiles=System.Configuration.ConfigurationSettings.AppSettings["ConcurrentFileCount"];
			int concurrentFiles=25;
			string fileName="";
			bool moveSuccess=false;
			try
			{
				if((noofFiles!="")&&(noofFiles!=null))
				{
					concurrentFiles=int.Parse(noofFiles);	
					if((concurrentFiles<=0)  || (concurrentFiles>25))
					{
						concurrentFiles=25;
					}
				}
				string[] txnFiles= System.IO.Directory.GetFiles(sourceFolder);
				if(txnFiles.Length<concurrentFiles)
				{
					concurrentFiles=txnFiles.Length;
				}
				for(int i=0;i<concurrentFiles;i++)
				{
					fileName=txnFiles[i].Substring(txnFiles[i].LastIndexOf(@"\"),txnFiles[i].Length-txnFiles[i].LastIndexOf(@"\"));
					while(!moveSuccess)
					{
						try
						{
							System.IO.File.Move(txnFiles[i],destFolder+@"\"+fileName);
							moveSuccess=true;
						}
						catch(Exception)
						{
							moveSuccess=false;
						}
					}
					moveSuccess=false;
				}
				if(txnFiles.Length>0)
				{
					StartProcessing();
				}
			}
			catch(Exception ex)
			{
				Logger.WriteToEventLog("NGCTxnBatch",ex.Message);
			}
		}

		#endregion
		
		#region ProcessTxnFile
        private void ProcessTxnFile()
		{
			string fileName="";
			try
			{
				fileName=Thread.CurrentThread.Name;
				Processor txnProcessor=new Processor(fileName);
				txnProcessor.StartProcessingTxnFile(xsdMutex);
			}
			catch(Exception ex)
			{
				Logger.WriteToEventLog("NGCTxnBatch",ex.Message);
			}
		}
		#endregion

	}
}
