using System;
using System.IO;
using System.Xml;
using System.Threading;
using System.Data;

namespace PollPartnerBatchService
{
	/// <summary>
	/// Summary description for Processor.
	/// </summary>
	public class Processor
	{
		private string filename;
		private XmlDocument responseDoc;
		private DataSet dsTxn;
		private DataSet dsPartnerOutlet;
		private DateTime dtProcessedDate;
		private decimal amount_spent_total;
		private decimal extra_points_2;
		#region Processor
		public Processor(string fileName)
		{
			this.extra_points_2=0;
			this.amount_spent_total=0;
			this.dtProcessedDate=DateTime.Now;
			this.filename=fileName;
			this.responseDoc=new XmlDocument();
			this.responseDoc.LoadXml("<pptxnr><file_name/><sending_org/><date_created/><time_created/><sequence_number/><transaction_count/><response_code/><error_description/></pptxnr>");
			//dataset for txn
			this.dsTxn=new DataSet();
			this.dsTxn.Tables.Add("Txn");
			this.dsTxn.Tables["Txn"].Columns.Add("txn_date");
			this.dsTxn.Tables["Txn"].Columns.Add("pos_id");
			this.dsTxn.Tables["Txn"].Columns.Add("txn_nbr");
			this.dsTxn.Tables["Txn"].Columns.Add("amount_spent");
			this.dsTxn.Tables["Txn"].Columns.Add("total_points");
			this.dsTxn.Tables["Txn"].Columns.Add("welcome_points");
			this.dsTxn.Tables["Txn"].Columns.Add("product_points");
			this.dsTxn.Tables["Txn"].Columns.Add("extra_points_1");
			this.dsTxn.Tables["Txn"].Columns.Add("extra_points_2");
			this.dsTxn.Tables["Txn"].Columns.Add("extra_points_3");
			this.dsTxn.Tables["Txn"].Columns.Add("amount_spent_at_partner");
			this.dsTxn.Tables["Txn"].Columns.Add("partner_reference");
			this.dsTxn.Tables["Txn"].Columns.Add("partner_pos_id");
			this.dsTxn.Tables["Txn"].Columns.Add("partner_outlet_ref");
			this.dsTxn.Tables["Txn"].Columns.Add("partner_outlet_name");
			this.dsTxn.Tables["Txn"].Columns.Add("points_partner_processed_dt");
			this.dsTxn.Tables["Txn"].Columns.Add("partner_number");
			this.dsTxn.Tables["Txn"].Columns.Add("official_id");
			this.dsTxn.Tables["Txn"].Columns.Add("card_account_number");
			this.dsTxn.Tables["Txn"].Columns.Add("add_skeleton_account");
			this.dsTxn.Tables["Txn"].Columns.Add("customer_crmid");
			//dataset for partner_outlet
			this.dsPartnerOutlet=new DataSet();
			this.dsPartnerOutlet.Tables.Add("Partner_Outlet");
			this.dsPartnerOutlet.Tables["Partner_Outlet"].Columns.Add("partner_outlet_number");
			this.dsPartnerOutlet.Tables["Partner_Outlet"].Columns.Add("partner_outlet_name");
			this.dsPartnerOutlet.Tables["Partner_Outlet"].Columns.Add("partner_outlet_reference");
			this.dsPartnerOutlet.Tables["Partner_Outlet"].Columns.Add("partner_number");
		}
		#endregion

		#region StartProcessingTxnFile
		/// <summary>
		/// StartProcessingTxnFile
		/// </summary>
		protected internal void StartProcessingTxnFile(System.Threading.Mutex xsdMutex)
		{
			Validator txnValidator=new Validator();
			DataAccess txnDataAccess=new DataAccess();
			Mailer txnMailer=new Mailer();
			Logger txnLogger=new Logger();
			XmlDocument txnDoc=new XmlDocument();			
			XmlDocument headerDoc=new XmlDocument();
			string agencyMailId="";
			string nameOfFile="";
			bool sendMail=true;
			bool validTxn=false;
			//header
			string fileName="";
			string batchResponseCode="";
			string org="";
			string date="";
			string time="";
			string seqNumber="";
			string txnCount="";
			string errMsg="";
			string errDesc="";
			//txnrecord
			string partnerNumber="";
			string partnerOutRef="";
			string partnerOutName="";
			string cardAccNumber="";
			string officialId="";
			string transactDate="";
			string transactTime="";
			string spend="";
			string partnerPoints="";
			string partnerRef="";
			string partnerPosId="";
			bool addPartnerOutlet=false;
			int rejectedRecordCount=0;
			bool inValidFile=false;
			bool invalidXml=false;
			int countTxn=0;
			int totalTxnCount=0;
			bool isAgency=false;
			bool addSkeleton=false;
			string customerCrmid="";
			int errorLimit=int.Parse(System.Configuration.ConfigurationSettings.AppSettings["ErrorLimit"]);
			try
			{
				Logger.WriteToEventLog("NGCTxnBatch","About to load the file "+this.filename);
				try
				{
					txnDoc.Load(this.filename);
				}
				catch(XmlException xmlEx)
				{
					invalidXml=true;
					errDesc=xmlEx.Message;
					this.responseDoc.SelectSingleNode("//pptxnr/response_code").InnerText="99";	
					this.responseDoc.SelectSingleNode("//pptxnr/error_description").InnerText=errDesc;
				}
				if(!invalidXml)
				{
					Logger.WriteToEventLog("NGCTxnBatch","Loaded the file "+this.filename);
					nameOfFile=this.filename.Substring(this.filename.LastIndexOf(@"\"),this.filename.Length-this.filename.LastIndexOf(@"\"));
					xsdMutex.WaitOne();
					int count=0;
					headerDoc.LoadXml(txnDoc.OuterXml);
					XmlNodeList txnList=headerDoc.SelectNodes("//transaction");
					//remove the childnodes and validate just the header
					foreach(XmlNode childTxn in txnList)
					{
						try
						{
							headerDoc.DocumentElement.RemoveChild(childTxn);
						}
						catch
						{
							invalidXml=true;
							errDesc="Invalid Xml";
							this.responseDoc.SelectSingleNode("//pptxnr/response_code").InnerText="99";	
							this.responseDoc.SelectSingleNode("//pptxnr/error_description").InnerText=errDesc;
							break;
						}
					}
					if(!invalidXml)
					{
						if(txnDoc.SelectSingleNode("pptxn/file_name")!=null)
						{
							fileName=txnDoc.SelectSingleNode("pptxn/file_name").InnerText.Trim();	
						}
						if(txnDoc.SelectSingleNode("pptxn/sending_org")!=null)
						{
							org=txnDoc.SelectSingleNode("pptxn/sending_org").InnerText.Trim();
						}
						if(txnDoc.SelectSingleNode("pptxn/date_created")!=null)
						{
							date=txnDoc.SelectSingleNode("pptxn/date_created").InnerText.Trim();
						}
						if(txnDoc.SelectSingleNode("pptxn/time_created")!=null)
						{
							time=txnDoc.SelectSingleNode("pptxn/time_created").InnerText.Trim();
						}
						if(txnDoc.SelectSingleNode("pptxn/sequence_number")!=null)
						{
							seqNumber=txnDoc.SelectSingleNode("pptxn/sequence_number").InnerText.Trim();
						}
						if(txnDoc.SelectSingleNode("pptxn/transaction_count")!=null)
						{
							txnCount=txnDoc.SelectSingleNode("pptxn/transaction_count").InnerText.Trim();
						}
						//prepare the response document
						this.responseDoc.SelectSingleNode("//pptxnr/file_name").InnerText=fileName;
						this.responseDoc.SelectSingleNode("//pptxnr/sending_org").InnerText=org;
						this.responseDoc.SelectSingleNode("//pptxnr/date_created").InnerText=date;
						this.responseDoc.SelectSingleNode("//pptxnr/time_created").InnerText=time;
						this.responseDoc.SelectSingleNode("//pptxnr/sequence_number").InnerText=seqNumber;
						//this.responseDoc.SelectSingleNode("//pptxnr/transaction_count").InnerText=txnCount;
						if(txnValidator.ValidateXml(headerDoc.OuterXml,"header",out errMsg))
						{
							Logger.WriteToEventLog("NGCTxnBatch","Validated XML "+this.filename);
							xsdMutex.ReleaseMutex();
							if(txnValidator.ValidateHeader(fileName,org,date,time,seqNumber,txnCount,txnDoc.SelectNodes("//pptxn/transaction").Count,out errMsg,out errDesc,out isAgency))
							{
								totalTxnCount=int.Parse(txnCount);
								Logger.WriteToEventLog("NGCTxnBatch","Validated Header "+this.filename);
								XmlNodeList listTxnRecords=txnDoc.SelectNodes("//pptxn/transaction");
								foreach(XmlNode nodeTxn in listTxnRecords)
								{
									//validate each of the txn records against the schema
									if(txnValidator.ValidateXml(nodeTxn.OuterXml,"detail",out errDesc))
									{
										validTxn=true;
									}
									if(validTxn)
									{
										partnerNumber=nodeTxn.SelectSingleNode("./partner_number").InnerText;
										if(nodeTxn.SelectSingleNode("./partner_outlet_ref")!=null)
										{
											partnerOutRef=nodeTxn.SelectSingleNode("./partner_outlet_ref").InnerText.Trim();
										}
										if(nodeTxn.SelectSingleNode("./partner_outlet_name")!=null)
										{
											partnerOutName=nodeTxn.SelectSingleNode("./partner_outlet_name").InnerText.Trim();
										}
										if(nodeTxn.SelectSingleNode("./card_account_number")!=null)
										{
											cardAccNumber=nodeTxn.SelectSingleNode("./card_account_number").InnerText.Trim();
										}
										if(nodeTxn.SelectSingleNode("./official_id")!=null)
										{
											officialId=nodeTxn.SelectSingleNode("./official_id").InnerText.Trim();
										}
										transactDate=nodeTxn.SelectSingleNode("./transaction_date").InnerText;
										transactTime=nodeTxn.SelectSingleNode("./transaction_time").InnerText;
										spend=nodeTxn.SelectSingleNode("./spend").InnerText;
										partnerPoints=nodeTxn.SelectSingleNode("./partner_points").InnerText;
										if(nodeTxn.SelectSingleNode("./partner_reference")!=null)
										{
											partnerRef=nodeTxn.SelectSingleNode("./partner_reference").InnerText;
										}
										if(nodeTxn.SelectSingleNode("./partner_pos_id")!=null)
										{
											partnerPosId=nodeTxn.SelectSingleNode("./partner_pos_id").InnerText;
										}
										//see if the xml is valid for txn record
										addPartnerOutlet=false;
										addSkeleton=false;
										customerCrmid="";
										if(txnValidator.ValidateTxn(org,partnerNumber,partnerOutRef,partnerOutName,cardAccNumber,officialId,transactDate,transactTime,spend,partnerPoints,partnerRef,partnerPosId,out addPartnerOutlet,out errMsg,out errDesc,isAgency,out addSkeleton,out customerCrmid))
										{
											if(spend=="" || spend==null)
											{
												spend="0";
											}
											this.amount_spent_total=this.amount_spent_total+Decimal.Parse(spend);
											if(partnerPoints=="" || partnerPoints==null)
											{
												partnerPoints="0";
											}
											this.extra_points_2=this.extra_points_2+Decimal.Parse(partnerPoints);
											AddTxnRecord(partnerNumber,partnerOutRef,partnerOutName,cardAccNumber,officialId,transactDate,transactTime,spend,partnerPoints,partnerRef,partnerPosId,addPartnerOutlet,addSkeleton,customerCrmid);
											validTxn=true;
											countTxn=countTxn+1;
										}
										else
										{
											validTxn=false;
										}
									}
									if(!validTxn)
									{
										AddRejectTxnToResponse(nodeTxn,errMsg,errDesc);
										rejectedRecordCount=rejectedRecordCount+1;
										if(rejectedRecordCount==errorLimit)
										{
											bool deleteRecords=false;
											StopProcessing(deleteRecords);
											batchResponseCode="99";
											inValidFile=true;
											this.responseDoc.SelectSingleNode("//pptxnr/transaction_count").InnerText="0";
											break;
										}
										//								else
										//								{
										//									AddRejectTxnToResponse(nodeTxn,errMsg,errDesc);
										//								}
									}
									partnerNumber="";
									partnerOutRef="";
									partnerOutName="";
									cardAccNumber="";
									officialId="";
									transactDate="";
									transactTime="";
									spend="";
									partnerPoints="";
									partnerRef="";
									partnerPosId="";
									addPartnerOutlet=false;
									count=count+1;
									validTxn=false;
								}

								if(!inValidFile)
								{
									if(rejectedRecordCount==0)
									{
										batchResponseCode="00";
									}
									else
									{
										batchResponseCode="01";
									}
									Logger.WriteToEventLog("NGCTxnBatch","Inserting records to DB "+this.filename);
									//Insert the records in DB
									txnDataAccess.AddRecordsToDB(this.dsTxn,this.dsPartnerOutlet,org,seqNumber,this.extra_points_2,this.amount_spent_total,this.dtProcessedDate,out errDesc);
									Logger.WriteToEventLog("NGCTxnBatch","Finished inserting records to DB "+this.filename);
									if(errDesc!="")
									{
										batchResponseCode="99";
										rejectedRecordCount=totalTxnCount;
										countTxn=0;
										this.responseDoc.SelectSingleNode("//pptxnr/error_description").InnerText=errDesc;
										bool deleteRecords=true;
										StopProcessing(deleteRecords);
									}
								}
								this.responseDoc.SelectSingleNode("//pptxnr/response_code").InnerText=batchResponseCode;
							}
							else//log error that the header was not as expected
							{
								Logger.WriteToEventLog("NGCTxnBatch","The file "+nameOfFile+" was rejected and no records were processed.Response code= "+errMsg+". Reason = "+errDesc);
								this.responseDoc.SelectSingleNode("//pptxnr/response_code").InnerText=errMsg;
								rejectedRecordCount=totalTxnCount;
								countTxn=0;
							}
						}
						else//log error that the xml was not as per the expected format
						{
							xsdMutex.ReleaseMutex();
							Logger.WriteToEventLog("NGCTxnBatch","The file "+nameOfFile+" was rejected and no records were processed as it was not as per the schema. "+errMsg);
							this.responseDoc.SelectSingleNode("//pptxnr/error_description").InnerText=errMsg;
							rejectedRecordCount=totalTxnCount;
							countTxn=0;
						}
					}
				}
				txnDoc=null;
				string justFileName="";
				ArchiveFile(this.filename,out justFileName);
				if(sendMail)
				{
					if(!txnDataAccess.GetMailIdForOrg(org,out agencyMailId))
					{
						Logger.WriteToEventLog("NGCTxnBatch","Could not send mail to partner/agency as the mailid for partner "+org+" could not be found");
					}
					txnMailer.MailResponse(this.responseDoc,agencyMailId,"",justFileName);	
				}
			}
			catch(Exception ex)
			{
				Logger.WriteToEventLog("NGCTxnBatch","Error while Processing File : - "+nameOfFile+" "+ex.Message);
			}
			finally
			{
				Logger.WriteToEventLog("NGCTxnBatch","Finished Processing File : - "+nameOfFile+" Partner Number : - "+org+" Total Records in File: - "+totalTxnCount.ToString()+" Records Processed : - "+countTxn.ToString()+" Records Rejected : - "+rejectedRecordCount.ToString());
			}
		}
		#endregion

		#region AddRejectTxnToResponse
		private bool AddRejectTxnToResponse(XmlNode nodeTxn,string errMsg,string errDesc)
		{
			bool success=true;
			try
			{
				XmlElement txnElem=this.responseDoc.CreateElement("rejected_transactions"); 
				txnElem.InnerXml=nodeTxn.InnerXml;
				XmlElement errorElem=this.responseDoc.CreateElement("error_code"); 
				errorElem.InnerText=errMsg;
				XmlElement errorDescElem=this.responseDoc.CreateElement("error_description"); 
				errorDescElem.InnerText=errDesc;

				txnElem.AppendChild(errorElem);
				txnElem.AppendChild(errorDescElem);

				this.responseDoc.SelectSingleNode("//pptxnr").AppendChild(txnElem);
			}
			catch(Exception)
			{
				success=false;
			}
			return success;
		}
		#endregion

		#region AddTxnRecord
		private bool AddTxnRecord(string partnerNumber,string partnerOutRef,string partnerOutName,string cardAccNumber,string officialId,string transactDate,string transactTime,string spend,string partnerPoints,string partnerRef,string partnerPosId,bool addPartnerOutlet,bool addSkeleton,string customerCrmid)
		{
			bool success=true;
			try
			{
				Util txnUtil=new Util();
				object[] txn=new object[21];
				txn[0]=txnUtil.ConvertToDateTime(transactDate,transactTime);//txn_date --need to populate txndate+txntime
				if(partnerPosId!="")
				{
					if(partnerPosId.Length>=6)
					{
						txn[1]=partnerPosId.Substring(0,6);//pos_id --need to populate partnerposid.Substring(0,6)
					}
					else
					{
						txn[1]=partnerPosId;
					}
				}
				else
				{
					txn[1]=System.DBNull.Value;
				}
				if(partnerRef!="")
				{
					if(partnerRef.Length>=5)
					{
						txn[2]=partnerRef.Substring(partnerRef.Length-5,5);//txn_nbr --need to populate the right 5 characters from partnerRef
					}
					else
					{
						txn[2]=partnerRef;
					}
				}
				else
				{
					txn[2]=System.DBNull.Value;;
				}
				txn[3]=0;//amount_spent - 0
				txn[4]=0;//total_points -
				txn[5]=0;//welcome_points
				txn[6]=0;//product_points
				txn[7]=0;//extra_points1
				txn[8]=partnerPoints;//extra_points2
				txn[9]=0;//extra_points3
				txn[10]=spend;//amount_spent_at_partner
				txn[11]=partnerRef;//partner_reference
				txn[12]=partnerPosId;//partner_pos_id
				txn[13]=partnerOutRef;//partner_outlet_ref
				txn[14]=partnerOutName;//partner_outlet_name
				txn[15]=this.dtProcessedDate;//points_partner_processed_dt -- need to populate with the same datettime for all the txns
				txn[16]=partnerNumber;//partner_number
				if(officialId!="")
				{
					txn[17]=officialId;//official_id 
				}
				else
				{
					txn[17]=System.DBNull.Value;
				}
				if(cardAccNumber!="")
				{
					txn[18]=cardAccNumber;//card_account_number
				}
				else
				{
					txn[18]=System.DBNull.Value;
				}
				if(addSkeleton)
				{
					txn[19]=true;
				}
				else
				{
					txn[19]=false;
				}
				if(customerCrmid=="")
				{
					txn[20]=System.DBNull.Value;
				}
				else
				{
					txn[20]=customerCrmid;
				}
				this.dsTxn.Tables["Txn"].Rows.Add(txn);
				if(addPartnerOutlet)
				{
					object[] partnerOutlet=new object[4];
					partnerOutlet[0]=999999;//partner_outlet_number
					partnerOutlet[1]=partnerOutName;//partner_outlet_name
					partnerOutlet[2]=partnerOutRef;//partner_outlet_reference
					partnerOutlet[3]=partnerNumber;//partner_number
					this.dsPartnerOutlet.Tables["Partner_Outlet"].Rows.Add(partnerOutlet);
				}
			}
			catch(Exception)
			{
				success=false;
			}
			return success;
		}
		#endregion

		#region ArchiveFile
		private void ArchiveFile(string fileName,out string justFileName)
		{
			justFileName=filename.Substring(filename.LastIndexOf(@"\"),fileName.Length-filename.LastIndexOf(@"\"));
			string destFolder=System.Configuration.ConfigurationSettings.AppSettings["ArchiveFolderPath"];
			try
			{
				File.Copy(filename,destFolder+@"\"+justFileName,true);
				File.Delete(filename);
			}
			catch(Exception ex)
			{
				Logger.WriteToEventLog("NGCTxnBatch","The file "+fileName+" could not be archived due to the following error "+ex.Message);
			}
		}
		#endregion

		#region StopProcessing
		private void StopProcessing(bool deleteRecords)
		{
			try
			{
				this.responseDoc.SelectSingleNode("//response_code").InnerText="99";
				if(deleteRecords)
				{
					XmlNodeList listResponseNodes=this.responseDoc.SelectNodes("//rejected_transactions");
					foreach(XmlNode nodeResponse in listResponseNodes)
					{
						nodeResponse.ParentNode.RemoveChild(nodeResponse);
					}
				}
			}
			catch(Exception)
			{
				
			}
		}
		#endregion
		
	}
}
