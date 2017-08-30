using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.Framework.UITesting.Services.ClubcardService;
using Tesco.Framework.UITesting;
using System.Collections;
using System.Xml.Linq;
using System.Linq;
using System.Data;
using System.Web;
using System.Xml;
using System.ServiceModel;
using System.IO;

namespace Tesco.Framework.UITesting.Services
{
    public class ClubcardServiceAdapter
    {
        ClubcardServiceClient clubcardServiceClient = null;
        public Int32 GetHouseholdCustomersCount(long customerID, string culture)
        {
            Int32 count = 0;
            XDocument xDoc = new XDocument();
            CustomerServiceAdaptor customerAdaptor = new CustomerServiceAdaptor();
            using (clubcardServiceClient = new ClubcardServiceClient())
            {
                string errorXml = string.Empty, resultXml = string.Empty;
                clubcardServiceClient.GetHouseholdCustomers(out errorXml, out resultXml, customerID, culture);
                XmlDocument resulDoc = new XmlDocument();
                resulDoc.LoadXml(resultXml);
                xDoc = XDocument.Parse(resultXml);
                List<string> customerDetails = (from t in xDoc.Descendants("HouseholdCustomers")
                                                select t.Element("CustomerID").GetValue<string>()).ToList();
                count = customerDetails.Count;
            }
            return count;
        }
        public XDocument GetHouseHoldCustomers(long customerID, string culture)
        {
            XDocument xDoc = new XDocument();
            string errorXml, resultXml;
            DataSet dsHHCustomers = new DataSet();
            clubcardServiceClient = new ClubcardServiceClient();
            if (clubcardServiceClient.GetHouseholdCustomers(out errorXml, out resultXml, customerID, culture))
            {

                XmlDocument resulDoc = new XmlDocument();
                resulDoc.LoadXml(resultXml);
                dsHHCustomers.ReadXml(new XmlNodeReader(resulDoc));
                xDoc = XDocument.Parse(resultXml);

            }
            return xDoc;

        }

        public long PrimaryCustomerID(long customerID, string culture)
        {
            long PrimaryCustomerID = 0;
            XDocument xDoc = new XDocument();
            xDoc = GetHouseHoldCustomers(customerID, culture);
            List<long> PrimaryCustomerIDlst = (from t in xDoc.Descendants("HouseholdCustomers")
                                               select t.Element("PrimaryCustomerID").GetValue<long>()).ToList();
            if (PrimaryCustomerIDlst.Count > 0)
            {
                PrimaryCustomerID = PrimaryCustomerIDlst.FirstOrDefault();
            }
            return PrimaryCustomerID;
        }

        public string MainSalutation(long customerID, string culture)
        {
            string MainSalutation = string.Empty;
            XDocument xDoc = new XDocument();
            xDoc = GetHouseHoldCustomers(customerID, culture);
            List<string> Tittle = (from t in xDoc.Descendants("HouseholdCustomers")
                                   select t.Element("Title").GetValue<string>()).ToList();
            List<string> FirstName = (from t in xDoc.Descendants("HouseholdCustomers")
                                      select t.Element("Name1").GetValue<string>()).ToList();
            List<string> LastName = (from t in xDoc.Descendants("HouseholdCustomers")
                                     select t.Element("Name3").GetValue<string>()).ToList();
            if (Tittle.Count > 0 && FirstName.Count > 0 && LastName.Count > 0)
            {
                MainSalutation = Tittle[0].ToString() + " " + FirstName[0].ToString() + " " + LastName[0].ToString();
            }
            return MainSalutation;
        }

        public string AssociateSalutation(long customerID, string culture)
        {
            string AssociateSalutation = string.Empty;
            XDocument xDoc = new XDocument();
            xDoc = GetHouseHoldCustomers(customerID, culture);
            List<string> Tittle = (from t in xDoc.Descendants("HouseholdCustomers")
                                   select t.Element("Title").GetValue<string>()).ToList();
            List<string> FirstName = (from t in xDoc.Descendants("HouseholdCustomers")
                                      select t.Element("Name1").GetValue<string>()).ToList();
            List<string> LastName = (from t in xDoc.Descendants("HouseholdCustomers")
                                     select t.Element("Name3").GetValue<string>()).ToList();
            if (Tittle.Count > 0 && FirstName.Count > 0 && LastName.Count > 0)
            {
                AssociateSalutation = Tittle[1].ToString() + " " + FirstName[1].ToString() + " " + LastName[1].ToString();
            }
            return AssociateSalutation;
        }

        public Dictionary<string, string> GetMaincustomerDetails(long customerID, string culture)
        {
            Dictionary<string, string> MaincustomerDetails = new Dictionary<string, string>();
            string MainSalutation = string.Empty;
            XDocument xDoc = new XDocument();
            xDoc = GetHouseHoldCustomers(customerID, culture);

            MaincustomerDetails = (from t in xDoc.Descendants("HouseholdCustomers").First().Descendants()
                                   select new
                                   {
                                       Name = t.Name.LocalName,
                                       Value = t.GetValue<String>(),
                                   }).ToDictionary(o => o.Name, o => o.Value);


            return MaincustomerDetails;
        }

        public Dictionary<string, string> GetAssociatecustomerDetails(long customerID, string culture)
        {
            Dictionary<string, string> AssociatecustomerDetails = new Dictionary<string, string>();
            string MainSalutation = string.Empty;
            XDocument xDoc = new XDocument();
            xDoc = GetHouseHoldCustomers(customerID, culture);
            AssociatecustomerDetails = (from t in xDoc.Descendants("HouseholdCustomers").Last().Descendants()
                                        select new
                                        {
                                            Name = t.Name.LocalName,
                                            Value = t.GetValue<String>(),
                                        }).ToDictionary(o => o.Name, o => o.Value);


            return AssociatecustomerDetails;
        }

        public List<string> GetClubcardsCustomer(long customerID, string culture, string columnName)
        {
            string errorXml, resultXml;
            DataSet dsHHCustomers = new DataSet();
            List<string> Clubcards = new List<string>();
            clubcardServiceClient = new ClubcardServiceClient();
            XDocument xDoc = new XDocument();
            if (clubcardServiceClient.GetClubcardsCustomer(out errorXml, out resultXml, customerID, culture))
            {
                if (resultXml != string.Empty && resultXml != "<NewDataSet />")
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

                    xDoc = WriteDt2Xml(dsHHCustomers.Tables["ClubcardDetails"]);
                    Clubcards = (from t in xDoc.Descendants("ClubcardDetails")
                                 select t.Element(columnName).GetValue<string>()).ToList();
                }
            }
            return Clubcards;
        }

        public XDocument WriteDt2Xml(DataTable dt)
        {
            using (var stream = new MemoryStream())
            {
                dt.WriteXml(stream);
                stream.Position = 0;
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ConformanceLevel = ConformanceLevel.Fragment;
                XmlReader reader = XmlReader.Create(stream, settings);
                reader.MoveToContent();
                if (reader.IsEmptyElement) { reader.Read(); return null; }
                return XDocument.Load(reader);
            }
        }
        public List<string> GetCollectionPeriods(long customerID, string culture, bool isDateRangeEnabled, string dateformat)
        {
            DateTime dtTemp = DateTime.Now;
            List<string> periods = new List<string>();
            List<string> tmpPeriods = new List<string>();
            string errorXml = string.Empty, resultXml = string.Empty, conditionXml = string.Empty;
            int rowCount, maxRowCount = 3;
            conditionXml = string.Format("<?xml version=\"1.0\" encoding=\"utf-16\"?><PointsInfoCondition><CustomerID>{0}</CustomerID></PointsInfoCondition>", customerID);
            clubcardServiceClient = new ClubcardServiceClient();
            if (clubcardServiceClient.GetPointsForAllCollPeriodByCustomer(out errorXml, out resultXml, out rowCount, conditionXml, maxRowCount, culture))
            {
                XDocument xDoc = XDocument.Parse(resultXml);
                if (isDateRangeEnabled)
                {
                    tmpPeriods = (from t in xDoc.Descendants("PointsInfoAllCollPrds")
                                  where t.Element("OfferPeriod").GetValue<string>().ToUpper() != "CURRENT"
                                  select string.Format("{0} - {1}",
                                  t.Element("StartDateTime").GetValue<string>().TryParseDate(out dtTemp) ? dtTemp.ToString(dateformat) : DateTime.Now.ToString(dateformat),
                                  t.Element("EndDateTime").GetValue<string>().TryParseDate(out dtTemp) ? dtTemp.ToString(dateformat) : DateTime.Now.ToString(dateformat)))
                               .ToList();
                    periods = (from t in xDoc.Descendants("PointsInfoAllCollPrds")
                               where t.Element("OfferPeriod").GetValue<string>().ToUpper() == "CURRENT"
                               select string.Format("{0} - {1}",
                               t.Element("StartDateTime").GetValue<string>().TryParseDate(out dtTemp) ? dtTemp.ToString(dateformat) : DateTime.Now.ToString(dateformat),
                               t.Element("EndDateTime").GetValue<string>().TryParseDate(out dtTemp) ? dtTemp.ToString(dateformat) : DateTime.Now.ToString(dateformat)))
                               .ToList();
                }
                else
                {
                    tmpPeriods = (from t in xDoc.Descendants("PointsInfoAllCollPrds")
                                  where t.Element("OfferPeriod").GetValue<string>().ToUpper() != "CURRENT"
                                  select t.Element("OfferPeriod").GetValue<string>()).ToList();
                    periods = (from t in xDoc.Descendants("PointsInfoAllCollPrds")
                               where t.Element("OfferPeriod").GetValue<string>().ToUpper() == "CURRENT"
                               select t.Element("OfferPeriod").GetValue<string>()).ToList();
                }
                periods.AddRange(tmpPeriods);
            }
            return periods;
        }

        public string GetOfferID(long customerID, string culture, int OfferIndex)
        {
            string offerId = string.Empty;
            string errorXml = string.Empty, resultXml = string.Empty, conditionXml = string.Empty;
            int rowCount, maxRowCount = 3;
            conditionXml = string.Format("<?xml version=\"1.0\" encoding=\"utf-16\"?><PointsInfoCondition><CustomerID>{0}</CustomerID></PointsInfoCondition>", customerID);
            clubcardServiceClient = new ClubcardServiceClient();
            if (clubcardServiceClient.GetPointsForAllCollPeriodByCustomer(out errorXml, out resultXml, out rowCount, conditionXml, maxRowCount, culture))
            {
                XDocument xDoc = XDocument.Parse(resultXml);
                List<string> offers = (from t in xDoc.Descendants("PointsInfoAllCollPrds")
                                       where t.Element("OfferPeriod").GetValue<string>().ToUpper() == "CURRENT"
                                       select t.Element("OfferID").GetValue<string>()).ToList();
                List<string> tmpOffers = (from t in xDoc.Descendants("PointsInfoAllCollPrds")
                                          where t.Element("OfferPeriod").GetValue<string>().ToUpper() != "CURRENT"
                                          select t.Element("OfferID").GetValue<string>()).ToList();
                offers.AddRange(tmpOffers);
                if (offers.Count > 0)
                {
                    offerId = offers[OfferIndex];
                }
            }
            return offerId;
        }

        public string GetTransactions(long customerID, string culture, string OfferId)
        {
            string transactions = string.Empty;
            string errorXml = string.Empty, resultXml = string.Empty, conditionXml = string.Empty;
            int rowCount, maxRowCount = 3;
            conditionXml = string.Format("<?xml version=\"1.0\" encoding=\"utf-16\"?><TransactionCondition><OfferID>{0}</OfferID><ShowMerchantFlag>1</ShowMerchantFlag><CustomerID>{1}</CustomerID></TransactionCondition>", OfferId, customerID);
            clubcardServiceClient = new ClubcardServiceClient();
            if (clubcardServiceClient.GetTxnDetailsByCustomerAndOfferID(out errorXml, out resultXml, out rowCount, conditionXml, maxRowCount, culture))
            {
                transactions = resultXml;
            }
            return transactions;
        }

        public List<string> GetOffersForCustomer(long customerID, string culture)
        {
            List<string> offers = new List<string>();
            string errorXml = string.Empty, resultXml = string.Empty, conditionXml = string.Empty;
            int rowCount, maxRowCount = 3;
            conditionXml = string.Format("<?xml version=\"1.0\" encoding=\"utf-16\"?><PointsInfoCondition><CustomerID>{0}</CustomerID></PointsInfoCondition>", customerID);
            clubcardServiceClient = new ClubcardServiceClient();
            if (clubcardServiceClient.GetPointsForAllCollPeriodByCustomer(out errorXml, out resultXml, out rowCount, conditionXml, maxRowCount, culture))
            {
                XDocument xDoc = XDocument.Parse(resultXml);
                offers = (from t in xDoc.Descendants("PointsInfoAllCollPrds")
                          select
                          t.Element("OfferID").GetValue<string>())
                           .ToList();
            }
            return offers;
        }

        public Dictionary<string, string> GetPointsSummary(long customerId, string previousOfferId, string culture)
        {
            #region Local variables

            string conditionalXml, resultXml = string.Empty, errorXml = string.Empty;
            int maxRowCount = 0, rowCount = 0;
            Dictionary<string, string> pointsDetails = new Dictionary<string, string>();

            #endregion

            conditionalXml = string.Format("<?xml version=\"1.0\" encoding=\"utf-16\"?><PointsSummaryCondition><OfferID>{0}</OfferID><CustomerID>{1}</CustomerID></PointsSummaryCondition>", previousOfferId, customerId);
            //call the service function GetPointsSummaryInfo() to get Points summary record
            clubcardServiceClient = new ClubcardServiceClient();

            if (clubcardServiceClient.GetPointsSummaryInfo(out errorXml, out resultXml, out rowCount, conditionalXml, maxRowCount, culture))
            {
                XDocument xDoc = XDocument.Parse(resultXml);
                pointsDetails = (from t in xDoc.Descendants("PointsSummaryRec").First().Descendants()
                                 select new
                                 {
                                     Name = t.Name.LocalName,
                                     Value = t.GetValue<String>(),
                                 }).ToDictionary(o => o.Name, o => o.Value);


            }
            return pointsDetails;
        }

        public DataSet GetChristmasSaverSummaryDataset(long customerID, DateTime startDate, DateTime endDate, string culture)
        {

            DataSet dsXmasSummary = new DataSet();
            string resultXml, errorXml;
            Hashtable XmasSaverSummary = new Hashtable();
            int maxRows = 0;
            int rowCount;
            clubcardServiceClient = new ClubcardServiceClient();
            try
            {
                XmasSaverSummary["CustomerID"] = customerID;
                XmasSaverSummary["StartDate"] = startDate;
                XmasSaverSummary["EndDate"] = endDate;
                string searchXML = Utility.HashTableToXML(XmasSaverSummary, "XmasSaver");
                if (clubcardServiceClient.GetChristmasSaverSummary(out errorXml, out resultXml, out rowCount, searchXML, maxRows, culture))
                {
                    if (resultXml != string.Empty && resultXml != "<NewDataSet />")
                    {
                        XmlDocument resulDoc = new XmlDocument();
                        resulDoc.LoadXml(resultXml);
                        dsXmasSummary.ReadXml(new XmlNodeReader(resulDoc));
                    }
                }
                return dsXmasSummary;

            }

            catch (Exception exp)
            {

                throw exp;
            }
            finally
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

        }

        public int GetOrderReplacementCount(long customerID, string culture)
        {
            int ordeReplacementCount = 0;
            string errorXml = string.Empty, resultXml = string.Empty, conditionXml = string.Empty;
            XmlDocument resulDoc = new XmlDocument();
            int rowCount, maxRowCount = 1;
            DataSet dsOrderReplacement = new DataSet();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            conditionXml = "<?xml version=\"1.0\" encoding=\"utf-16\"?><CheckOrderReplacement><OrderProcessWindow>14</OrderProcessWindow><CustomerID>" + customerID + "</CustomerID></CheckOrderReplacement>";
            clubcardServiceClient = new ClubcardServiceClient();
            if (clubcardServiceClient.IsNewOrderReplacementValid(out errorXml, out resultXml, out rowCount, conditionXml, maxRowCount, culture))
            {
                resulDoc.LoadXml(resultXml);
                dsOrderReplacement.ReadXml(new XmlNodeReader(resulDoc));
                dt = dsOrderReplacement.Tables[0];
                dsOrderReplacement.Tables.Remove(dt);
                ds.Tables.Add(dt);
                XDocument xmlDoc = XDocument.Parse(ds.GetXml());
                List<int> Attempts = (from e in xmlDoc.Descendants("OrderReplacement")
                                      select e.Element("countOrdersPlacedInYear").GetValue<int>()).ToList();
                if (Attempts.Count > 0)
                {
                    ordeReplacementCount = Attempts.FirstOrDefault();
                }

            }
            return ordeReplacementCount;
        }

        public bool ResetOrderReplacementData(long customerID, string ClubcardNumber, string culture)
        {
            string errorXml = string.Empty;
            bool bSuccess = false;
            clubcardServiceClient = new ClubcardServiceClient();
            try
            {
                bSuccess = clubcardServiceClient.ResetOrderReplacementData(out errorXml, customerID, ClubcardNumber, culture);
                // bSuccess = true;
            }
            catch
            {
                return false;
            }
            finally
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

            return bSuccess;



        }

        public string GetVouchers(long customerID, string culture, int OfferIndex)
        {
            string Vouchers = string.Empty;
            string errorXml = string.Empty, resultXml = string.Empty, conditionXml = string.Empty;
            int rowCount, maxRowCount = 3;
            conditionXml = string.Format("<?xml version=\"1.0\" encoding=\"utf-16\"?><PointsInfoCondition><CustomerID>{0}</CustomerID></PointsInfoCondition>", customerID);
            clubcardServiceClient = new ClubcardServiceClient();
            if (clubcardServiceClient.GetPointsForAllCollPeriodByCustomer(out errorXml, out resultXml, out rowCount, conditionXml, maxRowCount, culture))
            {
                XDocument xDoc = XDocument.Parse(resultXml);
                List<string> vouchers = (from t in xDoc.Descendants("PointsInfoAllCollPrds")
                                         where t.Element("OfferPeriod").GetValue<string>().ToUpper() == "CURRENT"
                                         select t.Element("Vouchers").GetValue<string>()).ToList();
                List<string> tmpVouchers = (from t in xDoc.Descendants("PointsInfoAllCollPrds")
                                            where t.Element("OfferPeriod").GetValue<string>().ToUpper() != "CURRENT"
                                            select t.Element("Vouchers").GetValue<string>()).ToList();
                vouchers.AddRange(tmpVouchers);
                if (vouchers.Count > 0)
                {
                    Vouchers = vouchers[OfferIndex];
                }
            }
            return Vouchers;
        }

        public string GetPointsTotal(long customerID, string culture)
        {
            string pointssummary = string.Empty;
            string errorXml = string.Empty, resultXml = string.Empty;

            clubcardServiceClient = new ClubcardServiceClient();

            if (clubcardServiceClient.GetMyAccountDetails(out errorXml, out resultXml, customerID, culture))
            {
                XDocument xmlDoc = XDocument.Parse(resultXml);
                pointssummary = (from e in xmlDoc.Descendants("ViewMyAccountDetails")
                                 select e.Element("PointsBalanceQty").GetValue<string>()).ToList().FirstOrDefault();
            }

            return pointssummary;
        }

        public string GetTotalVouchers(long customerID, string culture)
        {
            string str = string.Empty;
            float TotalVouchersummary = 0;
            string errorXml = string.Empty, resultXml = string.Empty;

            clubcardServiceClient = new ClubcardServiceClient();

            if (clubcardServiceClient.GetMyAccountDetails(out errorXml, out resultXml, customerID, culture))
            {
                XDocument xmlDoc = XDocument.Parse(resultXml);
                TotalVouchersummary = (from e in xmlDoc.Descendants("ViewMyAccountDetails")
                                       select e.Element("Vouchers").GetValue<float>()).ToList().FirstOrDefault();
                str = (string.Format("{0:0.00}", TotalVouchersummary)).ToString();
            }

            return str;
        }

        public DataSet GetCustomers(long customerID, string culture)
        {
            XDocument xDoc = new XDocument();
            string errorXml, resultXml;
            DataSet dsHHCustomers = new DataSet();
            clubcardServiceClient = new ClubcardServiceClient();
            if (clubcardServiceClient.GetHouseholdCustomers(out errorXml, out resultXml, customerID, culture))
            {
                XmlDocument resulDoc = new XmlDocument();
                resulDoc.LoadXml(resultXml);
                dsHHCustomers.ReadXml(new XmlNodeReader(resulDoc));
            }
            return dsHHCustomers;

        }

        public bool GetActivationstatus(string DotcomID, string culture)
        {
            bool Activated = false;
            ClubcardServiceClient Status = new ClubcardServiceClient();
            char activated;
            long custID;
            string errorXml, resultXml;
            Activated = Status.IGHSCheckCustomerActivated(out activated, out custID, out errorXml, out resultXml, DotcomID, culture);
            return Activated;
        }

        public List<string> GetCurrentCollectionPeriodStartDate(long customerID, string culture,string dateformat)
        {
            List<string> Dates = new List<string>();
            DateTime dtTemp = DateTime.Now;
            string errorXml = string.Empty, resultXml = string.Empty, conditionXml = string.Empty;
            int rowCount, maxRowCount = 3;
            conditionXml = string.Format("<?xml version=\"1.0\" encoding=\"utf-16\"?><PointsInfoCondition><CustomerID>{0}</CustomerID></PointsInfoCondition>", customerID);
            clubcardServiceClient = new ClubcardServiceClient();
            try
            {
                if (clubcardServiceClient.GetPointsForAllCollPeriodByCustomer(out errorXml, out resultXml, out rowCount, conditionXml, maxRowCount, culture))
                {
                    XDocument xDoc = XDocument.Parse(resultXml);
                    Dates = (from t in xDoc.Descendants("PointsInfoAllCollPrds")
                                 where t.Element("OfferPeriod").GetValue<string>().ToUpper() == "CURRENT"
                             select string.Format("{0}",
                         t.Element("StartDateTime").GetValue<string>().TryParseDate(out dtTemp) ? dtTemp.ToString(dateformat) : DateTime.Now.ToString(dateformat)
                               )).ToList();
                }
                 
            }
            catch(Exception ex)
            {
                throw ex;

            }
            return Dates;
        }

        public List<string> GetCurrentCollectionPeriodEndDate(long customerID, string culture, string dateformat)
        {
            List<string> Dates = new List<string>();
            DateTime dtTemp = DateTime.Now;
            string errorXml = string.Empty, resultXml = string.Empty, conditionXml = string.Empty;
            int rowCount, maxRowCount = 3;
            conditionXml = string.Format("<?xml version=\"1.0\" encoding=\"utf-16\"?><PointsInfoCondition><CustomerID>{0}</CustomerID></PointsInfoCondition>", customerID);
            clubcardServiceClient = new ClubcardServiceClient();
            try
            {
                if (clubcardServiceClient.GetPointsForAllCollPeriodByCustomer(out errorXml, out resultXml, out rowCount, conditionXml, maxRowCount, culture))
                {
                    XDocument xDoc = XDocument.Parse(resultXml);
                    Dates = (from t in xDoc.Descendants("PointsInfoAllCollPrds")
                             where t.Element("OfferPeriod").GetValue<string>().ToUpper() == "CURRENT"
                             select string.Format("{0}",
                             t.Element("EndDateTime").GetValue<string>().TryParseDate(out dtTemp) ? dtTemp.ToString(dateformat) : DateTime.Now.ToString(dateformat)))
                               .ToList();
                }
            }
            catch(Exception ex)
            {
                throw ex;

            }
            return Dates;
        }

        public List<string> GetCurrentCollectionPeriodPoints(long customerID, string culture)
        {
            List<string> Points = new List<string>();
            DateTime dtTemp = DateTime.Now;
            string errorXml = string.Empty, resultXml = string.Empty, conditionXml = string.Empty;
            int rowCount, maxRowCount = 3;
            conditionXml = string.Format("<?xml version=\"1.0\" encoding=\"utf-16\"?><PointsInfoCondition><CustomerID>{0}</CustomerID></PointsInfoCondition>", customerID);
            clubcardServiceClient = new ClubcardServiceClient();
            try
            {
                if (clubcardServiceClient.GetPointsForAllCollPeriodByCustomer(out errorXml, out resultXml, out rowCount, conditionXml, maxRowCount, culture))
                {
                    XDocument xDoc = XDocument.Parse(resultXml);
                    Points = (from t in xDoc.Descendants("PointsInfoAllCollPrds")
                             where t.Element("OfferPeriod").GetValue<string>().ToUpper() == "CURRENT"
                             select string.Format("{0}",
                             t.Element("PointsBalanceQty").GetValue<string>()))
                               .ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
            return Points;
        }

    }
}
