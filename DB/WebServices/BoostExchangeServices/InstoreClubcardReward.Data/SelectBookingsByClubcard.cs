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
    #region SelectBookingsByClubcardRow
    /// <summary>
    /// Stores result row level information from the loc_SelectBookingsByClubcard stored procedure.
    /// </summary>
    [Serializable]
    public class SelectBookingsByClubcardRow
    {
        #region Member Variables
        protected int? _bookingId;
        protected DateTime? _createDate;
        protected string _clubcard;
        protected string _status;
        protected DateTime? _statusDate;
        protected int? _storeId;
        protected int? _userId;
        protected int? _tillId;
        protected bool? _copiedForSupport;
        #endregion

        #region Constructors
        public SelectBookingsByClubcardRow()
        {
        }

        public SelectBookingsByClubcardRow(SqlDataReader reader)
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
                if (!reader.IsDBNull(1)) _createDate = reader.GetDateTime(1);
                if (!reader.IsDBNull(2)) _clubcard = reader.GetString(2);
                if (!reader.IsDBNull(3)) _status = reader.GetString(3);
                if (!reader.IsDBNull(4)) _statusDate = reader.GetDateTime(4);
                if (!reader.IsDBNull(5)) _storeId = reader.GetInt32(5);
                if (!reader.IsDBNull(6)) _userId = reader.GetInt32(6);
                if (!reader.IsDBNull(7)) _tillId = reader.GetInt32(7);
                if (!reader.IsDBNull(8)) _copiedForSupport = reader.GetBoolean(8);
            }
        }
        #endregion

        #region Public Properties
        public int? BookingId
        {
            get { return _bookingId; }
            set { _bookingId = value; }
        }

        public DateTime? CreateDate
        {
            get { return _createDate; }
            set { _createDate = value; }
        }

        public string Clubcard
        {
            get { return _clubcard; }
            set { _clubcard = value; }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public DateTime? StatusDate
        {
            get { return _statusDate; }
            set { _statusDate = value; }
        }

        public int? StoreId
        {
            get { return _storeId; }
            set { _storeId = value; }
        }

        public int? UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        public int? TillId
        {
            get { return _tillId; }
            set { _tillId = value; }
        }

        public bool? CopiedForSupport
        {
            get { return _copiedForSupport; }
            set { _copiedForSupport = value; }
        }
        #endregion
    }
    #endregion
    #region loc_SelectBookingsByClubcard Wrapper
    /// <summary>
    /// This class is a wrapper for the loc_SelectBookingsByClubcard stored procedure.
    /// </summary>
    public class SelectBookingsByClubcard
    {
        #region Member Variables

        protected string _connectionString = String.Empty;
        protected SqlConnection _connection;
        protected SqlTransaction _transaction;
        protected bool _ownsConnection = true;
        protected int _recordsAffected = -1;
        protected System.Int32 _returnValue;
        protected string _clubcard;
        protected DateTime? _start;
        protected DateTime? _end;

        #endregion

        #region Constructors

        public SelectBookingsByClubcard()
        {
        }

        public SelectBookingsByClubcard(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public SelectBookingsByClubcard(SqlConnection connection)
        {
            this.Connection = connection;
        }

        public SelectBookingsByClubcard(SqlConnection connection, SqlTransaction transaction)
        {
            this.Connection = connection;
            this.Transaction = transaction;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The connection string to use when executing the loc_SelectBookingsByClubcard stored procedure.
        /// </summary>
        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        /// <summary>
        /// The connection to use when executing the loc_SelectBookingsByClubcard stored procedure.
        /// If this is not null, it will be used instead of creating a new connection.
        /// </summary>
        public SqlConnection Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }

        /// <summary>
        /// The transaction to use when executing the loc_SelectBookingsByClubcard stored procedure.
        /// If this is not null, the stored procedure will be executing within the transaction.
        /// </summary>
        public SqlTransaction Transaction
        {
            get { return _transaction; }
            set { _transaction = value; }
        }

        /// <summary>
        /// Gets the return value from the loc_SelectBookingsByClubcard stored procedure.
        /// </summary>
        public System.Int32 ReturnValue
        {
            get { return _returnValue; }
        }

        /// <summary>
        /// Gets the number of rows changed, inserted, or deleted by execution of the loc_SelectBookingsByClubcard stored procedure.
        /// </summary>
        public int RecordsAffected
        {
            get { return _recordsAffected; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Clubcard
        {
            get { return _clubcard; }
            set
            {
                _clubcard = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? Start
        {
            get { return _start; }
            set
            {
                _start = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? End
        {
            get { return _end; }
            set
            {
                _end = value;
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
        /// This method calls the loc_SelectBookingsByClubcard stored procedure and returns a SqlDataReader with the results.
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
                cmd.CommandText = "[dbo].[loc_SelectBookingsByClubcard]";
                cmd.CommandType = CommandType.StoredProcedure;

                #region Populate Parameters

                SqlParameter prmReturnValue = cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                prmReturnValue.Direction = ParameterDirection.ReturnValue;

                SqlParameter prmClubcard = cmd.Parameters.Add("@Clubcard", SqlDbType.VarChar);
                prmClubcard.Direction = ParameterDirection.Input;
                prmClubcard.Size = 18;
                if (!string.IsNullOrEmpty(Clubcard))
                    prmClubcard.Value = Clubcard;
                else
                    prmClubcard.Value = DBNull.Value;


                SqlParameter prmStart = cmd.Parameters.Add("@Start", SqlDbType.DateTime);
                prmStart.Direction = ParameterDirection.Input;
                if (Start.HasValue)
                    prmStart.Value = Start.Value;
                else
                    prmStart.Value = DBNull.Value;


                SqlParameter prmEnd = cmd.Parameters.Add("@End", SqlDbType.DateTime);
                prmEnd.Direction = ParameterDirection.Input;
                if (End.HasValue)
                    prmEnd.Value = End.Value;
                else
                    prmEnd.Value = DBNull.Value;

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
        /// This method calls the loc_SelectBookingsByClubcard stored procedure and returns a DataSet with the results.
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
                cmd.CommandText = "[dbo].[loc_SelectBookingsByClubcard]";
                cmd.CommandType = CommandType.StoredProcedure;

                #region Populate Parameters
                SqlParameter prmReturnValue = cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                prmReturnValue.Direction = ParameterDirection.ReturnValue;

                SqlParameter prmClubcard = cmd.Parameters.Add("@Clubcard", SqlDbType.VarChar);
                prmClubcard.Direction = ParameterDirection.Input;
                prmClubcard.Size = 18;
                if (!string.IsNullOrEmpty(Clubcard))
                    prmClubcard.Value = Clubcard;
                else
                    prmClubcard.Value = DBNull.Value;


                SqlParameter prmStart = cmd.Parameters.Add("@Start", SqlDbType.DateTime);
                prmStart.Direction = ParameterDirection.Input;
                if (Start.HasValue)
                    prmStart.Value = Start.Value;
                else
                    prmStart.Value = DBNull.Value;


                SqlParameter prmEnd = cmd.Parameters.Add("@End", SqlDbType.DateTime);
                prmEnd.Direction = ParameterDirection.Input;
                if (End.HasValue)
                    prmEnd.Value = End.Value;
                else
                    prmEnd.Value = DBNull.Value;

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
        /// This method calls the loc_SelectBookingsByClubcard stored procedure and outputs the results to a custom strongly typed collection.
        /// </summary>
        /// <returns>Collection<SelectBookingsByClubcardRow></returns>
        public virtual Collection<SelectBookingsByClubcardRow> Execute()
        {
            Collection<SelectBookingsByClubcardRow> selectBookingsByClubcardRowList = new Collection<SelectBookingsByClubcardRow>();
            SqlCommand cmd = new SqlCommand();

            SqlConnection cn = this.GetConnection();

            try
            {
                cmd.Connection = cn;
                cmd.Transaction = this.Transaction;
                cmd.CommandText = "[dbo].[loc_SelectBookingsByClubcard]";
                cmd.CommandType = CommandType.StoredProcedure;

                #region Populate Parameters
                SqlParameter prmReturnValue = cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                prmReturnValue.Direction = ParameterDirection.ReturnValue;

                SqlParameter prmClubcard = cmd.Parameters.Add("@Clubcard", SqlDbType.VarChar);
                prmClubcard.Direction = ParameterDirection.Input;
                prmClubcard.Size = 18;
                if (!string.IsNullOrEmpty(Clubcard))
                    prmClubcard.Value = Clubcard;
                else
                    prmClubcard.Value = DBNull.Value;


                SqlParameter prmStart = cmd.Parameters.Add("@Start", SqlDbType.DateTime);
                prmStart.Direction = ParameterDirection.Input;
                if (Start.HasValue)
                    prmStart.Value = Start.Value;
                else
                    prmStart.Value = DBNull.Value;


                SqlParameter prmEnd = cmd.Parameters.Add("@End", SqlDbType.DateTime);
                prmEnd.Direction = ParameterDirection.Input;
                if (End.HasValue)
                    prmEnd.Value = End.Value;
                else
                    prmEnd.Value = DBNull.Value;

                #endregion

                #region Execute Command
                if (cn.State != ConnectionState.Open) cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        SelectBookingsByClubcardRow selectBookingsByClubcardRow = new SelectBookingsByClubcardRow(reader);
                        selectBookingsByClubcardRowList.Add(selectBookingsByClubcardRow);
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

            return selectBookingsByClubcardRowList;
        }

        /// <summary>
        /// This method calls the loc_SelectBookingsByClubcard stored procedure and returns a SqlDataReader with the results.
        /// </summary>
        /// <param name="connectionString">The connection string to use</param>
        /// <param name="clubcard"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(
        #region Parameters
string connectionString,
                string clubcard,
                DateTime? start,
                DateTime? end
        #endregion
)
        {
            SelectBookingsByClubcard selectBookingsByClubcard = new SelectBookingsByClubcard();

            #region Assign Property Values
            selectBookingsByClubcard.ConnectionString = connectionString;
            selectBookingsByClubcard.Clubcard = clubcard;
            selectBookingsByClubcard.Start = start;
            selectBookingsByClubcard.End = end;
            #endregion

            SqlDataReader reader = selectBookingsByClubcard.ExecuteReader();

            #region Get Property Values

            #endregion

            return reader;
        }

        /// <summary>
        /// This method calls the loc_SelectBookingsByClubcard stored procedure and returns a DataSet with the results.
        /// </summary>
        /// <param name="connectionString">The connection string to use</param>
        /// <param name="clubcard"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns>DataSet</returns>
        public static DataSet ExecuteDataSet(
        #region Parameters
string connectionString,
                string clubcard,
                DateTime? start,
                DateTime? end
        #endregion
)
        {
            SelectBookingsByClubcard selectBookingsByClubcard = new SelectBookingsByClubcard();

            #region Assign Property Values
            selectBookingsByClubcard.ConnectionString = connectionString;
            selectBookingsByClubcard.Clubcard = clubcard;
            selectBookingsByClubcard.Start = start;
            selectBookingsByClubcard.End = end;
            #endregion

            DataSet ds = selectBookingsByClubcard.ExecuteDataSet();

            #region Get Property Values

            #endregion

            return ds;
        }

        /// <summary>
        /// This method calls the loc_SelectBookingsByClubcard stored procedure and outputs the results to a custom strongly typed collection.
        /// </summary>
        /// <param name="connectionString">The connection string to use</param>
        /// <param name="clubcard"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns>Collection<SelectBookingsByClubcardRow></returns>
        public static Collection<SelectBookingsByClubcardRow> Execute(
        #region Parameters
string connectionString,
                string clubcard,
                DateTime? start,
                DateTime? end
        #endregion
)
        {
            SelectBookingsByClubcard selectBookingsByClubcard = new SelectBookingsByClubcard();

            #region Assign Property Values
            selectBookingsByClubcard.ConnectionString = connectionString;
            selectBookingsByClubcard.Clubcard = clubcard;
            selectBookingsByClubcard.Start = start;
            selectBookingsByClubcard.End = end;
            #endregion

            Collection<SelectBookingsByClubcardRow> selectBookingsByClubcardRowList = selectBookingsByClubcard.Execute();

            #region Get Property Values

            #endregion

            return selectBookingsByClubcardRowList;
        }
        #endregion
    }
    #endregion
}

