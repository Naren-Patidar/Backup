
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
	#region UpdateVoucherStatus Wrapper
    /// <summary>
    /// This class is a wrapper for the UpdateVoucherStatus stored procedure.
    /// </summary>
    public class UpdateVoucherStatus
    {
		#region Member Variables
		protected string _connectionString = String.Empty;
        protected SqlConnection _connection = null;
        protected SqlTransaction _transaction = null;
		protected bool _ownsConnection = true;
		protected int _recordsAffected = -1;
		protected int _returnValue = 0;
		protected SqlInt32 _vSId = SqlInt32.Null;
		protected bool _vSIdSet = false;
		protected SqlInt32 _voucherStatus = SqlInt32.Null;
		protected bool _voucherStatusSet = false;
		protected SqlDateTime _requestDate = SqlDateTime.Null;
		protected bool _requestDateSet = false;
		#endregion
		
		#region Constructors
		public UpdateVoucherStatus()
		{
		}
		
		public UpdateVoucherStatus(string connectionString)
		{
			this.ConnectionString = connectionString;
		}
		
		public UpdateVoucherStatus(SqlConnection connection)
		{
			this.Connection = connection;
		}
		
		public UpdateVoucherStatus(SqlConnection connection, SqlTransaction transaction)
		{
			this.Connection = connection;
			this.Transaction = transaction;
		}
		#endregion
		
		#region Public Properties
		/// <summary>
		/// The connection string to use when executing the UpdateVoucherStatus stored procedure.
		/// </summary>
		public string ConnectionString
		{
			get {return _connectionString;}
			set {_connectionString = value;}
		}
        
        /// <summary>
        /// The connection to use when executing the UpdateVoucherStatus stored procedure.
        /// If this is not null, it will be used instead of creating a new connection.
        /// </summary>
        public SqlConnection Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }
		
        /// <summary>
        /// The transaction to use when executing the UpdateVoucherStatus stored procedure.
        /// If this is not null, the stored procedure will be executing within the transaction.
        /// </summary>
        public SqlTransaction Transaction
        {
            get { return _transaction; }
            set { _transaction = value; }
        }
		
		/// <summary>
		/// Gets the return value from the UpdateVoucherStatus stored procedure.
		/// </summary>
		public int ReturnValue
		{
			get {return _returnValue;}
		}
		
		/// <summary>
		/// Gets the number of rows changed, inserted, or deleted by execution of the UpdateVoucherStatus stored procedure.
		/// </summary>
		public int RecordsAffected
		{
			get {return _recordsAffected;}
		}
		
		/// <summary>
		/// 
		/// </summary>
		public SqlInt32 VSId
		{
			get {return _vSId;}
			set
			{
				_vSId = value;
				_vSIdSet = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public SqlInt32 VoucherStatus
		{
			get {return _voucherStatus;}
			set
			{
				_voucherStatus = value;
				_voucherStatusSet = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public SqlDateTime RequestDate
		{
			get {return _requestDate;}
			set
			{
				_requestDate = value;
				_requestDateSet = true;
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
		/// This method calls the UpdateVoucherStatus stored procedure.
		/// </summary>
		public virtual void Execute()
		{
			SqlCommand cmd = new SqlCommand();
			
			SqlConnection cn = this.GetConnection();
			
			try
			{
				cmd.Connection = cn;
				cmd.Transaction = this.Transaction;
				cmd.CommandText = "[dbo].[UpdateVoucherStatus]";
				cmd.CommandType = CommandType.StoredProcedure;
				
				#region Populate Parameters
				SqlParameter prmReturnValue = cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
				prmReturnValue.Direction = ParameterDirection.ReturnValue;
				
				SqlParameter prmVSId = cmd.Parameters.Add("@VSId", SqlDbType.Int);
				prmVSId.Direction = ParameterDirection.Input;
				if (_vSIdSet == true || this.VSId.IsNull == false)
				{
					prmVSId.Value = this.VSId;
				}
				
				SqlParameter prmVoucherStatus = cmd.Parameters.Add("@VoucherStatus", SqlDbType.Int);
				prmVoucherStatus.Direction = ParameterDirection.Input;
				if (_voucherStatusSet == true || this.VoucherStatus.IsNull == false)
				{
					prmVoucherStatus.Value = this.VoucherStatus;
				}
				
				SqlParameter prmRequestDate = cmd.Parameters.Add("@RequestDate", SqlDbType.DateTime);
				prmRequestDate.Direction = ParameterDirection.Input;
				if (_requestDateSet == true || this.RequestDate.IsNull == false)
				{
					prmRequestDate.Value = this.RequestDate;
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
		/// This method calls the UpdateVoucherStatus stored procedure.
		/// </summary>
		/// <param name="connectionString">The connection string to use</param>
		/// <param name="vSId"></param>
		/// <param name="voucherStatus"></param>
		/// <param name="requestDate"></param>
		public static void Execute(
				#region Parameters
				string connectionString,
				SqlInt32 vSId,
				SqlInt32 voucherStatus,
				SqlDateTime requestDate
				#endregion
		    )
		{
			UpdateVoucherStatus updateVoucherStatus = new UpdateVoucherStatus();
			
			#region Assign Property Values
			updateVoucherStatus.ConnectionString = connectionString;
			updateVoucherStatus.VSId = vSId;
			updateVoucherStatus.VoucherStatus = voucherStatus;
			updateVoucherStatus.RequestDate = requestDate;
			#endregion
			
			updateVoucherStatus.Execute();
			
			#region Get Property Values
			
			#endregion
		}
		#endregion
	}
	#endregion
}

