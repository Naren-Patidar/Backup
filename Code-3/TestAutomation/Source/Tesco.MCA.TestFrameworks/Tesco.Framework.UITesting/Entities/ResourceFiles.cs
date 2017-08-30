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

         public string MYACCOUNTlOCAL_NODE = "MyAccount.master.{0}.resx";
         public string POINTSDETAILSPOINTS_NODE = "PointsDetail.aspx.{0}.resx";
         public string POINTSSUMMARYPOINTS_NODE = "PointsSummary.aspx.{0}.resx";
      

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

            MYACCOUNTlOCAL_NODE = string.Format(MYACCOUNTlOCAL_NODE, CountrySetting.culture);
            POINTSDETAILSPOINTS_NODE = string.Format(POINTSDETAILSPOINTS_NODE, CountrySetting.culture);
            POINTSSUMMARYPOINTS_NODE = string.Format(POINTSSUMMARYPOINTS_NODE, CountrySetting.culture);
        }

        

    }

}
