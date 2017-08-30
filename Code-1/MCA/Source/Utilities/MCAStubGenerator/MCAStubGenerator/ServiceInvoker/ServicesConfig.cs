using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace Tesco.ClubcardProducts.MCAStubGenerator.ServiceInvoker
{
    public static class ServicesConfig
    {
        public const string PAYLOADFILE = "PayLoadEndPoints.json";
        public const string ENDPOINTFILE = "endpoints.json";

        public static APIs LoadConfig()
        {
            string settingsFile = Path.Combine(
                                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                                    "Config", 
                                    "settings.json");

            string settings = File.ReadAllText(settingsFile);

            return JsonConvert.DeserializeObject<APIs>(settings);
        }

        public static List<EndPointDefinition> GetAPIMethodDefinitions(APIs serviceEndPoints)
        {
            List<EndPointDefinition> endpoints = new List<EndPointDefinition>();

            serviceEndPoints.apis.ForEach(s => s.methods.ForEach(m => endpoints.Add(new EndPointDefinition 
                                                                                        {
                                                                                            ServiceType = s.servicetype,
                                                                                            MethodName = m.name,                                                                                            
                                                                                            ProxyType = s.proxy,
                                                                                            MethodDef = m
                                                                                        })));
            return endpoints;
        }
    }
}
