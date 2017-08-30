using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace USLoyaltySecurityServiceLayer
{
    class SQLHelper
    {
        private static string connectionString = Convert.ToString(ConfigurationSettings.AppSettings["SecurityConnectionString"]);
        SqlConnection sqlCon;
        SqlCommand sqlCmd;


        #region private methods

        /// <summary>
        /// to update the username in Security framework data base
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        public bool UpdateUserName(string UserName, Int64 CustomerID)
        {
            bool status = false;
            sqlCon = new SqlConnection(connectionString);
            sqlCmd = new SqlCommand("UpdateUserName", sqlCon);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            SqlParameter sqlCustomerID = sqlCmd.Parameters.Add("@CustomerID", System.Data.SqlDbType.BigInt);
            sqlCustomerID.Value = CustomerID;
            SqlParameter sqlUserName = sqlCmd.Parameters.Add("@UserName", System.Data.SqlDbType.VarChar);
            sqlUserName.Value = UserName;
            int count = sqlCmd.ExecuteNonQuery();
            if (count > 0)
            {
                status = true;
            }
            else
            {
                status = false;
            }
            return status;
        }

        /// <summary>
        /// to get the customerID from the Security framework data base
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>

        public Int64 GetCutomerID(string UserName)
        {
            DataSet dsCustomer = null;
            Int64 customerID=0;

            sqlCon = new SqlConnection(connectionString);
            sqlCmd = new SqlCommand("GetCustomerID", sqlCon);

            sqlCmd.CommandType = CommandType.StoredProcedure;

            SqlParameter sqlParam = sqlCmd.Parameters.Add("@Email", SqlDbType.VarChar);

            sqlParam.Value = UserName;
            SqlDataAdapter adapter = new SqlDataAdapter(sqlCmd);
            dsCustomer = new DataSet();
            adapter.Fill(dsCustomer, "CustomerID");
            if (dsCustomer.Tables[0].Rows.Count > 0)
            {
                customerID = Convert.ToInt64(dsCustomer.Tables[0].Rows[0]["CustomerID"].ToString());
            }
            
            return customerID;

        }
        #endregion
    }
}
