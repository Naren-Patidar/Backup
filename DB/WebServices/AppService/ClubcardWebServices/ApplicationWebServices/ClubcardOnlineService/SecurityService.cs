using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Fujitsu.eCrm.Generic.SharedUtils;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using USLoyaltySecurityServiceLayer; 

namespace Tesco.com.ClubcardOnlineService
{
    
    // NOTE: If you change the class name "SecurityService" here, you must also update the reference to "SecurityService" in App.config.
    public class SecurityService : ISecurityService
    {
        USLoyaltySecurityServiceLayer.SecurityService objSecurityService = null;
        
        public bool CreateUser(string objectXml, out string UserStatus, out Int64 customerID)
        {   
            
            bool bResult = false;
            string usercreateStatus=string.Empty;
            Int64 customerNum=0;
            UserStatus = string.Empty;       
            customerID = 0;

            #region Trace
            Trace trace = new Trace();
            ITraceState trState = trace.StartProc("SecurityService.CreateUser");
            StringBuilder sb = new StringBuilder();
            sb.Append(" Objectxml: " + objectXml);            
            trace.WriteInfo(sb.ToString());
            #endregion

            try
            {

                bResult = objSecurityService.CreateUser(objectXml, out usercreateStatus);
                UserStatus = usercreateStatus;
                customerID = customerNum;

            }
            catch (Exception ex)
            {
                Logger.Write(ex, "General", 1, 6500, System.Diagnostics.TraceEventType.Error, "objectXml :" + objectXml);
                bResult = false;
            }
            finally
            {
                
                trState.EndProc();
            }
            return bResult;
 
        }
     
    }
}
