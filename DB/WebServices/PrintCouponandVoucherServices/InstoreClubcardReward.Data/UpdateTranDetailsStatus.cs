using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;

namespace InstoreClubcardReward.Data
{
    #region usp_UpdateTranDetailsStatus Wrapper
    /// <summary>
    /// This class is a wrapper for the usp_UpdatePaymentVoucher stored procedure.
    /// </summary>
    public class UpdateTranDetailsStatus
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
        protected SqlInt32 _status = SqlInt32.Null;
        protected bool _statusSet = false;
        protected SqlInt32 _CouponStatus = SqlInt32.Null;
        protected bool _CouponStatusSet = false;
        #endregion

        #region Constructors
        public UpdateTranDetailsStatus()
        {
        }

        public UpdateTranDetailsStatus(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public UpdateTranDetailsStatus(SqlConnection connection)
        {
            this.Connection = connection;
        }

        public UpdateTranDetailsStatus(SqlConnection connection, SqlTransaction transaction)
        {
            this.Connection = connection;
            this.Transaction = transaction;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// The connection string to use when executing the usp_UpdatePaymentVoucher stored procedure.
        /// </summary>
        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        /// <summary>
        /// The connection to use when executing the usp_UpdatePaymentVoucher stored procedure.
        /// If this is not null, it will be used instead of creating a new connection.
        /// </summary>
        public SqlConnection Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }

        /// <summary>
        /// The transaction to use when executing the usp_UpdatePaymentVoucher stored procedure.
        /// If this is not null, the stored procedure will be executing within the transaction.
        /// </summary>
        public SqlTransaction Transaction
        {
            get { return _transaction; }
            set { _transaction = value; }
        }

        /// <summary>
        /// Gets the return value from the usp_UpdatePaymentVoucher stored procedure.
        /// </summary>
        public int ReturnValue
        {
            get { return _returnValue; }
        }

        /// <summary>
        /// Gets the number of rows changed, inserted, or deleted by execution of the usp_UpdatePaymentVoucher stored procedure.
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
        public SqlInt32 Status
        {
            get { return _status; }
            set
            {
                _status = value;
                _statusSet = true;
            }
        }

        public SqlInt32 CouponStatus
        {
            get { return _CouponStatus; }
            set { _CouponStatus = value; _CouponStatusSet = true; }
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
        /// This method calls the usp_UpdatePaymentVoucher stored procedure.
        /// </summary>
        public virtual void Execute()
        {
            SqlCommand cmd = new SqlCommand();

            SqlConnection cn = this.GetConnection();

            try
            {
                cmd.Connection = cn;
                cmd.Transaction = this.Transaction;
                cmd.CommandText = "[dbo].[usp_UpdateTranDetailsStatus]";
                cmd.CommandType = CommandType.StoredProcedure;

                #region Populate Parameters
                //SqlParameter prmReturnValue = cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                //prmReturnValue.Direction = ParameterDirection.ReturnValue;

                SqlParameter prmTransactionID = cmd.Parameters.Add("@TransactionID", SqlDbType.Int);
                prmTransactionID.Direction = ParameterDirection.Input;
                if (_transactionIDSet == true || this.TransactionID.IsNull == false)
                {
                    prmTransactionID.Value = this.TransactionID;
                }

                SqlParameter prmStatus = cmd.Parameters.Add("@StatusID", SqlDbType.Int);
                prmStatus.Direction = ParameterDirection.Input;
                if (_statusSet == true || this.Status.IsNull == false)
                {
                    prmStatus.Value = this.Status;
                }

                SqlParameter prmCouponStatus = cmd.Parameters.Add("@CouponStatusID", SqlDbType.Int);
                prmCouponStatus.Direction = ParameterDirection.Input;
                if (_CouponStatusSet == true || this.CouponStatus.IsNull == false)
                {
                    prmCouponStatus.Value = this.CouponStatus;
                }
                #endregion

                #region Execute Command
                if (cn.State != ConnectionState.Open) cn.Open();
                _recordsAffected = cmd.ExecuteNonQuery();
                #endregion

                //#region Get Output Parameters
                //if (prmReturnValue.Value != null && prmReturnValue.Value != DBNull.Value)
                //{
                //    _returnValue = (int)prmReturnValue.Value;
                //}

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
        /// This method calls the usp_UpdateTranDetailsStatus stored procedure.
        /// </summary>
        /// <param name="connectionString">The connection string to use</param>
        /// <param name="voucherId"></param>
        /// <param name="status"></param>
        public static void Execute(
        #region Parameters
string connectionString,
                SqlInt32 transactionID,
                SqlInt32 status,
                SqlInt32 CouponStatus
        #endregion
)
        {
            UpdateTranDetailsStatus updateTranDetailsStatus = new UpdateTranDetailsStatus();

            #region Assign Property Values
            updateTranDetailsStatus.ConnectionString = connectionString;
            updateTranDetailsStatus.TransactionID = transactionID;
            updateTranDetailsStatus.Status = status;
            updateTranDetailsStatus.CouponStatus = CouponStatus;
            #endregion

            updateTranDetailsStatus.Execute();

            #region Get Property Values

            #endregion
        }
    #endregion

    }


}
