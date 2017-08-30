using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.Framework.UITesting.Services.PreferenceService;
using Tesco.Framework.UITesting;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Xml;
namespace Tesco.Framework.UITesting.Services
{
    public class Utility
    {

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
                        writer.WriteStartElement(item.Key.ToString().Trim());
                        writer.WriteValue(item.Value);
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    writer.Flush();
                }
            }
            catch 
            {
            }
            return Convert.ToString(sb);
        }

      
    }
}
