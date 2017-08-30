using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Configuration;
using AutomationTrigger.Entities;

namespace AutomationTrigger
{
    public class Trigger : ITrigger
    {

        #region ITrigger Members

        public TriggerResponse Run(string environment, string country, string category)
        {
            TriggerResponse response = new TriggerResponse
            {
                Environment = environment,
                Country = country,
                Category = category
            };
            string file = string.Empty;
            try
            {
                //int exitCode;
                ProcessStartInfo processInfo;
                Process process;
                file = GetBatchFile(environment, country, category);
                response.BatchFile = file;
                if (File.Exists(file))
                {
                    processInfo = new ProcessStartInfo("cmd.exe", "/c \"\"" + file + "\"\"");
                    processInfo.CreateNoWindow = false;
                    processInfo.UseShellExecute = true;
                    process = Process.Start(processInfo);
                    response.IsSuccess = true;
                }
                else {
                    response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Error = ex;
                return response;
            }
            return response; 
        }

        string GetBatchFile(string env, string country, string category)
        {
            string configurationName = string.Format("{0}_{1}_{2}", env, country, category);
            string fileName = ConfigurationManager.AppSettings[configurationName.ToUpper()];
            return fileName;
        }

        #endregion
    }
}
