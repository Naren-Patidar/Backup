#region Using
using System;
using System.Xml;
using System.Web.Mail;
using System.Configuration;
#endregion

namespace PollPartnerBatchService
{
	/// <summary>
	/// Summary description for Mailer.
	/// </summary>
	public class Mailer
	{
		public Mailer()
		{
			
		}

		#region MailResponse
		protected internal bool MailResponse(XmlDocument txnResponseDoc,string to,string subject,string fileName)
		{
			bool success=true;
			try
			{
				fileName=fileName.Replace(@"\","");
				fileName=fileName.Replace(".xml","_Response.xml");
				txnResponseDoc.SelectSingleNode("//pptxnr/transaction_count").InnerText=txnResponseDoc.SelectNodes("//rejected_transactions").Count.ToString();
				//txnResponseDoc.SelectSingleNode("//pptxnr/file_name").InnerText=fileName;
				txnResponseDoc.Save(ConfigurationSettings.AppSettings["ResponseArchiveFolderPath"]+@"\"+fileName);
				MailMessage txnMsg=new MailMessage();
				txnMsg.Bcc=ConfigurationSettings.AppSettings["MailControlAccount"];
				txnMsg.To=to;
				txnMsg.Subject=fileName+" ["+to+"]";
				txnMsg.From=ConfigurationSettings.AppSettings["MailFromAccount"];
				txnMsg.Body="";
				MailAttachment txnAttach=new MailAttachment(ConfigurationSettings.AppSettings["ResponseArchiveFolderPath"]+@"\"+fileName);
				txnMsg.Attachments.Add(txnAttach);
				SmtpMail.SmtpServer=ConfigurationSettings.AppSettings["SMTPServer"];
				SmtpMail.Send(txnMsg);
			}
			catch(Exception)
			{
				success=false;
			}
			return success;
		}
		#endregion
	}
}
