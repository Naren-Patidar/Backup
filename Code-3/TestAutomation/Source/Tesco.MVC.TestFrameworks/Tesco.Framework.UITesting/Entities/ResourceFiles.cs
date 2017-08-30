using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesco.Framework.UITesting.Entities
{
    public class ResourceFiles
    {
         public string ACTIVATION_RESOURCE = "Activation-App_LocalResources.{0}.xml";
         public string BOOST_RESOURCE = "BoostsAtTesco-App_LocalResources.{0}.xml";
         public string CHRISTMASSAVER_RESOURCE = "ChristmasSavers-App_LocalResources.{0}.xml";
         public string GLOBAL_RESOURCE = "ClubcardOnline.Web-App_GlobalResources.{0}.xml";
         public string LOCAL_RESOURCE = "ClubcardOnline.Web-App_LocalResources.{0}.xml";
         public string CONTROLS_RESOURCE = "Controls-App_LocalResources.{0}.xml";
         public string COUPON_RESOURCE = "eCoupon-App_LocalResources.{0}.xml";
         public string FUELSAVE_RESOURCE = "FuelSave_remove-App_LocalResources.{0}.xml";
         public string JOIN_RESOURCE = "Join-App_LocalResources.{0}.xml";
         public string LATESTSTATEMENT_RESOURCE = "LatestStatement-App_LocalResources.{0}.xml";
         public string MANAGECARDS_RESOURCE = "ManageCards-App_LocalResources.{0}.xml";
         public string OPTIONANDBENEFIT_RESOURCE = "Options-benefits-App_LocalResources.{0}.xml";
         public string ORDERREPLACEMENT_RESOURCE = "OrderAReplacement-App_LocalResources.{0}.xml";
         public string PERSONALDETAILS_RESOURCE = "PersonalDetails-App_LocalResources.{0}.xml";
         public string POINTS_RESOURCE = "Points-App_LocalResources.{0}.xml";
         public string PREFERENCES_RESOURCE = "Preferences-App_LocalResources.{0}.xml";
         public string HOMESECURITY_RESOURCE = "SecurityStage-App_LocalResources.{0}.xml";
         public string VOUCHER_RESOURCE = "Vouchers-App_LocalResources.{0}.xml";
         public string HOME_RESOURCE = "Home-App_LocalResources.{0}.xml";

         public string MYACCOUNTLOCAL_NODE = "_LeftNavigation.cshtml.{0}.resx";
         public string POINTSDETAILSPOINTS_NODE = "PointsDetail.cshtml.{0}.resx";
         public string POINTSSUMMARYPOINTS_NODE = "PointsSummaryView.cshtml.{0}.resx";
         public string _POINTS_NODE = "_Points.cshtml.{0}.resx";
         public string HEADER_RESOURCE = "Header-App_LocalResources.{0}.xml";
      

        public ResourceFiles()
        {
            ACTIVATION_RESOURCE = string.Format(ACTIVATION_RESOURCE, CountrySetting.culture);
            BOOST_RESOURCE = string.Format(BOOST_RESOURCE, CountrySetting.culture);
            CHRISTMASSAVER_RESOURCE = string.Format(CHRISTMASSAVER_RESOURCE, CountrySetting.culture);
            GLOBAL_RESOURCE = string.Format(GLOBAL_RESOURCE, CountrySetting.culture);
            LOCAL_RESOURCE = string.Format(LOCAL_RESOURCE, CountrySetting.culture);
            CONTROLS_RESOURCE = string.Format(CONTROLS_RESOURCE, CountrySetting.culture);
            COUPON_RESOURCE = string.Format(COUPON_RESOURCE, CountrySetting.culture);
            FUELSAVE_RESOURCE = string.Format(FUELSAVE_RESOURCE, CountrySetting.culture);
            JOIN_RESOURCE = string.Format(JOIN_RESOURCE, CountrySetting.culture);
            LATESTSTATEMENT_RESOURCE = string.Format(LATESTSTATEMENT_RESOURCE, CountrySetting.culture);
            MANAGECARDS_RESOURCE = string.Format(MANAGECARDS_RESOURCE, CountrySetting.culture);
            OPTIONANDBENEFIT_RESOURCE = string.Format(OPTIONANDBENEFIT_RESOURCE, CountrySetting.culture);
            ORDERREPLACEMENT_RESOURCE = string.Format(ORDERREPLACEMENT_RESOURCE, CountrySetting.culture);
            PERSONALDETAILS_RESOURCE = string.Format(PERSONALDETAILS_RESOURCE, CountrySetting.culture);
            POINTS_RESOURCE = string.Format(POINTS_RESOURCE, CountrySetting.culture);
            PREFERENCES_RESOURCE = string.Format(PREFERENCES_RESOURCE, CountrySetting.culture);
            HOMESECURITY_RESOURCE = string.Format(HOMESECURITY_RESOURCE, CountrySetting.culture);
            VOUCHER_RESOURCE = string.Format(VOUCHER_RESOURCE, CountrySetting.culture);
            HOME_RESOURCE = string.Format(HOME_RESOURCE, CountrySetting.culture);
            HEADER_RESOURCE = string.Format(HEADER_RESOURCE, CountrySetting.culture);

            MYACCOUNTLOCAL_NODE = string.Format(MYACCOUNTLOCAL_NODE, CountrySetting.culture);
            POINTSDETAILSPOINTS_NODE = string.Format(POINTSDETAILSPOINTS_NODE, CountrySetting.culture);
            POINTSSUMMARYPOINTS_NODE = string.Format(POINTSSUMMARYPOINTS_NODE, CountrySetting.culture);
            _POINTS_NODE = string.Format(_POINTS_NODE, CountrySetting.culture);
        }

        

    }
    public class HTMLFiles
    {
        public string HEADER_HTML = "Header.{0}.html";
        public string FOOTER_HTML = "Footer.{0}.html";
        public string MLS_FOOTER_HTML = "_Footer.html";

        public string BOOSTHOLDING_HTML = "BoostAtTescoHoldingPage.html";
        public string XMASHOLDING_HTML = "ChristmasSaversHoldingPage.html";
        public string COUPONHOLDING_HTML = "CouponHoldingPage.html";
        public string MLSHOLDING_HTML = "MyLatestStatementHoldingPage.html";
        public string ORDERREPLACEMNTHOLDING_HTML = "OrderAReplacementHoldingPage.html";
        public string POINTSHOLDING_HTML = "PointsHoldingPage.html";
        public string VOUCHERHOLDING_HTML = "VoucherHoldingPage.html";
        
        
        public string ERROR_HTML = "Error.html";

        public HTMLFiles()
        {
            HEADER_HTML = string.Format(HEADER_HTML, CountrySetting.culture);
            FOOTER_HTML = string.Format(FOOTER_HTML, CountrySetting.culture);
            MLS_FOOTER_HTML = string.Format(MLS_FOOTER_HTML);

            BOOSTHOLDING_HTML = string.Format(BOOSTHOLDING_HTML);
            XMASHOLDING_HTML = string.Format(XMASHOLDING_HTML);
            COUPONHOLDING_HTML = string.Format(COUPONHOLDING_HTML);
            MLSHOLDING_HTML = string.Format(MLSHOLDING_HTML);
            ORDERREPLACEMNTHOLDING_HTML = string.Format(ORDERREPLACEMNTHOLDING_HTML);
            POINTSHOLDING_HTML = string.Format(POINTSHOLDING_HTML);
            VOUCHERHOLDING_HTML = string.Format(VOUCHERHOLDING_HTML);
        }
    }



}
