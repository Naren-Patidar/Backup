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

        public void Prepare(Action<string, string, bool, bool> updateUICallBack)
        {
            Task.Factory.StartNew(() =>
                {
                    updateUICallBack("Getting end point configuration.", String.Empty, false, false);
                    List<EndPointDefinition> selectedEndPoints = this.GetEndPoints();

                    updateUICallBack("Getting customer list.", String.Empty, false, false);
                    List<long> customerIDs = this.GetCustomers();

                    updateUICallBack("Preparing endpoints.", String.Empty, false, false);
                    EndPointPreparer ep = new EndPointPreparer(selectedEndPoints, customerIDs, this._culture);

                    ep.OnLogEvent += (sender, e) =>
                    {
                        this.ExecutionResults.Add(e.Message);
                        updateUICallBack(e.Message, String.Empty, false, false);
                    };

                    var t1 = Task<List<ExecutableEndPoint>>.Factory.StartNew(() => ep.PrepareEndPoints());

                    updateUICallBack("Awaiting completion of endpoint preparation.", String.Empty, false, false);
                    t1.Wait();

                    var result = t1.Result;

                    updateUICallBack("Writing prepared endpoints to file system.", String.Empty, false, false);

                    string fileP = Path.Combine(this._processingDir, ServicesConfig.PAYLOADFILE);

                    if (File.Exists(fileP))
                    {
                        File.Delete(fileP);
                    }

                    using (StreamWriter file = new StreamWriter(fileP, false))
                    {
                        file.Write(t1.Result.JsonText());
                    }

                    updateUICallBack("Endpoint preparation is complete.", String.Empty, true, (File.Exists(fileP) && result.Count > 0));
                });            
        }

        public void StartProcessing(Action<string, string, bool, bool> updateUICallBack)
        {
            Task.Factory.StartNew(() =>
                {
                    updateUICallBack("Processing end points", String.Empty, false, false);
                    string fileP = System.IO.Path.Combine(this._processingDir, ServicesConfig.PAYLOADFILE);

                    updateUICallBack("Reading endpoints", String.Empty, false, false);
                    var executableEndPoints = JsonConvert.DeserializeObject<List<ExecutableEndPoint>>(File.ReadAllText(fileP));

                    Helper hp = new Helper(this._culture, 0);
                    var dates = hp.GetXmasDates();

                    this._requestParams.Add(ParameterNames.START_DATE, dates.Item1);
                    this._requestParams.Add(ParameterNames.END_DATE, dates.Item2);
                    this._requestParams.Add(ParameterNames.MAX_ROWS, 100);

                    while (executableEndPoints.Any(ep => ep.CurrentStatus == Status.ReadyToGo))
                    {
                        updateUICallBack(String.Empty,
                                        String.Format("{0} Endpoints remaing to be processed", executableEndPoints.Where(ep => ep.CurrentStatus == Status.ReadyToGo).Count()), 
                                        false, false);

                        IEnumerable<ExecutableEndPoint> eps = executableEndPoints.Where(ep => ep.CurrentStatus == Status.ReadyToGo).Take(2);
                        var completedEPS = this.ProcessBatch(eps.ToList(), updateUICallBack);

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
                    updateUICallBack("Completed processing all endpoints.", "Completed processing all endpoints.", false, false);
                });
        }

        private List<ExecutableEndPoint> ProcessBatch(List<ExecutableEndPoint> execPoints, Action<string, string, bool, bool> updateUICallBack)
        {
            object sync = new Object();
            var pickedEndPoints = execPoints.Take(2);
            List<ExecutableEndPoint> completedEPs = new List<ExecutableEndPoint>();

            pickedEndPoints.ToList().ForEach(ep => updateUICallBack(String.Format("Processing: {0}() method on {1} service for customer {2} ",
                                                                    ep.EndPointDef.MethodDef.name, ep.EndPointDef.ServiceType, ep.customerID), 
                                                                    String.Empty,
                                                                    false, false)
                                            );

            var task = Task.Factory.StartNew(() => Parallel.ForEach<ExecutableEndPoint>(pickedEndPoints, ep =>
                                                {
                                                    ep.CurrentStatus = Status.NowProcessing;
                                                    var cep = this.Invoke(ep);
                                                    if (cep != null)
                                                    {
                                                        completedEPs.Add(cep);
                                                    }
                                                }));

            task.Wait();

            completedEPs.ForEach(ep => updateUICallBack(String.Format("Completed: {0}() method on {1} service for customer {2} ",
                                                                    ep.EndPointDef.MethodDef.name, ep.EndPointDef.ServiceType, ep.customerID),
                                                                    String.Empty, 
                                                                    false, false)
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

                Type saType = Type.GetType(String.Format("{0},Tesco.ClubcardProducts.MCA.Web.ServiceAdapter", execP.EndPointDef.ProxyType));
                IServiceAdapter sa = (IServiceAdapter)Activator.CreateInstance(saType, recorder);

                var request = new MCARequest();
                request.Parameters.Add(ParameterNames.OPERATION_NAME, execP.EndPointDef.MethodDef.opname);
                request.Parameters.Add(ParameterNames.CUSTOMER_ID, execP.customerID);
                request.Parameters.Add(ParameterNames.CLUBCARD_NUMBER, execP.clubcardNumber);
                request.Parameters.Add(ParameterNames.CULTURE, this._culture);
                request.Parameters.Add(ParameterNames.CUSTOMER_ID_Points, execP.customerID);
                request.Parameters.Add(ParameterNames.REASON_CODE, execP.ReasonCode);

                Hashtable inputParams = new Hashtable();
                inputParams[ParameterNames.CUSTOMER_ID_Points] = execP.customerID;
                string conditionalXml = GeneralUtility.HashTableToXML(inputParams, "PointsInfoCondition");
                request.Parameters.Add(ParameterNames.CONDITIONAL_XML, conditionalXml);

                this._requestParams.Keys.ToList().ForEach(k => request.Parameters.Add(k.ToString(), this._requestParams[k]));

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
    }
}
