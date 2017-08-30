using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace InstoreClubcardReward.Data
{
    public class InsertCouponDetails
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
        protected SqlString _eAN = SqlString.Null;
        protected bool _eANSet = false;
        protected SqlString _couponDesc = SqlString.Null;
        protected bool _couponDescSet = false;
        #endregion

        #region Constructors
        public InsertCouponDetails()
        {
        }

        public InsertCouponDetails(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public InsertCouponDetails(SqlConnection connection)
        {
            this.Connection = connection;
        }

        public InsertCouponDetails(SqlConnection connection, SqlTransaction transaction)
        {
            this.Connection = connection;
            this.Transaction = transaction;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// The connection string to use when executing the usp_InsertPaymentVoucher stored procedure.
        /// </summary>
        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        /// <summary>
        /// The connection to use when executing the usp_InsertPaymentVoucher stored procedure.
        /// If this is not null, it will be used instead of creating a new connection.
        /// </summary>
        public SqlConnection Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }

        /// <summary>
        /// The transaction to use when executing the usp_InsertPaymentVoucher stored procedure.
        /// If this is not null, the stored procedure will be executing within the transaction.
        /// </summary>
        public SqlTransaction Transaction
        {
            get { return _transaction; }
            set { _transaction = value; }
        }

        /// <summary>
        /// Gets the return value from the usp_InsertPaymentVoucher stored procedure.
        /// </summary>
        public int ReturnValue
        {
            get { return _returnValue; }
        }

        /// <summary>
        /// Gets the number of rows changed, inserted, or deleted by execution of the usp_InsertPaymentVoucher stored procedure.
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
            get { return _eAN; }
            set
            {
                _eAN = value;
                _eANSet = true;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        public SqlString CouponDescription
        {
            get { return _couponDesc; }
            set
            {
                _couponDesc = value;
                _couponDescSet = true;
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
        /// This method calls the usp_InsertPaymentVoucher stored procedure.
        /// </summary>
        public virtual void Execute()
        {
            SqlCommand cmd = new SqlCommand();

            SqlConnection cn = this.GetConnection();

            try
            {
                cmd.Connection = cn;
                cmd.Transaction = this.Transaction;
                cmd.CommandText = "[dbo].[usp_InsertCouponDetails]";
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

                SqlParameter prmEAN = cmd.Parameters.Add("@EAN", SqlDbType.Char);
                prmEAN.Direction = ParameterDirection.Input;
                prmEAN.Size = 22;
                if (_eANSet == true || this.EAN.IsNull == false)
                {
                    prmEAN.Value = this.EAN;
                }

                SqlParameter prmCouponDescription = cmd.Parameters.Add("@CouponDescription", SqlDbType.VarChar);
                prmCouponDescription.Direction = ParameterDirection.Input;
                if (_couponDescSet == true || this.CouponDescription.IsNull == false)
                {
                    prmCouponDescription.Value = this.CouponDescription;
                }

                #endregion

                #region Execute Command
                if (cn.State != ConnectionState.Open) cn.Open();
                _recordsAffected = cmd.ExecuteNonQuery();
                #endregion

                #region Get Output Parameters
                
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
        /// This method calls the usp_InsertPaymentVoucher stored procedure.
        /// </summary>
        /// <param name="connectionString">The connection string to use</param>
        /// <param name="bookingId"></param>
        /// <param name="eAN"></param>
        /// <param name="alpha"></param>
        /// <param name="voucherType"></param>
        /// <param name="amount"></param>
        /// <param name="status"></param>
        /// <param name="voucherId"></param>
        public static void Execute(
        #region Parameters
string connectionString,
                SqlInt32 transactionID,
                SqlString eAN,
                SqlString CouponDescription
        #endregion
)
        {
            InsertCouponDetails insertCouponDetails = new InsertCouponDetails();

            #region Assign Property Values
            insertCouponDetails.ConnectionString = connectionString;
            insertCouponDetails.TransactionID = transactionID;
            insertCouponDetails.EAN = eAN;
            insertCouponDetails.CouponDescription = CouponDescription;
            #endregion

            insertCouponDetails.Execute();

            //#region Get Property Values
            //voucherId = insertVoucherDetails.VoucherId;
            //#endregion
        }
        #endregion

    }       
    
}
