using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
using System.ServiceModel;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Diagnostics;

namespace Tesco.Com.Marketing.Kiosk.JoinAtKiosk.Kiosk
{
    public partial class PrintClubcard : BaseUIPage
    {
        public string fontStyleStr = string.Empty;
        public string resourceStr = string.Empty;
        public string barcodeConfigStr = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Logger.Write("Start of PrintClubcard Page_Load()", "General");
                StringBuilder ClubcardPrintScript = new StringBuilder();

                ClubcardPrintScript.Append("<script language=\"javascript\" type=\"text/javascript\">\n");

                ClubcardPrintScript.Append("PrintClubcard('" + Request.QueryString["clubcardID"].ToString().Trim() + "');");

                //ClubcardPrintScript.Append("PrintClubcard('123456789');");

                ClubcardPrintScript.Append("</script>\n\n");

                ClientScript.RegisterStartupScript(typeof(String), "ClientManagerBottom", ClubcardPrintScript.ToString());

                
                SetResourceStr();
                Helper.DeleteAllCookies();

            }
            catch (Exception exception)
            {
                Logger.Write(exception, "General", 1, 3500, TraceEventType.Error, "Join at Kiosk");
                throw exception;
            }
        }

        private void SetResourceStr()
        {
            //fontStyleStr = ConfigurationManager.AppSettings["Font1"] + "," + ConfigurationManager.AppSettings["Font2"] + "," + ConfigurationManager.AppSettings["Font3"];
            fontStyleStr = ConfigurationReader.GetStringConfigKey("Font1") + "," + ConfigurationReader.GetStringConfigKey("Font2") + "," + ConfigurationReader.GetStringConfigKey("Font3");
            resourceStr = Resources.GlobalResources.ImagePath + "," + Resources.GlobalResources.TempCardWelcomeMsg + "," + Resources.GlobalResources.PointsMsg + "," +
                          Resources.GlobalResources.ScanMsg + "," + Resources.GlobalResources.ShopMsg + "," + Resources.GlobalResources.KeepTempCardMsg + "," +
                          Resources.GlobalResources.PostMsg + "," + Resources.GlobalResources.PostMsg1 + "," + Resources.GlobalResources.ThankYouMsg + "," + Resources.GlobalResources.Tesco;
            //barcodeConfigStr = ConfigurationManager.AppSettings["BarcodeLastNos"] + "," + ConfigurationManager.AppSettings["BarcodeForCountry"];
            barcodeConfigStr = ConfigurationReader.GetStringConfigKey("BarcodeLastNos") + "," + ConfigurationReader.GetStringConfigKey("BarcodeForCountry");
        }
    }
}
