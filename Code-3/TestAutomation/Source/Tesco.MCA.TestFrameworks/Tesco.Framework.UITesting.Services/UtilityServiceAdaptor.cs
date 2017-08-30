using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Tesco.Framework.UITesting.Services.UtilityService;
using System.Collections;
using System.Xml.Linq;

namespace Tesco.Framework.UITesting.Services
{
    public class UtilityServiceAdaptor : BaseAdaptor
    {
        public bool CheckProfanity(string profaneWord)
        {
            StackTrace stackTrace = new StackTrace();
            bool isProfane = false;
            CustomLogs.LogDebug("Scenario starting with Method", stackTrace.GetFrame(1).GetMethod().Name);
            using (UtilityServiceClient client = new UtilityServiceClient())
            {
                  Hashtable searchData = new Hashtable();
                  searchData["Name1"] = profaneWord;
                string errorXml = string.Empty, resultXml = string.Empty;
                int rowCount = 0;
                string conditionXML = Utility.HashTableToXML(searchData, "customer");
                if (client.ProfanityCheck(out errorXml, out resultXml,out rowCount,conditionXML))
                {
                    if (!string.IsNullOrEmpty(resultXml))
                    {
                        if (XElement.Parse(resultXml).DescendantsAndSelf("NewDataSet").Elements().First().Elements().First().Value == "0")
                        {
                            isProfane = true;
                        }
                        else
                            isProfane = false;
                    }


                }
                           }
            return isProfane;
        }

    }
}

