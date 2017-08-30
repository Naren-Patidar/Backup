using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Collections.ObjectModel;

namespace InstoreClubcardReward.Data
{
    #region SelectKioskMaster
    /// <summary>
    /// Stores result row level information from the SelectKiosk stored procedure.
    /// </summary>
    [Serializable]
    public class SelectKioskMasterRow
    {
        #region Member Variables
        protected SqlString _kioskIP = SqlString.Null;
        protected SqlInt32 _kioskID = SqlInt32.Null;
        protected SqlInt32 _storeID = SqlInt32.Null;
        protected SqlInt32 _kioskNo = SqlInt32.Null;
        #endregion

        #region Constructors
        public SelectKioskMasterRow()
        {
        }

        public SelectKioskMasterRow(SqlDataReader reader)
        {
            this.LoadFromReader(reader);
        }
        #endregion

        #region Helper Methods
        protected void LoadFromReader(SqlDataReader reader)
        {
            if (reader != null && !reader.IsClosed)
            {
                if (!reader.IsDBNull(0)) _kioskIP = reader.GetString(0);
                if (!reader.IsDBNull(1)) _kioskID = reader.GetInt32(1);
                if (!reader.IsDBNull(1)) _storeID = reader.GetInt32(2);
                if (!reader.IsDBNull(2)) _kioskNo = reader.GetInt32(3);

            }
        }
        #endregion

        #region Public Properties
        public SqlString KioskIP
        {
            get { return _kioskIP; }
            set { _kioskIP = value; }
        }

        public SqlInt32 KioskID
        {
            get { return _kioskID; }
            set { _kioskID = value; }
        }

        public SqlInt32 StoreID
        {
            get { return _storeID; }
            set { _storeID = value; }
        }

        public SqlInt32 KioskNo
        {
            get { return _kioskNo; }
            set { _kioskNo = value; }
        }

        #endregion
    }
    #endregion
    #region SelectKiosk Wrapper
    /// <summary>
    /// This class is a wrapper for the SelectKiosk stored procedure.
    /// </summary>
    public class SelectKioskMaster
    {
        #region Member Variables
        protected string _connectionString = String.Empty;
        protected SqlConnection _connection = null;
        protected SqlTransaction _transaction = null;
        protected bool _ownsConnection = true;
        protected int _recordsAffected = -1;
        protected int _returnValue = 0;
        protected SqlString _kioskIP = SqlString.Null;
        protected bool _kioskIPSet = false;
        #endregion

        #region Constructors
        public SelectKioskMaster()
        {
        }

        public SelectKioskMaster(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public SelectKioskMaster(SqlConnection connection)
        {
            this.Connection = connection;
        }

        public SelectKioskMaster(SqlConnection connection, SqlTransaction transaction)
        {
            this.Connection = connection;
            this.Transaction = transaction;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// The connection string to use when executing the SelectKiosk stored procedure.
        /// </summary>
        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        /// <summary>
        /// The connection to use when executing the SelectKiosk stored procedure.
        /// If this is not null, it will be used instead of creating a new connection.
        /// </summary>
        public SqlConnection Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }

        /// <summary>
        /// The transaction to use when executing the SelectKiosk stored procedure.
        /// If this is not null, the stored procedure will be executing within the transaction.
        /// </summary>
        public SqlTransaction Transaction
        {
            get { return _transaction; }
            set { _transaction = value; }
        }

        /// <summary>
        /// Gets the return value from the SelectKiosk stored procedure.
        /// </summary>
        public int ReturnValue
        {
            get { return _returnValue; }
        }

        /// <summary>
        /// Gets the number of rows changed, inserted, or deleted by execution of the SelectKiosk stored procedure.
        /// </summary>
        public int RecordsAffected
        {
            get { return _recordsAffected; }
        }

        /// <summary>
        /// 
        /// </summary>
        public SqlString KioskIP
        {
            get { return _kioskIP; }
            set
            {
                _kioskIP = value;
                _kioskIPSet = true;
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
        /// This method calls the SelectKiosk stored procedure and returns a SqlDataReader with the results.
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
                cmd.CommandText = "[dbo].[SelectKiosk]";
                cmd.CommandType = CommandType.StoredProcedure;

                #region Populate Parameters
                SqlParameter prmReturnValue = cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                prmReturnValue.Direction = ParameterDirection.ReturnValue;

                SqlParameter prmKioskIP = cmd.Parameters.Add("@KioskIP", SqlDbType.VarChar);
                prmKioskIP.Direction = ParameterDirection.Input;
                prmKioskIP.Size = 15;
                if (_kioskIPSet == true || this.KioskIP.IsNull == false)
                {
                    prmKioskIP.Value = this.KioskIP;
                }
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
                    _returnValue = (int)prmReturnValue.Value;
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
        /// This method calls the SelectKiosk stored procedure and returns a DataSet with the results.
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
                cmd.CommandText = "[dbo].[SelectKiosk]";
                cmd.CommandType = CommandType.StoredProcedure;

                #region Populate Parameters
                SqlParameter prmReturnValue = cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                prmReturnValue.Direction = ParameterDirection.ReturnValue;

                SqlParameter prmKioskIP = cmd.Parameters.Add("@KioskIP", SqlDbType.VarChar);
                prmKioskIP.Direction = ParameterDirection.Input;
                prmKioskIP.Size = 15;
                if (_kioskIPSet == true || this.KioskIP.IsNull == false)
                {
                    prmKioskIP.Value = this.KioskIP;
                }
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

            return ds;
        }

        /// <summary>
        /// This method calls the SelectKiosk stored procedure and outputs the results to a custom strongly typed collection.
        /// </summary>
        /// <returns>Collection<SelectKioskRow></returns>
        public virtual Collection<SelectKioskMasterRow> Execute()
        {
            Collection<SelectKioskMasterRow> selectKioskRowList = new Collection<SelectKioskMasterRow>();
            SqlCommand cmd = new SqlCommand();

            SqlConnection cn = this.GetConnection();

            try
            {
                cmd.Connection = cn;
                cmd.Transaction = this.Transaction;
                cmd.CommandText = "[dbo].[SelectKiosk]";
                cmd.CommandType = CommandType.StoredProcedure;

                #region Populate Parameters
                SqlParameter prmReturnValue = cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                prmReturnValue.Direction = ParameterDirection.ReturnValue;

                SqlParameter prmKioskIP = cmd.Parameters.Add("@KioskIP", SqlDbType.VarChar);
                prmKioskIP.Direction = ParameterDirection.Input;
                prmKioskIP.Size = 15;
                if (_kioskIPSet == true || this.KioskIP.IsNull == false)
                {
                    prmKioskIP.Value = this.KioskIP;
                }
                #endregion

                #region Execute Command
                if (cn.State != ConnectionState.Open) cn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        SelectKioskMasterRow selectKioskRow = new SelectKioskMasterRow(reader);
                        selectKioskRowList.Add(selectKioskRow);
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

            return selectKioskRowList;
        }

        /// <summary>
        /// This method calls the SelectKiosk stored procedure and returns a SqlDataReader with the results.
        /// </summary>
        /// <param name="connectionString">The connection string to use</param>
        /// <param name="ipAddress"></param>
        /// <returns>SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(
        #region Parameters
string connectionString,
                SqlString kioskIP
        #endregion
)
        {
            SelectKioskMaster selectKiosk = new SelectKioskMaster();

            #region Assign Property Values
            selectKiosk.ConnectionString = connectionString;
            selectKiosk.KioskIP = kioskIP;
            #endregion

            SqlDataReader reader = selectKiosk.ExecuteReader();

            #region Get Property Values

            #endregion

            return reader;
        }

        /// <summary>
        /// This method calls the SelectKiosk stored procedure and returns a DataSet with the results.
        /// </summary>
        /// <param name="connectionString">The connection string to use</param>
        /// <param name="ipAddress"></param>
        /// <returns>DataSet</returns>
        public static DataSet ExecuteDataSet(
        #region Parameters
string connectionString,
                SqlString kioskIP
        #endregion
)
        {
            SelectKioskMaster selectKiosk = new SelectKioskMaster();

            #region Assign Property Values
            selectKiosk.ConnectionString = connectionString;
            selectKiosk.KioskIP = kioskIP;
            #endregion

            DataSet ds = selectKiosk.ExecuteDataSet();

            #region Get Property Values

            #endregion

            return ds;
        }

        /// <summary>
        /// This method calls the SelectKiosk stored procedure and outputs the results to a custom strongly typed collection.
        /// </summary>
        /// <param name="connectionString">The connection string to use</param>
        /// <param name="ipAddress"></param>
        /// <returns>Collection<SelectKioskRow></returns>
        public static Collection<SelectKioskMasterRow> Execute(
        #region Parameters
string connectionString,
                SqlString kioskIP
        #endregion
)
        {
            SelectKioskMaster selectKiosk = new SelectKioskMaster();

            #region Assign Property Values
            selectKiosk.ConnectionString = connectionString;
            selectKiosk.KioskIP = kioskIP;
            #endregion

            Collection<SelectKioskMasterRow> selectKioskRowList = selectKiosk.Execute();

            #region Get Property Values

            #endregion

            return selectKioskRowList;
        }
        #endregion
    }
    #endregion
}

