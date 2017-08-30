using System;
using System.Xml;
using System.Xml.Schema;
using System.Configuration;

namespace PollPartnerBatchService
{
	/// <summary>
	/// Summary description for Validator.
	/// </summary>
	public class Validator
	{
		private static bool validXml;
		private static string errorDescription="";
		public Validator()
		{
		}

		#region ValidateXml
		protected internal bool ValidateXml(string xmlString,string type,out string errDesc)
		{
			validXml=true;
			errDesc="";
			XmlValidatingReader reader;			
			string xsdPath="";
			try
			{
				if(type.ToUpper()=="HEADER")
				{
					xsdPath=ConfigurationSettings.AppSettings["TxnHeaderXsd"];
				}
				else if(type.ToUpper()=="DETAIL")
				{
					xsdPath=ConfigurationSettings.AppSettings["TxnDetailXsd"];
				}
				
				XmlParserContext context = new XmlParserContext(null, null, "", XmlSpace.None);
				reader = new XmlValidatingReader(xmlString, XmlNodeType.Element, context);
				reader.ValidationType=ValidationType.Schema;
				XmlSchemaCollection schemaCollection = new XmlSchemaCollection();
				schemaCollection.Add("",xsdPath);
				reader.Schemas.Add(schemaCollection);
				reader.ValidationEventHandler += new ValidationEventHandler(reader_ValidationEventHandler);
				while(reader.Read())
				{
				}
				reader.Close();
				errDesc=errorDescription;
			}
			catch(Exception)
			{
				validXml=false;
			}
			return validXml;
		}
		#endregion
		
		#region reader_ValidationEventHandler
		private static void reader_ValidationEventHandler(object sender, ValidationEventArgs e)
		{
			try
			{
				if(e.Severity==XmlSeverityType.Error)
				{
					validXml=false;
					errorDescription=e.Exception.Message;
				}
			}
			catch(Exception ex)
			{
				Logger.WriteToEventLog("NGCTxnBatch",ex.Message);
			}
		}
		#endregion

        #region ValidateHeader
		protected internal bool ValidateHeader(string fileName,string org,string date,string time,string seqNumber,string txnCount,int actualTxnCount,out string errorMsg,out string errDesc,out bool isAgency)
		{
			bool isValid=false;
			errorMsg="";
			errDesc="";
			isAgency=false;
			try
			{
				if(!this.ValidateFileName(fileName,org,date))
				{
					errorMsg="03";
					errDesc="File Name is not valid";
					return false;
				}
				if(!this.ValidateOrg(org,out isAgency))
				{
					errorMsg="05";
					errDesc="The organisation is not an agency or a direct partner";
					return false;
				}
				if(!this.ValidateFileDateTime(fileName,date,time))
				{
					errorMsg="04";
					errDesc="File date time is not valid";
					return false;
				}
				if(!this.ValidateSequence(seqNumber,org,out errorMsg,out errDesc))
				{
					return false;
				}
				if(!this.ValidateTxnCount(txnCount,actualTxnCount))
				{
					errorMsg="07";
					errDesc="Transaction count is not correct";
					return false;
				}
				isValid=true;
			}
			catch(Exception)
			{
				isValid=false;
			}
			return isValid;
		}
		#endregion
		
		#region ValidateFileName
		private bool ValidateFileName(string fileName,string org,string dtDate)
		{
			bool isValid=false;
			try
			{
				if(fileName.IndexOf(org+"_"+dtDate)>-1)
				{
					isValid=true;
				}
			}
			catch(Exception)
			{
				isValid=false;
			}
			return isValid;
		}
		#endregion

		#region ValidateOrg
		private bool ValidateOrg(string org,out bool isAgency)
		{
			bool isValid=false;
			DataAccess txnDataAccess=new DataAccess();
			if(txnDataAccess.ValidateOrg(org,out isAgency))
			{
				isValid=true;
			}	
			return isValid;
		}
		#endregion

		#region ValidateDateTime
		private bool ValidateFileDateTime(string fileName,string date,string time)
		{
			bool isValid=false;
			string fileDate="";
			try
			{
				DateTime dtFileDate;
				Util txnUtil=new Util();
				DateTime dtHeaderDate;
				DateTime dtHeaderDateTime;
				int startIndex=fileName.IndexOf("_");
				fileDate=fileName.Substring(startIndex+1,8);
				dtFileDate=txnUtil.ConvertToDateTime(fileDate,"");
				dtHeaderDate=txnUtil.ConvertToDateTime(date,"");
				dtHeaderDateTime=txnUtil.ConvertToDateTime(date,time);
				if(dtFileDate.CompareTo(dtHeaderDate)<=0)
				{
					isValid=true;
				}
				if(isValid)
				{
					if(dtHeaderDateTime.CompareTo(DateTime.Now)>0)
					{
						isValid=false;
					}
				}
			}
			catch(Exception)
			{
				isValid=false;
			}
			return isValid;
		}
		#endregion

		#region ValidateSequence
		private bool ValidateSequence(string seqNumber,string org,out string errMsg,out string errDesc)
		{
			bool isValid=false;
			int maxSeqNum=-1;
			errDesc="";
			errMsg="";
			try
			{
				DataAccess txnDataAccess=new DataAccess();
				maxSeqNum=txnDataAccess.GetMaxSequenceNumber(org);
				if(int.Parse(seqNumber)==maxSeqNum+1)
				{
					isValid=true;
				}
				else if(int.Parse(seqNumber)==maxSeqNum)
				{
					errMsg="09";
					errDesc="This file is already processed";
				}
				else
				{
					errMsg="06";
					errDesc="Sequence number is not valid";
				}
			}
			catch(Exception)
			{
				isValid=false;
			}
			return isValid;
		}
		#endregion

		#region ValidateTxnCount
		private bool ValidateTxnCount(string txnCount,int actualTxnCount)
		{
			bool isValid=false;
			try
			{
				if(int.Parse(txnCount)==actualTxnCount)
				{
					isValid=true;
				}
			}
			catch(Exception)
			{
				isValid=false;
			}
			return isValid;
		}
		#endregion

		#region ValidateTxn
		protected internal bool ValidateTxn(string sendingOrg,string partnerNumber,string partOutRef,string partOutName,string cardAccNumber,string offId,string txnDate,string txnTime,string spend,string partPoints,string partRef,string partPosid,out bool addPartnerOutlet,out string errMsg,out string errDesc,bool isAgency,out bool addSkeleton,out string customerCrmid)
		{
			bool isValid=false;
			addPartnerOutlet=false;
			errMsg="";
			errDesc="";
			Util txnUtil=new Util();
			addSkeleton=false;
			customerCrmid="";
			try
			{
				//check to see that if direct partner then sending org should be same as partner number
				if(!isAgency)
				{
					if(sendingOrg.Trim()!=partnerNumber.Trim())
					{
						errMsg="01";
						errDesc="";
						return false;
					}
				}
				//both card account number and official id caanot be null
				if((cardAccNumber=="")&&(offId==""))
				{
					errMsg="12";
					errDesc="";
					return false;
				}
				//txn date time cannot be in the future
				DateTime dtTxn=txnUtil.ConvertToDateTime(txnDate,txnTime);
				if(DateTime.Now.CompareTo(dtTxn)<0)
				{
					errMsg="06";
					errDesc="";
					return false;
				}
				if(spend!="")
				{
					if(Double.Parse(spend)<0)
					{
						if(Double.Parse(partPoints)>0)
						{
							errMsg="07";
							errDesc="";
							return false;
						}
					}
					else if(Double.Parse(spend)>0)
					{
						if(Double.Parse(partPoints)<0)
						{
							errMsg="07";
							errDesc="";
							return false;
						}
					}
				}
				else
				{
					errMsg="07";
					errDesc="";
					return false;

				}
				if(Double.Parse(partPoints)==0)
				{
					errMsg="07";
					errDesc="";
					return false;
				}
				//validate that at least one of them (partOutRef or partPosid) is present
				if((partOutRef=="")&&(partPosid==""))
				{
					errMsg="10";
					errDesc="";
					return false;
				}
				DataAccess txnDataAccess=new DataAccess();
				return txnDataAccess.ValidateTxn(sendingOrg,partnerNumber,partOutRef,partOutName,cardAccNumber,offId,txnDate,txnTime,spend,partPoints,partRef,partPosid,out addPartnerOutlet,out errMsg,out errDesc,isAgency,out addSkeleton,out customerCrmid);
			}
			catch(Exception)
			{
				isValid=false;
			}
			return isValid;
		}
		#endregion
	}
}
