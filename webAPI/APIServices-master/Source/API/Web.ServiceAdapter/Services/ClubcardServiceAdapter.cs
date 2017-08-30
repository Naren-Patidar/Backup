using System;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;
using System.ServiceModel;
using System.Collections.Generic;
using System.Collections;
using System.Globalization;
using Tesco.ClubcardProducts.MCA.API.Common;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.ClubcardService;
using Tesco.ClubcardProducts.MCA.API.Common.Entities;
using Tesco.ClubcardProducts.MCA.API.Common.Utilities;
using Tesco.ClubcardProducts.MCA.API.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.API.Common.Entities.Common;
using Activation = Tesco.ClubcardProducts.MCA.API.Common.Entities.Activation;
using Tesco.ClubcardProducts.MCA.API.Common.Entities.Points;
using Tesco.ClubcardProducts.MCA.API.Common.Entities.ChristmasSaver;
using Tesco.ClubcardProducts.MCA.API.Common.Entities.OrderReplacement;
using Tesco.ClubcardProducts.MCA.API.Contracts;
using System.Reflection;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Tesco.ClubcardProducts.MCA.API.ServiceAdapter.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class ClubcardServiceAdapter : BaseNGCAdapter, IServiceAdapter
    {
        IClubcardService _clubcardServiceClient = null;
        DateTime _dtStart = DateTime.UtcNow;

        #region IService Members

        public Dictionary<string, object> GetSupportedOperations()
        {
            return new Dictionary<string, object>() 
            { 
                { "GetOffersForCustomer", new List<Offer>(){
                    { new Offer() }
                }},
                { "GetPointsSummary", new PointsSummary(){
                    pointsBreakup = new Dictionary<PointsTypeEnum, string>(){
                        { PointsTypeEnum.TescoBankBroughtForwardPoints, String.Empty }
                    },
                    rewardsBreakup = new Dictionary<RewardsTypeEnum, decimal>(){
                        { RewardsTypeEnum.TescoBankRewardMiles, decimal.MaxValue}
                    }
                }},
                { "GetCustomerTransactions", new CustomerTransactions(){
                    Offer = new OfferDetails(),
                    Transactions = new List<TransactionDetails>(){
                        { new TransactionDetails() }
                    }
                }},
                { "GetCustomerAccountDetails", new AccountDetails() },
                { "GetOrderReplacementExistingStatus", new OrderReplacementStatus() },
                { "ProcessOrderReplacementRequest", bool.FalseString },
                { "IsXmasClubMember", bool.FalseString },
                { "GetChristmasSaverSummary", new List<ChristmasSaverSummary>(){
                    { new ChristmasSaverSummary() }
                }},
                { "GetClubcardsCustomerData", new List<Clubcard>(){
                    { new Clubcard() }
                } },
                { "GetHouseHoldCustomersData", new List<HouseholdDetails>()
                {
                    { new HouseholdDetails() }
                }},
                { "IGHSCheckCustomerActivatedStatus", new Activation.CustomerActivationStatusdetails() },
                { "GetHouseHoldCards", new List<string>(){ "123" }}
            };
        }

        public string GetName()
        {
            return "clubcardservice";
        }

        public APIResponse Execute(APIRequest request)
        {
            APIResponse response = new APIResponse();
            try
            {
                this._internalStats = 0;

                switch (request.operation.ToLower())
                {
                    case "getchristmassaversummary":
                        response.data = this.GetChristmasSaverSummary(
                                            request.GetParameter<string>("dtstart"),
                                            request.GetParameter<string>("dtend"),
                                            request.GetParameter<string>("dateformat"));
                        break;

                    case "getoffersforcustomer":
                        response.data = this.GetOffersForCustomer(request.GetParameter<string>("maxrowcount"));
                        break;

                    case "getpointssummary":
                        response.data = this.GetPointsSummary(request.GetParameter<string>("previousofferid"));
                        break;
                    case "getcustomertransactions":
                        response.data = this.GetCustomerTransactions(
                                                request.GetParameter<string>("offerid"),
                                                request.GetParameter<bool>("showmerchantflag"));
                        break;
                    case "getcustomeraccountdetails":
                        response.data = this.GetCustomerAccountDetails();
                        break;

                    case "getorderreplacementexistingstatus":
                        response.data = this.GetOrderReplacementExistingStatus(request.GetParameter<string>("orderprocesswindow"));
                        break;
                    case "processorderreplacementrequest":
                        response.data = this.ProcessOrderReplacementRequest(
                                                request.GetParameter<string>("reason"),
                                                request.GetParameter<string>("serviceConsumer"),
                                                request.GetParameter<string>("orderprocesswindow"));
                        break;
                    case "isxmasclubmember":
                        response.data = this.IsXmasClubMember();
                        break;
                    case "ighscheckcustomeractivatedstatus":
                        response.data = this.IGHSCheckCustomerActivatedStatus();
                        break;
                    case "gethouseholdcustomersdata":
                        response.data = this.GetHouseHoldCustomersData();
                        break;
                    case "getclubcardscustomerdata":
                        response.data = this.GetClubcardsCustomerData();
                        break;
                    case "gethouseholdcards":
                        response.data = this.GetHouseHoldCards();
                        break;
                }
            }
            catch (Exception ex)
            {
                response.errors.Add(new KeyValuePair<string, string>("ERR-CLUBCARD-SERVICE", ex.ToString()));
            }
            finally
            {
                response.servicestats = this._internalStats.ToString();
            }
            return response;
        }

        #endregion

        #region Constructors

        public ClubcardServiceAdapter()
        {
        }

        public ClubcardServiceAdapter(string dotcomid, string uuid, string culture)
            : base(dotcomid, uuid, culture)
        {
            this._clubcardServiceClient = new ClubcardServiceClient();
        }

        #endregion Constructors

        #region Private Methods

        private DataSet GetTransactionDetailsByCustomerAndOfferIDDataset(long customerId, string offerId, bool showMerchantFlag, string culture)
        {
            string conditionalXml, resultXml = string.Empty, errorXml = string.Empty;
            int maxRowCount = 0, rowCount = 0;
            Hashtable inputParams = new Hashtable();
            XmlDocument resulDoc = new XmlDocument();
            DataSet dsTransactions = new DataSet();
            bool isServiceSuccessful = false;
            inputParams["CustomerID"] = customerId.ToString();
            inputParams["OfferID"] = offerId.ToString();
            inputParams["ShowMerchantFlag"] = 1; //1 - To include the ' - MerchantName' in Transaction Description

            try
            {
                conditionalXml = GeneralUtility.HashTableToXML(inputParams, "TransactionCondition");

                this._dtStart = DateTime.UtcNow;
                try
                {
                    isServiceSuccessful = this._clubcardServiceClient.GetTxnDetailsByCustomerAndOfferID(out errorXml, out resultXml, out rowCount,
                                                                   conditionalXml, maxRowCount,
                                                                   culture);
                }
                finally
                {
                    this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
                }

                this.HandleFailedResponse(isServiceSuccessful, errorXml);
                if (!string.IsNullOrEmpty(resultXml))
                {
                    //Load the result xml containing parameters into a data set if the xml is not empty
                    resulDoc.LoadXml(resultXml);
                    dsTransactions.ReadXml(new XmlNodeReader(resulDoc));
                }
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception in Clubcard Service Adapter while getting transactions data", ex, null);
            }
            return dsTransactions;
        }

        /// <summary>
        /// Gets the transaction details by customer and offer identifier dataset to list.
        /// </summary>
        /// <param name="dsTransactionsAndOffer">The ds transactions and offer.</param>
        /// <returns></returns>
        private CustomerTransactions GetTransactionDetailsByCustomerAndOfferIDDatasetToList(DataSet dsTransactionsAndOffer, long customerId)
        {
            string transactionxml = SerializerUtility<DataSet>.GetSerializedString(dsTransactionsAndOffer);
            try
            {
                if (dsTransactionsAndOffer == null || dsTransactionsAndOffer.Tables.Count == 0)
                {
                    return new CustomerTransactions();
                }

                DataTable transactionsDetailsTable = dsTransactionsAndOffer.Tables["Transactions"];
                DataTable offerDetailsTable = dsTransactionsAndOffer.Tables["OfferDetails"];

                //review - Check dataset to null
                //review - Check datatable doesnt exist, datarow doesnt exist;
                CustomerTransactions customerTransactionDetails = new CustomerTransactions();

                if (transactionsDetailsTable != null)
                {
                    //--Add missing columns in table
                    transactionsDetailsTable.AddMissingColumns(typeof(TransactionDetailsEnum));

                    IEnumerable<TransactionDetails> transactionDetails = transactionsDetailsTable.AsEnumerable().Select(r => new TransactionDetails()
                    {
                        ClubCardTransactionId = r[TransactionDetailsEnum.ClubCardTransactionId.ToString()].TryParse<string>(),
                        ClubcardId = Convert.ToString(r[TransactionDetailsEnum.ClubcardId.ToString()]),
                        ClubCardStatusDescEnglish = Convert.ToString(r[TransactionDetailsEnum.ClubCardStatusDescEnglish.ToString()]),
                        CustType = Convert.ToString(r[TransactionDetailsEnum.CustType.ToString()]),
                        TransactionDateTime = Convert.ToString(r[TransactionDetailsEnum.TransactionDateTime.ToString()]),
                        PartnerId = transactionsDetailsTable.Columns.Contains(TransactionDetailsEnum.PartnerId.ToString()) ?
                                        r[TransactionDetailsEnum.PartnerId.ToString()].ParseDbNull() == null ? "0" : r[TransactionDetailsEnum.PartnerId.ToString()].ToString()
                                    : "0",
                        TescoStoreId = transactionsDetailsTable.Columns.Contains(TransactionDetailsEnum.TescoStoreId.ToString()) ?
                                        r[TransactionDetailsEnum.TescoStoreId.ToString()].ParseDbNull() == null ? "0" : r[TransactionDetailsEnum.TescoStoreId.ToString()].ToString()
                                    : "0",
                        AmountSpent = Convert.ToString(r[TransactionDetailsEnum.AmountSpent.ToString()]),
                        NormalPoints = Convert.ToString(r[TransactionDetailsEnum.NormalPoints.ToString()]),
                        BonusPoints = Convert.ToString(r[TransactionDetailsEnum.BonusPoints.ToString()]),
                        TotalPoints = Convert.ToString(r[TransactionDetailsEnum.TotalPoints.ToString()]),
                        TransactionDescription = (transactionsDetailsTable.Columns.Contains(TransactionDetailsEnum.TransactionDescription.ToString()) == true) ? Convert.ToString(r[TransactionDetailsEnum.TransactionDescription.ToString()].ParseDbNull()) : string.Empty,
                        PointIssuePartnerGroupId = (transactionsDetailsTable.Columns.Contains(TransactionDetailsEnum.PointIssuePartnerGroupId.ToString()) == true) ? Convert.ToString(r[TransactionDetailsEnum.PointIssuePartnerGroupId.ToString()]) : string.Empty,
                        PointIssuePartnerGroupDesc = (transactionsDetailsTable.Columns.Contains(TransactionDetailsEnum.PointIssuePartnerGroupDesc.ToString()) == true) ? Convert.ToString(r[TransactionDetailsEnum.PointIssuePartnerGroupDesc.ToString()]) : string.Empty
                    });

                    transactionDetails.ToList();
                    customerTransactionDetails.Transactions = transactionDetails.ToList();
                }
                if (dsTransactionsAndOffer == null || dsTransactionsAndOffer.Tables.Count > 0)
                {
                    offerDetailsTable = dsTransactionsAndOffer.Tables["OfferDetails"];
                }
                OfferDetails offerDetails = new OfferDetails();
                if (offerDetailsTable != null)
                {
                    //--Add missing columns in table
                    offerDetailsTable.AddMissingColumns(typeof(OfferDetailsEnum));
                    DateTime dtStart, dtEnd;

                    DataRow dr = offerDetailsTable.Rows[0];
                    //review - Convert the date time according to the format AppConfiguration.Settings[AppConfigEnum.SpecifiedDateFormat];
                    offerDetails = new OfferDetails()
                    {
                        OfferID = Convert.ToInt32(dr[OfferDetailsEnum.OfferID.ToString()]),
                        StartDateTime = dr[OfferDetailsEnum.StartDateTime.ToString()].TryParse<string>().TryParseDate(out dtStart) ? dtStart.ToString("o") : String.Empty,
                        EndDateTime = dr[OfferDetailsEnum.EndDateTime.ToString()].TryParse<string>().TryParseDate(out dtEnd) ? dtEnd.ToString("o") : String.Empty,
                        PointsToRewardConversionRate = Convert.ToString(dr[OfferDetailsEnum.PointsToRewardConversionRate.ToString()]),
                        CollectionPeriodNumber = Convert.ToString(dr[OfferDetailsEnum.CollectionPeriodNumber.ToString()]),
                    };
                }
                customerTransactionDetails.Offer = offerDetails;

                return customerTransactionDetails;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception in Getting transactionbycustomerandofferid", ex, null);
            }
        }

        private List<ChristmasSaverSummary> GetChristmasSaverSummary(string dtStart, string dtEnd, string dateformat)
        {
            ChristmasSaverSummaryList christmasSaverDetailsList = new ChristmasSaverSummaryList();
            string resultXml;
            string errorXml;
            int maxRows = 0;
            int rowCount;
            
            var custInfo = this.GetCustInfo();
            if (custInfo == null)
            {
                throw new Exception("Customer details unavailable.");
            }

            try
            {
                DateTime dtStartDate = DateTime.MinValue;
                DateTime dtEndDate = DateTime.MinValue;
                DateTime dtTemp = DateTime.MinValue;

                if (!dtStart.TryParseDate(out dtStartDate) || !dtEnd.TryParseDate(out dtEndDate))
                {
                    throw new Exception("Invalid date range provided.");
                }

                if (!dtStartDate.ToString(dateformat).TryParseDate(out dtTemp))
                {
                    throw new Exception("Invalid date format provided.");
                }

                Hashtable XmasSaverSummary = new Hashtable();
                XmasSaverSummary["CustomerID"] = custInfo.ngccustomerid;
                XmasSaverSummary["StartDate"] = dtStartDate.ToString(dateformat);
                XmasSaverSummary["EndDate"] = dtEndDate.ToString(dateformat);
                string searchXML = Common.Utilities.GeneralUtility.HashTableToXML(XmasSaverSummary, "XmasSaver");

                bool bService = false;

                this._dtStart = DateTime.UtcNow;
                try
                {
                    bService = this._clubcardServiceClient.GetChristmasSaverSummary(
                                        out errorXml,
                                        out resultXml,
                                        out rowCount,
                                        searchXML,
                                        maxRows,
                                        base.Culture);
                }
                finally
                {
                    this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
                }

                this.HandleFailedResponse(bService, errorXml);

                if (!string.IsNullOrEmpty(resultXml) && resultXml != "<NewDataSet />")
                {
                    christmasSaverDetailsList.ConvertFromXml(resultXml);
                }
               
                return christmasSaverDetailsList.christmasSaverSummaryList;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception in Getting christmas saver summary", ex, null);
            }
        }

        #region CustomerActivationDetails Members

        private List<Offer> GetOffersForCustomer(string maxRowCount)
        {
            #region Local variables

            string resultXml, errorXml;
            int rowCount = 0;
            XmlDocument resulDoc = new XmlDocument();
            DataSet dsPointsInfo = new DataSet();
            bool isSuccessful = false;

            List<Offer> lstOffers = new List<Offer>();

            #endregion

            try
            {
                int iMaxRowCount = 0;

                var custInfo = this.GetCustInfo();
                if (custInfo == null)
                {
                    throw new Exception("Customer details unavailable.");
                }

                string conditionalXml = String.Format("<PointsInfoCondition><CustomerID>{0}</CustomerID></PointsInfoCondition>", custInfo.ngccustomerid);

                Int32.TryParse(maxRowCount, out iMaxRowCount);

                //load the customer latest collection period details into dataset
                //by passing the customerID and culture
                this._dtStart = DateTime.UtcNow;
                try
                {
                    isSuccessful = this._clubcardServiceClient.GetPointsForAllCollPeriodByCustomer(out errorXml, out resultXml, out rowCount, conditionalXml, iMaxRowCount, this.Culture);
                }
                finally
                {
                    this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
                }

                this.HandleFailedResponse(isSuccessful, errorXml);

                OfferList offers = new OfferList();
                if (resultXml != "" && resultXml != "<NewDataSet />")
                {
                    offers.ConvertFromXml(resultXml);
                }

                return offers.OfferDetailList;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception in Getting OffersForCustomer", ex, null);
            }
        }

        private PointsSummary GetPointsSummary(string previousOfferId)
        {
            string conditionalXml, resultXml = string.Empty, errorXml = string.Empty;
            int maxRowCount = 0, rowCount = 0;
            DataSet dsPointsSummaryRec = null;
            PointsSummaryList pointsSummaryList = new PointsSummaryList();
            bool isSuccessful = false;

            List<Offer> lstOffers = new List<Offer>();

            try
            {
                var custInfo = this.GetCustInfo();
                if (custInfo == null)
                {
                    throw new Exception("Customer details unavailable.");
                }

                long lCustomerID = custInfo.ngccustomerid.TryParse<long>();
                if (lCustomerID == default(long))
                {
                    throw new Exception("Parameter customerId is mandatory and must be passed for further processing.");
                }

                dsPointsSummaryRec = new DataSet();
                //Convert all input variables to xml
                Hashtable inputParams = new Hashtable();

                int iOfferID = 0;
                Int32.TryParse(previousOfferId, out iOfferID);

                inputParams["CustomerID"] = lCustomerID; //logged in customer id
                inputParams["OfferID"] = iOfferID; //offer id for the previous collection period

                conditionalXml = GeneralUtility.HashTableToXML(inputParams, "PointsSummaryCondition");
                //call the service function GetPointsSummaryInfo() to get Points summary record

                this._dtStart = DateTime.UtcNow;
                try
                {
                    isSuccessful = this._clubcardServiceClient.GetPointsSummaryInfo(out errorXml, out resultXml, out rowCount, conditionalXml, maxRowCount, this.Culture);
                }
                finally
                {
                    this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
                }

                this.HandleFailedResponse(isSuccessful, errorXml);

                if (!string.IsNullOrEmpty(resultXml))
                {
                    pointsSummaryList.ConvertFromXml(resultXml);
                }

                return pointsSummaryList.PointSummaryList.FirstOrDefault<PointsSummary>();
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception while getting Point Summary", ex, null);
            }
        }

        private CustomerTransactions GetCustomerTransactions(string offerId, bool showMerchantFlag)
        {
            try
            {
                var custInfo = this.GetCustInfo();
                if (custInfo == null)
                {
                    throw new Exception("Customer details unavailable.");
                }

                long lCustomerID = custInfo.ngccustomerid.TryParse<long>();
                if (lCustomerID == default(long))
                {
                    throw new Exception("Parameter customerId is mandatory and must be passed for further processing.");
                }

                // This step is not unnecessary. It has been added to validate the incoming string input.
                int iOfferID = 0;
                Int32.TryParse(offerId, out iOfferID);

                DataSet dsCustomerTransaction = this.GetTransactionDetailsByCustomerAndOfferIDDataset(lCustomerID, iOfferID.ToString(), showMerchantFlag, this.Culture);
                CustomerTransactions CustomerTransactions = this.GetTransactionDetailsByCustomerAndOfferIDDatasetToList(dsCustomerTransaction, lCustomerID);

                return CustomerTransactions;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception while getting Customer Transactions", ex, null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public AccountDetails GetCustomerAccountDetails()
        {
            AccountDetailsList accountDetailsList = new AccountDetailsList();
            try
            {
                var custInfo = this.GetCustInfo();
                if (custInfo == null)
                {
                    throw new Exception("Customer details unavailable.");
                }

                string resultXml;
                string errorXml;
                long lCustomerID = custInfo.ngccustomerid.TryParse<long>();

                if (lCustomerID == default(long))
                {
                    throw new Exception("Parameter customerId is mandatory and must be passed for further processing.");
                }

                bool bService = false;
                this._dtStart = DateTime.UtcNow;
                try
                {
                    bService = this._clubcardServiceClient.GetMyAccountDetails(out errorXml, out resultXml, lCustomerID, this.Culture);
                }
                finally
                {
                    this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
                }

                this.HandleFailedResponse(bService, errorXml);

                if (resultXml != "" && resultXml != "<NewDataSet />")
                {
                    accountDetailsList.ConvertFromXml(resultXml);
                }

                return accountDetailsList.AccountDetailList.FirstOrDefault<AccountDetails>();
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception in customeraccountdetails", ex, null);
            }
        }

        #endregion

        private OrderReplacementStatus GetOrderReplacementExistingStatus(string orderprocesswindow)
        {
            string resultXml;
            string errorXml;
            int rowCount = 0, maxRowCount = 1;
            XmlDocument resulDoc = new XmlDocument();
            DataSet dsOrderReplacement = new DataSet();
            Hashtable conditionParams = new Hashtable();
            
            try
            {
                var custInfo = this.GetCustInfo();
                if (custInfo == null)
                {
                    throw new Exception("Customer details unavailable.");
                }

                string conditionXml = String.Format("<CheckOrderReplacement><CustomerID>{0}</CustomerID><OrderProcessWindow>{1}</OrderProcessWindow></CheckOrderReplacement>", 
                                        custInfo.ngccustomerid, orderprocesswindow);

                bool isServiceSuccessful = false;
                this._dtStart = DateTime.UtcNow;
                try
                {
                    isServiceSuccessful = this._clubcardServiceClient.IsNewOrderReplacementValid(out errorXml, out resultXml,
                                                                             out rowCount, conditionXml, maxRowCount, this.Culture);
                }
                finally
                {
                    this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
                }

                this.HandleFailedResponse(isServiceSuccessful, errorXml);

                if (!string.IsNullOrEmpty(resultXml) && rowCount > 0)
                {
                    resulDoc.LoadXml(resultXml);
                    dsOrderReplacement.ReadXml(new XmlNodeReader(resulDoc));
                }

                if (dsOrderReplacement == null || dsOrderReplacement.Tables.Count == 0)
                {
                    return new OrderReplacementStatus();
                }
                OrderReplacementStatusList orderReplacement = new OrderReplacementStatusList();
                orderReplacement.ConvertFromDataset(dsOrderReplacement);
                return orderReplacement.OrderReplacementStatusEntity.FirstOrDefault<OrderReplacementStatus>();
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception while getting Order Replacement Existing Status", ex, null);
            }
        }

        private bool ProcessOrderReplacementRequest(string reason, string serviceConsumer, string orderprocesswindow)
        {
            string errorXml;
            long customerID;

            try
            {
                var custInfo = this.GetCustInfo();
                if (custInfo == null)
                {
                    throw new Exception("Customer details unavailable.");
                }

                OrderReplacementModel model = new OrderReplacementModel();

                model.CustomerId = custInfo.ngccustomerid.TryParse<long>();

                var orderStatus = this.GetOrderReplacementExistingStatus(orderprocesswindow);

                if (orderStatus != null)
                {
                    model.ClubcardNumber = orderStatus.StandardClubcardNumber.TryParse<long>();
                    model.Reason = reason;
                    model.RequestType = OrderReplacementTypeEnum.NewCardAndKeyFOB;
                }

                string modelXml = SerializerUtility<OrderReplacementModel>.GetSerializedString(model);
                if (model.ClubcardNumber > 0)
                {
                    bool isServiceSuccessful = false;
                    this._dtStart = DateTime.UtcNow;

                    try
                    {
                        isServiceSuccessful = this._clubcardServiceClient.AddNewOrderReplacement(out errorXml, 
                                                out customerID, modelXml, serviceConsumer);
                    }
                    finally
                    {
                        this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
                    }

                    this.HandleFailedResponse(isServiceSuccessful, errorXml);
                    return isServiceSuccessful;
                }
                else
                {
                    throw new Exception("No standard clubcard number found.");
                }
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception while processing Order Replacement Request", ex, null);
            }
        }

        private bool IsXmasClubMember()
        {
            DataSet dsXmasClubMember = new DataSet();
            string resultXml, errorXml;
            XmlDocument resulDoc = new XmlDocument();

            try
            {
                var custInfo = this.GetCustInfo();
                if (custInfo == null)
                {
                    throw new Exception("Customer details unavailable.");
                }

                long lCustomerID = custInfo.ngccustomerid.TryParse<long>();
                if (lCustomerID == default(long))
                {
                    throw new Exception("Parameter customerId is mandatory and must be passed for further processing.");
                }

                bool bService = false;
                this._dtStart = DateTime.UtcNow;
                try
                {
                    bService = this._clubcardServiceClient.IsXmasClubMember(out errorXml, out resultXml, lCustomerID, this.Culture);
                }
                finally
                {
                    this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
                }

                this.HandleFailedResponse(bService, errorXml);

                if (!string.IsNullOrEmpty(resultXml) && resultXml != "<NewDataSet/>")
                {
                    resulDoc.LoadXml(resultXml);
                    dsXmasClubMember.ReadXml(new XmlNodeReader(resulDoc));
                }

                if (dsXmasClubMember.Tables != null && dsXmasClubMember.Tables.Count == 1)
                {
                    return dsXmasClubMember.Tables[0].Rows[0][0].ToString() == "1";
                }

                return false;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception in XmasClubMember", ex, null);
            }
        }

        public Activation.CustomerActivationStatusdetails IGHSCheckCustomerActivatedStatus()
        {
            Activation.CustomerActivationStatusdetails customerActivationStatus = new Activation.CustomerActivationStatusdetails();
            char activated = '0';
            long customerID = 0;
            string resultXml = string.Empty;
            string errorXml = string.Empty;
            this._dtStart = DateTime.UtcNow;

            try
            {
                bool bService = false;

                try
                {
                    bService = this._clubcardServiceClient.IGHSCheckCustomerActivated(out activated, out customerID, out errorXml, out resultXml, this.Dotcomid, this.Culture);
                }
                finally
                {
                    this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
                }

                this.HandleFailedResponse(bService, errorXml);

                if (!String.IsNullOrWhiteSpace(resultXml) && resultXml != "<NewDataSet />")
                {
                    customerActivationStatus.ConvertFromXml(resultXml);
                    customerActivationStatus.DotcomID = this.Dotcomid;
                }
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception in checking IGHS Customer ActivatedStatus", ex, null);
            }

            return customerActivationStatus;
        }

        public List<HouseholdDetails> GetHouseHoldCustomersData()
        {
            string errorXml, resultXml;
            List<HouseholdDetails> houseHolds = new List<HouseholdDetails>();
            HouseholdDetailsList lstHouseHolds = new HouseholdDetailsList();

            try
            {
                var custInfo = this.GetCustInfo();
                if (custInfo == null)
                {
                    throw new Exception("Customer details unavailable.");
                }

                long lCustomerID = custInfo.ngccustomerid.TryParse<long>();
                if (lCustomerID == default(long))
                {
                    throw new Exception("Parameter customerID is mandatory and must be passed for further processing.");
                }

                bool bService = false;
                this._dtStart = DateTime.UtcNow;
                try
                {
                    bService = this._clubcardServiceClient.GetHouseholdCustomers(out errorXml, out resultXml, lCustomerID, this.Culture);
                }
                finally
                {
                    this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
                }

                this.HandleFailedResponse(bService, errorXml);

                if (resultXml != "" && resultXml != "<NewDataSet />")
                {
                    lstHouseHolds.ConvertFromXml(resultXml);
                }

                return lstHouseHolds.List;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception in Getting HouseHold Customers Data", ex, null);
            }
        }

        private List<Clubcard> GetClubcardsCustomerData()
        {
            string errorXml, resultXml;
            List<Clubcard> res = new List<Clubcard>();
            ClubcardDetailsList _clubcardDetailsList = new ClubcardDetailsList();

            try
            {
                var custInfo = this.GetCustInfo();
                if (custInfo == null)
                {
                    throw new Exception("Customer details unavailable.");
                }

                long lCustomerID = custInfo.ngccustomerid.TryParse<long>();
                if (lCustomerID == default(long))
                {
                    throw new Exception("Parameter customerID is mandatory and must be passed for further processing.");
                }

                bool bService = false;
                this._dtStart = DateTime.UtcNow;

                try
                {
                    bService = this._clubcardServiceClient.GetClubcardsCustomer(out errorXml, out resultXml, lCustomerID, this.Culture);
                }
                finally
                {
                    this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
                }

                this.HandleFailedResponse(bService, errorXml);

                if (resultXml != "" && resultXml != "<NewDataSet />")
                {
                    _clubcardDetailsList.ConvertFromXml(resultXml);
                    res = _clubcardDetailsList.List;

                    _clubcardDetailsList.List.ForEach(c =>
                    {
                        c.CardType = CardType.MyAccount;
                        custInfo.clubcards.Add(c.JsonText());
                    });
                }

                return res;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception in getting clubcard Customer Data", ex, null);
            }
        }

        public List<Clubcard> GetCustomerCards(string customerID)
        {
            string errorXml = String.Empty;
            string resultXml = String.Empty;

            long lCustomerID = customerID.TryParse<long>();
            if (lCustomerID == default(long))
            {
                return new List<Clubcard>();
            }

            bool bService = false;
            this._dtStart = DateTime.UtcNow;

            try
            {
                bService = this._clubcardServiceClient.GetClubcardsCustomer(out errorXml, out resultXml, lCustomerID, this.Culture);
            }
            finally
            {
                this._internalStats += DateTime.UtcNow.Subtract(this._dtStart).TotalMilliseconds;
            }

            this.HandleFailedResponse(bService, errorXml);

            if (resultXml != "" && resultXml != "<NewDataSet />")
            {
                ClubcardDetailsList clubcardDetailsList = new ClubcardDetailsList();
                clubcardDetailsList.ConvertFromXml(resultXml);
                if (clubcardDetailsList.List != null && clubcardDetailsList.List.Count > 0)
                {
                    return clubcardDetailsList.List;
                }
            }

            return new List<Clubcard>();
        }

        private List<Clubcard> GetHouseHoldCards()
        {
            return this.GetClubcardsByType(CardType.HouseHoldMembers);
        }

        #endregion Private Methods

        protected override CustomerInfo GetCustInfo()
        {
            CustomerInfo custInfo = base.GetCustInfo();

            if (custInfo == null)
            {
                var custActivationStatus = this.IGHSCheckCustomerActivatedStatus();

                custInfo = new CustomerInfo() { dotcomid = this.Dotcomid, uuid = this.UUID };
                custInfo.activationstatus = custActivationStatus.Activated;
                custInfo.ngccustomerid = custActivationStatus.CustomerId.ToString();
                long lCustomerID = custInfo.ngccustomerid.TryParse<long>();
                base.SaveCustomerInfoInCache(custInfo);
            }

            return base.GetCustInfo();
        }
     
        protected override List<Clubcard> GetClubcardsByType(CardType cType)
        {
            var custInfo = this.GetCustInfo();
            var cards = base.GetClubcardsByType(cType);

            if (cards == null || cards.Count < 1)
            {
                if (cType == CardType.HouseHoldMembers)
                {
                    var houseMembers = this.GetHouseHoldCustomersData();
                    foreach (var member in houseMembers)
                    {
                        this.GetCustomerCards(member.CustomerID).ForEach(c =>
                        {
                            if (!custInfo.clubcards.Any(cl => JsonConvert.DeserializeObject<Clubcard>(cl).ClubCardID == c.ClubCardID
                                                            && JsonConvert.DeserializeObject<Clubcard>(cl).CardType == cType))
                            {
                                c.CardType = CardType.HouseHoldMembers;
                                custInfo.clubcards.Add(c.JsonText());
                            }
                        });
                    }
                }

                if (cType == CardType.MyAccount || cType == CardType.AssociateClubcard)
                {
                    var accDetails = this.GetCustomerAccountDetails();
                    if (!custInfo.clubcards.Any(c => JsonConvert.DeserializeObject<Clubcard>(c).ClubCardID == accDetails.AssociateClubcardID
                                                    && JsonConvert.DeserializeObject<Clubcard>(c).CardType == CardType.AssociateClubcard))
                    {
                        Clubcard ccard = new Clubcard()
                        {
                            CardType = CardType.AssociateClubcard,
                            ClubCardID = accDetails.AssociateClubcardID
                        };
                        custInfo.clubcards.Add(ccard.JsonText());
                    }
                    if (!custInfo.clubcards.Any(c => JsonConvert.DeserializeObject<Clubcard>(c).ClubCardID == accDetails.ClubcardID
                                                    && JsonConvert.DeserializeObject<Clubcard>(c).CardType == CardType.MyAccount))
                    {
                        Clubcard ccard = new Clubcard()
                        {
                            CardType = CardType.MyAccount,
                            ClubCardID = accDetails.ClubcardID
                        };
                        custInfo.clubcards.Add(ccard.JsonText());
                    }
                }

                base.SaveCustomerInfoInCache(custInfo);

            }

            return custInfo.clubcards.Where(c => JsonConvert.DeserializeObject<Clubcard>(c).CardType == cType)
                    .Select<string, Clubcard>(c => JsonConvert.DeserializeObject<Clubcard>(c))
                    .ToList<Clubcard>();
        }
    }
}