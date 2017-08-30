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
        InvalidCharacterErrorMsg1 = 36,
        InvalidCharacterErrorMsg2 = 37,
        InvalidCharacterErrorMsg3 = 38,
        InvalidCharMsg = 39,
        HomeSecurity = 40,
        InvalidSecurityNumberMsg = 41,
        //Header
        WelcomeToTescoClubcard = 42,


        #endregion
    }

    public enum Domain
    {
        DBT,
        PPE,
        STG,
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
        Join,
        PersonalDetails,
        MyContactPreferences,
        OptionsBenefits,
        ViewMyCards,
        OrderAReplacement,
        Boost,
        Points,
        //PointsSummary,
        //PointsDetails,
        Vouchers,
        Coupons,
        MyLatestStatement,
        ChristmasSaver
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
        Province,
        PrimaryId,
        PreferredLanguage,
        MailingAddressPostCode,
        MailingAddressLine1,
        MailingAddressLine2,
        MailingAddressLine3,
        MailingAddressLine4,
        MailingAddressLine5,
        MailingAddressLine6,
        PostCodeBtn,
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
        InvalidMailingAddressLine4,
        InvalidMailingAddressLine5,
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
        MinLength,
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
        NoPreference
    }

    public enum FindBy
    {
        ID,
        CSS_CLASS,
        TAG_NAME,
        ATTRIBUTES,
        CSS_SELECTOR_ID,
        CSS_SELECTOR_CSS,
        XPATH_SELECTOR
    }

    public enum OptionPreference
    {
        None = 0,
        BA_Miles_Standard = 10,
        Airmiles_Standard = 11,
        Airmiles_Premium = 12,
        Xmas_Saver = 13,
        BA_Miles_Premium = 14,
        Save_Trees = 16,
        Virgin_Atlantic = 17,

    }

    public enum ContactPreference
    {
        None = 0,
        E_Mail_Contact = 43,
        Mobile_SMS,
        Post_Contact,
        Post_Large_Print = 46,
        Braille = 47,
    }

    public enum SecurityPreference
    {
        None = 0,
        Tesco_Products = 7,
        Tesco_Partners = 8,
        Customer_Research = 9,
        Tesco_Group_Mail = 27,
        Tesco_Group_Email = 28,
        Tesco_Group_Phone = 29,
        Tesco_Group_SMS = 30,
        Partner_3rd_Party_Mail = 31,
        Partner_3rd_Party_Email = 32,
        Partner_3rd_Party_Phone = 33,
        Partner_3rd_Party_SMS = 34,
        Research_Mail = 35,
        Research_Email = 36,
        Research_Phone = 37,
        Research_SMS = 38
    }

    public enum DropDownValue
    {
        AnyOption,
        SelectOption
    }

    public enum RewardEnum
    {
        BookingDate,
        TokenDescription,
        ProductStatus,
        TokenValue,
        SupplierTokenCode,
        ValidUntil
    }

    public enum UIControlTypes
    {
        TextBox = 0,
        RadioButton,
        DropDown,
        Button,
        None
    }
    public enum OptStatus
    {
        OptIn,
        OptOut
    }

    public enum Sync50
    {
        IsMerged,
        
    }
}
