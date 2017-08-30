using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigReader
{
    class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                string value = string.Empty;
                ConfigReader.AppConfigReader r = new AppConfigReader();
                string configFileName = string.Empty;
                string key = string.Empty;
                if (args.Length > 0)
                {
                    configFileName = args[0];
                }
                if (args.Length > 1)
                {
                    key = args[1];
                }
                if (!string.IsNullOrEmpty(configFileName))
                {
                    value = r.GetConfigValue(key, configFileName);
                    Console.WriteLine(value);
                }
            }
            catch { }
        }
    }
}
