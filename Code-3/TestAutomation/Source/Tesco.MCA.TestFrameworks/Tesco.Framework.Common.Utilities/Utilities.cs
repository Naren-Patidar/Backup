using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Mail;
using Tesco.Framework.Common.Logging.Logger;
using System.Configuration;
using System.Collections.Specialized;
using Tesco.Framework.Common.Utilities.Entity;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Web.Script.Serialization;
using System.Runtime.Serialization.Json;

namespace Tesco.Framework.Common.Utilities
{
    public static class Utilities
    {

        #region Private Members
        private static Dictionary<string, string> _environmentSettings = null;        
        private static int _implicitTimeOut = 0;
        private static int _explicitTimeOut = 0;
        private static int _pageLoadWaitTime = 0;
        private static int _portNumber;
        #endregion

        #region Public Members
        public static string utilError = string.Empty;      
        #endregion

        #region Properties

        public static int PortNumber
        { 
            get { return _portNumber; } 
            set { _portNumber = value; } 
        }

        public static Dictionary<string, string> EnvironmentSettings
        {
            get
            {
                return _environmentSettings;
            }
            set
            {
                _environmentSettings = value;
            }
        }        

        public static int ImplicitTimeOut
        {
            get
            {
                return _implicitTimeOut;
            }
            set 
            {
                value = _implicitTimeOut;
            }
        }

        public static int ExplicitTimeOut
        {
            get
            {
                return _explicitTimeOut;
            }
            set
            {
                value = _explicitTimeOut;
            }
        }

        public static int PageLoadWaitTime
        {
            get
            {
                return _pageLoadWaitTime;
            }
            set
            {
                value = _pageLoadWaitTime;
            }
        }
        #endregion

        
        /// <summary>
        /// Private Static Contructor
        /// </summary>
        static Utilities()
        {
            _environmentSettings = GetAllEnvironmentSpecificConfigValues();            
            _portNumber = 1000;

            _implicitTimeOut = Convert.ToInt32(GetConfigValue("ImplicitTimeOut"));
            _explicitTimeOut = Convert.ToInt32(GetConfigValue("ExplicitTimeOut"));
            _pageLoadWaitTime = Convert.ToInt32(GetConfigValue("PageLoadWaitTime"));
        }

        /// <summary>
        /// Provides an HTTP-specific WebResponse based as per GET/POST request
        /// </summary>
        /// <param name="authenticationToken"></param>
        /// <param name="baseUrl"></param>
        /// <param name="parameters"></param>
        /// <param name="contentType"></param>
        /// <param name="host"></param>
        /// <param name="requestBody"></param>
        /// <param name="requestMethod"></param>
        /// <returns></returns>
        public static WebResponse GetWebResponse(string authenticationToken, string baseUrl, string parameters, string contentType, string host, string requestBody, RequestMethod requestMethod)
        {
            WebResponse response = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseUrl + parameters);
                request.Method = requestMethod.ToString();

                if (host != null && host.Trim() != "")
                    request.Host = host;

                if (authenticationToken != null && authenticationToken.Trim() != "")
                    request.Headers.Add(HttpRequestHeader.Authorization, authenticationToken);

                if (contentType != null && contentType != "")
                    request.ContentType = contentType;

                byte[] byteArray = Encoding.UTF8.GetBytes(requestBody);
                request.ContentLength = byteArray.Length;

                if (request.Method == RequestMethod.POST.ToString())
                {
                    Stream dataStream = request.GetRequestStream();
                    // Write the data to the request stream.
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    // Close the Stream object.
                    dataStream.Close();
                }

                response = request.GetResponse();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }

        /// <summary>
        /// Methodto send email
        /// </summary>
        /// <param name="SmtpDetails"></param>
        /// <param name="emailMessage"></param>
        public static void SendMail(SMTPDetails SmtpDetails, EMailMessage emailMessage, Attachment[] attachments = null)
        {
            try
            {
                SmtpClient SmtpServer = new SmtpClient(SmtpDetails.SmtpHost);
                SmtpServer.Port = SmtpDetails.SmtpPort;
                SmtpServer.Credentials = new System.Net.NetworkCredential(SmtpDetails.SmtpUserName, SmtpDetails.SmtpPassword, SmtpDetails.SmtpDomain);
                SmtpServer.EnableSsl = SmtpDetails.IsSSLEnabled;

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(emailMessage.MailFrom);

                foreach(MailAddress mailTo in emailMessage.MailTo)
                {
                    mail.To.Add(mailTo);
                }

                if (attachments != null)
                {
                    foreach (Attachment attachment in attachments)
                    {
                        mail.Attachments.Add(attachment);
                    }
                }

                mail.Subject = emailMessage.Subject;
                mail.IsBodyHtml = emailMessage.IsBodyHtml;
                mail.Body = emailMessage.Body;

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Method to initialize logger object (logging)
        /// </summary>
        /// <param name="logger"></param>
        public static void InitializeLogger(ref ILogger logger, AppenderType appender = AppenderType.UNITTEST)
        {
            try
            {
                var fullAppenderType = appender.ToString() + "_" + DateTime.Now.ToLocalTime().ToString("dd-MM-yyyy-HH-mm-ss");
                LoggingFactory.InitializeLogFactory(Log4NetAdapter.SetInstance(fullAppenderType));                    

                logger = LoggingFactory.GetLogger();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to fetch the key value from app.config file
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfigValue(string key)
        {
            string configValue = string.Empty;

            try
            {
                configValue = ConfigurationManager.AppSettings[key];
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return configValue;
        }

        /// <summary>
        /// Method to fetch the key value for a particular environment
        /// </summary>       
        /// <param name="key"></param>
        /// <returns></returns>
        private static string GetEnvironmentSpecificConfigValue(string key)
        {
            string environment = Utilities.GetConfigValue("Environment");
            string value = string.Empty;
            string utilError = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(environment))
                {
                    NameValueCollection keyCollection = ConfigurationManager.GetSection(environment) as NameValueCollection;
                    if (keyCollection != null)
                    {
                        value = keyCollection[key].ToString();
                    }
                    else
                    {
                        utilError = "Either the value for Environment key is invalid Or there are no settings configured";
                    }
                }
                else
                {
                    utilError = "Environment key is not configured in app.config";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return value;
        }

        /// <summary>
        /// Method to fetch all the key value pairs for a particular environment
        /// </summary>        
        /// <returns></returns>
        private static Dictionary<string, string> GetAllEnvironmentSpecificConfigValues()
        {
            string environment = Utilities.GetConfigValue("Environment");
            Dictionary<string, string> configPairs = new Dictionary<string, string>();

            #region XML PATHS

            string configXmlPath = Utilities.GetConfigValue("ConfigXmlPath");
            string fullXmlPath = configXmlPath +"\\" + environment + ".xml";
            //string automationXmlPath = configXmlPath + "Automation.xml";

            #endregion

            try
            {
                if (!string.IsNullOrEmpty(environment))
                {
                    if (!(File.Exists(fullXmlPath)))
                    {
                        utilError = "Either the value for Environment key is invalid or Environment XML file is missing";
                    }
                    //else if (!(File.Exists(automationXmlPath)))
                    //{
                    //    utilError = "Automation XML file is missing";
                    //}
                    else
                    {
                        #region READ ENVIRONMENT XML

                        var envXml = XDocument.Load(fullXmlPath);
                        var rootNodes = envXml.Root.DescendantNodes().OfType<XElement>();

                        if (rootNodes != null)
                        {
                            configPairs = rootNodes.ToDictionary(n => n.Name.ToString(), n => n.Value);
                        }

                        #endregion

                        //#region READ AUTOMATION XML

                        //var automationXml = XDocument.Load(automationXmlPath);
                        //var roots = automationXml.Root.DescendantNodes().OfType<XElement>();

                        //if (roots != null)
                        //{
                        //    foreach(XElement node in roots)
                        //    {
                        //        configPairs.Add(node.Name.ToString(), node.Value);
                        //    }
                        //}

                        //#endregion
                    }
                }
                else
                {
                    utilError = "Environment key is not configured in app.config";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return configPairs;
        }

        /// <summary>
        /// Method to Read and Deserialize XmlData into Desired Class Object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlPath"></param>
        /// <returns></returns>
        public static T PopulateListOfXmlData<T>(string xmlPath)
        {
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(T));
                TextReader reader = new StreamReader(xmlPath);

                object xmlData = deserializer.Deserialize(reader);

                var deserializedData = (T)xmlData;
                reader.Close();

                return deserializedData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Deserialize Json Response to Desired Object using JavaScriptSerializer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jSon"></param>
        /// <returns></returns>
        public static T DeserializeResponseJson<T>(string jSon)
        {
            try
            {
                return new JavaScriptSerializer().Deserialize<T>(jSon);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Deserialize Json Response to Desired Object using DataContractJsonSerializer
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static T JsonDeserialize<T>(string jsonString)
        {
            try
            {
                DataContractJsonSerializer deSerializer = new DataContractJsonSerializer(typeof(T));
                MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                T obj = (T)deSerializer.ReadObject(stream);
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to Extract Data from Web Response
        /// </summary>
        /// <param name="wbResponse"></param>
        /// <returns></returns>
        public static string ExtractDataFromWebResponse(WebResponse wbResponse)
        {
            string data = string.Empty;
            
            try
            {
                //If the response is not empty
                if (wbResponse.ContentLength > 0)
                {
                    //Fetch and Read the response
                    Stream responseStream = wbResponse.GetResponseStream();
                    StreamReader streamReader = new StreamReader(responseStream);
                    data = streamReader.ReadToEnd();
                }

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        /// <summary>
        /// Method to Extract Response Headers from Web Response
        /// </summary>
        /// <param name="wbResponse"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ExtractResponseHeaders(WebResponse wbResponse)
        {
            try
            {
                //Dictionary which shall contain the header information
                Dictionary<string, string> headers = new Dictionary<string, string>();

                //If the response is not empty
                if (wbResponse.ContentLength > 0)
                {
                    //Read all headers from the response
                    foreach (string headerItem in wbResponse.Headers)
                    {
                        //Add header to the dictionary
                        headers.Add(headerItem, wbResponse.Headers[headerItem]);
                    }

                    //Close the WebResponse
                    wbResponse.Close();
                }                

                //return the Header Information
                return headers;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
