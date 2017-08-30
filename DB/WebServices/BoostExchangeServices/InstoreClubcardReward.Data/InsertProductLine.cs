
//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by CodeSmith.
//
//     Date:    4/6/2009
//     Time:    2:48 
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
	#region usp_InsertProductLine Wrapper
    /// <summary>
    /// This class is a wrapper for the usp_InsertProductLine stored procedure.
    /// </summary>
    public class InsertProductLine
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
		protected SqlInt32 _productLineId = SqlInt32.Null;
		protected bool _productLineIdSet = false;
		protected SqlString _productCode = SqlString.Null;
		protected bool _productCodeSet = false;
		protected SqlInt32 _productNumber = SqlInt32.Null;
		protected bool _productNumberSet = false;
		#endregion
		
		#region Constructors
		public InsertProductLine()
		{
		}
		
		public InsertProductLine(string connectionString)
		{
			this.ConnectionString = connectionString;
		}
		
		public InsertProductLine(SqlConnection connection)
		{
			this.Connection = connection;
		}
		
		public InsertProductLine(SqlConnection connection, SqlTransaction transaction)
		{
			this.Connection = connection;
			this.Transaction = transaction;
		}
		#endregion
		
		#region Public Properties
		/// <summary>
		/// The connection string to use when executing the usp_InsertProductLine stored procedure.
		/// </summary>
		public string ConnectionString
		{
			get {return _connectionString;}
			set {_connectionString = value;}
		}
        
        /// <summary>
        /// The connection to use when executing the usp_InsertProductLine stored procedure.
        /// If this is not null, it will be used instead of creating a new connection.
        /// </summary>
        public SqlConnection Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }
		
        /// <summary>
        /// The transaction to use when executing the usp_InsertProductLine stored procedure.
        /// If this is not null, the stored procedure will be executing within the transaction.
        /// </summary>
        public SqlTransaction Transaction
        {
            get { return _transaction; }
            set { _transaction = value; }
        }
		
		/// <summary>
		/// Gets the return value from the usp_InsertProductLine stored procedure.
		/// </summary>
		public int ReturnValue
		{
			get {return _returnValue;}
		}
		
		/// <summary>
		/// Gets the number of rows changed, inserted, or deleted by execution of the usp_InsertProductLine stored procedure.
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
		public SqlInt32 ProductLineId
		{
			get {return _productLineId;}
			set
			{
				_productLineId = value;
				_productLineIdSet = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public SqlString ProductCode
		{
			get {return _productCode;}
			set
			{
				_productCode = value;
				_productCodeSet = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public SqlInt32 ProductNumber
		{
			get {return _productNumber;}
			set
			{
				_productNumber = value;
				_productNumberSet = true;
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
		/// This method calls the usp_InsertProductLine stored procedure.
		/// </summary>
		public virtual void Execute()
		{
			SqlCommand cmd = new SqlCommand();
			
			SqlConnection cn = this.GetConnection();
			
			try
			{
				cmd.Connection = cn;
				cmd.Transaction = this.Transaction;
				cmd.CommandText = "[dbo].[usp_InsertProductLine]";
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
				
				SqlParameter prmProductLineId = cmd.Parameters.Add("@ProductLineId", SqlDbType.Int);
				prmProductLineId.Direction = ParameterDirection.Input;
				if (_productLineIdSet == true || this.ProductLineId.IsNull == false)
				{
					prmProductLineId.Value = this.ProductLineId;
				}
				
				SqlParameter prmProductCode = cmd.Parameters.Add("@ProductCode", SqlDbType.VarChar);
				prmProductCode.Direction = ParameterDirection.Input;
					prmProductCode.Size = 15;
				if (_productCodeSet == true || this.ProductCode.IsNull == false)
				{
					prmProductCode.Value = this.ProductCode;
				}
				
				SqlParameter prmProductNumber = cmd.Parameters.Add("@ProductNumber", SqlDbType.Int);
				prmProductNumber.Direction = ParameterDirection.Input;
				if (_productNumberSet == true || this.ProductNumber.IsNull == false)
				{
					prmProductNumber.Value = this.ProductNumber;
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
		/// This method calls the usp_InsertProductLine stored procedure.
		/// </summary>
		/// <param name="connectionString">The connection string to use</param>
		/// <param name="bookingId"></param>
		/// <param name="productLineId"></param>
		/// <param name="productCode"></param>
		/// <param name="productNumber"></param>
		public static void Execute(
				#region Parameters
				string connectionString,
				SqlInt32 bookingId,
				SqlInt32 productLineId,
				SqlString productCode,
				SqlInt32 productNumber
				#endregion
		    )
		{
			InsertProductLine insertProductLine = new InsertProductLine();
			
			#region Assign Property Values
			insertProductLine.ConnectionString = connectionString;
			insertProductLine.BookingId = bookingId;
			insertProductLine.ProductLineId = productLineId;
			insertProductLine.ProductCode = productCode;
			insertProductLine.ProductNumber = productNumber;
			#endregion
			
			insertProductLine.Execute();
			
			#region Get Property Values
			
			#endregion
		}
		#endregion
	}
	#endregion
}

