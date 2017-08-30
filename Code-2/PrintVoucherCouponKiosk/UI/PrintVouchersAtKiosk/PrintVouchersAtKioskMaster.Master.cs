using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using InstoreClubcardReward.Business;
using System.Diagnostics;
using System.Configuration;

namespace PrintVouchersAtKiosk
{
    public partial class PrintVouchersAtKioskMaster : System.Web.UI.MasterPage
    {

        /// <summary>
        /// Gets or sets the session booking.
        /// </summary>
        /// <value>The session booking.</value>
        public BigExchange.BookingPrintVoucher SessionBooking
        {
            get
            {
                if (Session["Booking"] == null)
                {
                    //write to the event log.
                    EventLog objEventLog = new EventLog("Application");
                    objEventLog.Source = "ICCR";
                    objEventLog.WriteEntry("ICCR Application Error: Session Expired");
                    
                    Server.Transfer("~/SessionExpired.aspx");
                    // not reached as server transfer above
                    return null;

                }
                else
                {
                    return (BigExchange.BookingPrintVoucher)Session["Booking"];
                }
            }
            set
            {
                Session["Booking"] = value;
            }
        }

        // variable that reads / sets the session variable 
        // kiosk. This is used to record the kiosk flag
        public bool Kiosk
        {
            get
            {
                if (Session["Kiosk"] == null)
                {
                    // default to normal CSD application
                    return true;
                }
                else
                {
                    return (bool)Session["Kiosk"];
                }
            }
            set
            {
                // session variable empty then can be set    
                if (Session["Kiosk"] == null)
                {
                    Session["Kiosk"] = value;
                }

            }
        }

        // variable that reads / sets the session variable 
        // ServerLocation. This is used to record the ServerLocation
        // CSD using a different server location, changes the page that 
        // user is redirected to on exit
        public string CSDServerName
        {
            get
            {
                if (Session["CSDServerName"] == null)
                {
                    // default to no different server location
                    return null;

                }
                else
                {
                    return (string)Session["CSDServerName"];
                }
            }
            set
            {
                // session variable empty then can be set  
                if (Session["CSDServerName"] == null)
                {
                    Session["CSDServerName"] = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [new CSD].
        /// </summary>
        /// <value><c>true</c> if [new CSD]; otherwise, <c>false</c>.</value>
        /// <remarks></remarks>
        public bool NewCSD
        {
            get
            {
                if (Session["NewCSD"] == null)
                {
                    return false;
                }
                else
                {
                    return (bool)Session["NewCSD"];
                }
            }
            set
            {
                if (Session["NewCSD"] == null)
                {
                    Session["NewCSD"] = value;
                }
            }
        }




        /// <summary>
        /// Gets or sets the page title.
        /// </summary>
        /// <value>The page title.</value>
        public string PageTitle
        {
            //get
            //{
            //    return lblHeaderPageSection.Text;
            //}
            set
            {
                // booking needs to be set before title can be used
                string trainingmode = "";
                //if (SessionBooking.TrainingMode)
                //{
                //    trainingmode = "TRAINING Mode ";
                //}
                // booking needs to be set before title can be used
                string kiosk = "";
                if (Kiosk)
                {
                    kiosk = "Kiosk ";
                }

                //lblHeaderPageSection.Text = kiosk + trainingmode + value;


//#if (DEBUG)
//                // in debug record page title data
//                //SessionBooking.SaveToTraining(lblHeaderPageSection.Text);
//#else

//                // when training record some page data
//                if (SessionBooking.TrainingMode)
//                {
//                    // use the title 
//                    SessionBooking.SaveToTraining("");
//                }
//#endif

            }
        }

        public bool Reprint
        {        
            set
            {
                if (value)
                {
                    //tblHeader.BackImageUrl = "Images/fwkHeaderFooterBar_training.png";
                    //tblFooter.BackImageUrl = "Images/fwkHeaderFooterBar_training.png";
                    //imgTescoStripe.ImageUrl = "Images/fwkFooterBarStyling_training.png";
                }
                else
                {
                    //tblHeader.BackImageUrl = "Images/fwkHeaderFooterBar.png";
                    //tblFooter.BackImageUrl = "Images/fwkHeaderFooterBar.png";
                    //imgTescoStripe.ImageUrl = "Images/fwkFooterBarStyling.png";                    
                }
                    
            }
        }


        // common routine for exiting CSD pages
        // uses web.config balue for link back to CSD menu page (typically)
        //public void ExitClick()
        //{

        //    Session["Booking"] = null;

        //    // call kiosk ... if kiosk ip address found
        //    // redirect
        //    string KioskUrl = InstoreClubcardReward.Business.Kiosk.KioskEntryPage(Request.UserHostAddress);
        //    if ( KioskUrl != string.Empty)
        //    {
        //        // kiosk found so redirect
        //        Response.Redirect(KioskUrl);
        //    }

        //    string homepageURL = string.Empty;

        //    if (NewCSD && !string.IsNullOrEmpty(ConfigurationManager.AppSettings["NewCSDHomepageURL"]))
        //    {
        //        homepageURL = ConfigurationManager.AppSettings["NewCSDHomepageURL"];
        //    }
        //    else if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["HomepageURL"]))
        //    {
        //        homepageURL = ConfigurationManager.AppSettings["HomepageURL"];
        //    }
            
        //    if (!string.IsNullOrEmpty(homepageURL))
        //    {

        //        if (string.IsNullOrEmpty(CSDServerName))
        //        {
        //            // no server name so just use app setting
        //            Response.Redirect(homepageURL);
        //        }
        //        // server name has a value
        //        else
        //        {
        //            try
        //            {
        //                // string to uri
        //                System.Uri uri = new System.Uri(homepageURL);              
        //                // use host name and replace with new server name
        //                string newuri = uri.OriginalString.Replace(uri.Host, CSDServerName);
        //                Response.Redirect(newuri);

        //            }
        //            catch (System.Threading.ThreadAbortException ex)
        //            {
        //                //thrown by the Response.Redirect but we want to suppress this as its an issue with Response.End within the Response.Redirect
        //            }
        //            catch (Exception ex)
        //            {
                        
        //                // use the original
        //                Response.Redirect(homepageURL);
                        
        //            }

        //        }
        //    }

        //}




        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //lblHeaderDate.Text = DateTime.Now.ToString("F");
            //if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["AjaxTimeoutSecs"]))
            //{
            //    ScriptManager1.AsyncPostBackTimeout = int.Parse(ConfigurationManager.AppSettings["AjaxTimeoutSecs"]);
            //}
        }
    }
}
