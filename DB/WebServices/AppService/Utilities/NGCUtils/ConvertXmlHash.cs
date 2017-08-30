/*
 * File   : ConvertXmlHash.cs
 * Author : Harshal VP (HSC) 
 * email  :
 * File   : This file contains methods for converting XML to HashTable and Vice Versa
 * Date   : 04/Aug/2008
 * 
 */

#region using

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Collections;

#endregion

namespace Tesco.NGC.Utils
{
    /// <summary>
    /// Convert XML to HashTable and Vice Versa
    /// </summary>
    public static class ConvertXmlHash
    {
        #region HashTable to XML

        /// <summary>Convert HashTable to XML</summary>
        /// <param name="ht"> HashTable to convert into XML </param>
        /// <param name="objName"> Name of the object </param>
        /// <returns> Returns XML </returns>
        /// <remarks>This method accepts a HashTable converts into XML and returning the XML in the form of string</remarks>

        public static string HashTableToXML(Hashtable ht, string objName)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                using (XmlWriter writer = XmlWriter.Create(sb))
                {
                    writer.WriteStartElement(objName);
                    foreach (DictionaryEntry item in ht)
                    {
                        writer.WriteStartElement(item.Key.ToString());
                        writer.WriteValue(item.Value);
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    writer.Flush();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return Convert.ToString(sb);
        }
        #endregion

        public static XmlDocument XmlDocToGridXmlDoc(XmlDocument xmlDoc, string rootElement, string childElement)
        {
            XmlDocument gridXmlDoc = new XmlDocument();
            try
            {
                bool setAttribute;
                XmlNodeList nodes = xmlDoc.SelectNodes(rootElement);

                XmlElement root = gridXmlDoc.CreateElement(rootElement);
                gridXmlDoc.AppendChild(root);

                foreach (XmlNode node in nodes)
                {
                    foreach (XmlElement element in node)
                    {
                        setAttribute = false;
                        XmlElement elem = gridXmlDoc.CreateElement(element.Name);
                        foreach (XmlElement childElem in element)
                        {
                            if (!setAttribute)
                            {
                                elem.SetAttribute(childElem.Name, childElem.InnerText);
                                setAttribute = true;
                                continue;
                            }

                            XmlElement elem2 = gridXmlDoc.CreateElement(childElem.Name);
                            elem2.InnerText = childElem.InnerText;
                            elem.AppendChild(elem2);
                        }
                        root.AppendChild(elem);

                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return gridXmlDoc;
        }

        #region XML to HashTable

        /// <summary>Convert XML to HashTable</summary>
        /// <param name="sXml"> XML data to convert into HashTable </param>
        /// <param name="objName"> Name of the node to search </param>
        /// <returns> Returns HashTable</returns>
        /// <remarks>This method accepts XML data in string format and converts into HashTable and returning the HashTable</remarks>
        public static Hashtable XMLToHashTable(string sXml, string nodeNametoSearch)
        {
            Hashtable ht = new Hashtable();
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(sXml);

                XmlNodeList nodes = doc.SelectNodes(nodeNametoSearch);
                foreach (XmlNode node in nodes)
                {
                    for (Int32 i = 0; i < node.ChildNodes.Count; i++)
                    {
                        if (node.ChildNodes.Item(i).NodeType != XmlNodeType.Text)
                        {
                            if (node.ChildNodes.Item(i).ChildNodes.Count > 1) //&& node.ChildNodes.Item(i).NodeType != XmlNodeType.Document )
                            {
                                HandleChildNodes(ht, node.SelectNodes(node.ChildNodes.Item(i).Name));
                            }
                            else
                            {
                                ht.Add(node.ChildNodes.Item(i).Name, node.ChildNodes.Item(i).InnerText);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return ht;
        }

        private static void HandleChildNodes(Hashtable ht, XmlNodeList nodeList)
        {
            foreach (XmlNode node in nodeList)
            {
                for (Int32 i = 0; i < node.ChildNodes.Count; i++)
                {
                    if (node.ChildNodes.Item(i).NodeType != XmlNodeType.Text)
                    {
                        if (node.ChildNodes.Item(i).ChildNodes.Count > 1)
                        {
                            HandleChildNodes(ht, node.SelectNodes(node.ChildNodes.Item(i).Name));
                        }
                        else
                        {
                            ht.Add(node.ChildNodes.Item(i).Name, node.ChildNodes.Item(i).InnerText);
                        }
                    }
                }
            }
        }

        #endregion

        #region XML to Array Of HashTable

        /// <summary>Convert XML to HashTable</summary>
        /// <param name="sXml"> XML data to convert into HashTable </param>
        /// <param name="objName"> Name of the node to search </param>
        /// <returns> Returns Array of HashTable</returns>
        /// <remarks>This method accepts XML data in string format and converts into HashTable and returning an array of HashTable</remarks>
        public static object[] XMLToArrayOfHashTable(string sXml, string nodeNametoSearch)
        {
            Hashtable ht = new Hashtable();
            object[] objArray ={};
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(sXml);
                XmlNodeList nodes = doc.SelectNodes(nodeNametoSearch);

                objArray = new object[nodes.Count]; 
                int arrPosition = 0;

                foreach (XmlNode node in nodes)
                {
                    for (Int32 i = 0; i < node.ChildNodes.Count; i++)
                    {
                        if (node.ChildNodes.Item(i).NodeType != XmlNodeType.Text)
                        {
                            if (node.ChildNodes.Item(i).ChildNodes.Count > 1) //&& node.ChildNodes.Item(i).NodeType != XmlNodeType.Document )
                            {
                                XmlNodeList nodeList = node.SelectNodes(node.ChildNodes.Item(i).Name);
                                foreach (XmlNode nodeTemp in nodeList)
                                {
                                    for (Int32 j = 0; j < nodeTemp.ChildNodes.Count; j++)
                                    {
                                        if (nodeTemp.ChildNodes.Item(j).NodeType != XmlNodeType.Text)
                                        {
                                            if (nodeTemp.ChildNodes.Item(j).ChildNodes.Count > 1)
                                            {
                                                HandleChildNodes(ht, nodeTemp.SelectNodes(nodeTemp.ChildNodes.Item(j).Name));
                                            }
                                            else
                                            {
                                                ht.Add(nodeTemp.ChildNodes.Item(j).Name, nodeTemp.ChildNodes.Item(j).InnerText);
                                            }
                                        }
                                    }
                                    objArray.SetValue(ht, arrPosition);
                                    ht = new Hashtable();
                                    arrPosition = arrPosition + 1;
                                }
                            }
                            else
                            {
                                ht.Add(node.ChildNodes.Item(i).Name, node.ChildNodes.Item(i).InnerText);
                            }
                        }
                    }
                    objArray.SetValue(ht, arrPosition);
                    ht = new Hashtable();
                    arrPosition = arrPosition + 1;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return objArray;
        }

       #endregion
    }
}
