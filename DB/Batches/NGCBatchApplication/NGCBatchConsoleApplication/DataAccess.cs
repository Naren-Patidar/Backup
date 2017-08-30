using System;
using System.Data;
using System.Configuration;
using System.Web;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Collections;

/// <summary>
/// Summary description for DataAccess
/// </summary>
public class DataAccess
{
    private static Hashtable paramCache = Hashtable.Synchronized(new Hashtable());
	public DataAccess()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public static int ExecuteNonQuery(string connectionString, string spName, ref object[] parameterValues)
    {
       
        //Trace trace = new Trace();

        // If we receive parameter values, we need to figure out where they go
        if ((parameterValues != null) && (parameterValues.Length > 0))
        {
            // Pull the parameters for this stored procedure from the parameter cache (or discover them & populate the cache)
            SqlParameter[] commandParameters = GetSpParameterSet(connectionString, spName);
            //SqlParameter[] commandParameters = SqlHelperParameterCache.GetSpParameterSet(connectionString, spName);
            try
            {
                // Assign the provided values to these parameters based on parameter order
                AssignParameterValues(commandParameters, parameterValues);
            }
            catch (Exception e)
            {
                //HandleSqlExecutionException(e, trace, spName);
                throw e;
                
            }

            // Call the overload that takes an array of SqlParameters
             Int32 iReturn = ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            //return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, commandParameters);
                       
            for (int i = 0; i <= commandParameters.Length - 1; i++)
            {
                if (commandParameters[i].Direction == ParameterDirection.Output || commandParameters[i].Direction == ParameterDirection.InputOutput)
                {
                    parameterValues[i] = (object)(commandParameters[i].Value);
                    //parameterValues[i] = (commandParameters[i].GetType())(commandParameters[i].Value);
                }
            }

            return iReturn;
        }
        else
        {
            // Otherwise we can just call the SP without params
            return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName);
        }
    }


    /// <summary>
    /// Retrieves the set of SqlParameters appropriate for the stored procedure
    /// </summary>
    /// <remarks>
    /// This method will query the database for this information, and then store it in a cache for future requests.
    /// </remarks>
    /// <param name="connectionString">A valid connection string for a SqlConnection</param>
    /// <param name="spName">The name of the stored procedure</param>
    /// <returns>An array of SqlParameters</returns>
    public static SqlParameter[] GetSpParameterSet(string connectionString, string spName)
    {
        return GetSpParameterSet(connectionString, spName, false);
    }

    /// <summary>
    /// Retrieves the set of SqlParameters appropriate for the stored procedure
    /// </summary>
    /// <remarks>
    /// This method will query the database for this information, and then store it in a cache for future requests.
    /// </remarks>
    /// <param name="connectionString">A valid connection string for a SqlConnection</param>
    /// <param name="spName">The name of the stored procedure</param>
    /// <param name="includeReturnValueParameter">A bool value indicating whether the return value parameter should be included in the results</param>
    /// <returns>An array of SqlParameters</returns>
    public static SqlParameter[] GetSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
    {
        if (connectionString == null || connectionString.Length == 0) throw new ArgumentNullException("connectionString");
        if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            return GetSpParameterSetInternal(connection, spName, includeReturnValueParameter);
        }
    }


    /// <summary>
        /// Retrieves the set of SqlParameters appropriate for the stored procedure
        /// </summary>
        /// <param name="connection">A valid SqlConnection object</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="includeReturnValueParameter">A bool value indicating whether the return value parameter should be included in the results</param>
        /// <returns>An array of SqlParameters</returns>
        private static SqlParameter[] GetSpParameterSetInternal(SqlConnection connection, string spName, bool includeReturnValueParameter)
        {
			if( connection == null ) throw new ArgumentNullException( "connection" );
			if( spName == null || spName.Length == 0 ) throw new ArgumentNullException( "spName" );

            string hashKey = connection.ConnectionString + ":" + spName + (includeReturnValueParameter ? ":include ReturnValue Parameter":"");

            SqlParameter[] cachedParameters;
        	
            cachedParameters = paramCache[hashKey] as SqlParameter[];
            if (cachedParameters == null)
            {	
                SqlParameter[] spParameters = DiscoverSpParameterSet(connection, spName, includeReturnValueParameter);
		        paramCache[hashKey] = spParameters;
                cachedParameters = spParameters;
            }
        	
            return CloneParameters(cachedParameters);
        }

        /// <summary>
        /// Resolve at run time the appropriate set of SqlParameters for a stored procedure
        /// </summary>
        /// <param name="connection">A valid SqlConnection object</param>
        /// <param name="spName">The name of the stored procedure</param>
        /// <param name="includeReturnValueParameter">Whether or not to include their return value parameter</param>
        /// <returns>The parameter array discovered.</returns>
        private static SqlParameter[] DiscoverSpParameterSet(SqlConnection connection, string spName, bool includeReturnValueParameter)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (spName == null || spName.Length == 0) throw new ArgumentNullException("spName");

            SqlCommand cmd = new SqlCommand(spName, connection);
            cmd.CommandType = CommandType.StoredProcedure;

            connection.Open();
            SqlCommandBuilder.DeriveParameters(cmd);
            connection.Close();

            if (!includeReturnValueParameter)
            {
                cmd.Parameters.RemoveAt(0);
            }

            SqlParameter[] discoveredParameters = new SqlParameter[cmd.Parameters.Count];

            cmd.Parameters.CopyTo(discoveredParameters, 0);

            // Init the parameters with a DBNull value
            foreach (SqlParameter discoveredParameter in discoveredParameters)
            {
                discoveredParameter.Value = DBNull.Value;
            }
            return discoveredParameters;
        }


        /// <summary>
        /// Deep copy of cached SqlParameter array
        /// </summary>
        /// <param name="originalParameters"></param>
        /// <returns></returns>
        private static SqlParameter[] CloneParameters(SqlParameter[] originalParameters)
        {
            SqlParameter[] clonedParameters = new SqlParameter[originalParameters.Length];

            for (int i = 0, j = originalParameters.Length; i < j; i++)
            {
                clonedParameters[i] = (SqlParameter)((ICloneable)originalParameters[i]).Clone();
            }

            return clonedParameters;
        }
		



    /// <summary>
    /// This method assigns an array of values to an array of SqlParameters
    /// </summary>
    /// <param name="commandParameters">Array of SqlParameters to be assigned values</param>
    /// <param name="parameterValues">Array of objects holding the values to be assigned</param>
    private static void AssignParameterValues(SqlParameter[] commandParameters, object[] parameterValues)
    {
        try
        {
            if ((commandParameters == null) || (parameterValues == null))
            {
                // Do nothing if we get no data
                return;
            }

            // We must have the same number of values as we pave parameters to put them in
            if (commandParameters.Length != parameterValues.Length)
            {
                throw new ArgumentException("Parameter count does not match Parameter Value count.");
            }

            // Iterate through the SqlParameters, assigning the values from the corresponding position in the 
            // value array
            for (int i = 0, j = commandParameters.Length; i < j; i++)
            {
                // If the current array value derives from IDbDataParameter, then assign its Value property
                if (parameterValues[i] is IDbDataParameter)
                {
                    IDbDataParameter paramInstance = (IDbDataParameter)parameterValues[i];
                    if (paramInstance.Value == null)
                    {
                        commandParameters[i].Value = DBNull.Value;
                    }
                    else
                    {
                        commandParameters[i].Value = paramInstance.Value;
                    }
                }
                else if (parameterValues[i] == null)
                {
                    commandParameters[i].Value = DBNull.Value;
                }
                else
                {
                    commandParameters[i].Value = parameterValues[i];
                }
            }
        }
        catch (Exception e)
        {
            /*CrmServiceException ce = new CrmServiceException(
                "Client",
                "ParameterError",
                e.Message, e, "AssignParameterValues");
            throw ce;*/
            e.Message.ToString();
        }
    }



    /// <summary>
    /// Execute a SqlCommand (that returns no resultset) against the database specified in the connection string 
    /// using the provided parameters
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="connectionString">A valid connection string for a SqlConnection</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
    /// <returns>An int representing the number of rows affected by the command</returns>
    public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {

        // Create & open a SqlConnection, and dispose of it after we are done
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            
            // Call the overload that takes a connection in place of the connection string
            return ExecuteNonQuery(connection, commandType, commandText, commandParameters);

        }

    }


    /// <summary>
    /// Execute a SqlCommand (that returns no resultset) against the specified SqlConnection 
    /// using the provided parameters.
    /// </summary>
    /// <remarks>
    /// e.g.:  
    ///  int result = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
    /// </remarks>
    /// <param name="connection">A valid SqlConnection</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">An array of SqlParamters used to execute the command</param>
    /// <returns>An int representing the number of rows affected by the command</returns>
    public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
    {
        //Trace trace = new Trace();
        //TraceState trState = trace.StartProc("SQLHelper.ExecuteNonQuery");
        int retval = 0;
        //result = new Result();
        //resultXml = string.Empty;
        //result.Flag = false;
        try
        {
            if (connection == null) throw new ArgumentNullException("connection");

            // Create a command and prepare it for execution
            SqlCommand cmd = new SqlCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);

            //HiResTimer timer = new HiResTimer();
            //timer.Start();
            // Finally, execute the command
            cmd.CommandTimeout = 0;  //Timeout set as unlimited since this is a long run process
            retval = cmd.ExecuteNonQuery();
            //timer.Stop();
            //result.Flag = true;

            //Check the parameter collection and return back the output paramater if any
            /*if (commandParameters != null)
            {
                if (commandParameters.Length > 0)
                {
                    int outParamIndex = commandParameters.Length - 1;
                    if (commandParameters[outParamIndex].Direction == ParameterDirection.Output || commandParameters[outParamIndex].Direction == ParameterDirection.InputOutput)
                    {
                        retval = Convert.ToInt32(commandParameters[outParamIndex].Value);                        
                    }
                }
            }*/
            //trace.WriteDebug("SQLHelper.ExecuteNonQuery Time=" + timer.ElapsedMilliseconds + " CommandType=" + commandType.ToString() + " CommandText=" + commandText);

            // Detach the SqlParameters from the command object, so they can be used again
            cmd.Parameters.Clear();
        }
        catch (Exception e)
        {
            //HandleSqlExecutionException(e, trace, commandText);
            throw e;
        }
        finally
        {
           // trState.EndProc();
        }
        return retval;
    }


    /// <summary>
    /// This method opens (if necessary) and assigns a connection, transaction, command type and parameters 
    /// to the provided command
    /// </summary>
    /// <param name="command">The SqlCommand to be prepared</param>
    /// <param name="connection">A valid SqlConnection, on which to execute this command</param>
    /// <param name="transaction">A valid SqlTransaction, or 'null'</param>
    /// <param name="commandType">The CommandType (stored procedure, text, etc.)</param>
    /// <param name="commandText">The stored procedure name or T-SQL command</param>
    /// <param name="commandParameters">An array of SqlParameters to be associated with the command or 'null' if no parameters are required</param>
    /// <param name="mustCloseConnection"><c>true</c> if the connection was opened by the method, otherwose is false.</param>
    private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, out bool mustCloseConnection)
    {
        mustCloseConnection = true;
            if (command == null) throw new ArgumentNullException("command");
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");

            // If the provided connection is not open, we will open it
            if (connection.State != ConnectionState.Open)
            {
                mustCloseConnection = true;
                connection.Open();
            }
            else
            {
                mustCloseConnection = false;
            }

            // Associate the connection with the command
            command.Connection = connection;

            // Set the command text (stored procedure name or SQL statement)
            command.CommandText = commandText;

            // If we were provided a transaction, assign it
            if (transaction != null)
            {
                if (transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
                command.Transaction = transaction;
            }

            // Set the command type
            command.CommandType = commandType;

            // Attach the command parameters if they are provided
            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }
            return;
                
    }


    private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
    {
            if (command == null) throw new ArgumentNullException("command");
            if (commandParameters != null)
            {
                foreach (SqlParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        // Check for derived output value with no value assigned
                        if ((p.Direction == ParameterDirection.InputOutput ||
                            p.Direction == ParameterDirection.Input) &&
                            (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }                
    }



}
