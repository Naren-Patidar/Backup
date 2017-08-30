using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Net;

namespace Tesco.ClubcardProducts.MCA.API.Contracts
{
    public class APIResponse
    {
        public APIResponse()
        {
            this.errors = new List<KeyValuePair<string, string>>();
        }

        [JsonIgnore]
        public HttpStatusCode httpstatuscode { get; set; }
        public object data { get; set; }
        public bool status { get; set; }        
        public string operationname { get; set; }
        public List<KeyValuePair<string, string>> errors { get; set; }
        public string receivedat { get; set; }
        public string overallduration { get; set; }
        public string servicestats { get; set; }
        public string servedby { get; set; }
        public string identifier { get; set; }
        public string clienttransactionid { get; set; }

        [JsonIgnore]
        public static string VersionInfo
        {
            get
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                return fvi.FileVersion;
            }
        }
    }  
}
