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

namespace Tesco.ClubcardProducts.MCAStubGenerator.ServiceInvoker
{
    public class EndPointPreparer
    {
        List<EndPointDefinition> _endPoints = null;
        List<long> _customers = null;
        int _totalExecs = 0;
        public List<string> ExecutionResults = new List<string>();
        string _currentCulture = String.Empty;

        public event EventHandler<StringEventArgs> OnLogEvent;

        public EndPointPreparer(List<EndPointDefinition> eps, List<long> customers, string culture)
        {
            this._endPoints = eps;
            this._customers = customers;
            this._currentCulture = culture;
            this._totalExecs = eps.Count * customers.Count;
        }

        public List<ExecutableEndPoint> PrepareEndPoints()
        {
            List<ExecutableEndPoint> execEPs = new List<ExecutableEndPoint>();

            this.AddLogEvent(String.Format("Starting preparation for {0} endpoints.", this._totalExecs));
            
            Helper hp = new Helper(this._currentCulture);

            this._customers.ForEach(c => {
                                            var ccNumber = this.GetCustomerClubcard(c);
                                            if (ccNumber.Item1 > 0)
                                            {
                                                var optedIns = hp.GetOptedForMile(c);
                                                this._endPoints.ForEach(ep =>
                                                                execEPs.Add(new ExecutableEndPoint
                                                                                {
                                                                                    ReasonCode = optedIns.Item3,
                                                                                    customerID = c,
                                                                                    CurrentStatus = Status.ReadyToGo,
                                                                                    EndPointDef = ep,
                                                                                    clubcardNumber = ccNumber.Item1,
                                                                                    ID = execEPs.Count + 1
                                                                                }));
                                            }
                                            else
                                            {
                                                this.AddLogEvent(String.Format("Error: Clubcard number not available for customerid {0}", c));
                                            }
                                        });

            this.AddLogEvent(String.Format("Prepared {0} endpoints.", execEPs.Count));

            return execEPs;
        }

        private void AddLogEvent(int latestCount)
        {
            string logmessage = string.Format("Preparing endpoints. So far {0} of {1} completed.", latestCount, this._totalExecs);
            this.AddLogEvent(logmessage);
            
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
    }
}
