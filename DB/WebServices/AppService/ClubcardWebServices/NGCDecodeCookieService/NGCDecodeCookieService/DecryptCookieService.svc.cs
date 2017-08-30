using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Security;
using System.Web;
using System.Web.Security;


#region Tesco Namaespace
using NGCTrace;
using Tesco.NGC.Utils;
using Microsoft.Practices.EnterpriseLibrary.Logging;
#endregion

namespace Tesco.com.NGCDecodeCookieService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DecryptCookieService" in code, svc and config file together.
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.PerCall,ConcurrencyMode=ConcurrencyMode.Multiple)]
    public class DecryptCookieService : IDecryptCookieService
    {

        #region GetDecodedCookie
        /// <summary>
        /// This method is used to get the decoded value for the encoded value what is passed to  Custobj object
        /// </summary>
        /// <param name="Custobj"></param>
        /// <returns></returns>
        public CustomerIdentity GetDecodedCookie(CustomerIdentity Custobj)
        {
            CustomerIdentity custResponse = new CustomerIdentity();
            string strEncdoded = Custobj.Encodedvalue;
             try
            {
                NGCTrace.NGCTrace.TraceInfo("Start:NGCDecodeCookieService.DecryptCookieService.GetDecodedCookie  CookieValue " + Custobj.Encodedvalue);
                NGCTrace.NGCTrace.TraceDebug("Start:NGCDecodeCookieService.DecryptCookieService.GetDecodedCookie CookieValue " + Custobj.Encodedvalue);
                if (!string.IsNullOrEmpty(strEncdoded))
                {
                    //Following code is to ensure that we get the DecodedValue for the Encodedvalue input been passed
                    custResponse.DecodedValue = Encoding.Unicode.GetString(MachineKey.Decode(strEncdoded, MachineKeyProtection.All));
                }
                NGCTrace.NGCTrace.TraceInfo("Start:NGCDecodeCookieService.DecryptCookieService.GetDecodedCookie  CookieValue" + Custobj.DecodedValue);
                NGCTrace.NGCTrace.TraceDebug("Start:NGCDecodeCookieService.DecryptCookieService.GetDecodedCookie CookieValue" + Custobj.DecodedValue);
            }
            catch (Exception ex)
            {
                custResponse.Error = ex.ToString();
                NGCTrace.NGCTrace.TraceCritical("Critical:NGCDecodeCookieService.DecryptCookieService.GetDecodedCookie ErorrXml:" + ex.ToString());
                NGCTrace.NGCTrace.TraceError("Error:NGCDecodeCookieService.DecryptCookieService.GetDecodedCookie ErorrXml:" + ex.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning:NGCDecodeCookieService.DecryptCookieService.GetDecodedCookie");
                NGCTrace.NGCTrace.ExeptionHandling(ex);
            }
            return custResponse;
        }
        #endregion

    }
}
