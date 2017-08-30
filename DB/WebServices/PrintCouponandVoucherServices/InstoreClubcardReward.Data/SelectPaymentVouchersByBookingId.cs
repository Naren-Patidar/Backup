﻿
//------------------------------------------------------------------------------
// <autogenerated>
//     This code was generated by CodeSmith.
//
//     Version: 5.0.0.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </autogenerated>
//------------------------------------------------------------------------------

using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.ObjectModel;

namespace InstoreClubcardReward.Data
{
    #region SelectPaymentVouchersByBookingIdRow
    /// <summary>
    /// Stores result row level information from the sup_SelectPaymentVouchersByBookingId stored procedure.
    /// </summary>
    [Serializable]
    public class SelectPaymentVouchersByBookingIdRow
    {
        #region Member Variables
        protected int? _voucherId;
        protected int? _bookingId;
        protected string _ean;
        protected string _alpha;
        protected int? _voucherType;
        protected int? _amount;
        protected int? _status;
        #endregion

        #region Constructors
        public SelectPaymentVouchersByBookingIdRow()
        {
        }

        public SelectPaymentVouchersByBookingIdRow(SqlDataReader reader)
        {
            this.LoadFromReader(reader);
        }
        #endregion

        #region Helper Methods
        protected void LoadFromReader(SqlDataReader reader)
        {
            if (reader != null && !reader.IsClosed)
            {
                if (!reader.IsDBNull(0)) _voucherId = reader.GetInt32(0);
                if (!reader.IsDBNull(1)) _bookingId = reader.GetInt32(1);
                if (!reader.IsDBNull(2)) _ean = reader.GetString(2);
                if (!reader.IsDBNull(3)) _alpha = reader.GetString(3);
                if (!reader.IsDBNull(4)) _voucherType = reader.GetInt32(4);
                if (!reader.IsDBNull(5)) _amount = reader.GetInt32(5);
                if (!reader.IsDBNull(6)) _status = reader.GetInt32(6);
            }
        }
        #endregion

        #region Public Properties
        public int? VoucherId
        {
            get { return _voucherId; }
            set { _voucherId = value; }
        }

        public int? BookingId
        {
            get { return _bookingId; }
            set { _bookingId = value; }
        }

        public string EAN
        {
            get { return _ean; }
            set { _ean = value; }
        }

        public string Alpha
        {
            get { return _alpha; }
            set { _alpha = value; }
        }

        public int? VoucherType
        {
            get { return _voucherType; }
            set { _voucherType = value; }
        }

        public int? Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        public int? Status
        {
            get { return _status; }
            set { _status = value; }
        }
        #endregion
    }
    #endregion
    #region sup_SelectPaymentVouchersByBookingId Wrapper
    /// <summary>
    /// This class is a wrapper for the sup_SelectPaymentVouchersByBookingId stored procedure.
    /// </summary>
    public class SelectPaymentVouchersByBookingId
    {
        #region Member Variables

        protected string _connectionString = String.Empty;
        protected SqlConnection _connection;
        protected SqlTransaction _transaction;
        protected bool _ownsConnection = true;
        protected int _recordsAffected = -1;
        protected System.Int32 _returnValue;
        protected int? _bookingId;

        #endregion

        #region Constructors

        public SelectPaymentVouchersByBookingId()
        {
        }

        public SelectPaymentVouchersByBookingId(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public SelectPaymentVouchersByBookingId(SqlConnection connection)
        {
            this.Connection = connection;
        }

        public SelectPaymentVouchersByBookingId(SqlConnection connection, SqlTransaction transaction)
        {
            this.Connection = connection;
            this.Transaction = transaction;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The connection string to use when executing the sup_SelectPaymentVouchersByBookingId stored procedure.
        /// </summary>
        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        /// <summary>
        /// The connection to use when executing the sup_SelectPaymentVouchersByBookingId stored procedure.
        /// If this is not null, it will be used instead of creating a new connection.
        /// </summary>
        public SqlConnection Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }

        /// <summary>
        /// The transaction to use when executing the sup_SelectPaymentVouchersByBookingId stored procedure.
        /// If this is not null, the stored procedure will be executing within the transaction.
        /// </summary>
        public SqlTransaction Transaction
        {
            get { return _transaction; }
            set { _transaction = value; }
        }

        /// <summary>
        /// Gets the return value from the sup_SelectPaymentVouchersByBookingId stored procedure.
        /// </summary>
        public System.Int32 ReturnValue
        {
            get { return _returnValue; }
        }

        /// <summary>
        /// Gets the number of rows changed, inserted, or deleted by execution of the sup_SelectPaymentVouchersByBookingId stored procedure.
        /// </summary>
        public int RecordsAffected
        {
            get { return _recordsAffected; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int? BookingId
        {
            get { return _bookingId; }
            set
            {
                _bookingId = value;
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

            System.Diagnostics.Debug.Assert(this.ConnectionString.Length != 0, "You must first set the ConnectioString property before calling an Execute method.");
            return new SqlConnection(this.ConnectionString);
        }

        #endregion

        #region Execute Methods

        /// <summary>
        /// This method calls the sup_SelectPaymentVouchersByBookingId stored procedure and returns a SqlDataReader with the results.
        /// </summary>
        /// <returns>SqlDataReader</returns>
        public virtual SqlDataReader ExecuteReader()
        {
            SqlDataReader reader = null;
            SqlCommand cmd = new SqlCommand();
            SqlConnection cn = this.GetConnection();

            try
            {
                cmd.Connection = cn;
                cmd.Transaction = this.Transaction;
                cmd.CommandText = "[dbo].[sup_SelectPaymentVouchersByBookingId]";
                cmd.CommandType = CommandType.StoredProcedure;

                #region Populate Parameters

                SqlParameter prmReturnValue = cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                prmReturnValue.Direction = ParameterDirection.ReturnValue;

                SqlParameter prmBookingId = cmd.Parameters.Add("@BookingId", SqlDbType.Int);
                prmBookingId.Direction = ParameterDirection.Input;
                if (BookingId.HasValue)
                    prmBookingId.Value = BookingId.Value;
                else
                    prmBookingId.Value = DBNull.Value;

                #endregion

                #region Execute Command
                if (cn.State != ConnectionState.Open) cn.Open();
                if (_ownsConnection)
                {
                    reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
                else
                {
                    reader = cmd.ExecuteReader();
                }
                #endregion

                #region Get Output Parameters
                if (prmReturnValue.Value != null && prmReturnValue.Value != DBNull.Value)
                {
                    _returnValue = (System.Int32)prmReturnValue.Value;
                }

                #endregion
            }
            finally
            {
                cmd.Dispose();
            }

            return reader;
        }

        /// <summary>
        /// This method calls the sup_SelectPaymentVouchersByBookingId stored procedure and returns a DataSet with the results.
        /// </summary>
        /// <returns>DataSet</returns>
        public virtual DataSet ExecuteDataSet()
        {
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand();

            SqlConnection cn = this.GetConnection();

            try
            {
                cmd.Connection = cn;
                cmd.Transaction = this.Transaction;
                cmd.CommandText = "[dbo].[sup_SelectPaymentVouchersByBookingId]";
                cmd.CommandType = CommandType.StoredProcedure;

                #region Populate Parameters
                SqlParameter prmReturnValue = cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                prmReturnValue.Direction = ParameterDirection.ReturnValue;

                SqlParameter prmBookingId = cmd.Parameters.Add("@BookingId", SqlDbType.Int);
                prmBookingId.Direction = ParameterDirection.Input;
                if (BookingId.HasValue)
                    prmBookingId.Value = BookingId.Value;
                else
                    prmBookingId.Value = DBNull.Value;

                #endregion

                #region Execute Command
                if (cn.State != ConnectionState.Open) cn.Open();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                _recordsAffected = ds.Tables[0].Rows.Count;
                #endregion

                #region Get Output Parameters
                if (prmReturnValue.Value != null && prmReturnValue.Value != DBNull.Value)
                {
                    _returnValue = (System.Int32)prmReturnValue.Value;
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

            return ds;
        }

        /// <summary>
        /// This method calls the sup_SelectPaymentVouchersByBookingId stored procedure and outputs the results to a custom strongly typed collection.
        /// </summary>
        /// <returns>Collection<SelectPaymentVouchersByBookingIdRow></returns>
        public virtual Collection<SelectPaymentVouchersByBookingIdRow> Execute()
        {
            Collection<SelectPaymentVouchersByBookingIdRow> selectPaymentVouchersByBookingIdRowList = new Collection<SelectPaymentVouchersByBookingIdRow>();
            SqlCommand cmd = new SqlCommand();

            SqlConnection cn = this.GetConnection();

            try
            {
                cmd.Connection = cn;
                cmd.Transaction = this.Transaction;
                cmd.CommandText = "[dbo].[sup_SelectPaymentVouchersByBookingId]";
                cmd.CommandType = CommandType.StoredProcedure;

                #region Populate Parameters
                SqlParameter prmReturnValue = cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                prmReturnValue.Direction = ParameterDirection.ReturnValue;

                SqlParameter prmBookingId = cmd.Parameters.Add("@BookingId", SqlDbType.Int);
                prmBookingId.Direction = ParameterDirection.Input;
                if (BookingId.HasValue)
                    prmBookingId.Value = BookingId.Value;
                else
                    prmBookingId.Value = DBNull.Value;

                #endregion

                #region Execute Command
                if (cn.State != ConnectionState.Open) cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        SelectPaymentVouchersByBookingIdRow selectPaymentVouchersByBookingIdRow = new SelectPaymentVouchersByBookingIdRow(reader);
                        selectPaymentVouchersByBookingIdRowList.Add(selectPaymentVouchersByBookingIdRow);
                    }
                }
                finally
                {
                    if (!reader.IsClosed)
                    {
                        reader.Close();
                        _recordsAffected = reader.RecordsAffected;
                    }
                }
                #endregion

                #region Get Output Parameters
                if (prmReturnValue.Value != null && prmReturnValue.Value != DBNull.Value)
                {
                    _returnValue = (System.Int32)prmReturnValue.Value;
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

            return selectPaymentVouchersByBookingIdRowList;
        }

        /// <summary>
        /// This method calls the sup_SelectPaymentVouchersByBookingId stored procedure and returns a SqlDataReader with the results.
        /// </summary>
        /// <param name="connectionString">The connection string to use</param>
        /// <param name="bookingId"></param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(
        #region Parameters
string connectionString,
                int? bookingId
        #endregion
)
        {
            SelectPaymentVouchersByBookingId selectPaymentVouchersByBookingId = new SelectPaymentVouchersByBookingId();

            #region Assign Property Values
            selectPaymentVouchersByBookingId.ConnectionString = connectionString;
            selectPaymentVouchersByBookingId.BookingId = bookingId;
            #endregion

            SqlDataReader reader = selectPaymentVouchersByBookingId.ExecuteReader();

            #region Get Property Values

            #endregion

            return reader;
        }

        /// <summary>
        /// This method calls the sup_SelectPaymentVouchersByBookingId stored procedure and returns a DataSet with the results.
        /// </summary>
        /// <param name="connectionString">The connection string to use</param>
        /// <param name="bookingId"></param>
        /// <returns>DataSet</returns>
        public static DataSet ExecuteDataSet(
        #region Parameters
string connectionString,
                int? bookingId
        #endregion
)
        {
            SelectPaymentVouchersByBookingId selectPaymentVouchersByBookingId = new SelectPaymentVouchersByBookingId();

            #region Assign Property Values
            selectPaymentVouchersByBookingId.ConnectionString = connectionString;
            selectPaymentVouchersByBookingId.BookingId = bookingId;
            #endregion

            DataSet ds = selectPaymentVouchersByBookingId.ExecuteDataSet();

            #region Get Property Values

            #endregion

            return ds;
        }

        /// <summary>
        /// This method calls the sup_SelectPaymentVouchersByBookingId stored procedure and outputs the results to a custom strongly typed collection.
        /// </summary>
        /// <param name="connectionString">The connection string to use</param>
        /// <param name="bookingId"></param>
        /// <returns>Collection<SelectPaymentVouchersByBookingIdRow></returns>
        public static Collection<SelectPaymentVouchersByBookingIdRow> Execute(
        #region Parameters
string connectionString,
                int? bookingId
        #endregion
)
        {
            SelectPaymentVouchersByBookingId selectPaymentVouchersByBookingId = new SelectPaymentVouchersByBookingId();

            #region Assign Property Values
            selectPaymentVouchersByBookingId.ConnectionString = connectionString;
            selectPaymentVouchersByBookingId.BookingId = bookingId;
            #endregion

            Collection<SelectPaymentVouchersByBookingIdRow> selectPaymentVouchersByBookingIdRowList = selectPaymentVouchersByBookingId.Execute();

            #region Get Property Values

            #endregion

            return selectPaymentVouchersByBookingIdRowList;
        }
        #endregion
    }
    #endregion
}

