using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlTypes;
using System.Data;
using System.Data.SqlClient;

namespace InstoreClubcardReward.Data
{
    public class InsertPrintTransaction
    {
        #region Member Variables
        protected string _connectionString = String.Empty;
        protected SqlConnection _connection = null;
        protected SqlTransaction _transaction = null;
        protected bool _ownsConnection = true;
        protected int _recordsAffected = -1;
        protected int _returnValue = 0;

        protected SqlInt32 _transactionID = SqlInt32.Null;
        protected bool _transactionIDset = false;
        protected SqlString _clubcard = SqlString.Null;
        protected bool _clubcardSet = false;
        protected SqlInt32 _kioskId = SqlInt32.Null;
        protected bool _kioskIdSet = false;
        protected SqlInt32 _statusId = SqlInt16.Null;
        protected bool _statusIdSet = false;
        protected SqlDateTime _TranStartTime = SqlDateTime.Null;
        protected bool _TranStartTimeset = false;

        //Added to record Coupon Status ID as part of International Kiosk Development
        protected SqlInt32 _CouponStatusID = SqlInt16.Null;
        protected bool _CouponStatusIDSet = false;
        #endregion

        #region Constructors
        public InsertPrintTransaction()
        {
        }

        public InsertPrintTransaction(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public InsertPrintTransaction(SqlConnection connection)
        {
            this.Connection = connection;
        }

        public InsertPrintTransaction(SqlConnection connection, SqlTransaction transaction)
        {
            this.Connection = connection;
            this.Transaction = transaction;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// The connection string to use when executing the usp_InsertTransaction stored procedure.
        /// </summary>
        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        /// <summary>
        /// The connection to use when executing the usp_InsertTransaction stored procedure.
        /// If this is not null, it will be used instead of creating a new connection.
        /// </summary>
        public SqlConnection Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }

        /// <summary>
        /// The transaction to use when executing the usp_InsertTransaction stored procedure.
        /// If this is not null, the stored procedure will be executing within the transaction.
        /// </summary>
        public SqlTransaction Transaction
        {
            get { return _transaction; }
            set { _transaction = value; }
        }

        /// <summary>
        /// Gets the return value from the usp_InsertTransaction stored procedure.
        /// </summary>
        public int ReturnValue
        {
            get { return _returnValue; }
        }

        /// <summary>
        /// Gets the number of rows changed, inserted, or deleted by execution of the usp_InsertTransaction stored procedure.
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
                _transactionIDset = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public SqlString Clubcard
        {
            get { return _clubcard; }
            set
            {
                _clubcard = value;
                _clubcardSet = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public SqlInt32 KioskId
        {
            get { return _kioskId; }
            set
            {
                _kioskId = value;
                _kioskIdSet = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public SqlInt32 StatusId
        {
            get { return _statusId; }
            set
            {
                _statusId = value;
                _statusIdSet = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public SqlDateTime TranStartTime
        {
            get { return _TranStartTime; }
            set
            {
                _TranStartTime = value;
                _TranStartTimeset = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        //Added to record Coupon Status ID as part of International Kiosk Development
        public SqlInt32 CouponStatusID
        {
            get { return _CouponStatusID; }
            set { _CouponStatusID = value; _CouponStatusIDSet = true; }
            
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

            System.Diagnostics.Debug.Assert(this.ConnectionString.Length != 0, "You must first set the ConnectioString property before calling an Execute method.");
            return new SqlConnection(this.ConnectionString);
        }
        #endregion

        #region Execute Methods
        /// <summary>
        /// This method calls the usp_InsertTransaction stored procedure.
        /// </summary>
        public virtual void Execute(int TransactionID)
        {
            SqlCommand cmd = new SqlCommand();

            SqlConnection cn = this.GetConnection();

            try
            {
                cmd.Connection = cn;
                cmd.Transaction = this.Transaction;
                cmd.CommandText = "[dbo].[usp_InsertTransaction]";
                cmd.CommandType = CommandType.StoredProcedure;

                #region Populate Parameters
                SqlParameter prmReturnValue = cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                prmReturnValue.Direction = ParameterDirection.ReturnValue;

                SqlParameter prmClubcard = cmd.Parameters.Add("@ClubcardNo", SqlDbType.VarChar);
                prmClubcard.Direction = ParameterDirection.Input;
                prmClubcard.Size = 18;
                if (_clubcardSet == true || this.Clubcard.IsNull == false)
                {
                    prmClubcard.Value = this.Clubcard;
                }

                SqlParameter prmStoreId = cmd.Parameters.Add("@KioskId", SqlDbType.Int);
                prmStoreId.Direction = ParameterDirection.Input;
                if (_kioskIdSet == true || this.KioskId.IsNull == false)
                {
                    prmStoreId.Value = this.KioskId;
                }

                SqlParameter prmStatus = cmd.Parameters.Add("@StatusID", SqlDbType.Int);
                prmStatus.Direction = ParameterDirection.Input;
                if (_statusIdSet == true || this.StatusId.IsNull == false)
                {
                    prmStatus.Value = this.StatusId;
                }

                SqlParameter prmTranStartTime = cmd.Parameters.Add("@TranStartTime", SqlDbType.DateTime);
                prmTranStartTime.Direction = ParameterDirection.Input;
                if (_TranStartTimeset == true || this.TranStartTime.IsNull == false)
                {
                    prmTranStartTime.Value = this.TranStartTime;
                }

                //Added to record Coupon Status ID as part of International Kiosk Development
                SqlParameter prmCouponStatusID = cmd.Parameters.Add("@CouponStatusID", SqlDbType.Int);
                prmCouponStatusID.Direction = ParameterDirection.Input;
                if (_CouponStatusIDSet == true || this.CouponStatusID.IsNull == false)
                {
                    prmCouponStatusID.Value = this.CouponStatusID;
                }

                SqlParameter prmTransactionID = cmd.Parameters.Add("@TransactionID", SqlDbType.Int);
                this.TransactionID = TransactionID;
                if (_transactionIDset == true)
                {
                    prmTransactionID.Direction = ParameterDirection.InputOutput;
                }
                else
                {
                    prmTransactionID.Direction = ParameterDirection.Output;
                }
                if (_transactionIDset == true || this.TransactionID.IsNull == false)
                {
                    prmTransactionID.Value = this.TransactionID;
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

                if (prmTransactionID != null && prmTransactionID.Value != null)
                {
                    if (prmTransactionID.Value is SqlInt32)
                    {
                        this.TransactionID = (SqlInt32)prmTransactionID.Value;
                    }
                    else
                    {
                        if (prmTransactionID.Value != DBNull.Value)
                        {
                            this.TransactionID = new SqlInt32((int)prmTransactionID.Value);
                        }
                        else
                        {
                            this.TransactionID = SqlInt32.Null;
                        }
                    }
                }
                else
                {
                    this.TransactionID = SqlInt32.Null;
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
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="clubcard"></param>
        /// <param name="statusId"></param>
        /// <param name="kioskId"></param>
        /// <param name="tranStartTime"></param>
        /// <param name="TransactionId"></param>
        public static void Execute(
        #region Parameters
string connectionString,
                SqlString clubcard,
                SqlInt32 statusId,
                SqlInt32 kioskId,
                SqlDateTime tranStartTime,
                SqlInt32 CouponStatusID,
                ref SqlInt32 TransactionId
        #endregion
)
        {
            InsertPrintTransaction insertPrintTransaction = new InsertPrintTransaction();

            #region Assign Property Values
            insertPrintTransaction.ConnectionString = connectionString;
            insertPrintTransaction.Clubcard = clubcard;
            insertPrintTransaction.StatusId = statusId;
            insertPrintTransaction.TranStartTime = tranStartTime;
            insertPrintTransaction.KioskId = kioskId;
            insertPrintTransaction.CouponStatusID = CouponStatusID;
            #endregion
            int transactionId = (int)TransactionId;
            insertPrintTransaction.Execute(transactionId);

            #region Get Property Values
            TransactionId = insertPrintTransaction.TransactionID;
            #endregion
        }
        #endregion




    }
}
