
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
	#region usp_InsertBookingError Wrapper
    /// <summary>
    /// This class is a wrapper for the usp_InsertBookingError stored procedure.
    /// </summary>
    public class InsertBookingError
    {
		#region Member Variables
		protected string _connectionString = String.Empty;
        protected SqlConnection _connection = null;
        protected SqlTransaction _transaction = null;
		protected bool _ownsConnection = true;
		protected int _recordsAffected = -1;
		protected int _returnValue = 0;
		protected SqlInt32 _bookingId = SqlInt32.Null;
		protected bool _bookingIdSet = false;
		protected SqlDateTime _errorDate = SqlDateTime.Null;
		protected bool _errorDateSet = false;
		protected SqlString _errorMessage = SqlString.Null;
		protected bool _errorMessageSet = false;
		#endregion
		
		#region Constructors
		public InsertBookingError()
		{
		}
		
		public InsertBookingError(string connectionString)
		{
			this.ConnectionString = connectionString;
		}
		
		public InsertBookingError(SqlConnection connection)
		{
			this.Connection = connection;
		}
		
		public InsertBookingError(SqlConnection connection, SqlTransaction transaction)
		{
			this.Connection = connection;
			this.Transaction = transaction;
		}
		#endregion
		
		#region Public Properties
		/// <summary>
		/// The connection string to use when executing the usp_InsertBookingError stored procedure.
		/// </summary>
		public string ConnectionString
		{
			get {return _connectionString;}
			set {_connectionString = value;}
		}
        
        /// <summary>
        /// The connection to use when executing the usp_InsertBookingError stored procedure.
        /// If this is not null, it will be used instead of creating a new connection.
        /// </summary>
        public SqlConnection Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }
		
        /// <summary>
        /// The transaction to use when executing the usp_InsertBookingError stored procedure.
        /// If this is not null, the stored procedure will be executing within the transaction.
        /// </summary>
        public SqlTransaction Transaction
        {
            get { return _transaction; }
            set { _transaction = value; }
        }
		
		/// <summary>
		/// Gets the return value from the usp_InsertBookingError stored procedure.
		/// </summary>
		public int ReturnValue
		{
			get {return _returnValue;}
		}
		
		/// <summary>
		/// Gets the number of rows changed, inserted, or deleted by execution of the usp_InsertBookingError stored procedure.
		/// </summary>
		public int RecordsAffected
		{
			get {return _recordsAffected;}
		}
		
		/// <summary>
		/// 
		/// </summary>
		public SqlInt32 BookingId
		{
			get {return _bookingId;}
			set
			{
				_bookingId = value;
				_bookingIdSet = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public SqlDateTime ErrorDate
		{
			get {return _errorDate;}
			set
			{
				_errorDate = value;
				_errorDateSet = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public SqlString ErrorMessage
		{
			get {return _errorMessage;}
			set
			{
				_errorMessage = value;
				_errorMessageSet = true;
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
		/// This method calls the usp_InsertBookingError stored procedure.
		/// </summary>
		public virtual void Execute()
		{
			SqlCommand cmd = new SqlCommand();
			
			SqlConnection cn = this.GetConnection();
			
			try
			{
				cmd.Connection = cn;
				cmd.Transaction = this.Transaction;
				cmd.CommandText = "[dbo].[usp_InsertBookingError]";
				cmd.CommandType = CommandType.StoredProcedure;
				
				#region Populate Parameters
				SqlParameter prmReturnValue = cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
				prmReturnValue.Direction = ParameterDirection.ReturnValue;
				
				SqlParameter prmBookingId = cmd.Parameters.Add("@BookingId", SqlDbType.Int);
				prmBookingId.Direction = ParameterDirection.Input;
				if (_bookingIdSet == true || this.BookingId.IsNull == false)
				{
					prmBookingId.Value = this.BookingId;
				}
				
				SqlParameter prmErrorDate = cmd.Parameters.Add("@ErrorDate", SqlDbType.DateTime);
				prmErrorDate.Direction = ParameterDirection.Input;
				if (_errorDateSet == true || this.ErrorDate.IsNull == false)
				{
					prmErrorDate.Value = this.ErrorDate;
				}
				
				SqlParameter prmErrorMessage = cmd.Parameters.Add("@ErrorMessage", SqlDbType.VarChar);
				prmErrorMessage.Direction = ParameterDirection.Input;
					prmErrorMessage.Size = -1;
				if (_errorMessageSet == true || this.ErrorMessage.IsNull == false)
				{
					prmErrorMessage.Value = this.ErrorMessage;
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
		/// This method calls the usp_InsertBookingError stored procedure.
		/// </summary>
		/// <param name="connectionString">The connection string to use</param>
		/// <param name="bookingId"></param>
		/// <param name="errorDate"></param>
		/// <param name="errorMessage"></param>
		public static void Execute(
				#region Parameters
				string connectionString,
				SqlInt32 bookingId,
				SqlDateTime errorDate,
				SqlString errorMessage
				#endregion
		    )
		{
			InsertBookingError insertBookingError = new InsertBookingError();
			
			#region Assign Property Values
			insertBookingError.ConnectionString = connectionString;
			insertBookingError.BookingId = bookingId;
			insertBookingError.ErrorDate = errorDate;
			insertBookingError.ErrorMessage = errorMessage;
			#endregion
			
			insertBookingError.Execute();
			
			#region Get Property Values
			
			#endregion
		}
		#endregion
	}
	#endregion
}

