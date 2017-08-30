#region Using
using System;
using System.IO.IsolatedStorage;
using System.IO;
using System.Configuration;
using System.Text;
using System.Diagnostics;
#endregion

namespace OfflineTransactionsBatch_V3._0
{
	/// <summary>
	/// Summary description for UtilityManager.
	/// </summary>
	public class UtilityManager
	{
		#region UtilityManagerConstructor
		public UtilityManager()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		#endregion

		#region DeleteFilesFromIsolatedStorage
		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileName"></param>
		public static void DeleteFilesFromIsolatedStorage(string fileName)
		{
			try
			{
				IsolatedStorageFile isoTxnResponse=IsolatedStorageFile.GetStore(IsolatedStorageScope.User|IsolatedStorageScope.Assembly,null,null);
				if(isoTxnResponse.GetFileNames(fileName).Length>0)
				{
					isoTxnResponse.DeleteFile(fileName);
				}
				string numberFileName=fileName.Substring(0,fileName.IndexOf("."))+"number.dat";
				if(isoTxnResponse.GetFileNames(numberFileName).Length>0)
				{
					isoTxnResponse.DeleteFile(numberFileName);
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}

		#endregion

		#region WriteResponseFile
		/// <summary>
		/// 
		/// </summary>
		/// <param name="contents"></param>
		/// <param name="fileName"></param>
		public static void WriteToFile(string contents,string fileName,string folderPath)
		{
			string responseFolderPath=ConfigurationSettings.AppSettings[folderPath];
			string responseFileName=responseFolderPath+"\\"+fileName;
			FileStream fs=new FileStream(responseFileName,FileMode.Create,FileAccess.ReadWrite);
			StreamWriter sw=new StreamWriter(fs); 
			sw.Write(contents);
			sw.Close();
		}
		#endregion

		#region LogError
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public static void LogError(string message)
		{
			if(!EventLog.SourceExists("NGC Apply Offline Transactions"))
			{
				EventLog.CreateEventSource("NGC Apply Offline Transactions","Application");
			}
			EventLog.WriteEntry("NGC Apply Offline Transactions",message,EventLogEntryType.Error);
		}
		#endregion

		#region LogInformation
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public static void LogInformation(string message)
		{
			if(!EventLog.SourceExists("NGC Apply Offline Transactions"))
			{
				EventLog.CreateEventSource("NGC Apply Offline Transactions","Application");
			}
			EventLog.WriteEntry("NGC Apply Offline Transactions",message,EventLogEntryType.Information);
		}
		#endregion
		
		#region ArchiveFile
		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileName"></param>
		public static void ArchiveFile(string fileName)
		{
			string sourceFolder=ConfigurationSettings.AppSettings["TransactionFileFolderPath"];
			string destFolder=ConfigurationSettings.AppSettings["TransactionFileArchivePath"];
			File.Copy(sourceFolder+@"\"+fileName,destFolder+@"\"+fileName,true);
			File.Delete(sourceFolder+@"\"+fileName);
		}
		#endregion

		#region LogMessage
		public static void LogMessage(string message)
		{
			try
			{
				string logFolderPath=ConfigurationSettings.AppSettings["LogFileFolderPath"];
				string logFileName=logFolderPath+"\\OfflineTransactionsLog"+DateTime.Now.ToString("yyyyMMDDHHmmss")+".txt";
				FileStream fs=new FileStream(logFileName,FileMode.Create,FileAccess.ReadWrite);
				StreamWriter sw=new StreamWriter(fs); 
				sw.Write(message);
				sw.Close();
			}
			catch(Exception ex)
			{
				throw ex;
			}
		}
		#endregion

		#region DeleteFile
		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileName"></param>
		public static void DeleteFile(string fileName)
		{
			File.Delete(fileName);
		}
		#endregion

		#region GenerateXML
		public static bool correctFile(string inFileName,string outFileName)
		{
			//Takes the input file and generates the output file
			string path;
			path=System.IO.Directory.GetParent(inFileName).ToString();

            //Get the dfault Data Protection Preference
            string defaultDataProtectionPreference = ConfigurationSettings.AppSettings["DataProtectionDefault"].ToString();
				
			string[] keyWrds = new string[48];

			keyWrds[0] = "customer";

			keyWrds[1] = "name_1";

			keyWrds[2] = "official_id";

			keyWrds[3] = "primary_card_account_number";

			keyWrds[4] = "card_account_number";

			keyWrds[5] = "family_member_1_gender_code";

			keyWrds[6] = "family_member_1_dob";

			keyWrds[7] = "family_member_2_dob";

			keyWrds[8] = "family_member_3_dob";

			keyWrds[9] = "family_member_4_dob";

			keyWrds[10] = "family_member_5_dob";

			keyWrds[11] = "address_line_1";

			keyWrds[12] = "address_line_2";

			keyWrds[13] = "address_line_3";

			keyWrds[14] = "customer_title";

			keyWrds[15] = "city";

			keyWrds[16] = "province_code";

			keyWrds[17] = "postal_code";

			keyWrds[18] = "daytime_phone_number";

			keyWrds[19] = "evening_phone_number";

			keyWrds[20] = "mobile_phone_number";

			keyWrds[21] = "email_address";

			keyWrds[22] = "preferred_store_code";

			keyWrds[23] = "joined_store_code";

			keyWrds[24] = "preferred_contact_type_code";

			keyWrds[25] = "address_in_error";

			keyWrds[26] = "title_code";

			keyWrds[27] = "diabetic_flag";

			keyWrds[28] = "vegetarian_flag";

			keyWrds[29] = "teetotal_flag";

			keyWrds[30] = "halal_flag";

			keyWrds[31] = "customer_created_by";

			keyWrds[32] = "customer_created_date";

			keyWrds[33] = "business_name";

			keyWrds[34] = "business_registration_number";

			keyWrds[35] = "business_type_code";

			keyWrds[36] = "business_address_line_1";

			keyWrds[37] = "business_address_line_2";

			keyWrds[38] = "business_address_line_3";

			keyWrds[39] = "business_postal_code";

			keyWrds[40] = "business_city";

			keyWrds[41] = "business_province_code";

			keyWrds[42] = "preferred_mailing_address_flag";

			keyWrds[43] = "race_code";

			keyWrds[44] = "previous_loyalty_scheme_card_number";

			keyWrds[45] = "fax_number";

			keyWrds[46] = "form_type";

			keyWrds[47] = "number_of_household_members";
			string s;
			int indexPos = 0;
			int stringIndex = 0;
			try
			{
				if (File.Exists(inFileName))
				{
					TextReader readFile =new StreamReader(inFileName);
					outFileName=System.IO.Directory.GetParent(inFileName).ToString()+"\\"+outFileName;
					TextWriter tw = new StreamWriter(outFileName);
					tw.WriteLine("<customers>");
					s=readFile.ReadLine();
					while(s != null)
					{
						string [] tempStrArr = null;
						tempStrArr=s.Split('\t');
						for(stringIndex=0;stringIndex<=48;stringIndex++)
						{
							indexPos = stringIndex;
							string outstr="";
							switch (indexPos)
							{
								case 0:
									outstr = "<" + keyWrds[indexPos] + ">";
									break;

								case 1:
                                    outstr = "<" + keyWrds[indexPos] + ">OfflineFile(" + defaultDataProtectionPreference + ")</" + keyWrds[indexPos] + ">";
									break;

								case 2:
									outstr = "<" + keyWrds[indexPos] + ">" + "</" + keyWrds[indexPos] + ">";
									break;

								case 3:
									outstr = "<" + keyWrds[indexPos] + ">" + tempStrArr[1].Replace(" ", "") + "</" + keyWrds[indexPos] + ">";
									break;

								case 4:
									outstr = "<" + keyWrds[indexPos] + ">" + tempStrArr[1].Replace(" ", "") + "</" + keyWrds[indexPos] + ">";
									break;

								case 5:
									outstr = "<" + keyWrds[indexPos] + ">" + "</" + keyWrds[indexPos] + ">";
									break;

								case 6:
									outstr = "<" + keyWrds[indexPos] + ">" + "</" + keyWrds[indexPos] + ">";
									break;

								case 7:
									outstr = "<" + keyWrds[indexPos] + ">" + "</" + keyWrds[indexPos] + ">";
									break;

								case 8:
									outstr = "<" + keyWrds[indexPos] + ">" + "</" + keyWrds[indexPos] + ">";
									break;

								case 9:
									outstr = "<" + keyWrds[indexPos] + ">" + "</" + keyWrds[indexPos] + ">";
									break;

								case 10:
									outstr = "<" + keyWrds[indexPos] + ">" + "</" + keyWrds[indexPos] + ">";
									break;

								case 11:
									outstr = "<" + keyWrds[indexPos] + ">" + "</" + keyWrds[indexPos] + ">";
									break;

								case 12:
									outstr = "<" + keyWrds[indexPos] + ">" + "</" + keyWrds[indexPos] + ">";
									break;

								case 13:
									outstr = "<" + keyWrds[indexPos] + ">" + "</" + keyWrds[indexPos] + ">";
									break;

								case 14:
									outstr = "<" + keyWrds[indexPos] + ">" + "</" + keyWrds[indexPos] + ">";
									break;

								case 15:
									outstr = "<" + keyWrds[indexPos] + ">" + "</" + keyWrds[indexPos] + ">";
									break;

								case 16:
									outstr = "<" + keyWrds[indexPos] + ">" + "-1</" + keyWrds[indexPos] + ">";
									break;

								case 17:
									outstr = "<" + keyWrds[indexPos] + ">" + "</" + keyWrds[indexPos] + ">";
									break;

								case 18:
									outstr = "<" + keyWrds[indexPos] + ">" + "</" + keyWrds[indexPos] + ">";
									break;

								case 19:
									outstr = "<" + keyWrds[indexPos] + ">" + "</" + keyWrds[indexPos] + ">";
									break;

								case 20:
									outstr = "<" + keyWrds[indexPos] + ">" + "</" + keyWrds[indexPos] + ">";
									break;

								case 21:
									outstr = "<" + keyWrds[indexPos] + ">" + "</" + keyWrds[indexPos] + ">";
									break;

								case 22:
									outstr = "<" + keyWrds[indexPos] + ">" + tempStrArr[2].Replace(" ", "") + "</" + keyWrds[indexPos] + ">";
									break;

								case 23:
									outstr = "<" + keyWrds[indexPos] + ">" + tempStrArr[2].Replace(" ", "") + "</" + keyWrds[indexPos] + ">";
									break;

								case 24:
									outstr = "<" + keyWrds[indexPos] + ">" + "13</" + keyWrds[indexPos] + ">";
									break;

								case 25:
									outstr = "<" + keyWrds[indexPos] + ">" + "1</" + keyWrds[indexPos] + ">";
									break;

								case 26:
									outstr = "<" + keyWrds[indexPos] + ">" + "</" + keyWrds[indexPos] + ">";
									break;

								case 27:
									outstr = "<" + keyWrds[indexPos] + ">" + "0</" + keyWrds[indexPos] + ">";
									break;

								case 28:
									outstr = "<" + keyWrds[indexPos] + ">" + "0</" + keyWrds[indexPos] + ">";
									break;

								case 29:
									outstr = "<" + keyWrds[indexPos] + ">" + "0</" + keyWrds[indexPos] + ">";
									break;

								case 30:
									outstr = "<" + keyWrds[indexPos] + ">" + "0</" + keyWrds[indexPos] + ">";
									break;

								case 31:
									outstr = "<" + keyWrds[indexPos] + ">" + "System</" + keyWrds[indexPos] + ">";
									break;

								case 32:
									outstr = "<" + keyWrds[indexPos] + ">" + tempStrArr[0] + "</" + keyWrds[indexPos] + ">";
									break;

								case 33:
									outstr = "<" + keyWrds[indexPos] + ">" + "</" + keyWrds[indexPos] + ">";
									break;

								case 34:
									outstr = "<" + keyWrds[indexPos] + ">" + "</" + keyWrds[indexPos] + ">";
									break;

								case 35:
									outstr = "<" + keyWrds[indexPos] + ">" + "</" + keyWrds[indexPos] + ">";
									break;

								case 36:
									outstr = "<" + keyWrds[indexPos] + ">" + "</" + keyWrds[indexPos] + ">";
									break;

								case 37:
									outstr = "<" + keyWrds[indexPos] + ">" + "</" + keyWrds[indexPos] + ">";
									break;

								case 38:
									outstr = "<" + keyWrds[indexPos] + ">" + "</" + keyWrds[indexPos] + ">";
									break;

								case 39:
									outstr = "<" + keyWrds[indexPos] + ">" + "</" + keyWrds[indexPos] + ">";
									break;

								case 40:
									outstr = "<" + keyWrds[indexPos] + ">" + "</" + keyWrds[indexPos] + ">";
									break;

								case 41:
									outstr = "<" + keyWrds[indexPos] + ">" + "</" + keyWrds[indexPos] + ">";
									break;

								case 42:
									outstr = "<" + keyWrds[indexPos] + ">" + "</" + keyWrds[indexPos] + ">";
									break;

								case 43:
									outstr = "<" + keyWrds[indexPos] + ">" + "</" + keyWrds[indexPos] + ">";
									break;

								case 44:
									outstr = "<" + keyWrds[indexPos] + ">" + "</" + keyWrds[indexPos] + ">";
									break;

								case 45:
									outstr = "<" + keyWrds[indexPos] + ">" + "</" + keyWrds[indexPos] + ">";
									break;

								case 46:
									outstr = "<" + keyWrds[indexPos] + ">" + "1</" + keyWrds[indexPos] + ">";
									break;

								case 47:
									outstr = "<" + keyWrds[indexPos] + ">" + "</" + keyWrds[indexPos] + ">";
									break;

								case 48:
									outstr = "</" + keyWrds[0] + ">";
									break;
							}//End of Switch
							tw.WriteLine(outstr);
							
						}//End of For Loop

						tempStrArr=null;
						s=readFile.ReadLine();
					}//End of While Loop
					tw.WriteLine("</customers>");
					readFile.Close();
					tw.Close();
				}
				return true;
			}
			catch(Exception ex)
			{
				return  false;
			}
		}
		public string getOutLine(string fdata ,string keyword)
		{
			try
			{
				if( fdata.Trim() == "")
				{
					return  "<" + keyword + "/>";
				}
				else
				{
					return "<" + keyword + ">" + fdata + "</" + keyword + ">";
				}
			}
			catch(Exception ex)
			{
				//Process the Exception
				return "";
			}

		}

		#endregion


	}
}
