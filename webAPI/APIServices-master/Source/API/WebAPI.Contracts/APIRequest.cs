using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.ComponentModel;

namespace Tesco.ClubcardProducts.MCA.API.Contracts
{
    public class APIRequest
    {
        public APIRequest()
        {
            this.inputParams = new List<object>();
            this.custInfo = new CustomerInfo();
        }
        
        public string service { get; set; }
        public string operation { get; set; }
        public List<KeyValuePair<string, object>> parameters { get; set; }
        public CustomerInfo custInfo { get; set; }

        [JsonIgnore]
        public List<object> inputParams { get; set; }

        public T GetParameter<T>(string key)
        {
            if (this.parameters != null && this.parameters.Count > 0 && this.parameters.Any(p => p.Key.ToLower().Equals(key.ToLower())))
            {
                var objValue = this.parameters.Where(p => p.Key.ToLower().Equals(key.ToLower())).FirstOrDefault().Value;
                T value = default(T);
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
                if (!Convert.IsDBNull(objValue) && objValue != null && converter.CanConvertFrom(typeof(string)))
                {
                    try
                    {
                        value = (T)converter.ConvertFrom(objValue.ToString());
                    }
                    catch
                    {
                        throw new Exception(String.Format("Parameter {0} cannot be converted to type {1}", objValue.ToString(), typeof(T).ToString()));
                    }
                }
                else
                {
                    throw new Exception(String.Format("Parameter value for {0} is either null or cannot be converted to type {1}", key, typeof(T).ToString()));
                }
                return value;
            }
            else
            {
                throw new Exception(String.Format("Parameter {0} not available", key));
            }
        }

        public string culture { get; set; }
    }

    public class CustomerInfo
    {
        public string uuid { get; set; }
        public string dotcomid { get; set; }
        public string ngccustomerid { get; set; }
        public string householdid { get; set; }
        public List<string> clubcards { get; set; }
        public string activationstatus { get; set; }
        public Dictionary<string, string> details { get; set; }

        public CustomerInfo()
        {
            this.clubcards = new List<string>();
        }
    }

    
}
