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

namespace InstoreClubcardReward.Data
{
    #region InsertTraining Wrapper
    /// <summary>
    /// This class is a wrapper for the InsertTraining stored procedure.
    /// </summary>
    public class InsertTraining
    {
        #region Member Variables

        protected string _connectionString = String.Empty;
        protected SqlConnection _connection;
        protected SqlTransaction _transaction;
        protected bool _ownsConnection = true;
        protected int _recordsAffected = -1;
        protected System.Int32 _returnValue;
        protected int? _storeId;
        protected int? _userId;
        protected int? _tillId;
        protected string _locationDescription;
        protected DateTime? _trainingDate;
        protected int? _bookingId;
        protected int? _id;

        #endregion

        #region Constructors

        public InsertTraining()
        {
        }

        public InsertTraining(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public InsertTraining(SqlConnection connection)
        {
            this.Connection = connection;
        }

        public InsertTraining(SqlConnection connection, SqlTransaction transaction)
        {
            this.Connection = connection;
            this.Transaction = transaction;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The connection string to use when executing the InsertTraining stored procedure.
        /// </summary>
        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        /// <summary>
        /// The connection to use when executing the InsertTraining stored procedure.
        /// If this is not null, it will be used instead of creating a new connection.
        /// </summary>
        public SqlConnection Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }

        /// <summary>
        /// The transaction to use when executing the InsertTraining stored procedure.
        /// If this is not null, the stored procedure will be executing within the transaction.
        /// </summary>
        public SqlTransaction Transaction
        {
            get { return _transaction; }
            set { _transaction = value; }
        }

        /// <summary>
        /// Gets the return value from the InsertTraining stored procedure.
        /// </summary>
        public System.Int32 ReturnValue
        {
            get { return _returnValue; }
        }

        /// <summary>
        /// Gets the number of rows changed, inserted, or deleted by execution of the InsertTraining stored procedure.
        /// </summary>
        public int RecordsAffected
        {
            get { return _recordsAffected; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int? StoreId
        {
            get { return _storeId; }
            set
            {
                _storeId = value;
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

        /// <summary>
        /// 
        /// </summary>
        public int? TillId
        {
            get { return _tillId; }
            set
            {
                _tillId = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string LocationDescription
        {
            get { return _locationDescription; }
            set
            {
                _locationDescription = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? TrainingDate
        {
            get { return _trainingDate; }
            set
            {
                _trainingDate = value;
            }
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
        public int? Id
        {
            get { return _id; }
            set
            {
                _id = value;
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
        /// This method calls the InsertTraining stored procedure.
        /// </summary>
        public virtual void Execute()
        {
            SqlCommand cmd = new SqlCommand();

            SqlConnection cn = this.GetConnection();

            try
            {
                cmd.Connection = cn;
                cmd.Transaction = this.Transaction;
                cmd.CommandText = "[dbo].[InsertTraining]";
                cmd.CommandType = CommandType.StoredProcedure;

                #region Populate Parameters
                SqlParameter prmReturnValue = cmd.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                prmReturnValue.Direction = ParameterDirection.ReturnValue;

                SqlParameter prmStoreId = cmd.Parameters.Add("@StoreId", SqlDbType.Int);
                prmStoreId.Direction = ParameterDirection.Input;
                if (StoreId.HasValue)
                    prmStoreId.Value = StoreId.Value;
                else
                    prmStoreId.Value = DBNull.Value;


                SqlParameter prmUserId = cmd.Parameters.Add("@UserId", SqlDbType.Int);
                prmUserId.Direction = ParameterDirection.Input;
                if (UserId.HasValue)
                    prmUserId.Value = UserId.Value;
                else
                    prmUserId.Value = DBNull.Value;


                SqlParameter prmTillId = cmd.Parameters.Add("@TillId", SqlDbType.Int);
                prmTillId.Direction = ParameterDirection.Input;
                if (TillId.HasValue)
                    prmTillId.Value = TillId.Value;
                else
                    prmTillId.Value = DBNull.Value;


                SqlParameter prmLocationDescription = cmd.Parameters.Add("@LocationDescription", SqlDbType.VarChar);
                prmLocationDescription.Direction = ParameterDirection.Input;
                prmLocationDescription.Size = 255;
                if (!string.IsNullOrEmpty(LocationDescription))
                    prmLocationDescription.Value = LocationDescription;
                else
                    prmLocationDescription.Value = DBNull.Value;


                SqlParameter prmTrainingDate = cmd.Parameters.Add("@TrainingDate", SqlDbType.DateTime);
                prmTrainingDate.Direction = ParameterDirection.Input;
                if (TrainingDate.HasValue)
                    prmTrainingDate.Value = TrainingDate.Value;
                else
                    prmTrainingDate.Value = DBNull.Value;


                SqlParameter prmBookingId = cmd.Parameters.Add("@BookingId", SqlDbType.Int);
                prmBookingId.Direction = ParameterDirection.Input;
                if (BookingId.HasValue)
                    prmBookingId.Value = BookingId.Value;
                else
                    prmBookingId.Value = DBNull.Value;


                SqlParameter prmId = cmd.Parameters.Add("@Id", SqlDbType.Int);
                if (Id.HasValue)
                {
                    prmId.Direction = ParameterDirection.InputOutput;
                }
                else
                {
                    prmId.Direction = ParameterDirection.Output;
                }
                if (Id.HasValue)
                    prmId.Value = Id.Value;
                else
                    prmId.Value = DBNull.Value;

                #endregion

                #region Execute Command
                if (cn.State != ConnectionState.Open) cn.Open();
                _recordsAffected = cmd.ExecuteNonQuery();
                #endregion

                #region Get Output Parameters
                if (prmReturnValue.Value != null && prmReturnValue.Value != DBNull.Value)
                {
                    _returnValue = (System.Int32)prmReturnValue.Value;
                }

                if (prmId != null && prmId.Value != null)
                {
                    if (prmId.Value is int?)
                    {
                        this.Id = (int?)prmId.Value;
                    }
                    else
                    {
                        if (prmId.Value != DBNull.Value)
                        {
                            this.Id = new int?((int)prmId.Value);
                        }
                        else
                        {
                            this.Id = null;
                        }
                    }
                }
                else
                {
                    this.Id = null;
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
        /// This method calls the InsertTraining stored procedure.
        /// </summary>
        /// <param name="connectionString">The connection string to use</param>
        /// <param name="storeId"></param>
        /// <param name="userId"></param>
        /// <param name="tillId"></param>
        /// <param name="locationDescription"></param>
        /// <param name="trainingDate"></param>
        /// <param name="bookingId"></param>
        /// <param name="id"></param>
        public static void Execute(
        #region Parameters
string connectionString,
                int? storeId,
                int? userId,
                int? tillId,
                string locationDescription,
                DateTime? trainingDate,
                int? bookingId,
                ref int? id
        #endregion
)
        {
            InsertTraining insertTraining = new InsertTraining();

            #region Assign Property Values
            insertTraining.ConnectionString = connectionString;
            insertTraining.StoreId = storeId;
            insertTraining.UserId = userId;
            insertTraining.TillId = tillId;
            insertTraining.LocationDescription = locationDescription;
            insertTraining.TrainingDate = trainingDate;
            insertTraining.BookingId = bookingId;
            insertTraining.Id = id;
            #endregion

            insertTraining.Execute();

            #region Get Property Values
            id = insertTraining.Id;
            #endregion
        }
        #endregion
    }
    #endregion
}

