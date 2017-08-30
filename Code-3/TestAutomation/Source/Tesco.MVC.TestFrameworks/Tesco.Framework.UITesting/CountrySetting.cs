using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesco.Framework.UITesting.Entities;

namespace Tesco.Framework.UITesting
{
    class CountrySetting
    {
        public static string country = null;
        public static string culture = null;
        private static object syncRoot = new Object();

        static CountrySetting()
        {            
            try
            {
                // read from file or write to file
                if (country == null)
                {
                    AppConfiguration config = new AppConfiguration();
                    string contents = null;
                    using (System.IO.StreamReader file = new System.IO.StreamReader(config.CultureFile))
                    {

                        while ((contents = file.ReadLine()) != null)
                        {
                            break;
                        }
                    }

                    lock (syncRoot)
                    {
                        if (country == null)
                            country = contents;
                    }
                }
                culture = GetCulture(country);
            }
            catch { }
        }

        public static string GetCulture(string country)
        {
            string culture = "en-GB";
            switch (country)
            {
                case "UK":
                    culture = "en-GB";
                    break;
                case "TH":
                    culture = "th-TH";
                    break;
                case "MY":
                    culture = "en-MY";
                    break;
                case "HU":
                    culture = "hu-HU";
                    break;
                case "PL":
                    culture = "pl-PL";
                    break;
                case "ROI":
                    culture = "en-IE";
                    break;
                case "SK":
                    culture = "sk-SK";
                    break;
                case "CZ":
                    culture = "cs-CZ";
                    break;
                case "TR":
                    culture = "tr-TR";
                    break;
            }
            return culture;
        }
    }
}
