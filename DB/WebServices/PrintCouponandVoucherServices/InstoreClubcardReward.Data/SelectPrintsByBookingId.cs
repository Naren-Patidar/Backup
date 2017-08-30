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
    #region SelectPrintsByBookingIdRow
    /// <summary>
    /// Stores result row level information from the sup_SelectPrintsByBookingId stored procedure.
    /// </summary>
    [Serializable]
    public class SelectPrintsByBookingIdRow
    {
        #region Member Variables
        protected int? _bookingId;
        protected int? _printNumber;
        protected DateTime? _printDate;
        protected string _printParameter;
        #endregion

        #region Constructors
        public SelectPrintsByBookingIdRow()
        {
        }

        public SelectPrintsByBookingIdRow(SqlDataReader reader)
        {
            this.LoadFromReader(reader);
        }
        #endregion

        #region Helper Methods
        protected void LoadFromReader(SqlDataReader reader)
        {
            if (reader != null && !reader.IsClosed)
            {
                if (!reader.IsDBNull(0)) _bookingId = reader.GetInt32(0);
                if (!reader.IsDBNull(1)) _printNumber = reader.GetInt32(1);
                if (!reader.IsDBNull(2)) _printDate = reader.GetDateTime(2);
                if (!reader.IsDBNull(3)) _printParameter = reader.GetString(3);
            }
        }
        #endregion

        #region Public Properties
        public int? BookingId
        {
            get { return _bookingId; }
            set { _bookingId = value; }
        }

        public int? PrintNumber
        {
            get { return _printNumber; }
            set { _printNumber = value; }
        }

        public DateTime? PrintDate
        {
            get { return _printDate; }
            set { _printDate = value; }
        }

        public string PrintParameter
        {
            get { return _printParameter; }
            set { _printParameter = value; }
        }
        #endregion
    }
    #endregion
    #region sup_SelectPrintsByBookingId Wrapper
    /// <summary>
    /// This class is a wrapper for the sup_SelectPrintsByBookingId stored procedure.
    /// </summary>
    public class SelectPrintsByBookingId
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

        public SelectPrintsByBookingId()
        {
        }

        public SelectPrintsByBookingId(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public SelectPrintsByBookingId(SqlConnection connection)
        {
            this.Connection = connection;
        }

        public SelectPrintsByBookingId(SqlConnection connection, SqlTransaction transaction)
        {
            this.Connection = connection;
            this.Transaction = transaction;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The connection string to use when executing the sup_SelectPrintsByBookingId stored procedure.
        /// </summary>
        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        /// <summary>
        /// The connection to use when executing the sup_SelectPrintsByBookingId stored procedure.
        /// If this is not null, it will be used instead of creating a new connection.
        /// </summary>
        public SqlConnection Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }

        /// <summary>
        /// The transaction to use when executing the sup_SelectPrintsByBookingId stored procedure.
        /// If this is not null, the stored procedure will be executing within the transaction.
        /// </summary>
        public SqlTransaction Transaction
        {
            get { return _transaction; }
            set { _transaction = value; }
        }

        /// <summary>
        /// Gets the return value from the sup_SelectPrintsByBookingId stored procedure.
        /// </summary>
        public System.Int32 ReturnValue
        {
            get { return _returnValue; }
        }

        /// <summary>
        /// Gets the number of rows changed, inserted, or deleted by execution of the sup_SelectPrintsByBookingId stored procedure.
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
        /// This method calls the sup_SelectPrintsByBookingId stored procedure and returns a SqlDataReader with the results.
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
                cmd.CommandText = "[dbo].[sup_SelectPrintsByBookingId]";
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
        /// This method calls the sup_SelectPrintsByBookingId stored procedure and returns a DataSet with the results.
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
                cmd.CommandText = "[dbo].[sup_SelectPrintsByBookingId]";
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
        /// This method calls the sup_SelectPrintsByBookingId stored procedure and outputs the results to a custom strongly typed collection.
        /// </summary>
        /// <returns>Collection<SelectPrintsByBookingIdRow></returns>
        public virtual Collection<SelectPrintsByBookingIdRow> Execute()
        {
            Collection<SelectPrintsByBookingIdRow> selectPrintsByBookingIdRowList = new Collection<SelectPrintsByBookingIdRow>();
            SqlCommand cmd = new SqlCommand();

            SqlConnection cn = this.GetConnection();

            try
            {
                cmd.Connection = cn;
                cmd.Transaction = this.Transaction;
                cmd.CommandText = "[dbo].[sup_SelectPrintsByBookingId]";
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
                        SelectPrintsByBookingIdRow selectPrintsByBookingIdRow = new SelectPrintsByBookingIdRow(reader);
                        selectPrintsByBookingIdRowList.Add(selectPrintsByBookingIdRow);
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

            return selectPrintsByBookingIdRowList;
        }

        /// <summary>
        /// This method calls the sup_SelectPrintsByBookingId stored procedure and returns a SqlDataReader with the results.
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
            SelectPrintsByBookingId selectPrintsByBookingId = new SelectPrintsByBookingId();

            #region Assign Property Values
            selectPrintsByBookingId.ConnectionString = connectionString;
            selectPrintsByBookingId.BookingId = bookingId;
            #endregion

            SqlDataReader reader = selectPrintsByBookingId.ExecuteReader();

            #region Get Property Values

            #endregion

            return reader;
        }

        /// <summary>
        /// This method calls the sup_SelectPrintsByBookingId stored procedure and returns a DataSet with the results.
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
            SelectPrintsByBookingId selectPrintsByBookingId = new SelectPrintsByBookingId();

            #region Assign Property Values
            selectPrintsByBookingId.ConnectionString = connectionString;
            selectPrintsByBookingId.BookingId = bookingId;
            #endregion

            DataSet ds = selectPrintsByBookingId.ExecuteDataSet();

            #region Get Property Values

            #endregion

            return ds;
        }

        /// <summary>
        /// This method calls the sup_SelectPrintsByBookingId stored procedure and outputs the results to a custom strongly typed collection.
        /// </summary>
        /// <param name="connectionString">The connection string to use</param>
        /// <param name="bookingId"></param>
        /// <returns>Collection<SelectPrintsByBookingIdRow></returns>
        public static Collection<SelectPrintsByBookingIdRow> Execute(
        #region Parameters
string connectionString,
                int? bookingId
        #endregion
)
        {
            SelectPrintsByBookingId selectPrintsByBookingId = new SelectPrintsByBookingId();

            #region Assign Property Values
            selectPrintsByBookingId.ConnectionString = connectionString;
            selectPrintsByBookingId.BookingId = bookingId;
            #endregion

            Collection<SelectPrintsByBookingIdRow> selectPrintsByBookingIdRowList = selectPrintsByBookingId.Execute();

            #region Get Property Values

            #endregion

            return selectPrintsByBookingIdRowList;
        }
        #endregion
    }
    #endregion
}

