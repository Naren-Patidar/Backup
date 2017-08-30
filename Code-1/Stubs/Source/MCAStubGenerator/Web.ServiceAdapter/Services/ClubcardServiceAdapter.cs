using System;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;
using System.ServiceModel;
using System.Collections.Generic;
using System.Collections;
//using System.Threading;
using System.Globalization;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.ClubcardService;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Activation = Tesco.ClubcardProducts.MCA.Web.Common.Entities.Activation;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Points;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.ChristmasSaver;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.OrderReplacement;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common;
//using System.Reflection;
//using Microsoft.Practices.ServiceLocation;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using Tesco.ClubcardProducts.MCA.Web.Common.Providers;
using Tesco.ClubcardProducts.MCA.Web.Common.ResponseRecorder;

namespace Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services
{

    /// <summary>
    /// 
    /// </summary>
    public class ClubcardServiceAdapter : IServiceAdapter
    {
        IClubcardService _clubcardServiceClient = new ClubcardServiceClient();
        private IConfigurationProvider _Config = new ConfigurationProvider();
        Recorder _recorder = null;

        public ClubcardServiceAdapter(Recorder recorder)
        {
            this._recorder = recorder;
        }

        #region IService Members

        public MCAResponse Get(MCARequest request)
        {
            MCAResponse res = new MCAResponse();
            List<string> blacklistData = new List<string>();

            if (request.Parameters.Keys.Contains(ParameterNames.MODEL))
            {
                OrderReplacementModel model = (OrderReplacementModel)request.Parameters[ParameterNames.MODEL];
                string clubcardnumber = model.ClubcardNumber.ToString();
                blacklistData.Add(clubcardnumber);
            }
            try
            {
                var operation = request.Parameters[ParameterNames.OPERATION_NAME].ToString();

                switch (operation)
                {
                    case OperationNames.GET_OFFERS_FOR_CUSTOMER:
                        if (request.Parameters.Keys.Contains(ParameterNames.CUSTOMER_ID_Points) &&
                            request.Parameters.Keys.Contains(ParameterNames.CONDITIONAL_XML) &&
                            request.Parameters.Keys.Contains(ParameterNames.CULTURE) &&
                            request.Parameters.Keys.Contains(ParameterNames.MAX_ROWS))
                        {
                            res.Data = this.GetOffersForCustomer(
                                request.Parameters[ParameterNames.CUSTOMER_ID_Points].TryParse<Int64>(),
                                request.Parameters[ParameterNames.CONDITIONAL_XML].TryParse<string>(),
                                request.Parameters[ParameterNames.CULTURE].TryParse<string>(),
                                request.Parameters[ParameterNames.MAX_ROWS].TryParse<Int32>());
                            res.Status = true;
                        }
                        break;
                    case OperationNames.GET_CUSTOMER_ACCOUNT_DETAILS:
                        if (request.Parameters.Keys.Contains(ParameterNames.CUSTOMER_ID) &&
                            request.Parameters.Keys.Contains(ParameterNames.CULTURE))
                        {
                            res.Data = this.GetCustomerAccountDetails(
                                                request.Parameters[ParameterNames.CUSTOMER_ID].TryParse<Int64>(),
                                                request.Parameters[ParameterNames.CULTURE].TryParse<string>());
                            res.Status = true;
                        }
                        break;
                    case OperationNames.IS_XMAS_CLUBMEMBER:
                        if (request.Parameters.Keys.Contains(ParameterNames.CUSTOMER_ID) &&
                            request.Parameters.Keys.Contains(ParameterNames.CULTURE))
                        {
                            res.Data = this.IsXmasClubMember(
                                                request.Parameters[ParameterNames.CUSTOMER_ID].TryParse<Int64>(),
                                                request.Parameters[ParameterNames.CULTURE].TryParse<string>());
                            res.Status = true;
                        }
                        break;
                    case OperationNames.GET_CHRISTMAS_SAVER_SUMMARY:
                        if (request.Parameters.Keys.Contains(ParameterNames.CUSTOMER_ID) &&
                            request.Parameters.Keys.Contains(ParameterNames.START_DATE) &&
                            request.Parameters.Keys.Contains(ParameterNames.END_DATE) &&
                            request.Parameters.Keys.Contains(ParameterNames.CULTURE))
                        {
                            res.Data = this.GetChristmasSaverSummary(
                                                        request.Parameters[ParameterNames.CUSTOMER_ID].TryParse<Int64>(),
                                                        request.Parameters[ParameterNames.START_DATE].TryParse<DateTime>(),
                                                        request.Parameters[ParameterNames.END_DATE].TryParse<DateTime>(),
                                                        request.Parameters[ParameterNames.CULTURE].TryParse<string>());
                            res.Status = true;
                        }
                        break;
                    case OperationNames.GET_MY_CUSTOMER_ACCOUNT_DETAILS:
                        if (request.Parameters.Keys.Contains(ParameterNames.CUSTOMER_ID)
                            && request.Parameters.Keys.Contains(ParameterNames.CULTURE))
                        {
                            res.Data = GetMyCustomerAccountDetails(
                                                    request.Parameters[ParameterNames.CUSTOMER_ID].TryParse<Int64>(),
                                                    request.Parameters[ParameterNames.CULTURE].TryParse<string>());
                            res.Status = true;
                        }
                        break;


                    case OperationNames.GET_CUSTOMER_ACTIVATION_STATUS_DETAILS:
                        if (request.Parameters.Keys.Contains(ParameterNames.DOTCOM_CUSTOMER_ID)
                            && request.Parameters.Keys.Contains(ParameterNames.CULTURE))
                        {
                            res.Data = IGHSCheckCustomerActivatedStatus(
                                                request.Parameters[ParameterNames.DOTCOM_CUSTOMER_ID].ToString(),
                                                request.Parameters[ParameterNames.CULTURE].ToString());
                            res.Status = true;
                        }
                        break;

                    case OperationNames.GET_HOUSEHOLD_CUSTOMER_DATA:
                        if (request.Parameters.Keys.Contains(ParameterNames.CUSTOMER_ID) && request.Parameters.Keys.Contains(ParameterNames.CULTURE))
                        {
                            res.Data = GetHouseHoldCustomersData(
                                                request.Parameters[ParameterNames.CUSTOMER_ID].TryParse<Int64>(),
                                                request.Parameters[ParameterNames.CULTURE].ToString());
                            res.Status = true;
                        }
                        break;
                    case OperationNames.GET_CLUBCARD_CUSTOMER_DATA:
                        if (request.Parameters.Keys.Contains(ParameterNames.CUSTOMER_ID) && request.Parameters.Keys.Contains(ParameterNames.CULTURE))
                        {
                            res.Data = GetClubcardsCustomerData(
                                                request.Parameters[ParameterNames.CUSTOMER_ID].TryParse<Int64>(),
                                                request.Parameters[ParameterNames.CULTURE].ToString());
                            res.Status = true;
                        }
                        break;

                    case OperationNames.GET_POINTS_SUMMARY:
                        if (request.Parameters.Keys.Contains(ParameterNames.CUSTOMER_ID) &&
                            request.Parameters.Keys.Contains(ParameterNames.OFFER_ID) &&
                            request.Parameters.Keys.Contains(ParameterNames.CULTURE))
                        {
                            res.Data = this.GetPointsSummary(
                                                    request.Parameters[ParameterNames.CUSTOMER_ID].TryParse<Int64>(),
                                                    request.Parameters[ParameterNames.OFFER_ID].TryParse<Int32>(),
                                                    request.Parameters[ParameterNames.CULTURE].TryParse<string>());
                            res.Status = true;
                        }
                        break;
                    case OperationNames.GET_CUSTOMER_TRANSACTIONS:
                        if (request.Parameters.Keys.Contains(ParameterNames.CUSTOMER_ID) &&
                            request.Parameters.Keys.Contains(ParameterNames.OFFER_ID) &&
                            request.Parameters.Keys.Contains(ParameterNames.MERCHANTFLAG) &&
                            request.Parameters.Keys.Contains(ParameterNames.CULTURE))
                        {
                            res.Data = this.GetCustomerTransactions(
                                            request.Parameters[ParameterNames.CUSTOMER_ID].TryParse<Int64>(),
                                            request.Parameters[ParameterNames.OFFER_ID].TryParse<Int32>(),
                                (request.Parameters[ParameterNames.MERCHANTFLAG]).TryParse<Boolean>(),
                                request.Parameters[ParameterNames.CULTURE].TryParse<String>());
                            res.Status = true;
                        }
                        break;
                    case OperationNames.GET_ORDER_REPLACEMENT_STATUS:
                        {
                            if (request.Parameters.Keys.Contains(ParameterNames.CUSTOMER_ID) && request.Parameters.Keys.Contains(ParameterNames.CULTURE))
                            {
                                res.Data = this.GetOrderReplacementExistingStatus((request.Parameters[ParameterNames.CUSTOMER_ID]).TryParse<Int64>(), request.Parameters[ParameterNames.CULTURE].ToString());
                                res.Status = true;
                            }
                        }
                        break;
                    case OperationNames.GET_PROCESS_ORDER_REPLACEMENT_REQUEST:
                        {
                            if (request.Parameters.Keys.Contains(ParameterNames.MODEL))
                            {
                                res.Data = this.ProcessOrderReplacementRequest((OrderReplacementModel)request.Parameters[ParameterNames.MODEL]);
                                res.Status = true;
                            }
                        }
                        break;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return res;
        }

        public Common.ResponseRecorder.Recorder GetRecorder()
        {
            return this._recorder;
        }

        #endregion

        #region Constructors

        public ClubcardServiceAdapter(IClubcardService clubcardServiceClient)
        {
            _clubcardServiceClient = clubcardServiceClient;
        }

        #endregion Constructors

        #region Private Methods

        public DataSet GetHouseHoldCustomersDataSet(long customerID, string culture)
        {
            string errorXml, resultXml;
            DataSet dsHHCustomers = new DataSet();
            try
            {
                if (_clubcardServiceClient.GetHouseholdCustomers(out errorXml, out resultXml, customerID, culture))
                {
                    this._recorder.RecordResponse(new RecordLog { Error = errorXml, Result = resultXml },
                        Tesco.ClubcardProducts.MCA.Web.Common.ResponseRecorder.Services.ClubCardService.ToString(),
                        "GetHouseholdCustomers", ResponseType.Xml);

                    XmlDocument resulDoc = new XmlDocument();
                    resulDoc.LoadXml(resultXml);
                    dsHHCustomers.ReadXml(new XmlNodeReader(resulDoc));

                    if (dsHHCustomers.Tables["HouseholdCustomers"].Columns.Contains("CustomerUseStatusID") == false)
                    {
                        dsHHCustomers.Tables["HouseholdCustomers"].Columns.Add("CustomerUseStatusID");
                    }
                    else if (!string.IsNullOrEmpty(errorXml))
                    {
                    }

                }
                return dsHHCustomers;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private DataSet GetClubcardsCustomerDataSet(long customerID, string culture)
        {
            string errorXml, resultXml;
            DataSet dsHHCustomers = new DataSet();
            try
            {
                if (_clubcardServiceClient.GetClubcardsCustomer(out errorXml, out resultXml, customerID, culture))
                {
                    this._recorder.RecordResponse(new RecordLog { Error = errorXml, Result = resultXml },
                        Tesco.ClubcardProducts.MCA.Web.Common.ResponseRecorder.Services.ClubCardService.ToString(),
                        "GetClubcardsCustomer", ResponseType.Xml);

                    if (resultXml != "" && resultXml != "<NewDataSet />")
                    {
                        XmlDocument resulDoc = new XmlDocument();
                        resulDoc.LoadXml(resultXml);

                        dsHHCustomers.ReadXml(new XmlNodeReader(resulDoc));

                        if (dsHHCustomers.Tables["ClubcardDetails"].Columns.Contains("TransactionDateTime") == false)
                        {
                            dsHHCustomers.Tables["ClubcardDetails"].Columns.Add("TransactionDateTime");
                        }

                        if (dsHHCustomers.Tables["ClubcardDetails"].Columns.Contains("StoreName") == false)
                        {
                            dsHHCustomers.Tables["ClubcardDetails"].Columns.Add("StoreName");
                        }

                        if (dsHHCustomers.Tables["ClubcardDetails"].Columns.Contains("TransactionType") == false)
                        {
                            dsHHCustomers.Tables["ClubcardDetails"].Columns.Add("TransactionType");
                        }

                    }
                    else if (!string.IsNullOrEmpty(errorXml))
                    {
                    }
                }
                return dsHHCustomers;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DataSet GetMyAccountDetailsDataset(long customerID, string culture)
        {
            string errorXml = string.Empty;
            string resultXml = string.Empty;
            XmlDocument resulDoc = null;
            DataSet dsMyAccountDetails = null;

            try
            {
                if (_clubcardServiceClient.GetMyAccountDetails(out errorXml, out resultXml, customerID, culture))
                {
                    if (resultXml != "" && resultXml != "<NewDataSet />")
                    {
                        dsMyAccountDetails = new DataSet();
                        resulDoc = new XmlDocument();
                        resulDoc.LoadXml(resultXml);
                        dsMyAccountDetails.ReadXml(new XmlNodeReader(resulDoc));
                    }
                    else if (!string.IsNullOrEmpty(errorXml))
                    {
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsMyAccountDetails;

        }

        private List<CustomerMLS_PDF_DownloadDetails> GetMyAccountDetailsDatasetToList(DataSet dsCustomer)
        {
            string customerxml = SerializerUtility<DataSet>.GetSerializedString(dsCustomer);
            DataTable customerDetailsTable = dsCustomer.Tables[0];
            if (customerDetailsTable == null)
            {
                return new List<CustomerMLS_PDF_DownloadDetails>();
            }
            try
            {
                //--Add missing columns in table
                customerDetailsTable.AddMissingColumns(typeof(CustomerMLS_PDF_DownloadDetailsEnum));

                IEnumerable<CustomerMLS_PDF_DownloadDetails> customerDetails = customerDetailsTable.AsEnumerable().Select(r => new CustomerMLS_PDF_DownloadDetails()
                {
                    PrimaryCustName1 = Convert.ToString(r[CustomerMLS_PDF_DownloadDetailsEnum.PrimaryCustName1.ToString()]),
                    PrimaryCustName2 = Convert.ToString(r[CustomerMLS_PDF_DownloadDetailsEnum.PrimaryCustName2.ToString()]),
                    PrimaryCustName3 = Convert.ToString(r[CustomerMLS_PDF_DownloadDetailsEnum.PrimaryCustName3.ToString()]),
                    // PrimaryClubcardId = Convert.ToInt64(r[CustomerMLS_PDF_DownloadDetailsEnum.PrimaryClubcardID.ToString()]),
                    PrimaryClubcardId = GeneralUtility.GetInt64Value(r, CustomerMLS_PDF_DownloadDetailsEnum.PrimaryClubcardID.ToString()),
                    AssociateCustName1 = Convert.ToString(r[CustomerMLS_PDF_DownloadDetailsEnum.AssociateCustName1.ToString()]),
                    AssociateCustName2 = Convert.ToString(r[CustomerMLS_PDF_DownloadDetailsEnum.AssociateCustName2.ToString()]),
                    AssociateCustName3 = Convert.ToString(r[CustomerMLS_PDF_DownloadDetailsEnum.AssociateCustName3.ToString()]),
                    // AssociateClubcardId = Convert.ToInt64(r[CustomerMLS_PDF_DownloadDetailsEnum.AssociateClubcardID.ToString()]),
                    AssociateClubcardId = GeneralUtility.GetInt64Value(r, CustomerMLS_PDF_DownloadDetailsEnum.AssociateClubcardID.ToString())
                });
                return customerDetails.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private List<HouseholdCustomerDetails> GetHouseHoldCustomers(DataSet dsHHCustomers, long customerID)
        {
            DataTable customersDetailsTable = dsHHCustomers.Tables["HouseholdCustomers"];

            try
            {
                if (customersDetailsTable == null)
                    return new List<HouseholdCustomerDetails>();

                //--Add missing columns in table
                customersDetailsTable.AddMissingColumns(typeof(HouseholdCustomerDetailsEnum));

                IEnumerable<HouseholdCustomerDetails> customersDetails = customersDetailsTable.AsEnumerable().Select(r => new HouseholdCustomerDetails()
                {
                    PrimaryCustomerID = Convert.ToInt64(r[HouseholdCustomerDetailsEnum.PrimaryCustomerID.ToString()]),
                    CustomerID = Convert.ToInt64(r[HouseholdCustomerDetailsEnum.CustomerID.ToString()]),
                    TitleEnglish = Convert.ToString(r[HouseholdCustomerDetailsEnum.TitleEnglish.ToString()]),
                    Name1 = Convert.ToString(r[HouseholdCustomerDetailsEnum.Name1.ToString()]),
                    Name2 = Convert.ToString(r[HouseholdCustomerDetailsEnum.Name2.ToString()]),
                    Name3 = Convert.ToString(r[HouseholdCustomerDetailsEnum.Name3.ToString()]),
                    MailingAddressLine1 = Convert.ToString(r[HouseholdCustomerDetailsEnum.MailingAddressLine1.ToString()]),
                    MailingAddressPostCode = Convert.ToString(r[HouseholdCustomerDetailsEnum.MailingAddressPostCode.ToString()]),
                    // CustomerUseStatusID = Convert.ToInt32(r[HouseholdCustomerDetailsEnum.CustomerUseStatusID.ToString()]),          //assigns 0 by default if it is null
                    CustomerUseStatusID = GeneralUtility.GetInt32Value(r, HouseholdCustomerDetailsEnum.CustomerUseStatusID.ToString()),          //assigns 0 by default if it is null
                    //CustomerMailStatus = Convert.ToInt32(r[HouseholdCustomerDetailsEnum.CustomerMailStatus.ToString()])
                    CustomerMailStatus = GeneralUtility.GetInt32Value(r, HouseholdCustomerDetailsEnum.CustomerMailStatus.ToString())
                });
                return customersDetails.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DataSet GetTransactionDetailsByCustomerAndOfferIDDataset(long customerId, int offerId, bool showMerchantFlag, string culture)
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
                isServiceSuccessful = _clubcardServiceClient.GetTxnDetailsByCustomerAndOfferID(out errorXml, out resultXml, out rowCount,
                                                                   conditionalXml, maxRowCount,
                                                                   culture);

                this._recorder.RecordResponse(new RecordLog { Error = errorXml, Result = resultXml, OfferID = offerId },
                       Tesco.ClubcardProducts.MCA.Web.Common.ResponseRecorder.Services.ClubCardService.ToString(),
                       "GetTxnDetailsByCustomerAndOfferID", ResponseType.Xml);

                if (isServiceSuccessful && string.IsNullOrEmpty(errorXml))
                {
                    if (!string.IsNullOrEmpty(resultXml))
                    {
                        //Load the result xml containing parameters into a data set if the xml is not empty
                        resulDoc.LoadXml(resultXml);
                        dsTransactions.ReadXml(new XmlNodeReader(resulDoc));
                    }
                }
                else if (!string.IsNullOrEmpty(errorXml))
                {
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
                    return new CustomerTransactions();

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
                        ClubCardTransactionId = Convert.ToInt64(r[TransactionDetailsEnum.ClubCardTransactionId.ToString()]),
                        ClubcardId = Convert.ToString(r[TransactionDetailsEnum.ClubcardId.ToString()]),
                        ClubCardStatusDescEnglish = Convert.ToString(r[TransactionDetailsEnum.ClubCardStatusDescEnglish.ToString()]),
                        CustType = Convert.ToString(r[TransactionDetailsEnum.CustType.ToString()]),
                        TransactionDateTime = Convert.ToString(r[TransactionDetailsEnum.TransactionDateTime.ToString()]),
                        PartnerId = (transactionsDetailsTable.Columns.Contains(TransactionDetailsEnum.PartnerId.ToString()) == true) ? Convert.ToInt64(r[TransactionDetailsEnum.PartnerId.ToString()].ParseDbNull()) : 0,
                        TescoStoreId = (transactionsDetailsTable.Columns.Contains(TransactionDetailsEnum.TescoStoreId.ToString()) == true) ? Convert.ToInt64(r[TransactionDetailsEnum.TescoStoreId.ToString()].ParseDbNull()) : 0,
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

                    DataRow dr = offerDetailsTable.Rows[0];
                    //review - Convert the date time according to the format AppConfiguration.Settings[AppConfigEnum.SpecifiedDateFormat];
                    offerDetails = new OfferDetails()
                    {
                        OfferID = Convert.ToInt32(dr[OfferDetailsEnum.OfferID.ToString()]),
                        StartDateTime = Convert.ToDateTime(dr[OfferDetailsEnum.StartDateTime.ToString()]),
                        EndDateTime = Convert.ToDateTime(dr[OfferDetailsEnum.EndDateTime.ToString()]),
                        PointsToRewardConversionRate = Convert.ToString(dr[OfferDetailsEnum.PointsToRewardConversionRate.ToString()]),
                        CollectionPeriodNumber = Convert.ToString(dr[OfferDetailsEnum.CollectionPeriodNumber.ToString()]),
                    };
                }
                customerTransactionDetails.Offer = offerDetails;
                return customerTransactionDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Private Methods

        #region ICustomerDetails Members

        private List<HouseholdCustomerDetails> GetHouseHoldCustomers(long customerID, string culture)
        {
            try
            {
                DataSet dsHHCustomers = GetHouseHoldCustomersDataSet(customerID, culture);
                List<HouseholdCustomerDetails> customersDetails = GetHouseHoldCustomers(dsHHCustomers, customerID);
                return customersDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private CustomerTransactions GetTransactionDetailsByCustomerAndOfferID(long customerId, int offerId, bool showMerchantFlag, string culture)
        {
            try
            {
                DataSet dsTransactionAndOffer = GetTransactionDetailsByCustomerAndOfferIDDataset(customerId, offerId, showMerchantFlag, culture);
                CustomerTransactions customerTransacitons = GetTransactionDetailsByCustomerAndOfferIDDatasetToList(dsTransactionAndOffer, customerId);
                return customerTransacitons;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<Clubcard> GetMyAccountDetails(long customerID, string culture)
        {
            try
            {
                DataSet dsHHClubcards = GetMyAccountDetailsDataset(customerID, culture);
                List<Clubcard> clubcardsDetails = GetMyAccountDetailsEntities(dsHHClubcards, customerID);
                return clubcardsDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<Clubcard> GetMyAccountDetailsEntities(DataSet dsCustomer, long customerID)
        {
            try
            {
                DataTable clubcardDetailsTable = dsCustomer.Tables["ClubcardDetails"];

                if (clubcardDetailsTable == null)
                    return new List<Clubcard>();

                //--Add missing columns in table
                clubcardDetailsTable.AddMissingColumns(typeof(ClubcardEnum));

                IEnumerable<Clubcard> clubcardsDetails = clubcardDetailsTable.AsEnumerable().Select(r => new Clubcard()
                {
                    ClubCardID = Convert.ToInt64(r[ClubcardEnum.ClubCardID.ToString()].ParseDbNull()),
                    //ClubcardType = r[ClubcardEnum.ClubcardType.ToString()].ToNullableInt(),
                    ClubcardType = GeneralUtility.GetInt32Value(r, ClubcardEnum.ClubcardType.ToString()),
                    ClubCardTypeDesc = Convert.ToString(r[ClubcardEnum.ClubCardTypeDesc.ToString()].ParseDbNull()),
                    //TransactionDateTime = Convert.ToDateTime(r[ClubcardEnum.TransactionDateTime.ToString()].ParseDbNull()),
                    TransactionDateTime = GeneralUtility.GetDateTimeValue(r, ClubcardEnum.TransactionDateTime.ToString()),
                    //TransactionType = r[ClubcardEnum.TransactionType.ToString()].ToNullableInt(),
                    TransactionType = GeneralUtility.GetInt32Value(r, ClubcardEnum.TransactionType.ToString()),
                    ClubcardStatusDescEnglish = Convert.ToString(r[ClubcardEnum.ClubcardStatusDescEnglish.ToString()].ParseDbNull()),
                    //CardIssuedDate = Convert.ToDateTime(r[ClubcardEnum.CardIssuedDate.ToString()].ParseDbNull()),
                    StoreName = Convert.ToString(r[ClubcardEnum.StoreName.ToString()].ParseDbNull())
                });
                return clubcardsDetails.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ChristmasSaverSummary> GetChristmasSaverSummary(long customerID, DateTime startDate, DateTime endDate, string culture)
        {
            List<ChristmasSaverSummary> christmasSaverDetails = new List<ChristmasSaverSummary>();
            ChristmasSaverSummaryList christmasSaverDetailsList = new ChristmasSaverSummaryList();
            string resultXml;
            string errorXml;
            Hashtable XmasSaverSummary = new Hashtable();
            int maxRows = 0;
            int rowCount;
            //already captured in Get call in for m of Requets object
            //logData.RecordStep(String.Format("Start date:{0}, End date:{1}", startDate, endDate));
            try
            {
                XmasSaverSummary["CustomerID"] = customerID;
                XmasSaverSummary["StartDate"] = startDate;
                XmasSaverSummary["EndDate"] = endDate;
                string searchXML = Common.Utilities.GeneralUtility.HashTableToXML(XmasSaverSummary, "XmasSaver");
                if (_clubcardServiceClient.GetChristmasSaverSummary(out errorXml, out resultXml, out rowCount, searchXML, maxRows, culture))
                {
                    this._recorder.RecordResponse(new RecordLog { Error = errorXml, Result = resultXml, RowCount = rowCount },
                        Tesco.ClubcardProducts.MCA.Web.Common.ResponseRecorder.Services.ClubCardService.ToString(),
                        "GetChristmasSaverSummary", ResponseType.Xml);

                    if (!string.IsNullOrEmpty(resultXml) && resultXml != "<NewDataSet />")
                    {
                        christmasSaverDetailsList.ConvertFromXml(resultXml);
                    }
                }
                else if (!string.IsNullOrEmpty(errorXml))
                {
                }
                return christmasSaverDetailsList.christmasSaverSummaryList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region CustomerActivationDetails Members

        private List<Offer> GetOffersForCustomer(long customerId, string conditionalXml, string culture, int maxRowCount)
        {
            #region Local variables

            string resultXml, errorXml;
            int rowCount = 0;
            XmlDocument resulDoc = new XmlDocument();
            DataSet dsPointsInfo = new DataSet();
            bool isSuccessful = false;
            List<Offer> lstOffers = new List<Offer>();

            #endregion

            //load the customer latest collection period details into dataset
            //by passing the customerID and culture
            isSuccessful = _clubcardServiceClient.GetPointsForAllCollPeriodByCustomer(out errorXml, out resultXml, out rowCount, conditionalXml, maxRowCount, culture);

            this._recorder.RecordResponse(new RecordLog { Error = errorXml, Result = resultXml, RowCount = rowCount }, 
                        Tesco.ClubcardProducts.MCA.Web.Common.ResponseRecorder.Services.ClubCardService.ToString(),
                        "GetPointsForAllCollPeriodByCustomer", ResponseType.Xml);
            
            OfferList offers = new OfferList();
            if (isSuccessful && string.IsNullOrEmpty(errorXml))
            {
                if (resultXml != "" && resultXml != "<NewDataSet />")
                {
                    offers.ConvertFromXml(resultXml);
                }
            }
            else if (!string.IsNullOrEmpty(errorXml))
            {
            }

            //return lstOffers;
            return offers.OfferDetailList;
        }

        private void CloseServiceConnection(ClubcardServiceClient clubcardServiceClient)
        {
            if (clubcardServiceClient != null)
            {
                if (clubcardServiceClient.State == CommunicationState.Faulted)
                {
                    clubcardServiceClient.Abort();
                }
                else if (clubcardServiceClient.State != CommunicationState.Closed)
                {
                    clubcardServiceClient.Close();
                }
            }
        }

        private PointsSummary GetPointsSummary(long customerId, int previousOfferId, string culture)
        {
            #region Local variables

            string conditionalXml, resultXml = string.Empty, errorXml = string.Empty;
            int maxRowCount = 0, rowCount = 0;
            DataSet dsPointsSummaryRec = null;
            PointsSummaryList pointsSummaryList = new PointsSummaryList();
            bool isSuccessful = false;
            LogData logData = new LogData();
            List<Offer> lstOffers = new List<Offer>();
            #endregion

            dsPointsSummaryRec = new DataSet();
            //Convert all input variables to xml
            Hashtable inputParams = new Hashtable();

            inputParams["CustomerID"] = customerId; //logged in customer id
            inputParams["OfferID"] = previousOfferId; //offer id for the previous collection period

            conditionalXml = GeneralUtility.HashTableToXML(inputParams, "PointsSummaryCondition");
            //call the service function GetPointsSummaryInfo() to get Points summary record
            isSuccessful = _clubcardServiceClient.GetPointsSummaryInfo(out errorXml, out resultXml, out rowCount, conditionalXml, maxRowCount, culture);

            this._recorder.RecordResponse(new RecordLog { Error = errorXml, Result = resultXml, RowCount = rowCount }, 
                Tesco.ClubcardProducts.MCA.Web.Common.ResponseRecorder.Services.ClubCardService.ToString(),
                    "GetPointsSummaryInfo", ResponseType.Xml);

            //If service is successful load the xml into the dsPointsSummaryRec dataset
            if (isSuccessful && string.IsNullOrEmpty(errorXml))
            {
                if (!string.IsNullOrEmpty(resultXml))
                {
                    pointsSummaryList.ConvertFromXml(resultXml);
                }
            }
            else if (!string.IsNullOrEmpty(errorXml))
            {
                logData.CaptureData("errorXml", errorXml);
            }
            return pointsSummaryList.PointSummaryList.FirstOrDefault<PointsSummary>();
        }

        private CustomerTransactions GetCustomerTransactions(long customerId, int offerId, bool showMerchantFlag, string culture)
        {
            DataSet dsCustomerTransaction = GetTransactionDetailsByCustomerAndOfferIDDataset(customerId, offerId, showMerchantFlag, culture);
            CustomerTransactions CustomerTransactions = GetTransactionDetailsByCustomerAndOfferIDDatasetToList(dsCustomerTransaction, customerId);
            return CustomerTransactions;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        private AccountDetails GetCustomerAccountDetails(long customerId, string culture)
        {
            AccountDetailsList accountDetailsList = new AccountDetailsList();

            string resultXml;
            string errorXml;
            if (_clubcardServiceClient.GetMyAccountDetails(out errorXml, out resultXml, customerId, culture))
            {
                this._recorder.RecordResponse(new RecordLog { Error = errorXml, Result = resultXml }, 
                Tesco.ClubcardProducts.MCA.Web.Common.ResponseRecorder.Services.ClubCardService.ToString(),
                "GetMyAccountDetails", ResponseType.Xml);

                if (resultXml != "" && resultXml != "<NewDataSet />")
                {
                    accountDetailsList.ConvertFromXml(resultXml);
                }
            }

            return accountDetailsList.AccountDetailList.FirstOrDefault<AccountDetails>();
        }

        private bool IsFuelSaverAccountExists(string clubCardId)
        {
            GetFuelBalanceRequest objReq = null;
            GetFuelBalanceResponse objResp = null;
            objReq = new GetFuelBalanceRequest();

            try
            {
                objReq.AccountID = clubCardId;
                objReq.AccountIdType = AccountIDType.ClubcardID;
                objResp = new GetFuelBalanceResponse();
                objResp = _clubcardServiceClient.GetFuelSaverPointsBalance(objReq);
                if (objResp.StatusCode == 1 && objResp.IsAccountExist)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private AccountDetails GetMyCustomerAccountDetails(long customerId, string culture)
        {
            AccountDetails accountDetails = new AccountDetails();
            AccountDetailsList accountDetailsLst = new AccountDetailsList();
            try
            {
                string resultXml;
                string errorXml;
                if (_clubcardServiceClient.GetMyAccountDetails(out errorXml, out resultXml, customerId, culture))
                {
                    if (!string.IsNullOrEmpty(resultXml))
                    {
                        accountDetailsLst.ConvertFromXml(resultXml);
                        if (accountDetailsLst.AccountDetailList.Count > 0)
                        {
                            accountDetails = accountDetailsLst.AccountDetailList[0];
                        }
                    }
                }
                if (!string.IsNullOrEmpty(errorXml))
                {
                }
                
                return accountDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        private DataSet GetOrderReplacementExistingStatusDataSet(long customerId)
        {
            string resultXml;
            string errorXml;
            int rowCount = 0, maxRowCount = 1;
            XmlDocument resulDoc = new XmlDocument();
            DataSet dsOrderReplacement = new DataSet();

            Hashtable conditionParams = new Hashtable();
            conditionParams["CustomerID"] = customerId;
            conditionParams["OrderProcessWindow"] = _Config.GetStringAppSetting(AppConfigEnum.ORDRPL_PROCESSWINDOW);

            //load the xml string with hashtable values
            string conditionXml = GeneralUtility.HashTableToXML(conditionParams, "CheckOrderReplacement");
            bool isServiceSuccessful = _clubcardServiceClient.IsNewOrderReplacementValid(out errorXml, out resultXml,
                                                     out rowCount, conditionXml, maxRowCount,
                                                     "en-GB");

            this._recorder.RecordResponse(new RecordLog { Error = errorXml, Result = resultXml, RowCount = rowCount },
                    Tesco.ClubcardProducts.MCA.Web.Common.ResponseRecorder.Services.ClubCardService.ToString(),
                    "IsNewOrderReplacementValid", ResponseType.Xml);

            if (!string.IsNullOrEmpty(errorXml))
            {
                throw new Exception(errorXml);
            }


            if (!string.IsNullOrEmpty(resultXml) && rowCount > 0)
            {
                resulDoc.LoadXml(resultXml);
                dsOrderReplacement.ReadXml(new XmlNodeReader(resulDoc));
            }

            return dsOrderReplacement;
        }

        private OrderReplacementStatus GetOrderReplacementExistingStatusEntity(DataSet dsOrderReplacement, long customerId)
        {
            DataTable orderReplacementTable = new DataTable();
            OrderReplacementStatus status = null;
            string ordreplacementxml = SerializerUtility<DataSet>.GetSerializedString(dsOrderReplacement);
            LogData logData = new LogData();
            List<string> blacklistData = new List<string>();
            try
            {
                if (dsOrderReplacement == null && dsOrderReplacement.Tables.Count == 0)
                {
                    logData.RecordStep("Order Replacement Data doesn't exist");
                    
                    return null;
                }

                orderReplacementTable = dsOrderReplacement.Tables[0];

                if (orderReplacementTable == null)
                {
                    logData.RecordStep("Order Replacement Data doesn't exist");
                    
                    return null;
                }

                //--Add missing columns in table
                orderReplacementTable.AddMissingColumns(typeof(OrderReplacementStatusEnum));

                DataRow dr = orderReplacementTable.Rows[0];

                status = new OrderReplacementStatus()
                {
                    // StandardClubcardNumber = Convert.ToInt64(dr[OrderReplacementStatusEnum.standardCardNumber.ToString()]),
                    StandardClubcardNumber = GeneralUtility.GetInt64Value(dr, OrderReplacementStatusEnum.standardCardNumber.ToString()),
                    ClubcardTypeIndicator = Convert.ToString(dr[OrderReplacementStatusEnum.ClubcardTypeIndicatior.ToString()]),
                    //NumDaysLeftToProcess = Convert.ToInt32(dr[OrderReplacementStatusEnum.noOfDaysLeftToProcess.ToString()]),
                    NumDaysLeftToProcess = GeneralUtility.GetInt32Value(dr, OrderReplacementStatusEnum.noOfDaysLeftToProcess.ToString()),
                    //NumOrdersPlacedInYear = Convert.ToInt32(dr[OrderReplacementStatusEnum.countOrdersPlacedInYear.ToString()]),
                    NumOrdersPlacedInYear = GeneralUtility.GetInt32Value(dr, OrderReplacementStatusEnum.countOrdersPlacedInYear.ToString()),
                    OldOrderExists = Convert.ToString(dr[OrderReplacementStatusEnum.oldOrderExists.ToString()]) != "0",
                };


                if (string.IsNullOrEmpty(status.StandardClubcardNumber.ToString()))
                {
                    blacklistData.Add(status.StandardClubcardNumber.ToString());
                    logData.BlackLists = blacklistData;
                }
                logData.CaptureData("Response Object", status);
                
                return status;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception in GetOrderReplacementExistingStatusEntity", ex,
             new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY,     logData         }
                            });
            }
        }

        private OrderReplacementStatus GetOrderReplacementExistingStatus(long customerId, string culture)
        {
            string resultXml;
            string errorXml;
            int rowCount = 0, maxRowCount = 1;
            XmlDocument resulDoc = new XmlDocument();
            DataSet dsOrderReplacement = new DataSet();
            LogData logData = new LogData();
            Hashtable conditionParams = new Hashtable();
            try
            {

                conditionParams["CustomerID"] = customerId;
                //conditionParams["OrderProcessWindow"] = _Config.GetStringAppSetting(AppConfigEnum.ORDRPL_PROCESSWINDOW);
                conditionParams["OrderProcessWindow"] = 14;

                string conditionXml = GeneralUtility.HashTableToXML(conditionParams, "CheckOrderReplacement");
                logData.CaptureData("conditionXml", conditionXml);
                bool isServiceSuccessful = _clubcardServiceClient.IsNewOrderReplacementValid(out errorXml, out resultXml,
                                                         out rowCount, conditionXml, maxRowCount, culture);

                this._recorder.RecordResponse(new RecordLog { Error = errorXml, Result = resultXml, RowCount = rowCount }, 
                    Tesco.ClubcardProducts.MCA.Web.Common.ResponseRecorder.Services.ClubCardService.ToString(),
                    "IsNewOrderReplacementValid", ResponseType.Xml);

                if (!string.IsNullOrEmpty(errorXml))
                {
                    logData.CaptureData("errorXml", errorXml);
                    
                    throw new Exception(errorXml);
                }

                if (!string.IsNullOrEmpty(resultXml) && rowCount > 0)
                {
                    resulDoc.LoadXml(resultXml);
                    dsOrderReplacement.ReadXml(new XmlNodeReader(resulDoc));
                }
                if (dsOrderReplacement == null || dsOrderReplacement.Tables.Count == 0)
                {
                    
                    logData.RecordStep("Order Replacement Data is empty");
                    return new OrderReplacementStatus();
                }
                OrderReplacementStatusList orderReplacement = new OrderReplacementStatusList();
                orderReplacement.ConvertFromDataset(dsOrderReplacement);
                return orderReplacement.OrderReplacementStatusEntity.FirstOrDefault<OrderReplacementStatus>();

            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception while getting Order Replacement Existing Status", ex,
             new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
        }

        private bool ProcessOrderReplacementRequest(OrderReplacementModel model)
        {
            string modelXml = SerializerUtility<OrderReplacementModel>.GetSerializedString(model);
            string errorXml;
            DataSet dsOrderReplacement = new DataSet();
            long customerID;

            bool isServiceSuccessful = _clubcardServiceClient.AddNewOrderReplacement(out errorXml, out customerID, modelXml, _Config.GetStringAppSetting(AppConfigEnum.ServiceConsumer));

            this._recorder.RecordResponse(new RecordLog { Error = errorXml, CustomerID = customerID}, 
                Tesco.ClubcardProducts.MCA.Web.Common.ResponseRecorder.Services.ClubCardService.ToString(),
                "AddNewOrderReplacement", ResponseType.Xml);

            if (!string.IsNullOrEmpty(errorXml))
            {
                throw new Exception(errorXml);
            }

            return isServiceSuccessful;
        }

        private GetFuelBalanceResponse GetFuelSaverPointsBalance(long customerId)
        {
            LogData logData = new LogData();
            GetFuelBalanceRequest fuelSaverBalaneRequest = new GetFuelBalanceRequest() { AccountID = customerId.ToString(), AccountIdType = AccountIDType.CustomerID };

            try
            {
                GetFuelBalanceResponse fuelSaverPointsBalance = _clubcardServiceClient.GetFuelSaverPointsBalance(fuelSaverBalaneRequest);
                logData.CaptureData("Response Object", fuelSaverPointsBalance);
                
                return fuelSaverPointsBalance;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception in getting FuelSaverPointsBalance", ex,
            new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }

        }

        private bool IsXmasClubMember(long customerId, string culture)
        {
            DataSet dsXmasClubMember = new DataSet();
            string resultXml, errorXml;
            XmlDocument resulDoc = new XmlDocument();
            //To call the service.
            if (_clubcardServiceClient.IsXmasClubMember(out errorXml, out resultXml, customerId, culture))
            {
                this._recorder.RecordResponse(new RecordLog { Error = errorXml, Result = resultXml },
                        Tesco.ClubcardProducts.MCA.Web.Common.ResponseRecorder.Services.ClubCardService.ToString(),
                        "IsXmasClubMember", ResponseType.Xml);

                if (!string.IsNullOrEmpty(resultXml) && resultXml != "<NewDataSet/>")
                {
                    resulDoc.LoadXml(resultXml);
                    dsXmasClubMember.ReadXml(new XmlNodeReader(resulDoc));
                }

                if (dsXmasClubMember.Tables != null && dsXmasClubMember.Tables.Count == 1)
                {
                    return dsXmasClubMember.Tables[0].Rows[0][0].ToString() == "1";
                }
            }

            return false;
        }

        private Activation.CustomerActivationStatusdetails IGHSCheckCustomerActivatedStatus(string dotcomCustomerID, string culture)
        {
            Activation.CustomerActivationStatusdetails customerActivationStatus = new Activation.CustomerActivationStatusdetails();
            char activated = '0';
            long customerID = 0;
            string resultXml = string.Empty;
            string errorXml = string.Empty;
            if (_clubcardServiceClient.IGHSCheckCustomerActivated(out activated, out customerID, out errorXml, out resultXml, dotcomCustomerID, culture))
            {
                this._recorder.RecordResponse(new RecordLog { Activated = activated, Result = resultXml, CustomerID = customerID, Error = errorXml }, 
                    Tesco.ClubcardProducts.MCA.Web.Common.ResponseRecorder.Services.ClubCardService.ToString(),
                    "IGHSCheckCustomerActivated", ResponseType.Xml);

                if (!String.IsNullOrWhiteSpace(resultXml) && resultXml != "<NewDataSet />")
                {
                    customerActivationStatus.ConvertFromXml(resultXml);
                }
            }
            return customerActivationStatus;
        }

        private List<HouseholdCustomerDetails> GetHouseHoldCustomersData(long customerID, string culture)
        {
            LogData logData = new LogData();
            string errorXml, resultXml;
            List<HouseholdCustomerDetails> houseHolds = new List<HouseholdCustomerDetails>();
            HouseholdCustomerDetailsList lstHouseHolds = new HouseholdCustomerDetailsList();
            try
            {
                if (_clubcardServiceClient.GetHouseholdCustomers(out errorXml, out resultXml, customerID, culture))
                {
                    this._recorder.RecordResponse(new RecordLog { Result = resultXml, Error = errorXml },
                        Tesco.ClubcardProducts.MCA.Web.Common.ResponseRecorder.Services.ClubCardService.ToString(),
                        "GetHouseholdCustomers", ResponseType.Xml);

                    if (resultXml != "" && resultXml != "<NewDataSet />")
                    {
                        lstHouseHolds.ConvertFromHouseholdtableXml(resultXml);

                    }
                    if (!string.IsNullOrEmpty(errorXml))
                    {
                        logData.CaptureData("errorXml", errorXml);
                    }

                }
                
                return lstHouseHolds.List;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception in Getting HouseHold Customers Data", ex,
            new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
        }

        private List<Clubcard> GetClubcardsCustomerData(long customerID, string culture)
        {
            string errorXml, resultXml;
            List<Clubcard> res = new List<Clubcard>();
            ClubcardDetailsList _clubcardDetailsList = new ClubcardDetailsList();
            LogData logData = new LogData();

            try
            {
                if (_clubcardServiceClient.GetClubcardsCustomer(out errorXml, out resultXml, customerID, culture))
                {
                    this._recorder.RecordResponse(new RecordLog { Error = errorXml, Result = resultXml },
                        Tesco.ClubcardProducts.MCA.Web.Common.ResponseRecorder.Services.ClubCardService.ToString(),
                        "GetClubcardsCustomer", ResponseType.Xml);

                    if (resultXml != "" && resultXml != "<NewDataSet />")
                    {
                        _clubcardDetailsList.ConvertFromXml(resultXml);
                        res = _clubcardDetailsList.List;
                    }
                    if (!string.IsNullOrEmpty(errorXml))
                    {
                        logData.CaptureData("errorXml", errorXml);
                    }
                }
                
                return res;
            }
            catch (Exception ex)
            {
                throw GeneralUtility.GetCustomException("Exception in getting clubcard Customer Data", ex,
            new Dictionary<string, object>() 
                            { 
                                { LogConfigProvider.EXCLOGDATAKEY, logData }
                            });
            }
        }
    }
}
