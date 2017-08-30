using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Tesco.ClubcardProducts.MCA.API.Models
{
    //Datastore for the apis
	public class APIs
    {
        public string libstore { get; set; }
        public List<api> apis { get; set; }
    }

    public class api
    {
        public string name { get; set; }
        public string version { get; set; }

        [JsonIgnore]
        public string assembly { get; set; }

        [JsonIgnore]
        public Type instance { get; set; }

        public string instanceName 
        { 
            get
            {
                return this.instance.ToString();
            }
            set
            {
                //Don't do anything
            }
        }

        public string assemblyName { get; set; }
        public List<operation> operations { get; set; }
        public string loaderror { get; set; }

        public api()
        {
            this.operations = new List<operation>();
        }
    }

    public class operation
    {
        public string name { get; set; }
        public List<parameter> Parameters { get; set; }

        public operation()
        {
            this.Parameters = new List<parameter>();
        }

        public object Output { get; set; }
    }

    public class parameter
    {
        public string name { get; set; }
        public string ptype { get; set; }
    }

    public class ClientAuth
    {
        public string customerid { get; set; }
        public string customername { get; set; }
        public string publickey { get; set; }
        public string secretkey { get; set; }
        public string clientid { get; set; }
    }
}