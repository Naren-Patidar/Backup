using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using System.Web;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.DBConfiguration;
using System.Web.Routing;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Utilities
{
    public class ValidatorUtility
    {
        #region ValidationMessage

        public static bool IsRequiredField(string fieldName)
        {
            bool isRequired;
            string configValue1 = null, configValue2;
            RouteData currentRoute = HttpContext.Current.Request.RequestContext.RouteData;
            string ctrl = currentRoute.GetRequiredString("controller");
            bool isJoin = false;
            //Set Property to pick  Join specific configuration
            isJoin = !(ctrl.ToLower().Equals("join")) ? false : true;

            List<DbConfigurationItem> test =  DBConfigurationManager.Instance.FindConfigs("Name1");
            switch (fieldName)
            {
                case "FirstName":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.Name1.ToString(), DbConfigurationTypeEnum.Mandatory_fields, out configValue1, out configValue2);
                    break;
                case "Initial":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.Name2.ToString(), DbConfigurationTypeEnum.Mandatory_fields, out configValue1, out configValue2);
                    break;
                case "LastName":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.Name3.ToString(), DbConfigurationTypeEnum.Mandatory_fields, out configValue1, out configValue2);
                    break;
                case "Title":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.TitleEnglish.ToString(), DbConfigurationTypeEnum.Mandatory_fields, out configValue1, out configValue2);
                    break;
                case "Sex":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.Sex.ToString(), DbConfigurationTypeEnum.Mandatory_fields, out configValue1, out configValue2);
                    break;
                case "ISOLanguageCode":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.Language.ToString(), DbConfigurationTypeEnum.Mandatory_fields, out configValue1, out configValue2);
                    break;
                case "RaceID":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.Race.ToString(), DbConfigurationTypeEnum.Mandatory_fields, out configValue1, out configValue2);
                    break;
                case "SSN":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.PrimaryId.ToString(), DbConfigurationTypeEnum.Mandatory_fields, out configValue1, out configValue2);
                    break;
                case "PassportNo":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.SecondaryId.ToString(), DbConfigurationTypeEnum.Mandatory_fields, out configValue1, out configValue2);
                    break;
                case "MailingAddressLine1":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.MailingAddressLine1.ToString(), DbConfigurationTypeEnum.Mandatory_fields, out configValue1, out configValue2);
                    break;
                case "MailingAddressLine2":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.MailingAddressLine2.ToString(), DbConfigurationTypeEnum.Mandatory_fields, out configValue1, out configValue2);
                    break;
                case "MailingAddressLine3":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.MailingAddressLine3.ToString(), DbConfigurationTypeEnum.Mandatory_fields, out configValue1, out configValue2);
                    break;
                case "MailingAddressLine4":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.MailingAddressLine4.ToString(), DbConfigurationTypeEnum.Mandatory_fields, out configValue1, out configValue2);
                    break;
                case "MailingAddressLine5":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.MailingAddressLine5.ToString(), DbConfigurationTypeEnum.Mandatory_fields, out configValue1, out configValue2);
                    break;
                case "MailingAddressLine6":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.MailingAddressLine6.ToString(), DbConfigurationTypeEnum.Mandatory_fields, out configValue1, out configValue2);
                    break;
                case "MailingAddressPostCode":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.MailingAddressPostCode.ToString(), DbConfigurationTypeEnum.Mandatory_fields, out configValue1, out configValue2);
                    break;
                case "DaytimePhonenumber":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.DaytimePhoneNumber.ToString(), DbConfigurationTypeEnum.Mandatory_fields, out configValue1, out configValue2);
                    break;
                case "MobilePhoneNumber":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.MobilePhoneNumber.ToString(), DbConfigurationTypeEnum.Mandatory_fields, out configValue1, out configValue2);
                    break;
                case "EveningPhonenumber":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.EveningPhoneNumber.ToString(), DbConfigurationTypeEnum.Mandatory_fields, out configValue1, out configValue2);
                    break;
                case "EmailAddress":
                    if (isJoin)
                        isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.EmailAddress.ToString(), DbConfigurationTypeEnum.JoinEmailMandatory, out configValue1, out configValue2);
                    else 
                        isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.EmailAddress.ToString(), DbConfigurationTypeEnum.Mandatory_fields, out configValue1, out configValue2);
                    break;
            }
            if (configValue1 == "1")
                return true;
            else
                return false;
        }

        public static void GetFormat(string fieldName, out string configValue1, out string configValue2)
        {
            configValue1 = string.Empty;
            configValue2 = string.Empty;
            try
            {
                switch (fieldName)
                {
                    case "FirstName":
                        ValidatorUtility.GetDBConfig(DbConfigurationItemNames.Name1.ToString(), DbConfigurationTypeEnum.Format, out configValue1, out configValue2);
                        break;
                    case "Initial":
                        ValidatorUtility.GetDBConfig(DbConfigurationItemNames.Name2.ToString(), DbConfigurationTypeEnum.Format, out configValue1, out configValue2);
                        break;
                    case "LastName":
                        ValidatorUtility.GetDBConfig(DbConfigurationItemNames.Name3.ToString(), DbConfigurationTypeEnum.Format, out configValue1, out configValue2);
                        break;
                    case "MailingAddressPostCode":
                        ValidatorUtility.GetDBConfig(DbConfigurationItemNames.MailingAddressPostCode.ToString(), DbConfigurationTypeEnum.Format, 
                            out configValue1,  out configValue2);
                        break;
                    case "EmailAddress":
                        ValidatorUtility.GetDBConfig(DbConfigurationItemNames.EmailAddress.ToString(), DbConfigurationTypeEnum.Format,
                              out configValue1, out configValue2);
                        break;
                    case "DaytimePhonenumber":
                        ValidatorUtility.GetDBConfig(DbConfigurationItemNames.PhoneNumber.ToString(), DbConfigurationTypeEnum.Format, out configValue1, out configValue2);
                        break;
                    case "EveningPhonenumber":
                        ValidatorUtility.GetDBConfig(DbConfigurationItemNames.PhoneNumber.ToString(), DbConfigurationTypeEnum.Format, out configValue1, out configValue2);
                        break;
                    case "MobilePhoneNumber":
                        ValidatorUtility.GetDBConfig(DbConfigurationItemNames.PhoneNumber.ToString(), DbConfigurationTypeEnum.Format, out configValue1, out configValue2);
                        break;
                    case "MailingAddressLine":
                        ValidatorUtility.GetDBConfig(DbConfigurationItemNames.MailingAddressLine.ToString(), DbConfigurationTypeEnum.Format, out configValue1, out configValue2);
                        break;
                    case "ClubcardNumber":
                        ValidatorUtility.GetDBConfig(DbConfigurationItemNames.ClubcardNumber.ToString(), DbConfigurationTypeEnum.Format, out configValue1, out configValue2);
                        break;
                    case "MailingAddressLine1":
                        ValidatorUtility.GetDBConfig(DbConfigurationItemNames.MailingAddressLine1.ToString(), DbConfigurationTypeEnum.Format, out configValue1, out configValue2);
                        break;
                    case "MailingAddressLine2":
                        ValidatorUtility.GetDBConfig(DbConfigurationItemNames.MailingAddressLine2.ToString(), DbConfigurationTypeEnum.Format, out configValue1, out configValue2);
                        break;
                    case "MailingAddressLine3":
                        ValidatorUtility.GetDBConfig(DbConfigurationItemNames.MailingAddressLine3.ToString(), DbConfigurationTypeEnum.Format, out configValue1, out configValue2);
                        break;
                    case "MailingAddressLine4":
                        ValidatorUtility.GetDBConfig(DbConfigurationItemNames.MailingAddressLine4.ToString(), DbConfigurationTypeEnum.Format, out configValue1, out configValue2);
                        break;
                    case "MailingAddressLine5":
                        ValidatorUtility.GetDBConfig(DbConfigurationItemNames.MailingAddressLine5.ToString(), DbConfigurationTypeEnum.Format, out configValue1, out configValue2);
                        break;
                    case "MailingAddressLine6":
                        ValidatorUtility.GetDBConfig(DbConfigurationItemNames.MailingAddressLine6.ToString(), DbConfigurationTypeEnum.Format, out configValue1, out configValue2);
                        break;
                    case "PassportNo":
                        ValidatorUtility.GetDBConfig(DbConfigurationItemNames.IdFormat.ToString(), DbConfigurationTypeEnum.Format, out configValue1, out configValue2);
                        break;
						case "SSN":
                        ValidatorUtility.GetDBConfig(DbConfigurationItemNames.IdFormat.ToString(), DbConfigurationTypeEnum.Format, out configValue1, out configValue2);
                        break;

                }

            }
            catch (Exception exp)
            {
                //add logging data for the exception
                throw exp;
            }
        }
        public static void GetLength(string fieldName, out string configValue1, out string configValue2)
        {
            configValue1 = string.Empty;
            configValue2 = string.Empty;
            try
            {
                switch (fieldName)
                {
                    case "FirstName":
                        ValidatorUtility.GetDBConfig(DbConfigurationItemNames.Name1.ToString(), DbConfigurationTypeEnum.Length_of_the_input_fields, out configValue1, out configValue2);
                        break;
                    case "Initial":
                        ValidatorUtility.GetDBConfig(DbConfigurationItemNames.Name2.ToString(), DbConfigurationTypeEnum.Length_of_the_input_fields, out configValue1, out configValue2);
                        break;
                    case "LastName":
                        ValidatorUtility.GetDBConfig(DbConfigurationItemNames.Name3.ToString(), DbConfigurationTypeEnum.Length_of_the_input_fields, out configValue1, out configValue2);
                        break;
                    case "MailingAddressPostCode":
                        ValidatorUtility.GetDBConfig(DbConfigurationItemNames.MailingAddressPostCode.ToString(), DbConfigurationTypeEnum.Length_of_the_input_fields,
                            out configValue1, out configValue2);
                        break;
                    //case "EmailAddress":
                    //    ValidatorUtility.GetDBConfig(DbConfigurationItemNames.EmailAddress.ToString(), DbConfigurationTypeEnum.Length_of_the_input_fields,
                    //          out configValue1, out configValue2);
                    //    break;
                    case "DaytimePhonenumber":
                        ValidatorUtility.GetDBConfig(DbConfigurationItemNames.DaytimePhoneNumber.ToString(), DbConfigurationTypeEnum.Length_of_the_input_fields, out configValue1, out configValue2);
                        break;
                    case "EveningPhonenumber":
                        ValidatorUtility.GetDBConfig(DbConfigurationItemNames.EveningPhoneNumber.ToString(), DbConfigurationTypeEnum.Length_of_the_input_fields, out configValue1, out configValue2);
                        break;
                    case "MobilePhoneNumber":
                        ValidatorUtility.GetDBConfig(DbConfigurationItemNames.MobilePhoneNumber.ToString(), DbConfigurationTypeEnum.Length_of_the_input_fields, out configValue1, out configValue2);
                        break;
                    case "MailingAddressLine1":
                        ValidatorUtility.GetDBConfig(DbConfigurationItemNames.MailingAddressLine1.ToString(), DbConfigurationTypeEnum.Length_of_the_input_fields, out configValue1, out configValue2);
                        break;
                    case "MailingAddressLine2":
                        ValidatorUtility.GetDBConfig(DbConfigurationItemNames.MailingAddressLine2.ToString(), DbConfigurationTypeEnum.Length_of_the_input_fields, out configValue1, out configValue2);
                        break;
                    case "MailingAddressLine3":
                        ValidatorUtility.GetDBConfig(DbConfigurationItemNames.MailingAddressLine3.ToString(), DbConfigurationTypeEnum.Length_of_the_input_fields, out configValue1, out configValue2);
                        break;
                    case "MailingAddressLine4":
                        ValidatorUtility.GetDBConfig(DbConfigurationItemNames.MailingAddressLine4.ToString(), DbConfigurationTypeEnum.Length_of_the_input_fields, out configValue1, out configValue2);
                        break;
                    case "MailingAddressLine5":
                        ValidatorUtility.GetDBConfig(DbConfigurationItemNames.MailingAddressLine5.ToString(), DbConfigurationTypeEnum.Length_of_the_input_fields, out configValue1, out configValue2);
                        break;
                    case "MailingAddressLine6":
                        ValidatorUtility.GetDBConfig(DbConfigurationItemNames.MailingAddressLine6.ToString(), DbConfigurationTypeEnum.Length_of_the_input_fields, out configValue1, out configValue2);
                        break;
                   

                }

            }
            catch (Exception exp)
            {
                //add logging data for the exception
                throw exp;
            }
        }
        public static void GetPrefix(string fieldName, out string configValue1, out string configValue2)
        {
            configValue1 = string.Empty;
            configValue2 = string.Empty;
            try
            {
                switch (fieldName)
                {
                    case "DaytimePhonenumber":
                        ValidatorUtility.GetDBConfig(DbConfigurationItemNames.DaytimePhoneNumber.ToString(), DbConfigurationTypeEnum.Prefix, out configValue1, out configValue2);
                        break;
                    case "MobilePhoneNumber":
                        ValidatorUtility.GetDBConfig(DbConfigurationItemNames.MobilePhoneNumber.ToString(), DbConfigurationTypeEnum.Prefix, out configValue1, out configValue2);
                        break;
                    case "EveningPhonenumber":
                        ValidatorUtility.GetDBConfig(DbConfigurationItemNames.EveningPhoneNumber.ToString(), DbConfigurationTypeEnum.Prefix, out configValue1, out configValue2);
                        break;
                }
              
            }
            catch (Exception exp)
            {
                //add logging data for the exception
                throw exp;
            }
        }
       
        
        public static bool IsNotVisible(string fieldName)
        {
            bool isRequired;
            string configValue1 = "0", configValue2;
            switch (fieldName)
            {
                case "Title":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.ChinaHiddenFunctionalityTitle.ToString(), DbConfigurationTypeEnum.ChinaHiddenFunctionality, out configValue1, out configValue2);
                    break;
                case "FirstName":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.HideFirstName.ToString(), DbConfigurationTypeEnum.ChinaHiddenFunctionality, out configValue1, out configValue2);
                    break;
                case "Initial":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.ChinaHiddenFunctionalityMiddleName.ToString(), DbConfigurationTypeEnum.ChinaHiddenFunctionality, out configValue1, out configValue2);
                    break;
                case "LastName":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.HideSurName.ToString(), DbConfigurationTypeEnum.ChinaHiddenFunctionality, out configValue1, out configValue2);
                    break;
                case "DOB":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.HideDOB.ToString(), DbConfigurationTypeEnum.ChinaHiddenFunctionality, out configValue1, out configValue2);
                    break;
                case "Sex":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.HideGender.ToString(), DbConfigurationTypeEnum.ChinaHiddenFunctionality, out configValue1, out configValue2);
                    break;
                case "MobilePhoneNumber":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.HideMobilePhoneNo.ToString(), DbConfigurationTypeEnum.ChinaHiddenFunctionality, out configValue1, out configValue2);
                    break;
                case "DaytimePhonenumber":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.HideDayTimePhoneNo.ToString(), DbConfigurationTypeEnum.ChinaHiddenFunctionality, out configValue1, out configValue2);
                    break;
                case "EveningPhonenumber":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.HideEveningPhoneNo.ToString(), DbConfigurationTypeEnum.ChinaHiddenFunctionality, out configValue1, out configValue2);
                    break;
                case "SSN":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.HidePrimaryId.ToString(), DbConfigurationTypeEnum.ChinaHiddenFunctionality, out configValue1, out configValue2);
                    break;
                case "PassportNo":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.HideSecondaryId.ToString(), DbConfigurationTypeEnum.ChinaHiddenFunctionality, out configValue1, out configValue2);
                    break;
                case "MailingAddressLine1":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.HideAddressLine1.ToString(), DbConfigurationTypeEnum.ChinaHiddenFunctionality, out configValue1, out configValue2);
                    break;
                case "MailingAddressLine2":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.HideAddressLine2.ToString(), DbConfigurationTypeEnum.ChinaHiddenFunctionality, out configValue1, out configValue2);
                    break;
                case "MailingAddressLine3":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.HideAddressLine3.ToString(), DbConfigurationTypeEnum.ChinaHiddenFunctionality, out configValue1, out configValue2);
                    break;
                case "MailingAddressLine4":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.HideAddressLine4.ToString(), DbConfigurationTypeEnum.ChinaHiddenFunctionality, out configValue1, out configValue2);
                    break;
                case "MailingAddressLine5":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.HideAddressLine5.ToString(), DbConfigurationTypeEnum.ChinaHiddenFunctionality, out configValue1, out configValue2);
                    break;
                case "MailingAddressLine6":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.HideAddressLine6.ToString(), DbConfigurationTypeEnum.ChinaHiddenFunctionality, out configValue1, out configValue2);
                    break;
                case "MailingAddressPostCode":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.MailingAddressPostCode.ToString(), DbConfigurationTypeEnum.ChinaHiddenFunctionality, out configValue1, out configValue2);
                    break;
                case "RaceID":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.HideRace.ToString(), DbConfigurationTypeEnum.ChinaHiddenFunctionality, out configValue1, out configValue2);
                    break;
                case "ISOLanguageCode":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.HidePreferredLanguage.ToString(), DbConfigurationTypeEnum.ChinaHiddenFunctionality, out configValue1, out configValue2);
                    break;
                case "EmailAddress":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.HideEmail.ToString(), DbConfigurationTypeEnum.ChinaHiddenFunctionality, out configValue1, out configValue2);
                    break;
                    
            }
            if (configValue1 == "1")
                return true;
            else
                return false;

        }

        public static bool IsActivationRequired(string fieldName)
        {
            bool isRequired;
            string configValue1 = "0", configValue2;
            switch (fieldName)
            {
                case "ClubcardNumber":
                    isRequired = true;
                    configValue1 = "1";
                    break;
                case "FirstName":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.Name1.ToString(), DbConfigurationTypeEnum.Activation, out configValue1, out configValue2);
                    break;
                case "LastName":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.Name3.ToString(), DbConfigurationTypeEnum.Activation, out configValue1, out configValue2);
                    break;
                case "MailingAddressLine1":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.MailingAddressLine1.ToString(), DbConfigurationTypeEnum.Activation, out configValue1, out configValue2);
                    break;
                case "MobilePhoneNumber":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.MobilePhoneNumber.ToString(), DbConfigurationTypeEnum.Activation, out configValue1, out configValue2);
                    break;
                case "EmailAddress":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.EmailAddress.ToString(), DbConfigurationTypeEnum.Activation, out configValue1, out configValue2);
                    break;
                case "MailingAddressPostCode":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.MailingAddressPostCode.ToString(), DbConfigurationTypeEnum.Activation, out configValue1, out configValue2)
                        || ValidatorUtility.GetDBConfig(DbConfigurationItemNames.MailingAddressPostCode3Digits.ToString(), DbConfigurationTypeEnum.Activation, out configValue1, out configValue2);
                    break;
                case "SSN":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.SSN.ToString(), DbConfigurationTypeEnum.Activation, out configValue1, out configValue2);
                    break;
                case "DayOfBirth":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.DayofBirth.ToString(), DbConfigurationTypeEnum.Activation, out configValue1, out configValue2);
                    break;
                case "MonthOfBirth":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.MonthofBirth.ToString(), DbConfigurationTypeEnum.Activation, out configValue1, out configValue2);
                    break;
                case "YearOfBirth":
                    isRequired = ValidatorUtility.GetDBConfig(DbConfigurationItemNames.YearofBirth.ToString(), DbConfigurationTypeEnum.Activation, out configValue1, out configValue2);
                    break;
            }
            if (configValue1 == "1")
                return true;
            else
                return false;

        }
        
        #endregion

        #region Get Configuration value

        private static bool GetDBConfig(string name, DbConfigurationTypeEnum type, out string configValue1, out string configValue2)
        {
            configValue1 = null;
            configValue2 = null;

            if (DBConfigurationManager.Instance[type][name] != null)
            {
                configValue1 = DBConfigurationManager.Instance[type][name].ConfigurationValue1.TryParse<string>();
                configValue2 = DBConfigurationManager.Instance[type][name].ConfigurationValue2.TryParse<string>();
                return true;
            }
            
            return false;
        }
        
        #endregion 

        #region Page Authorization methods

        public static bool AuthorizePage(string currentModule, string currentAction)
        {
            bool isAuthorized = false;
            DBConfigurations HideJoinFunctionalityConfiguration = DBConfigurationManager.Instance[DbConfigurationTypeEnum.HideJoinFunctionality];
            DBConfigurations HomePageStampsConfiguration = DBConfigurationManager.Instance[DbConfigurationTypeEnum.HomePageStamps];          
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
                    switch (currentAction.ToUpper())
                    {
                        case "HOME":
                            cfHidePage = HideJoinFunctionalityConfiguration.GetConfigurationItem(DbConfigurationItemNames.HidePointsPage);
                            isAuthorized = IsAuthorizedDetails(cfHidePage, cfHideStamp);
                            break;
                        case "POINTSSUMMARYVIEW":
                            cfHidePage = HideJoinFunctionalityConfiguration.GetConfigurationItem(DbConfigurationItemNames.HidePointsSummaryPage);
                            isAuthorized = IsAuthorizedDetails(cfHidePage, cfHideStamp);
                            break;
                        case "POINTSDETAIL":
                            cfHidePage = HideJoinFunctionalityConfiguration.GetConfigurationItem(DbConfigurationItemNames.HidePointDetailsPage);
                            isAuthorized = IsAuthorizedDetails(cfHidePage, cfHideStamp);
                            break;
                    }
                    break;
                case "BOOSTSATTESCO":
                    cfHidePage = HideJoinFunctionalityConfiguration.GetConfigurationItem(DbConfigurationItemNames.HideExchangesPage);
                    isAuthorized = IsAuthorizedDetails(cfHidePage, cfHideStamp);

                    break;
                case "MYLATESTSTATEMENT":
                    cfHidePage = HideJoinFunctionalityConfiguration.GetConfigurationItem(DbConfigurationItemNames.HideLatestStatementPage);
                    isAuthorized = IsAuthorizedDetails(cfHidePage, cfHideStamp);
                    break;
                case "CHRISTMASSAVER":
                    cfHidePage = HideJoinFunctionalityConfiguration.GetConfigurationItem(DbConfigurationItemNames.HideChristmasSaversPage);
                    isAuthorized = IsAuthorizedDetails(cfHidePage, cfHideStamp);
                    break;
                case "PERSONALDETAILS":
                    cfHidePage = HideJoinFunctionalityConfiguration.GetConfigurationItem(DbConfigurationItemNames.HidePersonalDetails);
                    isAuthorized = IsAuthorizedDetails(cfHidePage, cfHideStamp);
                    break;
                case "MYACCOUNTDETAILS":
                    switch (currentAction.ToUpper())
                    {
                        case "OPTIONSANDBENEFITS":
                            cfHidePage = HideJoinFunctionalityConfiguration.GetConfigurationItem(DbConfigurationItemNames.HideOptionsandBenefits);
                            isAuthorized = IsAuthorizedDetails(cfHidePage, cfHideStamp);
                            break;
                        case "MYCONTACTPREFERENCES":
                            cfHidePage = HideJoinFunctionalityConfiguration.GetConfigurationItem(DbConfigurationItemNames.HidePreferencesPage);
                            isAuthorized = IsAuthorizedDetails(cfHidePage, cfHideStamp);
                            break;
                        case "VIEWMYCARDS":
                            cfHidePage = HideJoinFunctionalityConfiguration.GetConfigurationItem(DbConfigurationItemNames.HideManageCardsPage);
                            isAuthorized = IsAuthorizedDetails(cfHidePage, cfHideStamp);
                            break;
                    }
                    break;
            }
            return isAuthorized;
        }

        private static bool IsAuthorizedDetails(DbConfigurationItem cfHidePage, DbConfigurationItem cfHideStamp)
        {
            return (
                        cfHidePage == null ||
                        (cfHidePage != null && cfHidePage.IsDeleted) ||
                        (cfHidePage != null && !cfHidePage.IsDeleted && cfHidePage.ConfigurationValue1.Equals("0"))
                        || (
                            cfHideStamp == null ||
                            (cfHideStamp != null && !cfHideStamp.IsDeleted)
                        )
                    );
        }

        #endregion
    }
}
