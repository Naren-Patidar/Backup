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
using System.Security.Cryptography;

namespace Tesco.Marketing.IT.ClubcardCoupon.AdHocCouponService
{
    /// <summary>
    /// This class is to define custom section in web.config
    /// that will hold userid and password for client authentication
    /// </summary>
    public class AdHocCouponCustomSecurityConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("RequestorCredentials")]
        public RequestorCredentialsCollection RequestorCredentialCollection
        {
            get
            {
                return this["RequestorCredentials"] as RequestorCredentialsCollection;
            }
        }

    }

    /// <summary>
    /// Defines configuration elements to be presented in configuration
    /// </summary>
    public class RequestorCredentials : ConfigurationElement
    {
        [ConfigurationProperty("RequestorId", IsRequired = true)]
        public string RequestorId
        {
            get
            {
                return this["RequestorId"] as string;
            }
        }

        [ConfigurationProperty("RequestorPassoword", IsRequired = false)]
        public string RequestorPassoword
        {
            get
            {
                return this["RequestorPassoword"] as string;
            }
        }
    }

    /// <summary>
    /// This class represents collection of RequestorCredentials
    /// </summary>
    public class RequestorCredentialsCollection : ConfigurationElementCollection
    {
        public RequestorCredentials this[int index]
        {
            get
            {
                return base.BaseGet(index) as RequestorCredentials;
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new RequestorCredentials();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RequestorCredentials)element).RequestorId;
        }
    }

    /// <summary>
    /// This class is used for encryption and decryption of config params
    /// </summary>
    public class CryptoUtil
    {
        //24 byte or 192 bit key and IV for TripleDES
        private static byte[] KEY_192 = {
                                            42,16,93,156,78,4,218,32,15,167,44,80,26,250,155,112,2,94,11,204,119,35,184,196
                                        };
        private static byte[] IV_192 = {
                                                55,103,246,79,36,99,167,3,42,5,62,83,184,7,209,13,145,23,200,58,173,10,121,221
                                          };
        //TRIPLE DES encryption
        public static string EncryptTripleDES(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                TripleDESCryptoServiceProvider cryptoProvider = new TripleDESCryptoServiceProvider();
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, cryptoProvider.CreateEncryptor(KEY_192, IV_192), CryptoStreamMode.Write);
                StreamWriter sw = new StreamWriter(cs);
                try
                {
                    sw.Write(value);
                    sw.Flush();
                    cs.FlushFinalBlock();
                    ms.Flush();
                    //convert back to a string
                    return Convert.ToBase64String(ms.GetBuffer(), 0, Convert.ToInt32(ms.Length));
                }
                finally
                {
                    sw.Dispose();
                    cs.Dispose();
                    ms.Dispose();
                }
            }
            else return string.Empty;
        }


        //TRIPLE DES decryption
        public static string DecryptTripleDES(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                TripleDESCryptoServiceProvider cryptoProvider = new TripleDESCryptoServiceProvider();

                //convert from string to byte array
                byte[] buffer = Convert.FromBase64String(value);
                MemoryStream ms = new MemoryStream(buffer);
                CryptoStream cs = new CryptoStream(ms, cryptoProvider.CreateDecryptor(KEY_192, IV_192), CryptoStreamMode.Read);
                StreamReader sr = new StreamReader(cs);
                try
                {
                    return sr.ReadToEnd();
                }
                finally
                {
                    sr.Dispose();
                    cs.Dispose();
                    ms.Dispose();
                }
            }
            else return string.Empty;
        }
    }
}
