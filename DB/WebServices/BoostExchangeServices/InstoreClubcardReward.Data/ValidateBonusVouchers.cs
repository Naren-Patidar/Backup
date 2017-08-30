using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;

namespace InstoreClubcardReward.Data
{   
    #region usp_InsertBooking Wrapper
    public class ValidateBonusVouchers
    {
        protected string _connectionString = String.Empty;
        protected SqlConnection _connection = null;
        protected bool _ownsConnection = true;
        public string _VoucherNumber=String.Empty;
        protected SqlInt32 _IsPresent = SqlInt32.Null;

        public ValidateBonusVouchers(string connectionString)
		{
			this.ConnectionString = connectionString;
		}
		
		public ValidateBonusVouchers(SqlConnection connection)
		{
			this.Connection = connection;
		}
		
		
		/// <summary>
		/// The connection string to use when executing the usp_InsertBooking stored procedure.
		/// </summary>
		public string ConnectionString
		{
			get {return _connectionString;}
			set {_connectionString = value;}
		}

        public string VoucherNumber
        {
            get { return _VoucherNumber; }
            set
            {
                _VoucherNumber = value;
            }
        }

        public SqlInt32 IsPresent
        {
            get { return _IsPresent; }
            set
            {
                _IsPresent = value;
            }
        }
        /// <summary>
        /// The connection to use when executing the usp_InsertBooking stored procedure.
        /// If this is not null, it will be used instead of creating a new connection.
        /// </summary>
        public SqlConnection Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }

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

        public virtual void Execute()
        {
            SqlCommand cmd = new SqlCommand();

            SqlConnection cn = this.GetConnection();
            SqlDataReader reader = null;

            try
            {
                cmd.Connection = cn;
                cmd.CommandText = "[dbo].[USP_ValidateBonusVouchers]";
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter voucherNumber = cmd.Parameters.Add("@VoucherNumber", SqlDbType.VarChar);
                voucherNumber.Direction = ParameterDirection.Input;
                voucherNumber.Value = this._VoucherNumber;

                SqlParameter prmReturnValue = cmd.Parameters.Add("@ReturnValue", SqlDbType.Int);
                prmReturnValue.Direction = ParameterDirection.Output;

                if (cn.State != ConnectionState.Open) cn.Open();
                reader=cmd.ExecuteReader();

                if (prmReturnValue.Value != null && prmReturnValue.Value != DBNull.Value)
                {
                    _IsPresent = (int)prmReturnValue.Value;
                }
				

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
    }
   #endregion
}

