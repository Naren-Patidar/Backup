using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.DBConfiguration;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using System.Web.Routing;
using System.Configuration;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using Microsoft.Practices.ServiceLocation;

namespace Tesco.ClubcardProducts.MCA.Web.MVCAttributes
{
    public class PageAuthorizationAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool isAuthorized = false;
            DBConfigurations HideJoinFunctionalityConfiguration = DBConfigurationManager.Instance[DbConfigurationTypeEnum.HideJoinFunctionality];
            DBConfigurations HomePageStampsConfiguration = DBConfigurationManager.Instance[DbConfigurationTypeEnum.HomePageStamps];
            RouteData currentRoute = httpContext.Request.RequestContext.RouteData;
            string currentModule = currentRoute.GetRequiredString("controller");
            string currentAction = currentRoute.GetRequiredString("action");
            DbConfigurationItem cfHidePage, cfHideStamp;

            cfHideStamp = HomePageStampsConfiguration.GetConfigurationByValue1(currentModule);
            switch (currentModule.ToUpper())
            {
                case "COUPONS":
                    cfHidePage = HideJoinFunctionalityConfiguration.GetConfigurationItem(DbConfigurationItemNames.HideeCouponPage);
                    isAuthorized = IsAuthorizedDetails(cfHidePage, cfHideStamp);
                    break;
                case "VOUCHERS":
                    cfHidePage = HideJoinFunctionalityConfiguration.GetConfigurationItem(DbConfigurationItemNames.HideVouchersPage);
                    isAuthorized = IsAuthorizedDetails(cfHidePage, cfHideStamp);
                    break;
                case "POINTS":
                    cfHideStamp = HomePageStampsConfiguration.GetConfigurationByValue1(currentAction);
                    switch (currentAction.ToUpper())
                    {
                        case "HOME":
                            cfHidePage = HideJoinFunctionalityConfiguration.GetConfigurationItem(DbConfigurationItemNames.HidePointsPage);
                            isAuthorized = IsAuthorizedDetails(cfHidePage, cfHideStamp);
                            break;
                        case "POINTSSTATEMENT":
                            cfHidePage = HideJoinFunctionalityConfiguration.GetConfigurationItem(DbConfigurationItemNames.HidePointsSummaryPage);
                            isAuthorized = IsAuthorizedDetails(cfHidePage, cfHideStamp);
                            break;
                        case "TRANSACTIONHISTORY":
                            cfHidePage = HideJoinFunctionalityConfiguration.GetConfigurationItem(DbConfigurationItemNames.HidePointDetailsPage);
                            isAuthorized = IsAuthorizedDetails(cfHidePage, cfHideStamp);
                            break;
                    }
                    break;
                case "BOOSTSATTESCO":
                    cfHidePage = HideJoinFunctionalityConfiguration.GetConfigurationItem(DbConfigurationItemNames.HideExchangesPage);
                    isAuthorized = IsAuthorizedDetails(cfHidePage, cfHideStamp);
                    break;
                case "LATESTSTATEMENT":
                    cfHidePage = HideJoinFunctionalityConfiguration.GetConfigurationItem(DbConfigurationItemNames.HideLatestStatementPage);
                    isAuthorized = IsAuthorizedDetails(cfHidePage, cfHideStamp);
                    break;
                case "CHRISTMASSAVERS":
                    cfHidePage = HideJoinFunctionalityConfiguration.GetConfigurationItem(DbConfigurationItemNames.HideChristmasSaversPage);
                    isAuthorized = IsAuthorizedDetails(cfHidePage, cfHideStamp);
                    break;
                case "ACCOUNTMANAGEMENT":
                    cfHideStamp = HomePageStampsConfiguration.GetConfigurationByValue1(currentAction);
                    switch (currentAction.ToUpper())
                    {
                        case "PERSONALDETAILS":
                            cfHidePage = HideJoinFunctionalityConfiguration.GetConfigurationItem(DbConfigurationItemNames.HidePersonalDetails);
                            isAuthorized = IsAuthorizedDetails(cfHidePage, cfHideStamp);
                            break;
                        case "CONTACTPREFERENCES":
                            cfHidePage = HideJoinFunctionalityConfiguration.GetConfigurationItem(DbConfigurationItemNames.HidePreferencesPage);
                            isAuthorized = IsAuthorizedDetails(cfHidePage, cfHideStamp);
                            break;
                        case "VOUCHERSCHEMES":
                            cfHidePage = HideJoinFunctionalityConfiguration.GetConfigurationItem(DbConfigurationItemNames.HideOptionsandBenefits);
                            isAuthorized = IsAuthorizedDetails(cfHidePage, cfHideStamp);
                            break;
                        case "CLUBCARDSONACCOUNT":
                            cfHidePage = HideJoinFunctionalityConfiguration.GetConfigurationItem(DbConfigurationItemNames.HideManageCardsPage);
                            isAuthorized = IsAuthorizedDetails(cfHidePage, cfHideStamp);
                            break;
                        case "ORDERANEWCARD":
                            cfHidePage = HideJoinFunctionalityConfiguration.GetConfigurationItem(DbConfigurationItemNames.HideManageCardsPage);
                            isAuthorized = IsAuthorizedDetails(cfHidePage, cfHideStamp);
                            break;
                    }
                    break;
            }
            return isAuthorized;
        }

        private bool IsAuthorizedDetails(DbConfigurationItem cfHidePage, DbConfigurationItem cfHideStamp)
        {
            return (
                        cfHidePage == null ||
                        (cfHidePage != null && cfHidePage.IsDeleted) ||
                        (cfHidePage != null && !cfHidePage.IsDeleted && cfHidePage.ConfigurationValue1.Equals("0"))
                        || (
                            cfHideStamp == null ||
                            (cfHideStamp != null && !cfHideStamp.IsDeleted && !string.IsNullOrEmpty(cfHideStamp.ConfigurationName))
                        )
                    );
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            IConfigurationProvider ConfigProvider = ServiceLocator.Current.GetInstance<IConfigurationProvider>();
            if (ConfigProvider.GetStringAppSetting(AppConfigEnum.IsDotcomEnvironment) == "1")
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(
                        new
                        {
                            controller = "Home",
                            action = "Home"
                        })
                    );
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(
                        new
                        {
                            controller = "Home",
                            action = "Home"
                        })
                    );
            }
        }
    }
}
