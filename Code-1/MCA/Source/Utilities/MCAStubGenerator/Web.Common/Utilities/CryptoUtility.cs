using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Utilities
{
    /// <summary>
    /// CryptoUtil has static functionality to TripleDES encryption and decryption
    /// <para>using 24 byte key</para>
    /// <para>Author: Padmanabh Ganorkar</para>
    /// <para>26/06/2010</para>
    /// </summary>
    public class CryptoUtility
    {
        //24 byte or 192 bit key and IV for TripleDES
        private static byte[] KEY_192 = {
                                            42,16,93,156,78,4,218,32,15,167,44,80,26,250,155,112,2,94,11,204,119,35,184,197
                                        };
        private static byte[] IV_192 = {
		                                    55,103,246,79,36,99,167,3,42,5,62,83,184,7,209,13,145,23,200,58,173,10,121,222
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
