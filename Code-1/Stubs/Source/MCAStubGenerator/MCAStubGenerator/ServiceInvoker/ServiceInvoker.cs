using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Collections;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services;
using Tesco.ClubcardProducts.MCA.Web.Common.ResponseRecorder;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using System.Reflection;

/*
 * To be safeguarded - 
 * -------------------
Clubcard Number
Coupon Code
Voucher Number
Boost Token ID
Postcode
Email
Phone Number
 */

namespace Tesco.ClubcardProducts.MCAStubGenerator.ServiceInvoker
{
    public class ServiceInvoker
    {
        string _processingDir = String.Empty;
        string _culture = String.Empty;
        public List<string> ExecutionResults = new List<string>();
        Dictionary<string, object> _requestParams = new Dictionary<string, object>();

        public ServiceInvoker(string path, string culture)
        {
            this._processingDir = path;
            this._culture = culture;
        }

        public void Prepare(Action<string, string, bool, bool, bool, string> updateUICallBack)
        {
            Task.Factory.StartNew(() =>
                {
                    updateUICallBack("Getting end point configuration.", String.Empty, false, false, true, String.Empty);
                    List<EndPointDefinition> selectedEndPoints = this.GetEndPoints();

                    updateUICallBack("Getting customer list.", String.Empty, false, false, true, String.Empty);
                    List<long> customerIDs = this.GetCustomers();

                    updateUICallBack("Preparing endpoints.", String.Empty, false, false, true, String.Empty);
                    EndPointPreparer ep = new EndPointPreparer(selectedEndPoints, customerIDs, this._culture, this._processingDir);

                    ep.OnLogEvent += (sender, e) =>
                    {
                        updateUICallBack(e.Message, String.Empty, false, false, true, String.Empty);
                    };

                    var t1 = Task.Factory.StartNew(() => ep.PrepareEndPoints());

                    updateUICallBack("Awaiting completion of endpoint preparation.", String.Empty, false, false, true, String.Empty);
                    t1.Wait();

                    updateUICallBack("Endpoint preparation is complete.", String.Empty, true, this.EndPointsPrepared(), false, String.Empty);
                });            
        }

        public void StartProcessing(Action<string, string, bool, bool, bool, string> updateUICallBack)
        {
            Task.Factory.StartNew(() =>
                {
                    updateUICallBack("Processing end points", String.Empty, false, false, true, String.Empty);

                    var payLoadFiles = Directory.GetFiles(this._processingDir, "PayLoadEndPoints-*");

                    Helper hp = new Helper(this._culture, 0);
                    var dates = hp.GetXmasDates();

                    this._requestParams.Add(ParameterNames.START_DATE, dates.Item1);
                    this._requestParams.Add(ParameterNames.END_DATE, dates.Item2);
                    this._requestParams.Add(ParameterNames.MAX_ROWS, 100);

                    StringBuilder configurationTypesCsv = new StringBuilder();
                    List<DbConfigurationTypeEnum> configurationTypes = new List<DbConfigurationTypeEnum>() { DbConfigurationTypeEnum.Holding_dates };
                    configurationTypes.ForEach(c => configurationTypesCsv.Append((int)c + ","));
                    this._requestParams.Add(ParameterNames.CONFIGURATION_TYPES, configurationTypesCsv.ToString());

                    int custCount = 0;
                    string custCountMessage = String.Empty;

                    foreach (string pf in payLoadFiles)
                    {
                        custCount++;

                        custCountMessage = String.Format("Customer: {0}/{1}", custCount, payLoadFiles.Length);

                        updateUICallBack(String.Format("Now processing endpoints for customer {0}",
                            pf.ToLower().Replace("PayLoadEndPoints-", String.Empty).Replace(".json", String.Empty)),
                            String.Empty, false, false, true, custCountMessage);

                        string fileP = System.IO.Path.Combine(this._processingDir, pf);

                        updateUICallBack("Reading endpoints", String.Empty, false, false, true, custCountMessage);
                        var executableEndPoints = JsonConvert.DeserializeObject<List<ExecutableEndPoint>>(File.ReadAllText(fileP));

                        while (executableEndPoints.Any(ep => ep.CurrentStatus == Status.ReadyToGo))
                        {
                            updateUICallBack(String.Empty,
                                            String.Format("{0} Endpoints remaing to be processed", executableEndPoints.Where(ep => ep.CurrentStatus == Status.ReadyToGo).Count()),
                                            false, false, true, custCountMessage);

                            IEnumerable<ExecutableEndPoint> eps = executableEndPoints.Where(ep => ep.CurrentStatus == Status.ReadyToGo).Take(2);
                            var completedEPS = this.ProcessBatch(eps.ToList(), custCountMessage, updateUICallBack);

                            completedEPS.ForEach(cep =>
                            {
                                executableEndPoints.Where(ep => ep.ID == cep.ID)
                                                                .ToList()
                                                                .ForEach(ep =>
                                                                {
                                                                    ep.CurrentStatus = cep.CurrentStatus;
                                                                    ep.Started = cep.Started;
                                                                    ep.Ended = cep.Ended;
                                                                    ep.FailureReason = cep.FailureReason;
                                                                    ep.ResultDirectory = cep.ResultDirectory;
                                                                });
                            });

                            using (StreamWriter file = new StreamWriter(fileP, false))
                            {
                                file.Write(executableEndPoints.JsonText());
                            }
                        }
                        updateUICallBack("Completed processing all endpoints.", "Completed processing all endpoints.", false, false, false, custCountMessage);
                    }
                });
        }

        private List<ExecutableEndPoint> ProcessBatch(List<ExecutableEndPoint> execPoints, string custCountMessage,
                        Action<string, string, bool, bool, bool, string> updateUICallBack)
        {
            object sync = new Object();
            var pickedEndPoints = execPoints.Take(2);
            List<ExecutableEndPoint> completedEPs = new List<ExecutableEndPoint>();

            pickedEndPoints.ToList().ForEach(ep => updateUICallBack(String.Format("Processing: {0}() method on {1} service for customer {2} ",
                                                                    ep.EndPointDef.MethodDef.name, ep.EndPointDef.ServiceType, ep.customerID), 
                                                                    String.Empty,
                                                                    false, false, true, custCountMessage)
                                            );

            var task = Task.Factory.StartNew(() => Parallel.ForEach<ExecutableEndPoint>(pickedEndPoints, ep =>
                                                {
                                                    ep.CurrentStatus = Status.NowProcessing;
                                                    completedEPs.Add(this.Invoke(ep));
                                                }));

            task.Wait(60000);

            completedEPs.ForEach(ep => updateUICallBack(String.Format("Completed: {0}() method on {1} service for customer {2} ",
                                                                    ep.EndPointDef.MethodDef.name, ep.EndPointDef.ServiceType, ep.customerID),
                                                                    String.Empty,
                                                                    false, false, true, custCountMessage)
                                            );
            return completedEPs;
        }

        private List<long> GetCustomers()
        {
            //Read from the file customer.json
            var customers = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(
                                        Path.Combine(this._processingDir, "customer.json")));

            var longCustomers = customers.ConvertAll<long>(c => long.Parse(c));

            return longCustomers;
        }

        private List<EndPointDefinition> GetEndPoints()
        {
            var endPoints = JsonConvert.DeserializeObject<List<EndPointDefinition>>(File.ReadAllText(
                                        Path.Combine(this._processingDir, "endpoints.json")));

            return endPoints;
        }

        private ExecutableEndPoint Invoke(ExecutableEndPoint execP)
        {
            try
            {
                execP.Started = DateTime.UtcNow;
                Recorder recorder = new Recorder(execP.customerID);

                var request = new MCARequest();
                request.Parameters.Add(ParameterNames.OPERATION_NAME, execP.EndPointDef.MethodDef.opname);
                request.Parameters.Add(ParameterNames.CUSTOMER_ID, execP.customerID);
                request.Parameters.Add(ParameterNames.CLUBCARD_NUMBER, CryptoUtility.DecryptTripleDES(execP.ActualClubcardNumber).TryParse<long>());
                request.Parameters.Add(ParameterNames.CULTURE, this._culture);
                request.Parameters.Add(ParameterNames.CUSTOMER_ID_Points, execP.customerID);
                request.Parameters.Add(ParameterNames.REASON_CODE, execP.ReasonCode);
                request.Parameters.Add(ParameterNames.HOUSEHOLD_ID, execP.HouseholdID);

                Hashtable inputParams = new Hashtable();
                inputParams[ParameterNames.CUSTOMER_ID_Points] = execP.customerID;
                string conditionalXml = GeneralUtility.HashTableToXML(inputParams, "PointsInfoCondition");
                request.Parameters.Add(ParameterNames.CONDITIONAL_XML, conditionalXml);

                this._requestParams.Keys.ToList().ForEach(k => request.Parameters.Add(k.ToString(), this._requestParams[k]));

                if (!String.IsNullOrWhiteSpace(execP.MethodName))
                {
                    Helper hp = new Helper(this._culture, execP.customerID);
                    Type helperType = Type.GetType("Tesco.ClubcardProducts.MCAStubGenerator.ServiceInvoker.Helper");
                    MethodInfo theMethod = helperType.GetMethod(execP.MethodName);
                    var result = theMethod.Invoke(hp, new object[] { request });
                    if (result != null && !String.IsNullOrWhiteSpace(result.ToString()) && Directory.Exists(result.ToString()))
                    {
                        execP.ResultDirectory = result.ToString();
                        execP.CurrentStatus = Status.Succeeded;
                    }
                    else
                    {
                        execP.CurrentStatus = Status.Failed;
                    }
                }
                else
                {
                    Type saType = Type.GetType(String.Format("{0},Tesco.ClubcardProducts.MCA.Web.ServiceAdapter", execP.EndPointDef.ProxyType));
                    IServiceAdapter sa = (IServiceAdapter)Activator.CreateInstance(saType, recorder);

                    var response = sa.Get(request);

                    execP.ResultDirectory = sa.GetRecorder().OutputDir;

                    if (response.Status)
                    {
                        execP.CurrentStatus = Status.Succeeded;
                    }
                    else
                    {
                        execP.CurrentStatus = Status.Failed;
                    }
                }
            }
            catch (Exception ex)
            {
                execP.FailureReason = ex.ToString();
                execP.CurrentStatus = Status.Failed;
            }
            finally
            {
                execP.Ended = DateTime.UtcNow;
            }

            return execP;
        }   

        private bool EndPointsPrepared()
        {
            DirectoryInfo dir = new DirectoryInfo(this._processingDir);
            var payLoadFiles = dir.EnumerateFiles("PayLoadEndPoints-*");
            return (payLoadFiles != null && payLoadFiles.Count() > 0);
        }
    }
}
