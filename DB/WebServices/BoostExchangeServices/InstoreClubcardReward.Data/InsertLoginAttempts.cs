using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlTypes;
using System.Data;
using System.Data.SqlClient;


namespace InstoreClubcardReward.Data
{
    #region InsertLoginAttempts Wrapper
    /// <summary>
    /// This class is a wrapper for the usp_InsertBookingError stored procedure.
    /// </summary>
    public class InsertLoginAttempts
    {
        #region Member Variables
        protected string _connectionString = String.Empty;
        protected SqlConnection _connection = null;
        protected SqlTransaction _transaction = null;
        protected bool _ownsConnection = true;
        protected int _recordsAffected = -1;
        protected int _returnValue = 0;
        protected SqlInt32 _transactionID = SqlInt32.Null;
        protected bool _transactionIDSet = false;
        protected bool _EANSet = false;
        protected SqlString _PostCode = SqlString.Null;
        protected bool _PostCodeSet = false;
        protected SqlString _EAN;
        protected bool _AddressLine1Set;
        protected SqlString _AddressLine1;
        protected bool _statusSet;
        protected SqlString _status;
        #endregion

        #region Constructors
        public InsertLoginAttempts()
        {
        }

        public InsertLoginAttempts(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public InsertLoginAttempts(SqlConnection connection)
        {
            this.Connection = connection;
        }

        public InsertLoginAttempts(SqlConnection connection, SqlTransaction transaction)
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
            get { return _connectionString; }
            set { _connectionString = value; }
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
            get { return _returnValue; }
        }

        /// <summary>
        /// Gets the number of rows changed, inserted, or deleted by execution of the usp_InsertBookingError stored procedure.
        /// </summary>
        public int RecordsAffected
        {
            get { return _recordsAffected; }
        }

        /// <summary>
        /// 
        /// </summary>
        public SqlInt32 TransactionID
        {
            get { return _transactionID; }
            set
            {
                _transactionID = value;
                _transactionIDSet = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public SqlString EAN
        {
            get { return _EAN; }
            set
            {
                _EAN = value;
                _EANSet = true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public SqlString Status
        {
            get { return _status; }
            set
            {
                _status = value;
                _statusSet = true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public SqlString PostCode
        {
            get { return _PostCode; }
            set
            {
                _PostCode = value;
                _PostCodeSet = true;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public SqlString AddressLine1
        {
            get { return _AddressLine1; }
            set
            {
                _AddressLine1 = value;
                _AddressLine1Set = true;
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
                //  cmd.Transaction = this.Transaction;
                cmd.CommandText = "[dbo].[usp_InsertLoginAttempts]";
                cmd.CommandType = CommandType.StoredProcedure;

                #region Populate Parameters

                SqlParameter prmTransactionID = cmd.Parameters.Add("@TransactionID", SqlDbType.Int);
                prmTransactionID.Direction = ParameterDirection.Input;
                if (_transactionIDSet == true || this.TransactionID.IsNull == false)
                {
                    prmTransactionID.Value = this.TransactionID;
                }

                SqlParameter prmEAN = cmd.Parameters.Add("@EAN", SqlDbType.VarChar);
                prmEAN.Direction = ParameterDirection.Input;
                prmEAN.Size = 18;
                if (_EANSet == true || this.EAN.IsNull == false)
                {
                    prmEAN.Value = this.EAN;
                }

                SqlParameter prmPostCode = cmd.Parameters.Add("@PostCode", SqlDbType.VarChar);
                prmPostCode.Direction = ParameterDirection.Input;
                prmPostCode.Size = 20;
                if (_PostCodeSet == true || this.PostCode.IsNull == false)
                {
                    prmPostCode.Value = this.PostCode;
                }
                SqlParameter prmAddressLine1 = cmd.Parameters.Add("@AddressLine1", SqlDbType.VarChar);
                prmAddressLine1.Direction = ParameterDirection.Input;
                prmAddressLine1.Size = 100;
                if (_AddressLine1Set == true || this.AddressLine1.IsNull == false)
                {
                    prmAddressLine1.Value = this.AddressLine1;
                }

                SqlParameter prmStatus = cmd.Parameters.Add("@Status", SqlDbType.VarChar);
                prmStatus.Direction = ParameterDirection.Input;
                if (_statusSet == true || this.Status.IsNull == false)
                {
                    prmStatus.Value = this.Status;
                }
                #endregion

                #region Execute Command
                if (cn.State != ConnectionState.Open) cn.Open();
                _recordsAffected = cmd.ExecuteNonQuery();
                #endregion


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
                SqlInt32 transactionID,
                SqlString EAN,
                SqlString PostCode,
            SqlString AddressLine1,
            SqlString Status
        #endregion
)
        {
            InsertLoginAttempts insertTransError = new InsertLoginAttempts();

            #region Assign Property Values
            insertTransError.ConnectionString = connectionString;
            insertTransError.TransactionID = transactionID;
            insertTransError.EAN = EAN;
            insertTransError.PostCode = PostCode;
            insertTransError.Status = Status;

            #endregion

            insertTransError.Execute();

            #region Get Property Values

            #endregion
        }
    #endregion
    }

}
