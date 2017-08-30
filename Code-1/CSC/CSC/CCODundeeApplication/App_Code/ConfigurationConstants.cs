using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Collections;
using System.Text;
using System.Xml;

namespace CCODundeeApplication
{
    public class ConfigurationConstants
    {
        #region Configuration Constants
            public const string EmailAddress = "EmailAddress" ;
            public const string NameANDAddress = "NameANDAddress" ;
            public const string MobilePhoneNumber = "MobilePhoneNumber" ;
            public const string TitleEnglish = "TitleEnglish" ;            
            public const string Sex = "Sex" ;
            public const string MailingAddressPostCode = "MailingAddressPostCode" ;            
            public const string MailingAddressLine1 = "MailingAddressLine1" ;
            public const string MailingAddressLine2 = "MailingAddressLine2" ;
            public const string MailingAddressLine3 = "MailingAddressLine3" ;
            public const string MailingAddressLine4 = "MailingAddressLine4" ;
            public const string MailingAddressLine5 = "MailingAddressLine5" ;
            public const string Language = "Language" ;
            public const string PrimaryId = "PrimaryId" ;
            public const string SecondaryId = "SecondaryId" ;
            public const string Race = "Race" ;
            public const string FourthFive = "45" ;
            public const string DaytimePhoneNumber = "DaytimePhoneNumber" ;          
            public const string PrimId = "PrimId" ;
            public const string SecId = "SecId" ;
            public const string Name1 = "Name1" ;
            public const string Name2 = "Name2" ;
            public const string Name3 = "Name3" ;
            public const string EveningPhoneNumber = "EveningPhoneNumber" ;
            public const string GroupCountryAddress = "GroupCountryAddress" ;
            public const string DisplayIds = "DisplayIds" ;
            public const string PtsSummaryDates = "PtsSummaryDates" ;
            public const string YourExchangesDates = "YourExchangesDates" ;
            public const string XmasSaverCurrDates = "XmasSaverCurrDates" ;
            public const string XmasSaverNextDates = "XmasSaverNextDates" ;
            public const string SmartVoucherDates = "SmartVoucherDates" ;
            public const string YourExchangesFlag = "YourExchangesFlag" ;
            public const string MyLatestStatementDates = "MyLatestStatementDates" ;            
            public const string IdFormat = "IdFormat" ;           
            public const string Phone = "Phone" ;
            public const string Numeric = "Numeric" ;
            public const string PhoneNumber = "PhoneNumber" ;
            public const string Date = "Date" ;
            public const string ClubcardNumber = "ClubcardNumber" ;
            public const string MailingAddressLine = "MailingAddressLine" ;           
            public const string JoinRouteCode = "Join Route Code" ;
            public const string IsProfanityCheckNeeded = "Is Profanity Check Needed" ;
            public const string TransactionlDB = "TransactionlDB" ;
            public const string ExpiredAccountProcesses = "ExpiredAccountProcesses" ;
            public const string UncontactableAccount = "Uncontactable Account" ;
            public const string InactiveAccount = "Inactive Account" ;
            public const string HideJoinFunctionality = "HideJoinFunctionality" ;
            public const string HideMyFuelSaverPage = "HideMyFuelSaverPage" ;
            public const string HideManageCardsPage = "HideManageCardsPage" ;
            public const string HideVouchersPage = "HideVouchersPage" ;
            public const string HideeCouponPage = "HideeCouponPage" ;
            public const string HideLatestStatementPage = "HideLatestStatementPage" ;
            public const string HideExchangesPage = "HideExchangesPage" ;
            public const string HideOrderAReplacementPage = "HideOrderAReplacementPage" ;           
            public const string MailingAddressPostCode3Digits = "MailingAddressPostCode3Digits" ;          
            public const string SSN = "SSN" ;           
            public const string DayofBirth = "DayofBirth" ;
            public const string MonthofBirth = "MonthofBirth" ;
            public const string YearofBirth = "YearofBirth" ;           
            public const string BirthDay = "Birth Day" ;
            public const string ShowMyCouponPage = "ShowMyCouponPage" ;
            public const string StoreNamePrefix = "StoreNamePrefix" ;
            public const string StringConcatenator = "StringConcatenator" ;
            public const string CarriedForward = "CarriedForward" ;
            public const string ChinaHiddenFunctionalityEveningPhoneNo = "ChinaHiddenFunctionalityEveningPhoneNo" ;
            public const string ChinaHiddenFunctionalityTitle = "ChinaHiddenFunctionalityTitle" ;
            public const string ChinaHiddenFunctionalityMiddleName = "ChinaHiddenFunctionalityMiddleName" ;
            public const string HideFirstName = "HideFirstName" ;
            public const string FourthThree = "43" ;
            public const string FourthEight = "48" ;         
            public const string EmailLinkExpirationTime = "EmailLinkExpirationTime" ;
            public const string CollectionPeriodMonth = "CollectionPeriodMonth" ;
            public const string SecureVoucherPage = "SecureVoucherPage" ;
            public const string ClubcardSecureDigits = "ClubcardSecureDigits" ;
            public const string EmailNotification = "EmailNotification" ;
            public const string TrackFirstName = "TrackFirstName" ;
            public const string TrackMiddleName = "TrackMiddleName" ;
            public const string TrackSurname = "TrackSurname" ;
            public const string TrackDOB = "TrackDOB" ;
            public const string TrackGender = "TrackGender" ;
            public const string TrackAddress = "TrackAddress" ;
            public const string TrackEmail = "TrackEmail" ;
            public const string TrackMobileNumber = "TrackMobileNumber" ;
            public const string TrackDayTimePhoneNumber = "TrackDayTimePhoneNumber" ;
            public const string TrackEvengingPhoneNumber = "TrackEvengingPhoneNumber" ;

            // public const string MobilePhoneNumber = "MobilePhoneNumber" ;
            //  public const string MailingAddressPostCode = "MailingAddressPostCode" ;
            //public const string MailingAddressLine1 = "MailingAddressLine1" ;
            //public const string MailingAddressLine2 = "MailingAddressLine2" ;
            //public const string MailingAddressLine3 = "MailingAddressLine3" ;
            //public const string MailingAddressLine4 = "MailingAddressLine4" ;
            //public const string MailingAddressLine5 = "MailingAddressLine5" ;
            //public const string Name1 = "Name1" ;
            //public const string Name3 = "Name3" ;
            //public const string EmailAddress = "EmailAddress" ;
            //public const string MobilePhoneNumber = "MobilePhoneNumber" ;
            //public const string Name1 = "Name1" ;
            //public const string Name2 = "Name2" ;
            //public const string Name3 = "Name3" ;
            //  public const string MailingAddressPostCode = "MailingAddressPostCode" ;
            //public const string MailingAddressLine1 = "MailingAddressLine1" ;
            //public const string MailingAddressLine2 = "MailingAddressLine2" ;
            //public const string MailingAddressLine3 = "MailingAddressLine3" ;
            //public const string MailingAddressLine4 = "MailingAddressLine4" ;
            //public const string MailingAddressLine5 = "MailingAddressLine5" ;
            // public const string PrimaryId = "PrimaryId" ;
            // public const string SecondaryId = "SecondaryId" ;
            // public const string DaytimePhoneNumber = "DaytimePhoneNumber" ;
            // public const string MobilePhoneNumber = "MobilePhoneNumber" ;
            // public const string MailingAddressPostCode = "MailingAddressPostCode" ;
            //  public const string MailingAddressPostCode = "MailingAddressPostCode" ;
            //public const string Name1 = "Name1" ;
            //public const string Name3 = "Name3" ;
            //  public const string EmailAddress = "EmailAddress" ;
            // public const string MailingAddressPostCode = "MailingAddressPostCode" ;
            //public const string Name2 = "Name2" ;
            // public const string Name3 = "Name3" ;
            // public const string MailingAddressLine1 = "MailingAddressLine1" ;
            // public const string Name1 = "Name1" ;
            // public const string MobilePhoneNumber = "MobilePhoneNumber" ;
            //  public const string EmailAddress = "EmailAddress" ;
            //public const string Name2 = "Name2" ;
            // public const string EmailAddress = "EmailAddress" ;
        #endregion

        #region for Configuration type

            public enum ConfigurationTypes
            {
                DuplicateCheckFields = 1,
                MandatoryFields = 2,
                DefaultContactPreference = 3,
                ErrorMessage = 4,
                LengthOfTheInputFields = 5,
                GroupConfigValues = 6,
                HoldingDates = 7,
                ProfanityCheckFields = 8,
                Prefix = 9,
                Format = 10,
                JoinRouteCode = 11,
                IsProfanityCheckNeeded = 12,
                ActivateNonMailableInDuplicateCheck = 13,
                TransactionalDatabaseName = 14,
                PreferenceFields = 15,
                ExpiredAccountProcesses = 16,
                UncontactableAccount = 17,
                InactiveAccount = 18,
                HideJoinFunctionality = 19,
                Activation = 20,
                BabyTodlerClubConfiguration = 21,
                ShowMyCouponPage = 22,
                UI_Parameter = 23,
                ChinaHiddenFunctionality = 25,
                SendPreferenceEmail = 27,
                JoinEmailMandatory = 28,
                EmailLinkExpirationTime = 29,
                ColMonthName = 30,
                SecurityCheck = 31,
                EmailNotification = 34

            }

        #endregion


    }
}
