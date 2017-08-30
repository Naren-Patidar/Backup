using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using InstoreClubcardReward.Data;

namespace InstoreClubcardRewardKiosk
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["InstoreRewardBooking"].ConnectionString;
            // basic url - web config setting
            string EntryPageURL = ConfigurationManager.AppSettings["EntryPageURL"];

            // client IP address
            string clientIP = Request.UserHostAddress;

            // string for constructing parameters
            string parameters="";

            // collection for output from stored procedure
            try
            {
                System.Collections.ObjectModel.Collection<SelectKioskRow> KioskRow;
                KioskRow = SelectKiosk.Execute(ConnectionString, clientIP);

                // if there is a record for the ip address
                if (KioskRow.Count == 1)
                {
                    // select on key so only one entry
                    foreach( SelectKioskRow row in KioskRow)
                    {
                        parameters =  string.Format("?TillNumber={0}&StoreID={1}&Mode={2}&UserId={3}&Kiosk=1", row.TillId, row.StoreId, row.TrainingMode, row.UserId);
                        break;
                    }


                }
                else
                {
                    parameters = string.Format("?TillNumber={0}&StoreID={1}&Mode={2}&UserId={3}&Kiosk=1", 6, 6, 1, 6);

                }
            }
            catch(Exception ex)
            {
                // standard parameters
                parameters = string.Format("?TillNumber={0}&StoreID={1}&Mode={2}&UserId={3}&Kiosk=1", 6, 6, 1, 6);
            }

                finally
            {
                Response.Redirect(EntryPageURL + parameters);
       
            }


        }
    }
}
