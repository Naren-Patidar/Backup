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
    #region SelectSupplierTokenCodesByBookingIdRow
    /// <summary>
    /// Stores result row level information from the sup_SelectSupplierTokenCodesByBookingId stored procedure.
    /// </summary>
    [Serializable]
    public class SelectSupplierTokenCodesByBookingIdRow
    {
        #region Member Variables
        protected int? _id;
        protected string _supplierCode;
        protected string _supplierTokenId;
        protected string _supplierTokenCode;
        protected DateTime? _supplyDate;
        protected DateTime? _customerDate;
        protected int? _tokenId;
        protected DateTime? _endDate;
        protected string _status;
        protected int? _bookingId;
        protected int? _productLineId;
        #endregion

        #region Constructors
        public SelectSupplierTokenCodesByBookingIdRow()
        {
        }

        public SelectSupplierTokenCodesByBookingIdRow(SqlDataReader reader)
        {
            this.LoadFromReader(reader);
        }
        #endregion

        #region Helper Methods
        protected void LoadFromReader(SqlDataReader reader)
        {
            if (reader != null && !reader.IsClosed)
            {
                if (!reader.IsDBNull(0)) _id = reader.GetInt32(0);
                if (!reader.IsDBNull(1)) _supplierCode = reader.GetString(1);
                if (!reader.IsDBNull(2)) _supplierTokenId = reader.GetString(2);
                if (!reader.IsDBNull(3)) _supplierTokenCode = reader.GetString(3);
                if (!reader.IsDBNull(4)) _supplyDate = reader.GetDateTime(4);
                if (!reader.IsDBNull(5)) _customerDate = reader.GetDateTime(5);
                if (!reader.IsDBNull(6)) _tokenId = reader.GetInt32(6);
                if (!reader.IsDBNull(7)) _endDate = reader.GetDateTime(7);
                if (!reader.IsDBNull(8)) _status = reader.GetString(8);
                if (!reader.IsDBNull(9)) _bookingId = reader.GetInt32(9);
                if (!reader.IsDBNull(10)) _productLineId = reader.GetInt32(10);
            }
        }
        #endregion

        #region Public Properties
        public int? Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string SupplierCode
        {
            get { return _supplierCode; }
            set { _supplierCode = value; }
        }

        public string SupplierTokenId
        {
            get { return _supplierTokenId; }
            set { _supplierTokenId = value; }
        }

        public string SupplierTokenCode
        {
            get { return _supplierTokenCode; }
            set { _supplierTokenCode = value; }
        }

        public DateTime? SupplyDate
        {
            get { return _supplyDate; }
            set { _supplyDate = value; }
        }

        public DateTime? CustomerDate
        {
            get { return _customerDate; }
            set { _customerDate = value; }
        }

        public int? TokenId
        {
            get { return _tokenId; }
            set { _tokenId = value; }
        }

        public DateTime? EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public int? BookingId
        {
            get { return _bookingId; }
            set { _bookingId = value; }
        }

        public int? ProductLineId
        {
            get { return _productLineId; }
            set { _productLineId = value; }
        }
        #endregion
    }
    #endregion
    #region sup_SelectSupplierTokenCodesByBookingId Wrapper
    /// <summary>
    /// This class is a wrapper for the sup_SelectSupplierTokenCodesByBookingId stored procedure.
    /// </summary>
    public class SelectSupplierTokenCodesByBookingId
    {
        #region Member Variables

        protected string _connectionString = String.Empty;
        protected SqlConnection _connection;
        protected SqlTransaction _transaction;
        protected bool _ownsConnection = true;
        protected int _recordsAffected = -1;
        protected System.Int32 _returnValue;
        protected int? _bookingId;
        protected int? _userId;

        #endregion

        #region Constructors

        public SelectSupplierTokenCodesByBookingId()
        {
        }

        public SelectSupplierTokenCodesByBookingId(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public SelectSupplierTokenCodesByBookingId(SqlConnection connection)
        {
            this.Connection = connection;
        }

        public SelectSupplierTokenCodesByBookingId(SqlConnection connection, SqlTransaction transaction)
        {
            this.Connection = connection;
            this.Transaction = transaction;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The connection string to use when executing the sup_SelectSupplierTokenCodesByBookingId stored procedure.
        /// </summary>
        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        /// <summary>
        /// The connection to use when executing the sup_SelectSupplierTokenCodesByBookingId stored procedure.
        /// If this is not null, it will be used instead of creating a new connection.
        /// </summary>
        public SqlConnection Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }

        /// <summary>
        /// The transaction to use when executing the sup_SelectSupplierTokenCodesByBookingId stored procedure.
        /// If this is not null, the stored procedure will be executing within the transaction.
        /// </summary>
        public SqlTransaction Transaction
        {
            get { return _transaction; }
            set { _transaction = value; }
        }

        /// <summary>
        /// Gets the return value from the sup_SelectSupplierTokenCodesByBookingId stored procedure.
        /// </summary>
        public System.Int32 ReturnValue
        {
            get { return _returnValue; }
        }

        /// <summary>
        /// Gets the number of rows changed, inserted, or deleted by execution of the sup_SelectSupplierTokenCodesByBookingId stored procedure.
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

        /// <summary>
        /// 
        /// </summary>
        public int? UserId
        {
            get { return _userId; }
            set
            {
                _userId = value;
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
        /// This method calls the sup_SelectSupplierTokenCodesByBookingId stored procedure and returns a SqlDataReader with the results.
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
                cmd.CommandText = "[dbo].[sup_SelectSupplierTokenCodesByBookingId]";
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


                SqlParameter prmUserId = cmd.Parameters.Add("@UserId", SqlDbType.Int);
                prmUserId.Direction = ParameterDirection.Input;
                if (UserId.HasValue)
                    prmUserId.Value = UserId.Value;
                else
                    prmUserId.Value = DBNull.Value;

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
        /// This method calls the sup_SelectSupplierTokenCodesByBookingId stored procedure and returns a DataSet with the results.
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
                cmd.CommandText = "[dbo].[sup_SelectSupplierTokenCodesByBookingId]";
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


                SqlParameter prmUserId = cmd.Parameters.Add("@UserId", SqlDbType.Int);
                prmUserId.Direction = ParameterDirection.Input;
                if (UserId.HasValue)
                    prmUserId.Value = UserId.Value;
                else
                    prmUserId.Value = DBNull.Value;

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
        /// This method calls the sup_SelectSupplierTokenCodesByBookingId stored procedure and outputs the results to a custom strongly typed collection.
        /// </summary>
        /// <returns>Collection<SelectSupplierTokenCodesByBookingIdRow></returns>
        public virtual Collection<SelectSupplierTokenCodesByBookingIdRow> Execute()
        {
            Collection<SelectSupplierTokenCodesByBookingIdRow> selectSupplierTokenCodesByBookingIdRowList = new Collection<SelectSupplierTokenCodesByBookingIdRow>();
            SqlCommand cmd = new SqlCommand();

            SqlConnection cn = this.GetConnection();

            try
            {
                cmd.Connection = cn;
                cmd.Transaction = this.Transaction;
                cmd.CommandText = "[dbo].[sup_SelectSupplierTokenCodesByBookingId]";
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


                SqlParameter prmUserId = cmd.Parameters.Add("@UserId", SqlDbType.Int);
                prmUserId.Direction = ParameterDirection.Input;
                if (UserId.HasValue)
                    prmUserId.Value = UserId.Value;
                else
                    prmUserId.Value = DBNull.Value;

                #endregion

                #region Execute Command
                if (cn.State != ConnectionState.Open) cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        SelectSupplierTokenCodesByBookingIdRow selectSupplierTokenCodesByBookingIdRow = new SelectSupplierTokenCodesByBookingIdRow(reader);
                        selectSupplierTokenCodesByBookingIdRowList.Add(selectSupplierTokenCodesByBookingIdRow);
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

            return selectSupplierTokenCodesByBookingIdRowList;
        }

        /// <summary>
        /// This method calls the sup_SelectSupplierTokenCodesByBookingId stored procedure and returns a SqlDataReader with the results.
        /// </summary>
        /// <param name="connectionString">The connection string to use</param>
        /// <param name="bookingId"></param>
        /// <param name="userId"></param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(
        #region Parameters
string connectionString,
                int? bookingId,
                int? userId
        #endregion
)
        {
            SelectSupplierTokenCodesByBookingId selectSupplierTokenCodesByBookingId = new SelectSupplierTokenCodesByBookingId();

            #region Assign Property Values
            selectSupplierTokenCodesByBookingId.ConnectionString = connectionString;
            selectSupplierTokenCodesByBookingId.BookingId = bookingId;
            selectSupplierTokenCodesByBookingId.UserId = userId;
            #endregion

            SqlDataReader reader = selectSupplierTokenCodesByBookingId.ExecuteReader();

            #region Get Property Values

            #endregion

            return reader;
        }

        /// <summary>
        /// This method calls the sup_SelectSupplierTokenCodesByBookingId stored procedure and returns a DataSet with the results.
        /// </summary>
        /// <param name="connectionString">The connection string to use</param>
        /// <param name="bookingId"></param>
        /// <param name="userId"></param>
        /// <returns>DataSet</returns>
        public static DataSet ExecuteDataSet(
        #region Parameters
string connectionString,
                int? bookingId,
                int? userId
        #endregion
)
        {
            SelectSupplierTokenCodesByBookingId selectSupplierTokenCodesByBookingId = new SelectSupplierTokenCodesByBookingId();

            #region Assign Property Values
            selectSupplierTokenCodesByBookingId.ConnectionString = connectionString;
            selectSupplierTokenCodesByBookingId.BookingId = bookingId;
            selectSupplierTokenCodesByBookingId.UserId = userId;
            #endregion

            DataSet ds = selectSupplierTokenCodesByBookingId.ExecuteDataSet();

            #region Get Property Values

            #endregion

            return ds;
        }

        /// <summary>
        /// This method calls the sup_SelectSupplierTokenCodesByBookingId stored procedure and outputs the results to a custom strongly typed collection.
        /// </summary>
        /// <param name="connectionString">The connection string to use</param>
        /// <param name="bookingId"></param>
        /// <param name="userId"></param>
        /// <returns>Collection<SelectSupplierTokenCodesByBookingIdRow></returns>
        public static Collection<SelectSupplierTokenCodesByBookingIdRow> Execute(
        #region Parameters
string connectionString,
                int? bookingId,
                int? userId
        #endregion
)
        {
            SelectSupplierTokenCodesByBookingId selectSupplierTokenCodesByBookingId = new SelectSupplierTokenCodesByBookingId();

            #region Assign Property Values
            selectSupplierTokenCodesByBookingId.ConnectionString = connectionString;
            selectSupplierTokenCodesByBookingId.BookingId = bookingId;
            selectSupplierTokenCodesByBookingId.UserId = userId;
            #endregion

            Collection<SelectSupplierTokenCodesByBookingIdRow> selectSupplierTokenCodesByBookingIdRowList = selectSupplierTokenCodesByBookingId.Execute();

            #region Get Property Values

            #endregion

            return selectSupplierTokenCodesByBookingIdRowList;
        }
        #endregion
    }
    #endregion
}

