using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.ClubcardService;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.CustomerActivationServices;
using Tesco.ClubcardProducts.MCA.Web.Common.ResponseRecorder;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using System.IO;
using Tesco.ClubcardProducts.MCA.Web.Common;

namespace Tesco.ClubcardProducts.MCAStubGenerator.ServiceInvoker
{
    public class EndPointPreparer
    {
        List<EndPointDefinition> _endPoints = null;
        List<long> _customers = null;
        int _totalExecs = 0;
        public List<string> ExecutionResults = new List<string>();
        string _currentCulture = String.Empty;
        List<CustomerProperties> _custData = null;
        string _processingDir = String.Empty;

        public event EventHandler<StringEventArgs> OnLogEvent;

        public EndPointPreparer(List<EndPointDefinition> eps, List<long> customers, string culture, string path)
        {
            this._endPoints = eps;
            this._customers = customers;
            this._currentCulture = culture;
            this._totalExecs = eps.Count * customers.Count;
            this._custData = new List<CustomerProperties>();
            this._processingDir = path;
        }

        public void PrepareEndPoints()
        {
            var payLoadFiles = Directory.GetFiles(this._processingDir, "PayLoadEndPoints-*");

            foreach (string pf in payLoadFiles)
            {
                File.Delete(Path.Combine(this._processingDir, pf));                
            }

            int iTotalEndPoints = 0;
            List<ExecutableEndPoint> execEPs = null;

            this.AddLogEvent(String.Format("Starting preparation for {0} endpoints.", this._totalExecs));

            Helper hp = null;

            this._customers.ForEach(c => {
                                            this.AddLogEvent(String.Format("Started preparing endpoint for customer {0}.", c.ToString()));
                                            var ccNumber = this.GetCustomerClubcard(c);
                                            long dummyCCNumber = this.LongRandom(100000000000000000, 100000000000000050, new Random());
                                            var encryCCNumber = CryptoUtility.EncryptTripleDES(ccNumber.Item1.ToString());
                                            if (ccNumber.Item1 > 0)
                                            {
                                                hp = new Helper(this._currentCulture, c);

                                                execEPs = new List<ExecutableEndPoint>();
                                                var optedIns = hp.GetOptedForMile(c);

                                                var householdID = hp.GetHouseholdID();

                                                this._endPoints.ForEach(ep =>
                                                                execEPs.Add(new ExecutableEndPoint
                                                                                {
                                                                                    ReasonCode = optedIns.Item3,
                                                                                    customerID = c,
                                                                                    CurrentStatus = Status.ReadyToGo,
                                                                                    EndPointDef = ep,
                                                                                    ActualClubcardNumber = encryCCNumber,
                                                                                    DummyClubcardNumber = dummyCCNumber,
                                                                                    HouseholdID = householdID,
                                                                                    MethodName = ep.MethodDef.invoker,
                                                                                    ID = execEPs.Count + 1
                                                                                }));
                                                string fileP = Path.Combine(this._processingDir, String.Format(ServicesConfig.PAYLOADFILE, c.ToString()));

                                                if (File.Exists(fileP))
                                                {
                                                    File.Delete(fileP);
                                                }

                                                iTotalEndPoints += execEPs.Count;

                                                using (StreamWriter file = new StreamWriter(fileP, false))
                                                {
                                                    file.Write(execEPs.JsonText());
                                                }

                                                this.AddLogEvent(String.Format("Completed preparing endpoints for customer {0}.", c.ToString()));
                                            }
                                            else
                                            {
                                                this.AddLogEvent(String.Format("Error: Clubcard number not available for customerid {0}", c));
                                            }
                                        });

            this.AddLogEvent(String.Format("Prepared {0} endpoints for all customers.", iTotalEndPoints));
        }

        private void AddLogEvent(string message)
        {
            this.ExecutionResults.Add(message);

            if (this.OnLogEvent != null)
            {
                this.OnLogEvent(this, new StringEventArgs(message));
            }
        }

        private Tuple<long, string> GetCustomerClubcard(long customerID)
        {
            Tuple<long, string> tResult = new Tuple<long, string>(long.MinValue, String.Empty);

            try
            {
                ClubcardServiceAdapter ca = new ClubcardServiceAdapter(new Recorder(customerID));
                
                MCARequest request = new MCARequest();
                request.Parameters.Add(ParameterNames.OPERATION_NAME, OperationNames.GET_CLUBCARD_CUSTOMER_DATA);
                request.Parameters.Add(ParameterNames.CUSTOMER_ID, customerID);
                request.Parameters.Add(ParameterNames.CULTURE, this._currentCulture);

                var response = ca.Get(request);

                if (response != null && response.Status)
                {
                    var clubcards = response.Data as List<Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common.Clubcard>;
                    tResult = new Tuple<long, string>(clubcards.FirstOrDefault().ClubCardID, String.Empty);
                }
                else if (response != null)
                {
                    tResult = new Tuple<long,string>(long.MinValue, 
                                    String.Format("ErrorMessage: {0}, ErrorException: {1}", response.ErrorMessage, response.ErrorException));
                }
                else
                {
                    tResult = new Tuple<long, string>(long.MinValue, "Null Response received");
                }
            }
            catch (Exception ex)
            {
                tResult = new Tuple<long, string>(long.MinValue, 
                                String.Format("Internal error!. Details - {0}", ex.ToString()));
            }
            return tResult;
        }

        long LongRandom(long min, long max, Random rand)
        {
            byte[] buf = new byte[8];
            rand.NextBytes(buf);
            long longRand = BitConverter.ToInt64(buf, 0);

            return (Math.Abs(longRand % (max - min)) + min);
        }
    }
}
