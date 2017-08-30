using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Data;
using System.Web;
using System.Configuration;
using System.Web.Services;
using System.Threading;
using System.Web.Services.Protocols;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using Tesco.NGC.Security;
using Tesco.NGC.Utils;

namespace Tesco.NGC.NGCWebService
{
    /// <summary>
    /// Summary description for Service
    /// </summary>
    [WebService(Namespace = "urn:Tesco.NGC.NGCWebService", Description = "Supports Customer Relationship Management via Web Services")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class Service : System.Web.Services.WebService
    {
        private static ReaderWriterLock rwLock = new ReaderWriterLock();
        string ApplicationName = ConfigurationSettings.AppSettings["ApplicationName"];
        public Service()
        {
            //CODEGEN: This call is required by the ASP.NET Web Services Designer
            InitializeComponent();
        }

        #region Component Designer generated code

        //Required by the Web Services Designer 
        private IContainer components = null;

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {

        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Close
        /// <summary>
        /// Close all resources tidily.
        /// </summary>
        public static void Close()
        {

            // Close the sessionTable, especially its thread
            try
            {
                Global.sessionTable.Dispose();
            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e);
            }

            // Now complete by calling the Trace close.
            Trace.Close();
        }
        #endregion

        #region Ping
        [WebMethod(Description = "Test Server is available")]
        public bool Ping()
        {
            bool success = false;
            try
            {
                Int32 WaitInterval = Convert.ToInt32(ConfigurationSettings.AppSettings["WaitInterval"]);
                rwLock.AcquireReaderLock(WaitInterval);
                success = true;
            }
            catch
            {
                success = false;
            }
            finally
            {
                rwLock.ReleaseReaderLock();
            }
            return success;
        }
        #endregion

        #region Connect
        [WebMethod(Description = "Authenticate User's.  All user's must connect to access any other services")]
        public bool Connect(
            string userName,
            string password,
            string culture,
            string AppName,
            out string sessionId,
            out string capabilityXml,
            out string resultXml)
        {

            Trace trace = new Trace();
            sessionId = String.Empty;
            capabilityXml = String.Empty;
            resultXml = String.Empty;

            TraceState trState = trace.StartProc("WebService.Connect UserName=" + userName);
            Result result = new Result();

            try
            {
                Int32 WaitInterval = Convert.ToInt32(ConfigurationSettings.AppSettings["WaitInterval"]);
                rwLock.AcquireReaderLock(WaitInterval);

                IsInitialised(String.Empty, userName, password, culture);
                sessionId = Global.sessionTable.Add(trace, userName, password, culture, AppName, out capabilityXml);
                Global.sessionTable.Validate(trace, sessionId, "Connect");
            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                result.Add(e);
                resultXml = result.OuterXml;
                sessionId = Guid.NewGuid().ToString();
                capabilityXml = String.Empty;
            }
            finally
            {
                rwLock.ReleaseReaderLock();
            }
            trState.EndProc();
            return result.Flag;
        }

        /// <summary>
        /// Check All incoming parameters of public services aren't null
        /// </summary>
        /// <param name="paramValueList">The list of parameters to check</param>
        protected static void IsInitialised(params object[] paramValueList)
        {
            for (int i = paramValueList.GetLowerBound(0); i <= paramValueList.GetUpperBound(0); i++)
            {
                if (paramValueList[i] == null)
                {
                    System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
                    MethodBase method = stackTrace.GetFrame(1).GetMethod();
                    string methodName = method.Name;
                    string parameterName = method.GetParameters()[i].Name;
                    CrmServiceException ce = new CrmServiceException(
                        "Client",
                        "ParameterError",
                        "NullParameter", methodName, parameterName);
                    throw ce;
                }
            }
        }

        #endregion

        #region Disconnect
        [WebMethod(Description = "End user's connection.  Will not be able to use any further services, until user calls Connect")]
        public bool Disconnect(
            string sessionId,
            out string resultXml)
        {

            Trace trace = new Trace();
            resultXml = String.Empty;
            TraceState trState = trace.StartProc("NGCWebService.Disconnect SessionId=" + sessionId);
            Result result = new Result();

            try
            {
                rwLock.AcquireReaderLock(Convert.ToInt32(ConfigurationSettings.AppSettings["WaitInterval"]));
                IsInitialised(sessionId);
                Global.sessionTable.Validate(trace, sessionId, "Disconnect");
                Global.sessionTable.Remove(sessionId);
            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                result.Add(e);
            }
            finally
            {
                rwLock.ReleaseReaderLock();
            }

            resultXml = result.OuterXml;
            trState.EndProc();
            return result.Flag;
        }
        #endregion

        #region InitialiseServer
        [WebMethod(Description = "Initialise the Server.  Required to complete reconfiguration of the CRM Server")]
        public bool InitialiseServer(
            string sessionId,
            out string resultXml)
        {

            Trace trace = new Trace();
            TraceState trState = trace.StartProc("NGCWebService.InitialiseServer");
            Result result = new Result();
            resultXml = string.Empty;

            try
            {
                rwLock.AcquireWriterLock(Convert.ToInt32(ConfigurationSettings.AppSettings["WaitInterval"]));
                IsInitialised(sessionId);
                Global.sessionTable.Validate(trace, sessionId, "InitialiseServer");
            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                result.Add(e);
            }
            finally
            {
                rwLock.ReleaseWriterLock();
            }

            resultXml = result.OuterXml;
            trState.EndProc();
            return result.Flag;
        }
        #endregion

        #region Get
        [WebMethod(Description = "Request some CRM data based on a key")]
        public bool Get(
            string sessionId,
            string objName,
            string methodName,
            long viewObjectId,
            out string resultXml,
            out string viewXml)
        {

            Trace trace = new Trace();
            TraceState trState = trace.StartProc("WebService.Get");

            viewXml = String.Empty;
            resultXml = String.Empty;
            Result result = new Result();
            Assembly asm;
            String typeName;
            try
            {
                rwLock.AcquireReaderLock(Convert.ToInt32(ConfigurationSettings.AppSettings["WaitInterval"]));
                IsInitialised(sessionId, objName, methodName, viewObjectId);
                Session currentSession = Global.sessionTable.Validate(trace, sessionId, "Get", objName, methodName);
                if (currentSession.AppName.ToString().ToUpper() == ApplicationName.ToUpper())
                {
                    asm = System.Reflection.Assembly.LoadFrom(ConfigurationSettings.AppSettings["CampaignEntityServiceLayerPath"]);
                    typeName = "Tesco.NGC.Campaign.EntityServiceLayer." + objName;
                }
                else
                {
                    asm = System.Reflection.Assembly.LoadFrom(ConfigurationSettings.AppSettings["LoyaltyEntityServiceLayerPath"]);
                    typeName = "Tesco.NGC.Loyalty.EntityServiceLayer." + objName;                    
                }
                //String typeName = "Tesco.NGC.BusinessLayer." + objName;
                Type myType = asm.GetType(typeName);
                object obj = Activator.CreateInstance(myType);
                MethodInfo mi = myType.GetMethod(methodName);

                object[] arrParms = { viewObjectId, currentSession.Culture };
                viewXml = (string)mi.Invoke(obj, arrParms);
            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                result.Add(e);
            }
            finally
            {
                rwLock.ReleaseReaderLock();
            }

            resultXml = result.OuterXml;
            trState.EndProc();
            return result.Flag;

        }
        #endregion

        #region Find
        [WebMethod(Description = "Request some CRM data based on some values")]
        public bool Find(
            string sessionId,
            string objName,
            string methodName,
            string conditionXml,
            int maxRowCount,
            out string resultXml,
            out string viewXml,
            out int rowCount
            )
        {

            Trace trace = new Trace();
            TraceState trState = trace.StartProc("WebService.Get");

            viewXml = String.Empty;
            resultXml = String.Empty;
            Result result = new Result();
            rowCount = 0;
            Assembly asm;
            String typeName;
            try
            {
                rwLock.AcquireReaderLock(Convert.ToInt32(ConfigurationSettings.AppSettings["WaitInterval"]));
                IsInitialised(sessionId, objName, methodName, maxRowCount, conditionXml);
                Session currentSession = Global.sessionTable.Validate(trace, sessionId, "Get", objName, methodName);
                if (currentSession.AppName.ToString().ToUpper() == ApplicationName.ToUpper())
                {
                    asm = System.Reflection.Assembly.LoadFrom(ConfigurationSettings.AppSettings["CampaignEntityServiceLayerPath"]);
                    typeName = "Tesco.NGC.Campaign.EntityServiceLayer." + objName;
                }
                else
                {
                    asm = System.Reflection.Assembly.LoadFrom(ConfigurationSettings.AppSettings["LoyaltyEntityServiceLayerPath"]);
                    typeName = "Tesco.NGC.Loyalty.EntityServiceLayer." + objName;                    
                }
                //Assembly asm = System.Reflection.Assembly.LoadFrom(ConfigurationSettings.AppSettings["NGCBusinessLayerPath"]);
                //String typeName = "Tesco.NGC.BusinessLayer." + objName;
                Type myType = asm.GetType(typeName);
                object obj = Activator.CreateInstance(myType);
                MethodInfo mi = myType.GetMethod(methodName);

                object[] arrParms = { conditionXml, maxRowCount, rowCount, currentSession.Culture };
                viewXml = (string)mi.Invoke(obj, arrParms);
                rowCount = (int)arrParms[2];
            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                result.Add(e);
            }
            resultXml = result.OuterXml;
            trState.EndProc();
            return result.Flag;
        }
        #endregion

        #region Set
        [WebMethod(Description = "Update, Add and/or Delete NGC data")]
        public bool Set(
            string sessionId,
            string objName,
            string methodName,
            string objectXml,
            out string resultXml,
            out long objectId)
        {

            Trace trace = new Trace();
            TraceState trState = trace.StartProc("WebService.Get");
            Result result = new Result();
            objectId = 0;
            resultXml = String.Empty;
            bool success = false;
            Assembly asm;
            String typeName;
            try
            {
                rwLock.AcquireReaderLock(Convert.ToInt32(ConfigurationSettings.AppSettings["WaitInterval"]));
                IsInitialised(sessionId, objName, methodName, objectId);
                Session currentSession = Global.sessionTable.Validate(trace, sessionId, "Get", objName, methodName);
                if (currentSession.AppName.ToString().ToUpper() == ApplicationName.ToUpper())
                {
                    asm = System.Reflection.Assembly.LoadFrom(ConfigurationSettings.AppSettings["CampaignEntityServiceLayerPath"]);
                    typeName = "Tesco.NGC.Campaign.EntityServiceLayer." + objName;
                }
                else
                {
                    asm = System.Reflection.Assembly.LoadFrom(ConfigurationSettings.AppSettings["LoyaltyEntityServiceLayerPath"]);
                    typeName = "Tesco.NGC.Loyalty.EntityServiceLayer." + objName;
                }
                //Assembly asm = System.Reflection.Assembly.LoadFrom(ConfigurationSettings.AppSettings["NGCBusinessLayerPath"]);
                //String typeName = "Tesco.NGC.BusinessLayer." + objName;
                Type myType = asm.GetType(typeName);
                object obj = Activator.CreateInstance(myType);
                MethodInfo mi = myType.GetMethod(methodName);

                object[] arrParms = { objectXml, currentSession.UserId, objectId, resultXml };

                success = (bool)mi.Invoke(obj, arrParms);
                objectId = (long)arrParms[2];
                resultXml = (string)arrParms[3];
            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                result.Add(e);
                resultXml = result.OuterXml;
            }
            trState.EndProc();
            return success;
        }
        #endregion

        #region Get
        //Get the connection string from Web Server. Done for UK Changes
        [WebMethod(Description = "Request some CRM data based on a key")]
        public bool GetConnectionString(            
            string objName,
            string methodName,
            long viewObjectId,
            out string resultXml,
            out string viewXml)
        {

            Trace trace = new Trace();
            TraceState trState = trace.StartProc("WebService.Get");

            viewXml = String.Empty;
            resultXml = String.Empty;
            Result result = new Result();
            Assembly asm;
            String typeName;
            try
            {
                rwLock.AcquireReaderLock(Convert.ToInt32(ConfigurationSettings.AppSettings["WaitInterval"]));
                IsInitialised("", objName, methodName, viewObjectId);
              
                asm = System.Reflection.Assembly.LoadFrom(ConfigurationSettings.AppSettings["LoyaltyEntityServiceLayerPath"]);
                typeName = "Tesco.NGC.Loyalty.EntityServiceLayer." + objName;
                
                //String typeName = "Tesco.NGC.BusinessLayer." + objName;
                Type myType = asm.GetType(typeName);
                object obj = Activator.CreateInstance(myType);
                MethodInfo mi = myType.GetMethod(methodName);

                object[] arrParms = { viewObjectId, "en-GB"};
                viewXml = (string)mi.Invoke(obj, arrParms);
            }
            catch (Exception e)
            {
                ExceptionManager.Publish(e);
                result.Add(e);
            }
            finally
            {
                rwLock.ReleaseReaderLock();
            }

            resultXml = result.OuterXml;
            trState.EndProc();
            return result.Flag;

        }
        #endregion
    }
}
