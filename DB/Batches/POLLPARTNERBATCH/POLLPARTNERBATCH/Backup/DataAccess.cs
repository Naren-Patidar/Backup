
#region Using
using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
#endregion

namespace PollPartnerBatchService
{
	/// <summary>
	/// This class takes care of all the database access required
	/// </summary>
	public class DataAccess
	{
		private string connString="";

		#region DataAccess
		public DataAccess()
		{
			this.connString=ConfigurationSettings.AppSettings["AdminConnectionString"];
		}
		#endregion

		#region ValidateOrg
		protected internal bool ValidateOrg(string orgNumber,out bool isAgency)
		{
			string validateOrgSp="sp_poll_validateorg";
			bool valid=false;
			int returnCode=-1;
			SqlConnection txnConnection=null;
			isAgency=false;
			try
			{
				txnConnection=new SqlConnection(this.connString);
				txnConnection.Open();
				SqlCommand txnCommand=new SqlCommand();
				txnCommand.CommandType=CommandType.StoredProcedure;
				txnCommand.CommandText=validateOrgSp;
				txnCommand.Connection=txnConnection;
				txnCommand.Parameters.Add("@org_number",orgNumber);
				SqlParameter retParam=new SqlParameter("@Return_Value",SqlDbType.Int);
				retParam.Direction=ParameterDirection.ReturnValue;
				txnCommand.Parameters.Add(retParam);
				txnCommand.ExecuteNonQuery();
				returnCode = (int)retParam.Value;
				if(returnCode==2)
				{
					valid=true;
					isAgency=true;
				}
				else if(returnCode==1)
				{
					valid=true;
					isAgency=false;
				}
			}
			catch(Exception)
			{
				valid=false;
			}
			finally
			{
				txnConnection.Close();
			}
			return valid;
		}
		#endregion

		#region GetMaxSequenceNumber
		protected internal int GetMaxSequenceNumber(string orgNumber)
		{
			string validateOrgSp="sp_poll_getmaxsequencenumber";
			int seqNumber=0;
			SqlConnection txnConnection=null;
			try
			{
				SqlDataReader txnReader;
				txnConnection=new SqlConnection(this.connString);
				txnConnection.Open();
				SqlCommand txnCommand=new SqlCommand();
				txnCommand.CommandType=CommandType.StoredProcedure;
				txnCommand.CommandText=validateOrgSp;
				txnCommand.Connection=txnConnection;
				txnCommand.Parameters.Add("@org_number",orgNumber);
				txnReader=txnCommand.ExecuteReader();
				while(txnReader.Read())
				{
					if(txnReader[0]!=System.DBNull.Value)
					{
						seqNumber=int.Parse(txnReader[0].ToString());
					}
				}	
			}
			catch(Exception)
			{
				seqNumber=0;
			}
			finally
			{
				txnConnection.Close();
			}
			return seqNumber;
		}
		#endregion

		#region AddRecordsToDB
		protected internal void AddRecordsToDB(DataSet dsTxn,DataSet dsPartnerOutlet,string orgNumber,string seqNumber,decimal extraPointsTotal,decimal amtSpendTotal,DateTime processedDate,out string errDescription)
		{
			SqlTransaction txnTransaction;
			SqlConnection txnConnection=new SqlConnection(this.connString);
			txnConnection.Open();
			txnTransaction=txnConnection.BeginTransaction(IsolationLevel.ReadCommitted);
			errDescription="";
			try
			{
//				if(dsTxn.Tables[0].Rows.Count>0)//should update the sequence number even if no records are inserted
//				{
					SqlCommand cmdTxn=new SqlCommand("sp_poll_insertpartnertxn");
					cmdTxn.CommandType=CommandType.StoredProcedure;
					cmdTxn.Connection=txnConnection;
					cmdTxn.Transaction=txnTransaction;
					cmdTxn.Parameters.Add("@txn_date",SqlDbType.DateTime,8,"txn_date");
					cmdTxn.Parameters.Add("@pos_id",SqlDbType.VarChar,6,"pos_id");
					cmdTxn.Parameters.Add("@txn_nbr",SqlDbType.VarChar,5,"txn_nbr");
					cmdTxn.Parameters.Add("@amount_spent",SqlDbType.Decimal,15,"amount_spent");
					cmdTxn.Parameters.Add("@total_points",SqlDbType.Decimal,13,"total_points");
					cmdTxn.Parameters.Add("@welcome_points",SqlDbType.Decimal,13,"welcome_points");
					cmdTxn.Parameters.Add("@product_points",SqlDbType.Decimal,13,"product_points");
					cmdTxn.Parameters.Add("@extra_points_1",SqlDbType.Decimal,13,"extra_points_1");
					cmdTxn.Parameters.Add("@extra_points_2",SqlDbType.Decimal,13,"extra_points_2");
					cmdTxn.Parameters.Add("@extra_points_3",SqlDbType.Decimal,13,"extra_points_3");
					cmdTxn.Parameters.Add("@amount_spent_at_partner",SqlDbType.Decimal,15,"amount_spent_at_partner");
					cmdTxn.Parameters.Add("@partner_reference",SqlDbType.NVarChar,30,"partner_reference");
					cmdTxn.Parameters.Add("@partner_pos_id",SqlDbType.NVarChar,30,"partner_pos_id");
					cmdTxn.Parameters.Add("@partner_outlet_ref",SqlDbType.NVarChar,30,"partner_outlet_ref");
					cmdTxn.Parameters.Add("@partner_outlet_name",SqlDbType.NVarChar,30,"partner_outlet_name");
					cmdTxn.Parameters.Add("@points_partner_processed_dt",SqlDbType.DateTime,8,"points_partner_processed_dt");
					cmdTxn.Parameters.Add("@partner_number",SqlDbType.NVarChar,20,"partner_number");
					cmdTxn.Parameters.Add("@official_id",SqlDbType.NVarChar,13,"official_id");
					cmdTxn.Parameters.Add("@card_account_number",SqlDbType.VarChar,20,"card_account_number");
				    cmdTxn.Parameters.Add("@add_skeleton_account",SqlDbType.Bit,1,"add_skeleton_account");
					cmdTxn.Parameters.Add("@customer_crmidforoffid",SqlDbType.VarChar,50,"customer_crmid");
					SqlParameter sqlParam=new SqlParameter("@return_code",SqlDbType.TinyInt);
					sqlParam.Direction=ParameterDirection.Output;
					cmdTxn.Parameters.Add(sqlParam);
					SqlDataAdapter txnAdapter=new SqlDataAdapter();
					txnAdapter.InsertCommand=cmdTxn;
					//txnAdapter.RowUpdated+=new SqlRowUpdatedEventHandler(TxnRowUpdated);
					txnAdapter.Update(dsTxn,"Txn");
			
					SqlCommand cmdOffer=new SqlCommand("sp_poll_updateoffer");
					cmdOffer.CommandType=CommandType.StoredProcedure;
					cmdOffer.Connection=txnConnection;
					cmdOffer.Transaction=txnTransaction;
					cmdOffer.Parameters.Add("@points_partner_processed_dt",processedDate);
					cmdOffer.Parameters.Add("@extra_points_2",extraPointsTotal);
					cmdOffer.Parameters.Add("@number_of_partner_transactions",dsTxn.Tables["Txn"].Rows.Count);
					cmdOffer.Parameters.Add("@amount_spent_at_partner",amtSpendTotal);
					cmdOffer.ExecuteNonQuery();

					SqlCommand cmdPartOutlet=new SqlCommand("sp_poll_insertpartneroutlet");
					cmdPartOutlet.CommandType=CommandType.StoredProcedure;
					cmdPartOutlet.Connection=txnConnection;
					cmdPartOutlet.Transaction=txnTransaction;
					cmdPartOutlet.Parameters.Add("@partner_outlet_number",SqlDbType.NVarChar,20,"partner_outlet_number");
					cmdPartOutlet.Parameters.Add("@partner_outlet_name",SqlDbType.NVarChar,30,"partner_outlet_name");
					cmdPartOutlet.Parameters.Add("@partner_outlet_reference",SqlDbType.NVarChar,30,"partner_outlet_reference");
					cmdPartOutlet.Parameters.Add("@partner_number",SqlDbType.NVarChar,20,"partner_number");
					SqlDataAdapter partnerOutletAdapter=new SqlDataAdapter();
					partnerOutletAdapter.InsertCommand=cmdPartOutlet;
					partnerOutletAdapter.Update(dsPartnerOutlet,"Partner_Outlet");

					SqlCommand cmdBatchSeqUpdate=new SqlCommand("sp_poll_updatebatchsequencenumber");
					cmdBatchSeqUpdate.CommandType=CommandType.StoredProcedure;
					cmdBatchSeqUpdate.Connection=txnConnection;
					cmdBatchSeqUpdate.Transaction=txnTransaction;
					cmdBatchSeqUpdate.Parameters.Add("@org_number",orgNumber);
					cmdBatchSeqUpdate.Parameters.Add("@batch_sequence_number",seqNumber);
					cmdBatchSeqUpdate.ExecuteNonQuery();
					txnTransaction.Commit();
//				}
			}
			catch(Exception ex)
			{
				txnTransaction.Rollback();
				errDescription=ex.Message;
			}
			finally
			{
				txnConnection.Close();
			}
		}
		#endregion

		#region GetMailIdForOrg
		protected internal bool GetMailIdForOrg(string orgNumber,out string orgMailId)
		{
			orgMailId="";
			string getMailIdSp="sp_poll_getmailidfororg";
			SqlConnection txnConnection=null;
			bool success=true;
			try
			{
				if(orgNumber !="")
				{
					txnConnection=new SqlConnection(this.connString);
					txnConnection.Open();
					SqlCommand txnCommand=new SqlCommand();
					txnCommand.CommandType=CommandType.StoredProcedure;
					txnCommand.CommandText=getMailIdSp;
					txnCommand.Connection=txnConnection;
					txnCommand.Parameters.Add("@org_number",orgNumber);
					SqlDataReader txnReader=txnCommand.ExecuteReader();
					while(txnReader.Read())
					{
						orgMailId=txnReader["org_mail_id"].ToString();
					}
				}
				else
				{
					success=false;
				}
			}
			catch(Exception)
			{
				success=false;
			}
			return success;
		}
		
		#endregion

		#region ValidateTxn
		protected internal bool ValidateTxn(string sendingOrg,string partnerNumber,string partOutRef,string partOutName,string cardAccNumber,string offId,string txnDate,string txnTime,string spend,string partPoints,string partRef,string partPosid,out bool addPartnerOutlet,out string errMsg,out string errDesc,bool isAgency,out bool addSkeleton,out string customerCrmid)
		{
			string getMailIdSp="sp_poll_validatetxn";
			Int16 retCode=0;
			Int16 skeletonAddFlag=0;
			Int16 outletAddFlag=0;
			SqlConnection txnConnection=null;
			Util txnUtil=new Util();
			bool valid=false;
			errDesc="";
			errMsg="";
			addPartnerOutlet=false;
			addSkeleton=false;
			customerCrmid="";
			try
			{
				txnConnection=new SqlConnection(this.connString);
				txnConnection.Open();
				SqlCommand txnCommand=new SqlCommand();
				txnCommand.CommandType=CommandType.StoredProcedure;
				txnCommand.CommandText=getMailIdSp;
				txnCommand.Connection=txnConnection;
				SqlParameter sqlParam= txnCommand.Parameters.Add("@org_number",SqlDbType.NVarChar,20);
				sqlParam.Value = sendingOrg;
				sqlParam= txnCommand.Parameters.Add("@agency_flag",SqlDbType.TinyInt);
				if(isAgency)
				{
					sqlParam.Value = 1;
				}
				else
				{
					sqlParam.Value = 0;
				}
				sqlParam = txnCommand.Parameters.Add("@Partner_Number",SqlDbType.NVarChar,20);
				sqlParam.Value = partnerNumber;
				sqlParam = txnCommand.Parameters.Add("@card_flag",SqlDbType.SmallInt); 
				if(cardAccNumber != null && cardAccNumber.Trim() !="") 
					sqlParam.Value = 1;
				else
					sqlParam.Value = 0;
				sqlParam = txnCommand.Parameters.Add("@partner_points",SqlDbType.Decimal);   
				sqlParam.Value = Decimal.Parse(partPoints);     
				sqlParam = txnCommand.Parameters.Add("@partner_outlet_ref",SqlDbType.NVarChar,30); 
				if(partOutRef  != null && partOutRef.Trim() !="") 
					sqlParam.Value = partOutRef;
				else
					sqlParam.Value = DBNull.Value;
				sqlParam = txnCommand.Parameters.Add("@card_account_number",SqlDbType.NVarChar,20);      
				if(cardAccNumber  != null && cardAccNumber.Trim() !="") 
					sqlParam.Value = cardAccNumber;
				else
					sqlParam.Value = DBNull.Value; 
				sqlParam = txnCommand.Parameters.Add("@official_id",SqlDbType.NVarChar,13);  
				sqlParam.Value = offId;
				sqlParam = txnCommand.Parameters.Add("@return_code",SqlDbType.TinyInt);     
				sqlParam.Direction = ParameterDirection.Output;  
				sqlParam = txnCommand.Parameters.Add("@create_skeleton_flag",SqlDbType.TinyInt); 
				sqlParam.Direction = ParameterDirection.Output;  
				sqlParam = txnCommand.Parameters.Add("@add_partner_outlet",SqlDbType.TinyInt);  
				sqlParam.Direction = ParameterDirection.Output;  
				sqlParam = txnCommand.Parameters.Add("@customer_crmidforoffid",SqlDbType.UniqueIdentifier);  
				sqlParam.Direction = ParameterDirection.Output;  
				txnCommand.ExecuteReader();
				retCode = Int16.Parse(txnCommand.Parameters["@return_code"].Value.ToString() );
				skeletonAddFlag= Int16.Parse(txnCommand.Parameters["@create_skeleton_flag"].Value.ToString() );
				outletAddFlag= Int16.Parse(txnCommand.Parameters["@add_partner_outlet"].Value.ToString() );  
				if(txnCommand.Parameters["@customer_crmidforoffid"].Value!=System.DBNull.Value)
				{
					customerCrmid= txnCommand.Parameters["@customer_crmidforoffid"].Value.ToString();  
				}
				if(skeletonAddFlag==1)
				{
					addSkeleton=true;
					if(ConfigurationSettings.AppSettings["CreateSkeletonAccount"].ToString()!="1")
					{
						errMsg="04";
						return false;
					}
					if(ConfigurationSettings.AppSettings["PerformCheckDigitValidation"]=="1")
					{
						if(!txnUtil.CalculateCheckDigit(cardAccNumber))
						{
							errMsg="13";
							return false;
						}
					}
				}
				if(outletAddFlag==1)
				{
					addPartnerOutlet=true;
				}
				switch (retCode)
				{
					case 0:
						valid=true;
						break;
					case 1:
						valid=false;
						errMsg="01";
                        break;
					case 2:
						valid=false;
						errMsg="05";
						break;
					case 3:
						valid=false;
						errMsg="05";
						break;
					case 4:
						valid=false;
						errMsg="04";
						break;
					case 5:
						valid=false;
						errMsg="11";
						break;
					case 6:
						valid=false;
						errMsg="08";
						break;
					case 7:
						valid=false;
						errMsg="13";
						break;
					default:
						break;

				}
			}
			catch(Exception)
			{
				valid=false;
			}
			finally
			{
				txnConnection.Close();        
			}
			return valid;
		}
		#endregion
	}
}
