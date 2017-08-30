using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.IdentityModel;
using System.IdentityModel.Selectors;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Tesco.Marketing.IT.ClubcardCoupon.AdHocCouponService
{
    /// <summary>
    /// This class is responsible to validate user name and password that is 
    /// passed along with message with Active Directory
    /// </summary>
    public class AdHocCouponUserNamePassValidator : System.IdentityModel.Selectors.UserNamePasswordValidator
    {
        public override void Validate(string userName, string password)
        {
            
            //AD validation is not required as part of project clock 1.6
            //Will check with AD existance of user and password
            //if (CheckUserExist(userName, password) == false)
            //{
            //    Logger.Write("Incorrect Credentials: User = " + userName + " Password = " + password, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
            //    throw new FaultException("Incorrect credentials");
            //}

            bool flag = false;

            AdHocCouponCustomSecurityConfiguration configInfo = (AdHocCouponCustomSecurityConfiguration)System.Configuration.ConfigurationManager.GetSection("CustomSecurityConfiguration");

            userName = CryptoUtil.EncryptTripleDES(userName);
            password = CryptoUtil.EncryptTripleDES(password);

            foreach (RequestorCredentials a in configInfo.RequestorCredentialCollection)
            {
                if ((string.Compare(a.RequestorId, userName, false) == 0) && (string.Compare(a.RequestorPassoword, password, false) == 0))
                {
                    flag = true;
                }
            }
            if (flag == false)
            {
                Logger.Write("Incorrect Credentials: User = " + userName + " Password = " + password, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
                throw new FaultException("Incorrect credentials");
            }
        }

        /// <summary>
        /// Will check the passed credentials with 
        /// configured Active Directory
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private bool CheckUserExist(string userId, string password)
        {
            try
            {
                string domainId = Convert.ToString(ConfigurationManager.AppSettings["DomainId"]);
                //check in active directory if the user exist           
                if (domainId != null)
                {
                    String strLDAPPath = Convert.ToString(ConfigurationManager.AppSettings["LDAPPath"]);
                    String domainAndUserType = domainId + @"\" + userId;
                    DirectoryEntry objDirEntry = new DirectoryEntry(strLDAPPath, domainAndUserType, password);
                    DirectorySearcher search = new DirectorySearcher(objDirEntry);
                    search.Filter = "(SAMAccountName=" + userId + ")";
                    SearchResult result;
                    result = search.FindOne();
                    if (result == null || result.Path == "")
                    {
                        throw new Exception("Unknown UserName and Password");
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
