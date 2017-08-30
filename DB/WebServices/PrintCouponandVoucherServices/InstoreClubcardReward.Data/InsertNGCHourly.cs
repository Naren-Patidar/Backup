
//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by CodeSmith.
//
//     Date:    4/6/2009
//     Time:    2:36 
//     Version: 5.0.0.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

using System;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;

namespace InstoreClubcardReward.Data
{
	#region InsertNGCHourly Wrapper
    /// <summary>
    /// This class is a wrapper for the InsertNGCHourly stored procedure.
    /// </summary>
    public class InsertNGCHourly
    {
		#region Member Variables
		protected string _connectionString = String.Empty;
        protected SqlConnection _connection = null;
        protected SqlTransaction _transaction = null;
		protected bool _ownsConnection = true;
		protected int _recordsAffected = -1;
		protected int _returnValue = 0;
		protected SqlDateTime _replyDateHour = SqlDateTime.Null;
		protected bool _replyDateHourSet = false;
		protected SqlString _msgUrl = SqlString.Null;
		protected bool _msgUrlSet = false;
		protected SqlInt32 _msgCount = SqlInt32.Null;
		protected bool _msgCountSet = false;
		protected SqlInt32 _msgCount5sec = SqlInt32.Null;
		protected bool _msgCount5secSet = false;
		protected SqlInt32 _msgAvg = SqlInt32.Null;
		protected bool _msgAvgSet = false;
		protected SqlInt32 _msgException = SqlInt32.Null;
		protected bool _msgExceptionSet = false;
		protected SqlInt32 _msgError91 = SqlInt32.Null;
		protected bool _msgError91Set = false;
		protected SqlInt32 _msgError100 = SqlInt32.Null;
		protected bool _msgError100Set = false;
		#endregion
		
		#region Constructors
		public InsertNGCHourly()
		{
		}
		
		public InsertNGCHourly(string connectionString)
		{
			this.ConnectionString = connectionString;
		}
		
		public InsertNGCHourly(SqlConnection connection)
		{
			this.Connection = connection;
		}
		
		public InsertNGCHourly(SqlConnection connection, SqlTransaction transaction)
		{
			this.Connection = connection;
			this.Transaction = transaction;
		}
		#endregion
		
		#region Public Properties
		/// <summary>
		/// The connection string to use when executing the InsertNGCHourly stored procedure.
		/// </summary>
		public string ConnectionString
		{
			get {return _connectionString;}
			set {_connectionString = value;}
		}
        
        /// <summary>
        /// The connection to use when executing the InsertNGCHourly stored procedure.
        /// If this is not null, it will be used instead of creating a new connection.
        /// </summary>
        public SqlConnection Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }
		
        /// <summary>
        /// The transaction to use when executing the InsertNGCHourly stored procedure.
        /// If this is not null, the stored procedure will be executing within the transaction.
        /// </summary>
        public SqlTransaction Transaction
        {
            get { return _transaction; }
            set { _transaction = value; }
        }
		
		/// <summary>
		/// Gets the return value from the InsertNGCHourly stored procedure.
		/// </summary>
		public int ReturnValue
		{
			get {return _returnValue;}
		}
		
		/// <summary>
		/// Gets the number of rows changed, inserted, or deleted by execution of the InsertNGCHourly stored procedure.
		/// </summary>
		public int RecordsAffected
		{
			get {return _recordsAffected;}
		}
		
		/// <summary>
		/// 
		/// </summary>
		public SqlDateTime ReplyDateHour
		{
			get {return _replyDateHour;}
			set
			{
				_replyDateHour = value;
				_replyDateHourSet = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public SqlString MsgUrl
		{
			get {return _msgUrl;}
			set
			{
				_msgUrl = value;
				_msgUrlSet = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public SqlInt32 MsgCount
		{
			get {return _msgCount;}
			set
			{
				_msgCount = value;
				_msgCountSet = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public SqlInt32 MsgCount5sec
		{
			get {return _msgCount5sec;}
			set
			{
				_msgCount5sec = value;
				_msgCount5secSet = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public SqlInt32 MsgAvg
		{
			get {return _msgAvg;}
			set
			{
				_msgAvg = value;
				_msgAvgSet = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public SqlInt32 MsgException
		{
			get {return _msgException;}
			set
			{
				_msgException = value;
				_msgExceptionSet = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public SqlInt32 MsgError91
		{
			get {return _msgError91;}
			set
			{
				_msgError91 = value;
				_msgError91Set = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public SqlInt32 MsgError100
		{
			get {return _msgError100;}
			set
			{
				_msgError100 = value;
				_msgError100Set = true;
			}
		}
		#endregion
		
		#region Helper Methods
        private SqlConnection GetConnection()
        {
            if (this.Connection != null)
            {
				_ownsConnection = false;
                return this.Connection;
            }
            else
            {
 				System.Diagnostics.Debug.Assert(this.ConnectionString.Length != 0, "You must first set the ConnectioString property before calling an Execute method.");
            	return new SqlConnection(this.ConnectionString);
            }
        }
		#endregion
		
		#region Execute Methods
		/// <summary>
		/// This method calls the InsertNGCHourly stored procedure.
		/// </summary>
		public virtual void Execute()
		{
			SqlCommand cmd = new SqlCommand();
			
			SqlConnection cn = this.GetConnection();
			
			try
			{
				cmd.Connection = cn;
				cmd.Transaction = this.Transaction;
				cmd.CommandText = "[dbo].[InsertNGCHourly]";
				cmd.CommandType = CommandType.StoredProcedure;
				
				#region Populate Parameters
				SqlParameter prmReturnValue = cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
				prmReturnValue.Direction = ParameterDirection.ReturnValue;
				
				SqlParameter prmReplyDateHour = cmd.Parameters.Add("@ReplyDateHour", SqlDbType.DateTime);
				prmReplyDateHour.Direction = ParameterDirection.Input;
				if (_replyDateHourSet == true || this.ReplyDateHour.IsNull == false)
				{
					prmReplyDateHour.Value = this.ReplyDateHour;
				}
				
				SqlParameter prmMsgUrl = cmd.Parameters.Add("@MsgUrl", SqlDbType.VarChar);
				prmMsgUrl.Direction = ParameterDirection.Input;
					prmMsgUrl.Size = 70;
				if (_msgUrlSet == true || this.MsgUrl.IsNull == false)
				{
					prmMsgUrl.Value = this.MsgUrl;
				}
				
				SqlParameter prmMsgCount = cmd.Parameters.Add("@MsgCount", SqlDbType.Int);
				prmMsgCount.Direction = ParameterDirection.Input;
				if (_msgCountSet == true || this.MsgCount.IsNull == false)
				{
					prmMsgCount.Value = this.MsgCount;
				}
				
				SqlParameter prmMsgCount5sec = cmd.Parameters.Add("@MsgCount5sec", SqlDbType.Int);
				prmMsgCount5sec.Direction = ParameterDirection.Input;
				if (_msgCount5secSet == true || this.MsgCount5sec.IsNull == false)
				{
					prmMsgCount5sec.Value = this.MsgCount5sec;
				}
				
				SqlParameter prmMsgAvg = cmd.Parameters.Add("@MsgAvg", SqlDbType.Int);
				prmMsgAvg.Direction = ParameterDirection.Input;
				if (_msgAvgSet == true || this.MsgAvg.IsNull == false)
				{
					prmMsgAvg.Value = this.MsgAvg;
				}
				
				SqlParameter prmMsgException = cmd.Parameters.Add("@MsgException", SqlDbType.Int);
				prmMsgException.Direction = ParameterDirection.Input;
				if (_msgExceptionSet == true || this.MsgException.IsNull == false)
				{
					prmMsgException.Value = this.MsgException;
				}
				
				SqlParameter prmMsgError91 = cmd.Parameters.Add("@MsgError91", SqlDbType.Int);
				prmMsgError91.Direction = ParameterDirection.Input;
				if (_msgError91Set == true || this.MsgError91.IsNull == false)
				{
					prmMsgError91.Value = this.MsgError91;
				}
				
				SqlParameter prmMsgError100 = cmd.Parameters.Add("@MsgError100", SqlDbType.Int);
				prmMsgError100.Direction = ParameterDirection.Input;
				if (_msgError100Set == true || this.MsgError100.IsNull == false)
				{
					prmMsgError100.Value = this.MsgError100;
				}
				#endregion
				
				#region Execute Command
				if (cn.State != ConnectionState.Open) cn.Open();
				_recordsAffected = cmd.ExecuteNonQuery();
				#endregion
				
				#region Get Output Parameters
				if (prmReturnValue.Value != null && prmReturnValue.Value != DBNull.Value)
				{
					_returnValue = (int)prmReturnValue.Value;
				}
				
				#endregion
			}
			finally
			{
				if (_ownsConnection)
				{
					if (cn.State == ConnectionState.Open)
					{
						cn.Close();
					}
					
					cn.Dispose();
				}
				cmd.Dispose();
			}
		}
		
		/// <summary>
		/// This method calls the InsertNGCHourly stored procedure.
		/// </summary>
		/// <param name="connectionString">The connection string to use</param>
		/// <param name="replyDateHour"></param>
		/// <param name="msgUrl"></param>
		/// <param name="msgCount"></param>
		/// <param name="msgCount5sec"></param>
		/// <param name="msgAvg"></param>
		/// <param name="msgException"></param>
		/// <param name="msgError91"></param>
		/// <param name="msgError100"></param>
		public static void Execute(
				#region Parameters
				string connectionString,
				SqlDateTime replyDateHour,
				SqlString msgUrl,
				SqlInt32 msgCount,
				SqlInt32 msgCount5sec,
				SqlInt32 msgAvg,
				SqlInt32 msgException,
				SqlInt32 msgError91,
				SqlInt32 msgError100
				#endregion
		    )
		{
			InsertNGCHourly insertNGCHourly = new InsertNGCHourly();
			
			#region Assign Property Values
			insertNGCHourly.ConnectionString = connectionString;
			insertNGCHourly.ReplyDateHour = replyDateHour;
			insertNGCHourly.MsgUrl = msgUrl;
			insertNGCHourly.MsgCount = msgCount;
			insertNGCHourly.MsgCount5sec = msgCount5sec;
			insertNGCHourly.MsgAvg = msgAvg;
			insertNGCHourly.MsgException = msgException;
			insertNGCHourly.MsgError91 = msgError91;
			insertNGCHourly.MsgError100 = msgError100;
			#endregion
			
			insertNGCHourly.Execute();
			
			#region Get Property Values
			
			#endregion
		}
		#endregion
	}
	#endregion
}

