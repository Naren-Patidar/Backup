using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.IO;
using ServiceUtility;
using System.Xml.Serialization;
using System.Text;

namespace NGCUtilityService
{
    public class UtilityServiceProvider
    {
        Helper helper = new Helper();
        #region ServiceMethods
        public bool ProfanityCheck(out string errorXml, out string resultXml, out int rowCount, string conditionXml)
        {

            bool chk = true;
            string xmlFile = string.Empty;
            rowCount = 0;
            resultXml = string.Empty;
            errorXml = string.Empty;
            string strCustID = string.Empty;
            string fileName = string.Empty;
            XmlDocument response = new XmlDocument();
            try
            {
                XmlDocument doc = new XmlDocument();
                if (conditionXml != null)
                {
                    xmlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"DataSource/WordList.xml"));
                    List<string> profaneWords = conditionXml.Split(',').ToList<string>();
                    foreach (string userInput in profaneWords)
                    {
                        chk = IsBadWord(userInput, xmlFile);
                        if (chk)
                        {
                            fileName = string.Format(@"DataSource\ProfanityCheckTrue.xml");
                            fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
                            if (File.Exists(fileName))
                            {
                                response = helper.LoadXMLDoc(fileName);
                            }

                            XmlNode root = response.DocumentElement;

                            // create ns manager
                            XmlNamespaceManager xmlnsManager = new XmlNamespaceManager(response.NameTable);
                            xmlnsManager.AddNamespace("def", "http://tempuri.org/");


                            XmlNode resultNode = response.SelectSingleNode("//def:resultXml", xmlnsManager);
                            resultXml = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                            resultXml = resultXml.Replace("xmlns=\"http://tempuri.org/\"", "");
                            XmlNode errorNode = response.SelectSingleNode("//def:errorXml", xmlnsManager);
                            errorXml = (errorNode != null) ? errorNode.InnerXml : string.Empty;

                            XmlNode rowCountNode = response.SelectSingleNode("//def:rowCount", xmlnsManager);
                            rowCount = (rowCountNode != null) ? Int32.TryParse(rowCountNode.InnerXml, out rowCount) ? rowCount : 0 : -1;
                            return true;
                        }

                    }
                    if (!chk)
                    {
                        fileName = string.Format(@"DataSource\ProfanityCheckFalse.xml");

                        fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
                        if (File.Exists(fileName))
                        {
                            response = helper.LoadXMLDoc(fileName);
                        }

                        XmlNode root = response.DocumentElement;

                        // create ns manager
                        XmlNamespaceManager xmlnsManager = new XmlNamespaceManager(response.NameTable);
                        xmlnsManager.AddNamespace("def", "http://tempuri.org/");


                        XmlNode resultNode = response.SelectSingleNode("//def:resultXml", xmlnsManager);
                        resultXml = (resultNode != null) ? resultNode.InnerXml : string.Empty;
                        resultXml = resultXml.Replace("xmlns=\"http://tempuri.org/\"", "");
                        XmlNode errorNode = response.SelectSingleNode("//def:errorXml", xmlnsManager);
                        errorXml = (errorNode != null) ? errorNode.InnerXml : string.Empty;

                        XmlNode rowCountNode = response.SelectSingleNode("//def:rowCount", xmlnsManager);
                        rowCount = (rowCountNode != null) ? Int32.TryParse(rowCountNode.InnerXml, out rowCount) ? rowCount : 0 : -1;
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                resultXml = string.Format("{0} |Details: {1}", ex.Message, ex.StackTrace);
                chk = false;
            }
            return chk;

        }

        public bool IsBadWord( string word, string file)
        {
            bool isBadWord = false;
            try
            {
                List<string> badWords = BadWordList(file);

                for (int i = 0; i < badWords.Count; i++)
                {
                    if (word.Trim().ToLower() == badWords[i].Trim().ToLower())
                    {
                        isBadWord = true;
                        return isBadWord;
                    }
                        
                }
            }
            catch (Exception ex)
            {
                isBadWord = false;
                throw ex;
               
            }
            return isBadWord;

        }

        #endregion ServiceMethods


        public  List<string> BadWordList(string file)
        {
            try
            {
                List<string> words = new List<string>();
                XmlDocument xmlDoc = new XmlDocument();
                string query = "/WordList/word";
                xmlDoc.Load(file);

                foreach (XmlNode node in xmlDoc.SelectNodes(query))
                {
                    words.Add(node.ChildNodes[0].InnerText);
                }
                return words;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}