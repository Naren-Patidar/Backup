using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesco.Framework.UITesting.Enums
{
    public enum Messages
    {

        #region Common


        ErrorForMandatoryDestination = 0,
        Login = 1,
        Sanity = 2,
        Home = 3,
        MyAccount = 4,
        MyPersonalDetails = 5,
        MyContactPref = 6,
        MyPoints = 7,
        MyCurrentPoints = 8,
        MyCurrentPointsHeader = 9,
        MyVoucher = 10,
        MyCoupon = 11,
        MyLatestStatement = 12,
        OptionBenefit = 13,
        ViewMyCard = 14,
        OrderReplacement = 15,
        TescoBoost = 16,
        FuelSaving = 17,
        MyOptionBenefit = 18,
        MyCurrentPointsSummary = 19,
        ChristmasSaver = 20,
        ValidationMessageForSaveContactPreference = 21,
        ErrorForMandatoryClubCard = 22,
        ErrorForMandatoryFirstName = 23,
        ErrorForMandatorySurname = 24,
        ErrorForMandatoryPostCode = 25,
        Activation = 26,
        ErrorCheck = 27,
        ContactPreferenceConfirmButton = 28,
        JoinConfirmationMsg = 29,
        JoinConfirmationImage = 30,
        DefaultSecurityMsg = 31,
        MessageForActivatingClubcard = 32,
        MessageForActivationError = 33,
        SuccessfulMessageForActivation = 34,
        termsAndconditions = 35,
        InvalidCharacterErrorMsg1=36,
        InvalidCharacterErrorMsg2=37,
        InvalidCharacterErrorMsg3=38,
        InvalidCharMsg=39,
        HomeSecurity=40,
        InvalidSecurityNumberMsg=41,
        //Header
        WelcomeToTescoClubcard=42,


        #endregion
    }

    public enum Domain
    {
        DBT,
        PPE,
        Staging,
        GD
    }

    public enum WaitInterval
    {
        UltraShortSpan,
        ShortSpan,
        MediumSpan,
        LargeSpan
    }

    public enum ErrorType
    {
        InternalError,
        ApplicationError
    }

    public enum ApplicationPages
    {
        Activation,
        Join
    }

    public enum JoinElements
    {
        TitleEnglish,
        Name1,
        Name2,
        Name3,
        Date,
        Month,
        Year,
        Sex,
        Race,
        PreferredLanguage,
        Province,
        MailingAddressPostCode,
        MailingAddressLine1,
        MailingAddressLine2,
        MailingAddressLine3,
        MailingAddressLine4,
        MailingAddressLine5,
        MailingAddressLine6,
        Findaddress,
        EmailAddress,
        MobilePhoneNumber,
        DayTimePhoneNumber,
        EveningPhoneNumber,
        InvalidName1,
        InvalidName2,
        InvalidName3,
        InvalidDate,
        InvalidMonth,
        InvalidYear,
        InvalidSex,
        InvalidMailingAddressPostCode,
        InvalidMailingAddressLine1,
        InvalidMailingAddressLine2,
        InvalidMailingAddressLine3,
        InvalidMailingAddressLine4,
        InvalidMailingAddressLine5,
        InvalidMailingAddressLine6,
        InvalidEmailAddress,
        InvalidMobilePhoneNumber,
        InvalidDayTimePhoneNumber,
        InvalidEveningPhoneNumber,
        ProfaneName1,
        ProfaneName2,
        ProfaneName3,
        ProfaneHousenumber,
        ProfaneMailingAddressLine1,
        ProfaneMailingAddressLine2,
        ProfaneMailingAddressLine4,
        ProfaneMailingAddressLine5

    }

    public enum FieldType
    {
        Valid,
        Invalid,
        InvalidLength1,
        InvalidLength2,
        ProfaneName1,
        ProfaneName2,
        ProfaneName3,
        ProfaneHouseName,
        ProfaneMailingAddressLine1,
        ProfaneMailingAddressLine2,
        ProfaneMailingAddressLine3,
        ProfaneMailingAddressLine4,
        ProfaneMailingAddressLine5,
        All,
        Mandatory,
        DuplicateNameANDAddress,
        DuplicateEmail,
        DuplicateMobileNumber,
    }

    public enum VoucherType
    {
        TopUp,
        Bonus,
    }
    public enum VoucherSection
    {
        Displayed,
        NotDisplayed,
        SendInLast2Years,
        Spent,
        StillToSpend,
        Used,
        UnUsed,
        BothEnabled,
        BothDisabled,
        TwitterEnabled,
        FacebookEnabled
    }
    public enum Preferences
    {
        XmasSaver,
        VirginAtlantic,
        Avios,
        BAAvios,
        NoPreference,       
    }

    public enum OptStatus
    {
        OptIn,
        OptOut
    }
    public enum DropDownValue
    {
        AnyOption,
        SelectOption
    }
    
}
