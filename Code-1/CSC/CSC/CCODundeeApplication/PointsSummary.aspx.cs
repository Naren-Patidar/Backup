using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Threading;
using System.Configuration;
using CCODundeeApplication.ClubcardService;
using System.Xml;
using System.Globalization;
using ClubcardOnline.PointsSummarySequencing;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.ServiceModel;
using System.Web.UI.HtmlControls;
using CCODundeeApplication.CustomerService;

namespace CCODundeeApplication
{
    /// <summary>
    /// Previous points summary page, having functionality to load previous collection period
    /// <para>points data for a logged in customer</para>
    /// <para>it loads the UI arrangement from the xml file for PointsBoxes</para>
    /// <para>which informs customer how much customer has earned points and where</para>
    /// <para>customer points data is fetched from PointsSummary table</para>
    /// <para>Author: Padmanabh Ganorkar</para>
    /// <para>Date: 25/06/2010</para>
    /// </summary>
    public partial class PointsSummary : System.Web.UI.Page
    {
        #region Page Variables
        protected int offerIdNov;
        protected DateTime offerStartDateNov;
        protected DateTime offerEndDateNov;
        private StatementTypes statementType;
        XmlDocument xmlCapability = null;
        DataSet dsCapability = null;

        protected enum StatementTypes
        {
            Reward,
            AirmilesReward,
            XmasSavers,
            BAmilesReward,
            NonReward,
            VirginMilesReward
        }

        #endregion
        /// <summary>
        /// on first page request, load the page controls by calling LoadPointsSummaryInfo()
        /// Also check whether the OfferID is present
        /// </summary>
       
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region Trace Start
                //NGCTrace.NGCTrace.TraceInfo("Start: CSC PointsSummary.Page_Load()");
                //HtmlAnchor PointsEarnedReports = (HtmlAnchor)Master.FindControl("PointsEarnedReport");
                Control link = (HtmlGenericControl)Master.FindControl("liDataconfig");
                //HtmlAnchor PointsearnedReport = (HtmlAnchor)Master.FindControl("PointsEarnedReport");
                //if (dsCapability.Tables[0].Columns.Contains("PointsEarnedReport") != false)
                //{
                //    PointsearnedReport.Disabled = false;
                //}
                //else
                //{
                //    PointsearnedReport.Disabled = true;
                //    PointsearnedReport.HRef = "";
                //}
                //if (dsCapability.Tables[0].Columns.Contains("PointsEarnedReport") != false)
                //{
                //    PointsEarnedReports.Disabled = false;
                //}
                //else
                //{
                //    PointsEarnedReports.Disabled = true;
                //    PointsEarnedReports.HRef = "";
                //}
                //NGCTrace.NGCTrace.TraceDebug("Start: CSC PointsSummary.Page_Load()");
                #endregion

                //Customer ID is not there in cookie, redirect to home page
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Culture")))
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo(Helper.GetTripleDESEncryptedCookieValue("Culture").ToString());
                }
                if (string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("CustomerID")))
                {
                    Response.Redirect("~/Home.aspx", false);
                    System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();
                }

                if (!IsPostBack &&
                    !string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("CustomerID")))
                {
                    #region RoleCapabilityImplementation

                    xmlCapability = new XmlDocument();
                    dsCapability = new DataSet();

                    if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("UserCapability")))
                    {
                        xmlCapability.LoadXml(Helper.GetTripleDESEncryptedCookieValue("UserCapability"));
                        dsCapability.ReadXml(new XmlNodeReader(xmlCapability));

                        if (dsCapability.Tables.Count > 0)
                        {
                            HtmlAnchor DataConfig = (HtmlAnchor)Master.FindControl("dataconfig");
                            if (dsCapability.Tables[0].Columns.Contains("ViewDataConfiguaration") != false)
                            {
                                link.Visible = true;
                            }
                            else
                            {
                                link.Visible = false;
                            }
                        }
                    }

                    #endregion

                    //******* Release 1.5 changes start *********//
                    SetHouseHoldStatus();
                    //******* Release 1.5 changes end *********//

                    //if previous period offer id is present
                    if (Request.QueryString["o"] != null)
                    {
                        //load the offer id in page variable and call LoadPointsSummaryInfo()
                        if (Int32.TryParse(Request.QueryString["o"].ToString().Trim(), out offerIdNov))
                            LoadPointsSummaryInfoNov();
                    }
                }

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC PointsSummary.Page_Load()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC PointsSummary.Page_Load()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC PointsSummary.Page_Load() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC PointsSummary.Page_Load() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC PointsSummary.Page_Load()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error

                throw exp;
            }
            finally
            { 

            }
        }
        #region Initialize the culture


        /// <summary>
        /// Initialize the culture
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void InitializeCulture()
        {
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Culture")))
            {
                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(Helper.GetTripleDESEncryptedCookieValue("Culture"));
                System.Threading.Thread.CurrentThread.CurrentCulture = ci;
                System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
                base.InitializeCulture();
            }
            else
                Response.Redirect("~/Default.aspx", false);
        }
        #endregion
        /// <summary>
        /// Connect to the Clubcard service and fetch the dsPointsSummaryRec by calling GetPointsSummaryInfo()
        /// <para>Also check whether the dsPointsSummaryRec is available in ViewState</para>
        /// <para>Then call the FillPageValueswithDataSetNov() which will then fill out Points boxes</para>
        /// <para>and other fields on the page</para>
        /// </summary>
        private void LoadPointsSummaryInfoNov()
        {
            #region Local variables
            string conditionalXml, resultXml, viewXml = string.Empty, errorXml;
            int maxRowCount = 0, rowCount = 0;
            Hashtable inputParams = new Hashtable();
            XmlDocument resulDoc = new XmlDocument();
            DataSet dsPointsSummaryRec = new DataSet();
            string culture = string.Empty;
            bool isSuccessful = false;
            ClubcardService.ClubcardServiceClient serviceClient = null;
            #endregion

            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC PointsSummary.LoadPointsSummaryInfoNov()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC PointsSummary.LoadPointsSummaryInfoNov()");
                #endregion

                //Check ViewState first if the dataset is present in ViewState
                //Initialize it from ViewState and bypass the NGC service call
                if (ViewState["dsPointsSummaryRec"] != null)
                {
                    dsPointsSummaryRec = ViewState["dsPointsSummaryRec"] as DataSet;
                    FillPageValueswithDataSetNov(dsPointsSummaryRec);
                }
                else
                {
                    //Initialize the service reference
                    serviceClient = new ClubcardService.ClubcardServiceClient();

                    inputParams["CustomerID"] = Convert.ToInt64(Helper.GetTripleDESEncryptedCookieValue("CustomerID")); //logged in customer id
                    inputParams["OfferID"] = offerIdNov; //offer id for the previous collection period
                    culture = ConfigurationManager.AppSettings["Culture"];

                    //Convert all input variables to xml
                    conditionalXml = Helper.HashTableToXML(inputParams, "PointsSummaryCondition");

                    //call the service function GetPointsSummaryInfo() to get Points summary record
                    isSuccessful = serviceClient.GetPointsSummaryInfo(out errorXml, out resultXml, out rowCount, conditionalXml, maxRowCount, culture);

                    //If service is successful load the xml into the dsPointsSummaryRec dataset
                    if (isSuccessful && string.IsNullOrEmpty(errorXml))
                    {
                        if (!string.IsNullOrEmpty(resultXml))
                        {
                            //Load the result xml containing parameters into a data set
                            resulDoc.LoadXml(resultXml);
                            dsPointsSummaryRec.ReadXml(new XmlNodeReader(resulDoc));
                        }
                        if (dsPointsSummaryRec != null &&
                            dsPointsSummaryRec.Tables.Count > 0)
                        {
                            //If the loyaltyEntityServiceLayer function has return the valid dataset then
                            //Pass it to the FillPageValueswithDataSetNov() which fills Points boxes
                            //and all the required UI variables
                            FillPageValueswithDataSetNov(dsPointsSummaryRec);
                            ViewState["dsPointsSummaryRec"] = dsPointsSummaryRec;//Save the dataset to view state for postback cycles
                        }
                    }
                }

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC PointsSummary.LoadPointsSummaryInfoNov()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC PointsSummary.LoadPointsSummaryInfoNov()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC PointsSummary.LoadPointsSummaryInfoNov() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC PointsSummary.LoadPointsSummaryInfoNov() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC PointsSummary.LoadPointsSummaryInfoNov()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error

                throw exp;
            }
            finally
            {
                if (serviceClient != null)
                {
                    if (serviceClient.State == CommunicationState.Faulted)
                    {
                        serviceClient.Abort();
                    }
                    else if (serviceClient.State != CommunicationState.Closed)
                    {
                        serviceClient.Close();
                    }
                }
            }
        }
        /// <summary>
        /// Loads the other page variables like Offer StartDate and EndDate
        /// <para>Also sets the statement type for the customer and the collection period</para>
        /// </summary>
        /// <param name="dsPointsSummaryInfo">DataSet having Points Summary information</param>
        private void FillPageValueswithDataSetNov(DataSet dsPointsSummaryInfo)
        {
            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("PrevOfferID")))
            {
                if (Helper.GetTripleDESEncryptedCookieValue("PrevOfferID") != "211")
                {
                    btnSeePointsDetail.HRef = "PointsSummary.aspx?o=" + Helper.GetTripleDESEncryptedCookieValue("PrevOfferID") + "&v=smry";
                }
                else
                {
                    btnSeePointsDetail.HRef = "Points.aspx?o=" + Helper.GetTripleDESEncryptedCookieValue("PrevOfferID") + "&v=smry";
                }
            }
            else
            {
                btnSeePointsDetail.Visible = false;
            }

            if (dsPointsSummaryInfo.Tables.Count > 0)
            {
                //set page variables Offer StartDate and EndDate and StatementType
                DataRow drPtsSmr = dsPointsSummaryInfo.Tables[0].Rows[0];
                DateTime.TryParse(drPtsSmr["StartDateTime"].ToString(), out offerStartDateNov);
                DateTime.TryParse(drPtsSmr["EndDateTime"].ToString(), out offerEndDateNov);
                statementType = (StatementTypes)Enum.Parse(typeof(StatementTypes), drPtsSmr["PointSummaryDescEnglish"].ToString());

                //following functions loads the page elements with values from the dataset
                LoadOtherPageFieldsNov(drPtsSmr);
                LoadPointsBoxesNov(drPtsSmr, "TescoPoints", ref pnlTescoPointsTotals);
                LoadPointsBoxesNov(drPtsSmr, "TescoBankPoints", ref pnlTescoBankPointsTotals);
            }
        }

        public static string LoadConfigDetails()
        {
            string resultXml = string.Empty;
            string objName = string.Empty;
            string methodName = string.Empty;
            XmlDocument resulDoc = null;
            string conditionXML = "30";
            string errorXml = string.Empty;
            int rowCount = 0;
            string culture = ConfigurationManager.AppSettings["Culture"];
            DataSet dsConfigDetails = new DataSet();
            string strConfigColDays = string.Empty;

            CustomerServiceClient serviceClient = new CustomerServiceClient();

            if (serviceClient.GetConfigDetails(out errorXml, out resultXml, out rowCount, conditionXML, culture))
            {
                resulDoc = new XmlDocument();
                resulDoc.LoadXml(resultXml);
                dsConfigDetails.ReadXml(new XmlNodeReader(resulDoc));
                if (dsConfigDetails.Tables.Count > 0)
                {
                    foreach (DataRow dr in dsConfigDetails.Tables["ActiveDateRangeConfig"].Rows)
                    {
                        if (dr["ConfigurationType"].ToString().Trim() == "30" && dr["ConfigurationName"].ToString().Trim() == "CollectionPeriodMonth")
                        {
                            strConfigColDays = dr["ConfigurationValue1"].ToString();
                        }
                    }
                }
            }
            if (String.IsNullOrEmpty(strConfigColDays))
            {
                strConfigColDays = "30";
            }
            return strConfigColDays;
        }

        /// <summary>
        /// Loads the following fields other than PointsBoxes with values in the dataset:
        /// <para>-------------------------------------</para>
        /// <para>Fields which other than Tesco and Tesco Bank section:</para>
        /// <para>ltrStatementTitle</para>
        /// <para>ltrStatementRange</para>                 
        /// <para>ltrTotalRewardLabel</para>               
        /// <para>ltrTotalReward</para>  
        /// <para>-------------------------------------</para>
        /// <para>Fields in Tesco section:</para>
        /// <para>ltrTescoPoints</para>                    
        /// <para>ltrTescoBroughtForwardPoints</para>      
        /// <para>ltrTescoPointsChangeFromRewards</para>   
        /// <para>ltrOfferEndDate1</para>                  
        /// <para>ltrTescoPointsTotal</para>               
        /// <para>ltrTescoTotalRewardLabel</para>         
        /// <para>ltrTescoTotalReward</para>               
        /// <para>ltrTescoCarriedForwardPoints</para>   
        /// <para>-------------------------------------</para>
        /// <para>Fields in TescoBank section:</para>
        /// <para>ltrTescoBankPoints</para>                
        /// <para>ltrTescoBankBroughtForwardPoints</para>  
        /// <para>ltrOfferEndDate2</para>                  
        /// <para>ltrTescoBankPointsTotal</para>           
        /// <para>ltrTescoBankTotalRewardLabel</para>      
        /// <para>ltrTescoBankTotalReward</para>           
        /// <para>ltrTescoBankCarriedForwardPoints</para>  
        /// </summary>
        /// <param name="drPtsSmr">DataSet having Points Summary information</param>
        private void LoadOtherPageFieldsNov(DataRow drPtsSmr)
        {
            #region Local Variables
            int tescoPoints = 0, tescoBroughtForwardPoints = 0, tescoPointsChangeFromRewards = 0;
            int tescoBankPoints = 0, tescoBankBroughtForwardPoints = 0;
            decimal totalRewards = 0, topUpVouchers = 0, bonusVoucher = 0;
            //Ngc Change Date Configurable
            string culture = ConfigurationSettings.AppSettings["Culture"].ToString();
            System.Globalization.CultureInfo cultures = new System.Globalization.CultureInfo(culture);
            #endregion

            //#######################################################################
            //#    Set the fields which are in Tesco Section                        #
            //#######################################################################
            tescoPoints = Convert.ToInt32(drPtsSmr["TescoPoints"].ToString());

            ltrTescoBroughtForwardPointsNov.Text = drPtsSmr["TescoBroughtForwardPoints"].ToString();
            tescoBroughtForwardPoints = Convert.ToInt32(drPtsSmr["TescoBroughtForwardPoints"].ToString());

            ltrTescoPointsChangeFromRewardsNov.Text = drPtsSmr["TescoPointsChangeFromRewards"].ToString();
            tescoPointsChangeFromRewards = Convert.ToInt32(drPtsSmr["TescoPointsChangeFromRewards"].ToString());

            ltrTescoPointsTotalNov.Text = (tescoPoints + tescoBroughtForwardPoints + tescoPointsChangeFromRewards).ToString();
            ltrTescoPointsNov.Text = ltrTescoPointsTotalNov.Text;
            ltrTescoCarriedForwardPointsNov.Text = drPtsSmr["TescoCarriedForwardPoints"].ToString();

            //#######################################################################
            //#    Set the fields which are in Tesco Bank Section                   #
            //#######################################################################
            tescoBankPoints = Convert.ToInt32(drPtsSmr["TescoBankPoints"].ToString());

            ltrTescoBankBroughtForwardPointsNov.Text = drPtsSmr["TescoBankBroughtForwardPoints"].ToString();
            tescoBankBroughtForwardPoints = Convert.ToInt32(drPtsSmr["TescoBankBroughtForwardPoints"].ToString());

            ltrTescoBankPointsTotalNov.Text = (tescoBankPoints + tescoBankBroughtForwardPoints).ToString();
            ltrTescoBankPointsNov.Text = ltrTescoBankPointsTotalNov.Text;
            ltrTescoBankCarriedForwardPointsNov.Text = drPtsSmr["TescoBankCarriedForwardPoints"].ToString();
            ltrTtlCarriedForwardPointsNov.Text = drPtsSmr["TotalCarriedForwardPoints"].ToString();

            //#######################################################################
            //#    Set the fields which are common to Tesco & Tesco Bank sections   #
            //#######################################################################

            //ltrStatementTitleNov.Text = Helper.GetColMonthName(offerEndDateNov) + " statement";
            ltrStatementTitleNov.Text = Helper.GetColMonthName(offerEndDateNov, LoadConfigDetails()) + " statement";
            ltrStatementRangeNov.Text = "Points collected from " +
                                        offerStartDateNov.ToString("dd/MM/yy", cultures) +
                                        " to " + offerEndDateNov.ToString("dd/MM/yy", cultures);

            ltrPointsTotalNov.Text = drPtsSmr["TotalPoints"].ToString();

            //For Airmiles or BA Miles
            LtrPtsConvertedToMilesNov.Text = (Convert.ToInt32(Convert.ToDecimal(drPtsSmr["TotalReward"].ToString()) * 100)).ToString();
            LtrMilesAwardedNov.Text = drPtsSmr["TotalRewardMiles"].ToString();

            //For december XMas saver statement
            if (offerIdNov == 81 && statementType == StatementTypes.XmasSavers)
            {
                statementType = StatementTypes.NonReward;
                nonXmasRewardLable.Visible = false;
                ltrTescoTotalRewardLabelXmas.Text = "Total points carried forward towards your Christmas " + Helper.GetColYear(offerEndDateNov.AddYears(1)) + " statement";
            }

            //##########################################################################################
            //#    Fields which changes with statement types (Common, in Tesco and TescoBank sections  #
            //##########################################################################################
            switch (statementType)
            {
                case StatementTypes.XmasSavers:
                    //Following fields are for Xmas statement types
                    dvXmasSaver.Visible = true;
                    dvCarryFrdPts.Visible = false;
                    dvXmasVoucher.Visible = true;
                    dvNonXmasVoucher.Visible = false;

                    if (offerIdNov != 81)
                    {
                        ltrTotalRewardLabelNovXmas.Text = "So far your Christmas " + Helper.GetColYear(offerEndDateNov) + " total is";
                    }
                    else
                    {
                        ltrTotalRewardLabelNovXmas.Text = "Voucher total";
                    }

                    //ltrTotalRewardLabelNovXmas.Text = "Your Christmas " + Helper.GetColYear(offerEndDateNov) + " total is";
                   // LtrCCVoucherTtlNov.Text = "$" + drPtsSmr["TotalReward"].ToString();
                    LtrCCVoucherTtlNov.Text = String.Format("{0:C}", Convert.ToDouble(drPtsSmr["TotalReward"].ToString()));
                    totalRewards = Convert.ToDecimal(drPtsSmr["TotalReward"].ToString());
                    topUpVouchers = Convert.ToDecimal(drPtsSmr["TopUpVouchers"].ToString());
                    bonusVoucher = Convert.ToDecimal(drPtsSmr["BonusVouchers"].ToString());
                    //Sum of XMas saver vouchers.
                    //ltrTotalRewardNovXmas.Text = "$" + (totalRewards + topUpVouchers + bonusVoucher).ToString();
                    ltrTotalRewardNovXmas.Text = String.Format("{0:C}", Convert.ToDouble((totalRewards + topUpVouchers + bonusVoucher).ToString()));
                    ltrTescoTotalRewardLabelNov.Text = "Total points carried forward to your next statement";
                    ltrLabelVoucherTtlNov.Text = "Tesco Bank voucher total";
                    LtrLabelTescoVchrTtlNov.Text = "Tesco voucher total";
                    //ltrTescoTotalRewardNov.Text = "$" + drPtsSmr["TescoTotalReward"].ToString();
                    ltrTescoTotalRewardNov.Text = String.Format("{0:C}", Convert.ToDouble(drPtsSmr["TescoTotalReward"].ToString()));
                    //ltrTescoBankTotalRewardNov.Text = "$" + drPtsSmr["TescoBankTotalReward"].ToString();
                    ltrTescoBankTotalRewardNov.Text = String.Format("{0:C}", Convert.ToDouble(drPtsSmr["TescoBankTotalReward"].ToString()));
                    //LtrTopUpTtlNov.Text = "$" + drPtsSmr["TopUpVouchers"].ToString();
                    LtrTopUpTtlNov.Text=string.Format("{0:C}",Convert.ToDouble(drPtsSmr["TopUpVouchers"].ToString()));
                    //LtrBonusVoucherTtlNov.Text = "$" + drPtsSmr["BonusVouchers"].ToString();
                    LtrBonusVoucherTtlNov.Text = string.Format("{0:C}", Convert.ToDouble(drPtsSmr["BonusVouchers"].ToString()));

                    xmasRewardLabel.Visible = false;

                    break;
                case StatementTypes.Reward:
                case StatementTypes.NonReward:
                    //Following fields are same for Standard and NonReward statement types
                    dvXmasVoucher.Visible = false;
                    dvNonXmasVoucher.Visible = true;
                    ltrTotalRewardLabelNov.Text = "Voucher total";
                    //ltrTotalRewardNov.Text = "$" + drPtsSmr["TotalReward"].ToString();
                    ltrTotalRewardNov.Text = string.Format("{0:C}", Convert.ToDouble(drPtsSmr["TotalReward"].ToString()));

                    ltrTescoTotalRewardLabelNov.Text = "Total points carried forward to your next statement";
                    //ltrTescoTotalRewardNov.Text = "$" + drPtsSmr["TescoTotalReward"].ToString();
                    ltrTescoTotalRewardNov.Text = string.Format("{0:C}", Convert.ToDouble(drPtsSmr["TescoTotalReward"].ToString()));

                    //ltrTescoBankTotalRewardNov.Text = "$" + drPtsSmr["TescoBankTotalReward"].ToString();
                    ltrTescoBankTotalRewardNov.Text = string.Format("{0:C}", Convert.ToDouble(drPtsSmr["TescoBankTotalReward"].ToString()));
                    ltrLabelVoucherTtlNov.Text = "Tesco Bank voucher total";
                    LtrLabelTescoVchrTtlNov.Text = "Tesco voucher total";

                    //For December xmas point summary
                    ltrTescoTotalRewardLabelNov.Text = "Total points carried forward to your next statement";
                    break;
                case StatementTypes.AirmilesReward:
                    //Enable the Airmiles or BA Miles section
                    dvMilesSection.Visible = true;
                    dvXmasVoucher.Visible = false;
                    dvNonXmasVoucher.Visible = true;
                    //Following fields are same for AirMiles and BAMiles statement types
                    //Only the wording changes with AirMiles and BAMiles statement types
                    ltrTotalRewardLabelNov.Text = "Avios total";
                    ltrTescoTotalRewardLabelNov.Text = "Total points not converted and carried forward to your next statement";

                    ltrLabelPtsConvertedNov.Text = "Points converted to Avios";
                    ltrLabelVoucherTtlNov.Text = ltrLabelVoucherTtl1Nov.Text = LtrLabelTescoVchrTtlNov.Text = "Avios awarded";
                    ltrTotalRewardNov.Text = drPtsSmr["TotalRewardMiles"].ToString();
                    ltrTescoTotalRewardNov.Text = drPtsSmr["TescoRewardMiles"].ToString();
                    ltrTescoBankTotalRewardNov.Text = drPtsSmr["TescoBankRewardMiles"].ToString();
                    break;
                case StatementTypes.BAmilesReward:
                    //Enable the Airmiles or BA Miles section
                    dvMilesSection.Visible = true;
                    dvXmasVoucher.Visible = false;
                    dvNonXmasVoucher.Visible = true;
                    //Following fields are same for AirMiles and BAMiles statement types
                    //Only the wording changes with AirMiles and BAMiles statement types
                    ltrTotalRewardLabelNov.Text = "Avios total";
                    ltrTescoTotalRewardLabelNov.Text = "Total points not converted and carried forward to your next statement";

                    ltrLabelPtsConvertedNov.Text = "Points converted to Avios";
                    ltrLabelVoucherTtlNov.Text = ltrLabelVoucherTtl1Nov.Text = LtrLabelTescoVchrTtlNov.Text = "Avios awarded";
                    ltrTotalRewardNov.Text = drPtsSmr["TotalRewardMiles"].ToString();
                    ltrTescoTotalRewardNov.Text = drPtsSmr["TescoRewardMiles"].ToString();
                    ltrTescoBankTotalRewardNov.Text = drPtsSmr["TescoBankRewardMiles"].ToString();
                    break;
                case StatementTypes.VirginMilesReward:
                    //Enable the Flying Club miles section
                    dvMilesSection.Visible = true;
                    dvXmasVoucher.Visible = false;
                    dvNonXmasVoucher.Visible = true;
                    ltrTotalRewardLabelNov.Text = "Flying Club miles total";
                    ltrTescoTotalRewardLabelNov.Text = "Total points not converted and carried forward to your next statement";

                    ltrLabelPtsConvertedNov.Text = "Points converted to Flying Club miles";
                    ltrLabelVoucherTtlNov.Text = ltrLabelVoucherTtl1Nov.Text = LtrLabelTescoVchrTtlNov.Text = "Flying Club miles awarded";
                    ltrTotalRewardNov.Text = drPtsSmr["TotalRewardMiles"].ToString();
                    ltrTescoTotalRewardNov.Text = drPtsSmr["TescoRewardMiles"].ToString();
                    ltrTescoBankTotalRewardNov.Text = drPtsSmr["TescoBankRewardMiles"].ToString();
                    break;
            }
        }

        /// <summary>
        /// Loads the PointsBoxes (page elements) dynamically with the defined sequence and logo information
        /// <para>defied in the XML file for the given collection period</para>
        /// <para>the path for the XML file has </para>
        /// </summary>
        /// <param name="drPtsSmr">DataSet having Points Summary information</param>
        /// <param name="sectionType">Standard, NonReward, Xmas, AirMiles or BAMiles</param>
        /// <param name="pnlTarget">Tesco / TescoBank html panels in which the PointsBoxes will get rendered</param>
        private void LoadPointsBoxesNov(DataRow drPtsSmr, string sectionType, ref Panel pnlTarget)
        {
            #region Local Variables
            StatementFormat stformat;
            Statement selStatement;
            ArrayList pointsBoxes;
            short boxCounter = 0, totalRows, totalColumns;
            string rowClass = string.Empty, columnClass = string.Empty, spacer = string.Empty;
            //double rowsd;
            bool isLastRow = false;
            #endregion

            pnlTarget.AddLiteral("<table><tbody>");
            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC PointsSummary.LoadPointsBoxesNov()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC PointsSummary.LoadPointsBoxesNov()");
                #endregion

                //Load the appropriate statement format from the xml with respect to offer id
                stformat = XMLSerializer<StatementFormat>.
                        Load(ConfigurationSettings.AppSettings["StatementFormatPath"] + offerIdNov + ".xml");

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC PointsSummary.LoadPointsBoxesNov()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC PointsSummary.LoadPointsBoxesNov()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC PointsSummary.LoadPointsBoxesNov() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC PointsSummary.LoadPointsBoxesNov() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC PointsSummary.LoadPointsBoxesNov()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error

                throw exp;
            }
            //Load the appropriate statement from statement format with respect to the statement types :
            //Standard, NonReward, Xmas, AirMiles or BAMiles
            selStatement = stformat[statementType.ToString()];

            //Load the PointsBoxes with respect to the section type provided
            //Tesco or TescoBank
            pointsBoxes = selStatement[sectionType];

            //initialize the local variables with rowcount and column count (html grid)
            //rowsd = (double)pointsBoxes.Count / (double)3;
            //totalRows = Convert.ToInt16(Math.Ceiling((double)pointsBoxes.Count));
            totalRows = Convert.ToInt16(pointsBoxes.Count);
            totalColumns = 1;

            //Row Loop row 1 to total rows (html grid)
            for (int row = 1; row <= totalRows; row++)
            {
                if (row == totalRows)
                {
                    rowClass = " class=\"last\"";
                    isLastRow = true;
                }
                //start the row tag
                pnlTarget.AddLiteral("<tr" + rowClass + ">");

                //Column Loop column 1 to 3 (html grid)
                for (int column = 1; column <= totalColumns; column++)
                {
                    //set the class name for every td
                    //if the row is last then the class name for every td changes
                    if (isLastRow)
                    {
                        if (column == totalColumns)
                            columnClass = " class=\"rowLast colLast\"";
                        else
                            columnClass = " class=\"rowLast\"";
                    }
                    else if (column == totalColumns)
                        columnClass = " class=\"colLast\"";
                    else
                        columnClass = "";

                    //start the td tag
                    //pnlTarget.AddLiteral("<td" + columnClass + ">");

                    if (boxCounter < pointsBoxes.Count)
                    {
                        //get the appropriate PointsBox object from the Array
                        PointsBox box = pointsBoxes[boxCounter] as PointsBox;
                        pnlTarget.AddLiteral("<th>");

                        //render the image if the Box has a logo name
                        if (!string.IsNullOrEmpty(box.BoxLogoFileName))
                        {
                            if (box.BoxLogoFileName == "icentre.jpg")
                            {
                                Label LblIcentre = new Label();
                                LblIcentre.Text = "The Nutri Centre";
                                pnlTarget.Controls.Add(LblIcentre);
                            }
                            else if (box.BoxLogoFileName == "other.jpg")
                            {
                                Label LblOther = new Label();
                                LblOther.Text = "Other";
                                pnlTarget.Controls.Add(LblOther);
                            }
                            else
                            {
                                Image boxImage = new Image();
                                boxImage.ImageUrl = "~/I/" + box.BoxLogoFileName;
                                boxImage.AlternateText = box.BoxName;
                                pnlTarget.Controls.Add(boxImage);
                            }
                        }
                        pnlTarget.AddLiteral("</th>");
                        pnlTarget.AddLiteral("<td class=\"tc_bottomline\">&shy;</td>");
                        pnlTarget.AddLiteral("<td style=\"float:right;margin-top:1.6em;\">");
                        pnlTarget.AddLiteral("<span id=\"sp" + sectionType + "_" + (boxCounter) + "\">");
                        //fetch the value from DataSet if PointsBox has DataColumn defined
                        if (!string.IsNullOrEmpty(box.DataColumnName))
                        {
                            //Check if column is not available in the table.
                            if (drPtsSmr.Table.Columns.Contains(box.DataColumnName))
                            {
                                pnlTarget.AddLiteral(drPtsSmr[box.DataColumnName].ToString());
                            }
                        }

                        pnlTarget.AddLiteral("</span>");
                        pnlTarget.AddLiteral("</td>");
                    }
                    else
                        pnlTarget.AddLiteral("&nbsp;");
                    //end td tag

                    //pnlTarget.AddLiteral("</td>");
                    boxCounter++;
                }
                //end column loop (html grid)
                //end tr tag 
                pnlTarget.AddLiteral("</tr>");
            }
            //end row loop (html grid)
            pnlTarget.AddLiteral("</table>");
        }

        #region SetHouseHoldStatus
        /// <summary>
        /// To set household status on LHN
        /// </summary>
        /// <param name="pCustomerID">Primary customer ID</param>
        public void SetHouseHoldStatus()
        {
            string customerUserStatus = string.Empty;
            string customerMailStatus = string.Empty;
            string CustomerEmailStatus = string.Empty;
            string CustomerMobilePhoneStatus = string.Empty;
            try
            {
                #region Trace Start
                NGCTrace.NGCTrace.TraceInfo("Start: CSC PointsSummary.SetHouseHoldStatus()");
                NGCTrace.NGCTrace.TraceDebug("Start: CSC PointsSummary.SetHouseHoldStatus()");
                #endregion

                customerUserStatus = Helper.GetTripleDESEncryptedCookieValue("customerUserStatus");
                customerMailStatus = Helper.GetTripleDESEncryptedCookieValue("customerMailStatus");
                CustomerEmailStatus = Helper.GetTripleDESEncryptedCookieValue("CustomerEmailStatus");
                CustomerMobilePhoneStatus = Helper.GetTripleDESEncryptedCookieValue("CustomerMobilePhoneStatus");
                ContentPlaceHolder leftNav = (ContentPlaceHolder)Master.FindControl("CustDetailsLeftNav");

                HtmlControl spnBannedError = (HtmlControl)leftNav.FindControl("spnBannedError");
                HtmlControl spnLeftError = (HtmlControl)leftNav.FindControl("spnLeftError");
                HtmlControl spnDuplicateError = (HtmlControl)leftNav.FindControl("spnDuplicateError");
                HtmlControl spnAddressError = (HtmlControl)leftNav.FindControl("spnAddressError");
                HtmlControl spnEmailError = (HtmlControl)leftNav.FindControl("spnEmailError");
                HtmlControl spnMobileNoError = (HtmlControl)leftNav.FindControl("spnMobileNoError");
                if (customerUserStatus != "1" || customerMailStatus != "1")
                {
                    // for banned house hold
                    if (customerUserStatus == "2")
                    {
                        spnBannedError.Visible = true;
                    }
                    // for Left Scheme
                    else if (customerUserStatus == "3")
                    {
                        spnLeftError.Visible = true;
                    }
                    //for duplicate
                    else if (customerUserStatus == "5")
                    {
                        spnDuplicateError.Visible = true;
                    }
                    else
                    {
                        //for address in error
                        if (customerMailStatus == "3")
                        {
                            spnAddressError.Visible = true;
                        }
                        if (CustomerEmailStatus == "8")
                        {
                            spnEmailError.Visible = true;
                        }
                        if (CustomerMobilePhoneStatus == "8")
                        {
                            spnMobileNoError.Visible = true;
                        }
                    }
                }

                #region Trace End
                NGCTrace.NGCTrace.TraceInfo("End: CSC PointsSummary.SetHouseHoldStatus()");
                NGCTrace.NGCTrace.TraceDebug("End: CSC PointsSummary.SetHouseHoldStatus()");
                #endregion
            }
            catch (Exception exp)
            {
                #region Trace Error
                NGCTrace.NGCTrace.TraceCritical("Critical: CSC PointsSummary.SetHouseHoldStatus() - Error Message :" + exp.ToString() + " Clubcard CustomerID:" + Helper.GetTripleDESEncryptedCookieValue("CustomerID"));
                NGCTrace.NGCTrace.TraceError("Error: CSC PointsSummary.SetHouseHoldStatus() - Error Message :" + exp.ToString());
                NGCTrace.NGCTrace.TraceWarning("Warning: CSC PointsSummary.SetHouseHoldStatus()");
                NGCTrace.NGCTrace.ExeptionHandling(exp);
                #endregion Trace Error

                throw exp;
            }
            finally
            {

            }
        }

        #endregion
    }
}