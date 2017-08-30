#region Namespace
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;

#endregion

namespace Tesco.Com.Marketing.Kiosk.JoinAtKiosk.Kiosk
{
    public partial class FurtherPersonalDetails2 : BaseUIPage
    {
        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                /// Call ShowRaces function to display reces based on the configuration value
                ShowRaces();

                try
                {
                    Logger.Write("Start of FurtherPersonalDetails2 Page_Load()", "General");
                    /// Check session value to show timeout
                    if (string.IsNullOrEmpty(Helper.CheckAndResetCookieExpiration("UserFirstName")))
                    {
                        Response.Redirect("~/Kiosk/TimeOut.aspx", false);
                    }
                    else
                    {
                        Helper.CheckAndResetCookie();
                    }
                    Helper.GetAndLoadConfigurationDetails();
                    //Helper.SetTripleDESEncryptedCookie("FurtherPersonalDetails", "FurtherPersonalDetails");

                    /// Call CheckPageRedirection to check on click of next should redirect to next page or summary page
                    CheckPageRedirection();

                }
                catch (Exception ex)
                {
                    Logger.Write("WhatIsYourDOBAndHHAges.aspx.cs:PageLoad():Failed in page load" + ex.Message, "Critical", 1, 1, System.Diagnostics.TraceEventType.Error);
                    string ctrlID = "final";
                    string resID = "SorryErr";
                    string imgID = "DOBAndHHAgesBreadCrumb";
                    string qstring = "~/Kiosk/ErrorMessage.aspx?PgName=Default&ctrlID=" + ctrlID + "&resID=" + resID + "&imgID=" + imgID;
                }

                /// Call RaceFocus function to set focus on the selected control
                RaceFocus();
            }
        }

        #endregion

        #region Race Click
        /// <summary>
        /// On click of Race1 button will highlight the race1 button and unselect the other buttons.
        /// It will set selected raceid to cookie
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void title_Race1Click(object sender, EventArgs e)
        {
            divRace1.Attributes.Add("class", "title_Race1 select");
            divRace2.Attributes.Add("class", "title_Race2 unselect");
            divRace4.Attributes.Add("class", "title_Race4 unselect");
            divRace3.Attributes.Add("class", "title_Race3 unselect");
            divTitle.Attributes.Add("class", "");
            Span1.Attributes.Add("class", "titletext grey");

            //string[] strRace = ConfigurationManager.AppSettings["Race1"].ToString().Trim().Split('|');
            string[] strRace = ConfigurationReader.GetStringConfigKeyTrimmed("Race1").Split('|');

            Helper.SetTripleDESEncryptedCookie("Race", strRace[1]);
            Helper.SetTripleDESEncryptedCookie("RaceID", strRace[0]);
            Helper.SetTripleDESEncryptedCookie("RaceSelect", "Race1");
        }

        /// <summary>
        /// On click of Race2 button will highlight the race2 button and unselect the other buttons.
        /// It will set selected raceid to cookie
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void title_Race2Click(object sender, EventArgs e)
        {
            divRace2.Attributes.Add("class", "title_Race2 select");
            divRace1.Attributes.Add("class", "title_Race1 unselect");
            divRace4.Attributes.Add("class", "title_Race4 unselect");
            divRace3.Attributes.Add("class", "title_Race3 unselect");
            divTitle.Attributes.Add("class", "");
            Span1.Attributes.Add("class", "titletext grey");

            string[] strRace = ConfigurationReader.GetStringConfigKeyTrimmed("Race2").Split('|');

            Helper.SetTripleDESEncryptedCookie("Race", strRace[1]);
            Helper.SetTripleDESEncryptedCookie("RaceID", strRace[0]);
            Helper.SetTripleDESEncryptedCookie("RaceSelect", "Race2");
        }

        /// <summary>
        /// On click of Race3 button will highlight the race3 button and unselect the other buttons.
        /// It will set selected raceid to cookie
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void title_Race3Click(object sender, EventArgs e)
        {
            divRace3.Attributes.Add("class", "title_Race3 select");
            divRace2.Attributes.Add("class", "title_Race2 unselect");
            divRace1.Attributes.Add("class", "title_Race1 unselect");
            divRace4.Attributes.Add("class", "title_Race4 unselect");
            divTitle.Attributes.Add("class", "");
            Span1.Attributes.Add("class", "titletext grey");

            string[] strRace = ConfigurationReader.GetStringConfigKeyTrimmed("Race3").Split('|');

            Helper.SetTripleDESEncryptedCookie("Race", strRace[1]);
            Helper.SetTripleDESEncryptedCookie("RaceID", strRace[0]);
            Helper.SetTripleDESEncryptedCookie("RaceSelect", "Race3");
        }

        /// <summary>
        /// On click of Race4 button will highlight the race4 button and unselect the other buttons.
        /// It will set selected raceid to cookie
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void title_Race4Click(object sender, EventArgs e)
        {
            divRace4.Attributes.Add("class", "title_Race4 select");
            divRace1.Attributes.Add("class", "title_Race1 unselect");
            divRace2.Attributes.Add("class", "title_Race2 unselect");
            divRace3.Attributes.Add("class", "title_Race3 unselect");
            //divnextbutton.Attributes.Add("class", "nextbutton nextfirstnamefocus");
            Span1.Attributes.Add("class", "titletext grey");
            divTitle.Attributes.Add("class", "");

            string[] strRace = ConfigurationReader.GetStringConfigKeyTrimmed("Race4").Split('|');

            Helper.SetTripleDESEncryptedCookie("Race", strRace[1]);
            Helper.SetTripleDESEncryptedCookie("RaceID", strRace[0]);
            Helper.SetTripleDESEncryptedCookie("RaceSelect", "Race4");

        }
        #endregion

        #region Cancel
        /// <summary>
        /// On click of cancel will redirect to CancelAndRestart page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Cancel_Restart(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect("~/Kiosk/CancelAndRestart.aspx", false);
            }
            catch (Exception ex)
            {
                Logger.Write(ex, "General", 1, 3500, System.Diagnostics.TraceEventType.Error,
              "Failed on click of cancel");
                throw ex;
            }
        }
        #endregion

        #region Next
        /// <summary>
        /// On click of next will perform following functionalities,
        /// 1. Checks whether the race is mandotory, if it is mandatory and race is not selected,
        ///     it will show the message to select race
        /// 2. If this page is called from summary page, on click of next will redict to summary page
        /// 3. If it is not from summary page then it will redirect to the next page as configured in configuration
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Next_Click(object sender, EventArgs e)
        {
            string strNextPageName = string.Empty;
            try
            {
                /// Refer Step 1 in summary
                if (CommonClassForJoin.RaceRequired == true)
                {
                    if (string.IsNullOrWhiteSpace(Helper.GetTripleDESEncryptedCookieValue("Race")))
                    {
                        Response.Redirect("~/Kiosk/ErrorMessage.aspx?PgName=FurtherPersonalDetails2&ctrlID=&resID=RaceRequired&imgID=", false);
                    }
                }
                /// Refer Step 2 in summary
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("FurtherPersonalDetails")))
                {
                    Helper.DeleteTripleDESEncryptedCookie("FurtherPersonalDetails");
                    Response.Redirect("~/Kiosk/FinalSubmitionScreen.aspx", false);

                }
                /// Refer Step 3 in summary
                else
                {
                    //strNextPageName = Helper.NextPage(ConfigurationManager.AppSettings["FurtherPersonalDetails2"].ToString().Trim());
                    strNextPageName = Helper.NextPage(ConfigurationReader.GetStringConfigKeyTrimmed("FurtherPersonalDetails2"));
                    Response.Redirect("~/Kiosk/" + strNextPageName + ".aspx", false);
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex, "General", 1, 3500, System.Diagnostics.TraceEventType.Error, "Failed on click of Next, Race="
                    + Helper.GetTripleDESEncryptedCookieValue("RaceRequired")
                    + ",FurtherPersonalDetails=" + Helper.GetTripleDESEncryptedCookieValue("FurtherPersonalDetails") +
                     ",FurtherPersonalDetails2=" + strNextPageName);
                throw ex;
            }
        }
        #endregion

        #region Previous
        /// <summary>
        /// On click of previous will redirect to the previous page as configured
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Previous_Click(object sender, EventArgs e)
        {
            string strPreviousPageName = Helper.PreviousPage(ConfigurationReader.GetStringConfigKeyTrimmed("FurtherPersonalDetails2"));
            Response.Redirect("~/Kiosk/" + strPreviousPageName + ".aspx?FurtherPersonalDetails2", false);

        }
        #endregion

        #region Functions

        #region RaceFocus
        /// <summary>
        /// This function highlights the race button which is selected
        /// </summary>
        private void RaceFocus()
        {
            try
            {
                if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("RaceSelect")))
                {
                    switch (Helper.GetTripleDESEncryptedCookieValue("RaceSelect"))
                    {
                        case "Race1":
                            divRace1.Attributes.Add("class", "title_Race1 select");
                            break;
                        case "Race2":
                            divRace2.Attributes.Add("class", "title_Race2 select");
                            break;
                        case "Race3":
                            divRace3.Attributes.Add("class", "title_Race3 select");
                            break;
                        case "Race4":
                            divRace4.Attributes.Add("class", "title_Race4 select");
                            break;
                    }
                    divnext.Style.Add("display", "none");
                    divs.Style.Add("display", "block");
                    divnextbutton.Attributes.Add("display", "block");
                    divsummary.Attributes.Add("display", "none");
                }
            }
            catch (Exception ex)
            {
                Logger.Write(ex, "General", 1, 3500, System.Diagnostics.TraceEventType.Error,
            "Failed in funciton RaceFocus");
                throw ex;
            }
        }
        #endregion

        #region ShowRaces
        /// <summary>
        /// Read ShowRaces key value from appsetting, as per the value from the key display the race buttons
        /// IT can have maximum 4 races
        /// </summary>
        private void ShowRaces()
        {
            try
            {
                string strShowRaces = ConfigurationReader.GetStringConfigKeyTrimmed("ShowRaces");
                #region
                string[] showRaces =strShowRaces.Split(',');
                for (int showRacesCount = 0; showRacesCount < showRaces.Length; showRacesCount++)
                {
                    if (showRaces[showRacesCount] == "1")
                    {
                        divRace1.Visible = true;
                    }
                    else if (showRaces[showRacesCount] == "2")
                    {
                        divRace2.Visible = true;
                    }
                    else if (showRaces[showRacesCount] == "3")
                    {
                        divRace3.Visible = true;
                    }
                    else if (showRaces[showRacesCount] == "4")
                    {
                        divRace4.Visible = true;
                    }
                }
                #endregion 
               // switch (strShowRaces)
               // {
                   
               //     //case "1":
               //     //    divRace2.Visible = false;
               //     //    divRace3.Visible = false;
               //     //    divRace4.Visible = false;
               //     //    break;
               //     //case "2":
               //     //    divRace3.Visible = false;
               //     //    divRace4.Visible = false;
               //     //    break;
               //     //case "3":
               //     //    divRace4.Visible = false;
               //     //    break;

               //     #region
               //case "1":
               //     divRace2.Visible = false;
               //     divRace3.Visible = false;
               //     divRace4.Visible = false;
               //     break;
               // case "2":
               //     divRace1.Visible = false;
               //     divRace3.Visible = false;
               //     divRace4.Visible = false;
               //     break;
               // case "3":
               //     divRace2.Visible = false;
               //     divRace1.Visible = false;
               //     divRace4.Visible = false;
               //     break;
               // case "4":
               //     divRace2.Visible = false;
               //     divRace3.Visible = false;
               //     divRace1.Visible = false;
               //     break;
               // case "1,2":
               //     divRace3.Visible = false;
               //     divRace4.Visible = false;
               //     break;
               // case "1,3":
               //     divRace2.Visible = false;
               //     divRace4.Visible = false;
               //     break;
               // case "1,4":
               //     divRace2.Visible = false;
               //     divRace3.Visible = false;
               //     break;
               // case "2,3":
               //     divRace1.Visible = false;
               //     divRace4.Visible = false;
               //     break;
               // case "2,4":
               //     divRace1.Visible = false;
               //     divRace3.Visible = false;
               //     break;
               // case "3,4":
               //     divRace1.Visible = false;
               //     divRace2.Visible = false;
               //     break;
               //     #endregion

               // }
            }
            catch (Exception ex)
            {
                Logger.Write(ex, "General", 1, 3500, System.Diagnostics.TraceEventType.Error,
            "Failed in funciton ShowRaces");
                throw ex;
            }

        }
        #endregion

        #region Check Summary Page Redirection
        /// <summary>
        /// The cookie value will get set in FurtherPersonalDetails1 page when the page gets redirected from summary page.
        /// It will check the FurtherPersonalDetails cookie is null or with value. The value is set ion hidden control to load in javascript.
        /// If the cookie has value then summayr button should be enabled and confirm button should be disabled
        /// </summary>
        private void CheckPageRedirection()
        {
            hdnRedirectionVal.Value = null;
            hdnBack.Value = "";

            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("Race")))
            {
                hdnBack.Value = "back";
            }

            if (!string.IsNullOrEmpty(Helper.GetTripleDESEncryptedCookieValue("FurtherPersonalDetails")))
            {
                hdnRedirectionVal.Value = "FurtherPersonalDetails";
               // divspacer.Attributes.Add("style", "display:none");
                 divspacer.Attributes.Add("class", "buttonspacer2");
                divnext.Attributes.Add("class", "display:none");
                divs.Attributes.Add("style", "display:none");
                divsummary.Attributes.Add("style", "display:block");
                divnextbutton.Attributes.Add("display", "none");
            }
        }
        #endregion

        #endregion

    }
}