using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Configuration;
using System.Web;
using Tesco.ClubcardProducts.MCA.API.Models;
using System.Reflection;
using Tesco.ClubcardProducts.MCA.API.Contracts;
using System.ComponentModel;
using Tesco.ClubcardProducts.MCA.API.Logger;
using System.Diagnostics;
using System.Net;

namespace Tesco.ClubcardProducts.MCA.API.ServiceManager
{
    //http://www.codeproject.com/Articles/756423/How-to-Cache-Objects-Simply-using-System-Runtime-C
    public class ServiceUtility
    {
        public const string CLIENT_PREFIX = "client";
        public const string SERVICE_PREFIX = "service";

        private static Dictionary<string, api> _services = new Dictionary<string, api>();

        public static void LoadServices()
        {
            _services = new Dictionary<string, api>();
            string libPath = GlobalCachingProvider.Instance.GetAppSetting(AppSettingKeys.ServiceLibrary);
            
            string serviceLibPath = System.Web.HttpContext.Current.Server.MapPath(libPath);

            if (!Directory.Exists(serviceLibPath))
            {
                throw new Exception(String.Format("No directory found at - {0}", serviceLibPath));
            }

            DirectoryInfo dInfo = new DirectoryInfo(serviceLibPath);
            var assemblies = dInfo.EnumerateFiles("*.dll");

            foreach (FileInfo f in assemblies)
            {
                Assembly serviceAssembly = null;
                try
                {
                    serviceAssembly = Assembly.LoadFrom(f.FullName);
                }
                catch (Exception)
                {
                    //Log error that f.FullName failed to load. Moving on to next file.
                    continue;
                }

                Type[] types = serviceAssembly.GetTypes();
                string tName = String.Empty;
                string cacheKey = "{0}_{1}";
                string sVersion = FileVersionInfo.GetVersionInfo(f.FullName).FileVersion;

                foreach (Type t in types)
                {
                    if (t.GetInterfaces().Contains(typeof(IServiceAdapter)))
                    {
                        api apiItem = new api()
                        {
                            assembly = f.FullName,
                            assemblyName = f.Name,
                            instance = t,
                            name = "NA",
                            version = sVersion
                        };

                        IServiceAdapter saInst = null;
                        try
                        {
                            saInst = Activator.CreateInstance(t) as IServiceAdapter;
                        }
                        catch (Exception ex)
                        {
                            //Log error that Type t failed to be instantiated as IServiceAdapter. 
                            apiItem.loaderror = String.Format("Type {0} could not be instantiated. Details {1}", t.ToString(), ex.ToString());
                            ServiceUtility._services[String.Format("failedservice_{0}_{1}", t.ToString(), Guid.NewGuid().ToString())] = apiItem;
                            continue;
                        }

                        if (saInst == null)
                        {
                            //Log error that Type t failed to be instantiated as IServiceAdapter. 
                            apiItem.loaderror = String.Format("Type {0} could not be instantiated and is therefore null.", t.ToString());
                            ServiceUtility._services[String.Format("failedservice_{0}_{1}", t.ToString(), Guid.NewGuid().ToString())] = apiItem;
                            continue;
                        }

                        tName = saInst.GetName();
                        cacheKey = String.Format(cacheKey, ServiceUtility.SERVICE_PREFIX, tName.ToLower());

                        var cacheItem = GlobalCachingProvider.Instance.GetItem(cacheKey);

                        if (cacheItem == null)
                        {
                            apiItem.name = tName;
                            try
                            {
                                foreach (var op in saInst.GetSupportedOperations())
                                {
                                    try
                                    {
                                        var method = apiItem.instance.GetMethod(op.Key, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                                        apiItem.operations.Add(
                                            new operation()
                                            {
                                                name = method.Name,
                                                Parameters = method.GetParameters().ToList().Select(p => new parameter()
                                                {
                                                    name = p.Name,
                                                    ptype = p.ParameterType.ToString()
                                                }).ToList<parameter>(),
                                                Output = ServiceUtility.IsTypeJsonPrimitive(method.ReturnType) ?
                                                             method.ReturnType.ToString() : op.Value
                                                
                                            });

                                        foreach (ParameterInfo pi in method.GetParameters())
                                        {
                                            if (!ServiceUtility.IsTypeJsonPrimitive(pi.ParameterType))
                                            {
                                                apiItem.loaderror = String.Format("Type '{0}' of parameter '{1}' of operation '{2}' is not primitive.", 
                                                    pi.ParameterType.ToString(), pi.Name, method.Name);
                                                break;
                                            }
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        apiItem.loaderror = String.Format("Method {0} could not be loaded.", op);
                                        continue;
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                throw;
                            }

                            GlobalCachingProvider.Instance.AddItem(cacheKey, apiItem);

                            if (ServiceUtility._services.ContainsKey(cacheKey))
                            {
                                ServiceUtility._services[cacheKey] = apiItem;
                            }
                            else
                            {
                                ServiceUtility._services.Add(cacheKey, apiItem);
                            }
                        }
                        else
                        {
                            //Raise error in log
                        }
                        
                        cacheKey = "{0}_{1}";
                    }
                }
            }
        }

        public static void LoadClientAuth()
        {
            OAuthBase oAuthBase = new OAuthBase();
            //oAuthBase.EncryptFile();
            string validations = oAuthBase.DecryptFile();
            
            var clientAuths = JsonConvert.DeserializeObject<List<ClientAuth>>(validations);
            GlobalCachingProvider.Instance.AddItem(ServiceUtility.CLIENT_PREFIX, clientAuths);
        }

        public Tuple<APIRequest, api> ValidateRequest(APIRequest request, ref LogData logData)
        {
            logData.RecordStep("Validation Started...");
            string requestedService = String.Format("{0}_{1}", ServiceUtility.SERVICE_PREFIX, request.service.ToLower());
            logData._serviceRequested = request.service;
            logData._operationRequested = request.operation;

            var service = GlobalCachingProvider.Instance.GetItem(requestedService);

            if (service == null)
            {
                throw new APIBadRequestException(APIErrors.E_400_4, String.Format("Cannot locate service by name - {0}", request.service));
            }

            api requestedAPI = service as api;

            if (!String.IsNullOrWhiteSpace(requestedAPI.loaderror))
            {
                throw new APIBadRequestException(APIErrors.E_400_5, String.Format("Service hasn't been accepted yet because - {0}", requestedAPI.loaderror));
            }

            logData._assemblyInvoked = requestedAPI.assemblyName;
            logData._typeInvoked = requestedAPI.instanceName;

            var methods = requestedAPI.instance.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            var filteredMethods = methods.Where(m => m.Module.FullyQualifiedName.ToLower().Equals(requestedAPI.assembly.ToLower())).ToList<MethodInfo>();

            var requestedMethod = filteredMethods.Where(m => m.Name.ToLower().Equals(request.operation.ToLower())).FirstOrDefault();

            if (requestedMethod == null)
            {
                throw new APIBadRequestException(APIErrors.E_400_6, String.Format("Cannot locate operation {0} in assembly - {1}", request.operation, requestedAPI.assembly));
            }

            logData._operationInvoked = request.operation;

            request.operation = requestedMethod.Name;

            if (!this.IsOperationSafe(requestedAPI, request))
            {
                throw new APIForbiddenException(APIErrors.E_403_1, String.Format("Operation {0} not supported", request.operation));
            }

            var mParams = requestedMethod.GetParameters();
            KeyValuePair<string, object> inputParam = new KeyValuePair<string,object>();

            foreach (ParameterInfo p in mParams)
            {
                if (request.parameters.Any(rp => rp.Key.ToLower().Equals(p.Name.ToLower())))
                {
                    inputParam = request.parameters.Where(rp => rp.Key.ToLower().Equals(p.Name.ToLower())).FirstOrDefault();

                    try
                    {
                        //TypeConverter typeConverter = TypeDescriptor.GetConverter(p.ParameterType);
                        //request.inputParams.Add(typeConverter.ConvertFromString(inputParam.Value));
                        request.inputParams.Add(inputParam.Value);
                    }
                    catch
                    {
                        throw new APIBadRequestException(APIErrors.E_400_7, String.Format("Parameter {0} must be of type {1}", p.Name, p.ParameterType.ToString()));
                    }                    
                }
                else
                {
                    throw new APIBadRequestException(APIErrors.E_400_8, String.Format("parameter {0} missing", p.Name));
                }
            }

            logData.RecordStep("Validation Complete.");
            return new Tuple<APIRequest, api>(request, requestedAPI);
        }

        public APIResponse ExecuteRequest(api requestedAPI, APIRequest request)
        {
            //var targetMethod = requestedAPI.instance.GetMethod(request.operation, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            //return targetMethod.Invoke(Activator.CreateInstance(requestedAPI.instance), request.inputParams.ToArray());

            return ((IServiceAdapter)Activator.CreateInstance(
                                                    requestedAPI.instance, 
                                                    request.custInfo.dotcomid, 
                                                    request.custInfo.uuid, 
                                                    request.culture))
                                                .Execute(request);
        }

        public string GetServerIdentifier()
        {
            IPHostEntry host;
            string localIP = "?";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }

            if (!String.IsNullOrWhiteSpace(localIP) && localIP.LastIndexOf(".") > 0)
            {
                int istartIndex = localIP.LastIndexOf(".");
                localIP = localIP.Substring(istartIndex, localIP.Length - istartIndex);
            }
            return localIP;
        }

        private bool IsOperationSafe(api requestedAPI, APIRequest request)
        {
            return requestedAPI.operations.Any(o => o.name.Equals(request.operation));
        }

        public string GetMetadata(string appkey)
        {
            int failedServices = ServiceUtility._services.Where(s => !String.IsNullOrWhiteSpace(s.Value.loaderror)).Count();
            var resultText = JsonConvert.SerializeObject(ServiceUtility._services).Replace("service_", String.Empty);

            JsonSerializerSettings jsonSettings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize
                //PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };

            return JsonConvert.SerializeObject(new
            {
                UnavailableServices = failedServices,
                AvailableServices = ServiceUtility._services.Count - failedServices,
                FailedServices = ServiceUtility._services.Where(s => !String.IsNullOrWhiteSpace(s.Value.loaderror)),
                VersionDetails = this.GetVersionInfo(),
                Metadata = ServiceUtility._services.Where(s => String.IsNullOrWhiteSpace(s.Value.loaderror))
            }, Formatting.None, jsonSettings).Replace("service_", String.Empty);

            //return resultText.Replace("service_", String.Empty);
        }

        public string HealthCheck()
        {
            JsonSerializerSettings jsonSettings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize
                //PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };

            return JsonConvert.SerializeObject(new
            {
                VersionDetails = this.GetVersionInfo(),
                ServedBy = this.GetServerIdentifier()
            }, Formatting.None, jsonSettings);
        }

        private static bool IsTypeJsonPrimitive(Type t)
        {
            return (
                t.Equals(typeof(System.String)) ||
                t.Equals(typeof(System.String)) ||
                t.Equals(typeof(bool)) ||
                t.Equals(typeof(Int16)) ||
                t.Equals(typeof(Int32)) ||
                t.Equals(typeof(Int64)));
        }

        private object GetVersionInfo()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

            return new { WebAPI = fvi.FileVersion, Contract = APIResponse.VersionInfo };
        }

    }
  
    public class AppSettingKeys
    {
        public const string ValidateAppKey          = "ValidateAppKey";
        public const string ValidateOAuth           = "ValidateOAuth";
        //public const string ValidateWithEnterprise  = "ValidateWithEnterprise";
        public const string NLogConfigPath          = "NLogConfigPath";
        public const string LoggingLevel            = "LoggingLevel";
        public const string ServiceLibrary          = "ServiceLibrary";
        public const string SaltKey                 = "SaltKey";
        public const string EncryptionKey           = "EncryptionKey";
        public const string EncryptedClientAuthFile = "EncryptedClientAuthFile";
        public const string IdentityURL             = "IdentityURL";
        public const string IsEventLogEnable        = "IsEventLogEnabled";
        public const string Culture                 = "culture";
    }

    public class HeaderKeys
    {
        public const string IdentityUUID            = "uuid";
        public const string IdentityAccessToken     = "accesstoken";
        public const string TimeStamp               = "timestamp";
        public const string AppKey                  = "appkey";
        public const string Nonce                   = "nonce";
        public const string Signature               = "signature";
        public const string ClientTransactionID     = "X-TransactionID";
    }
}