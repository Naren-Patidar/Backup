using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Tesco.Framework.UITesting.Entities
{
    [XmlRoot("TestingData")]
    public class TestingData
    {
        [XmlElement("TestData")]
        public List<TestData> lstTestingData { get; set; }
    }

    public class TestData
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
        [XmlAttribute("title")]
        public string Title { get; set; }
        [XmlAttribute("type")]
        public string Type { get; set; }
        [XmlAttribute("description")]
        public string Description { get; set; }
        [XmlAttribute("Clubcard")]
        public string Clubcard { get; set; }
        [XmlAttribute("Password")]
        public string Password { get; set; }
        [XmlAttribute("EmailID")]
        public string EmailID { get; set; }
        [XmlAttribute("PasswordPPE")]
        public string PasswordPPE { get; set; }
        [XmlAttribute("ActivationClubcard")]
        public string ActivationClubcard { get; set; }
        [XmlAttribute("Space")]
        public string Space { get; set; }
        [XmlAttribute("SpecialCharcter")]
        public string SpecialCharcter { get; set; }
        [XmlAttribute("Alphabet")]
        public string Alphabet { get; set; }
        [XmlAttribute("Numbers")]
        public string Numbers { get; set; }

        [XmlText]
        public string Text { get; set; }

        public List<TestData> lstTestingData { get; set; }
    }

    public class TestDataBase
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
    }

    public class TestData_AccountDetails : TestDataBase
    {
        [XmlElement("Clubcard")]
        public string Clubcard { get; set; }
        [XmlElement("Clubcard1")]
        public string Clubcard1 { get; set; }
        [XmlElement("Clubcard2")]
        public string Clubcard2 { get; set; }
        [XmlElement("StandardClubcard")]
        public string StandardClubcard { get; set; }
        [XmlElement("VirginClubcard")]
        public string VirginClubcard { get; set; }
        [XmlElement("BlockedClubcard")]
        public string BlockedClubcard { get; set; }
        [XmlElement("ChristmasSaver")]
        public string ChristmasSaver { get; set; }
        [XmlElement("Password")]
        public string Password { get; set; }
        [XmlElement("EmailID")]
        public string EmailID { get; set; }
        [XmlElement("NoActiveCouponClubcard")]
        public string NoActiveCouponClubcard { get; set; }
        [XmlElement("NoCouponRedeemedClubcard")]
        public string NoCouponRedeemedClubcard { get; set; }
        [XmlElement("ClubcardforDate")]
        public string ClubcardforDate { get; set; }
        [XmlElement("ClubcardforHouseholdID")]
        public string ClubcardforHouseholdID { get; set; }
        [XmlElement("ClubcardBAAviosPre")]
        public string ClubcardBAAviosPre { get; set; }
        [XmlElement("ClubcardBAAviosStd")]
        public string ClubcardBAAviosStd { get; set; }
        [XmlElement("ClubcardAviosPre")]
        public string ClubcardAviosPre { get; set; }
        [XmlElement("ClubcardAviosStd")]
        public string ClubcardAviosStd { get; set; }
        [XmlElement("ClubcardNonStdTypeB")]
        public string ClubcardNonStdTypeB { get; set; }
        [XmlElement("ClubcardNonStdTypeN")]
        public string ClubcardNonStdTypeN { get; set; }
        [XmlElement("ClubcardMaxOrdersReached")]
        public string ClubcardMaxOrdersReached { get; set; }
        [XmlElement("ResetClubcard1")]
        public string ResetClubcard1 { get; set; }
        [XmlElement("ResetClubcard2")]
        public string ResetClubcard2 { get; set; }
    }

    public class TestData_HomeSecurity : TestDataBase
    {
        [XmlElement("Clubcard")]
        public string Clubcard { get; set; }
        [XmlElement("Password")]
        public string Password { get; set; }
        [XmlElement("SpecialChars")]
        public string SpecialChars { get; set; }
        [XmlElement("SpaceChar")]
        public string SpaceChar { get; set; }
        [XmlElement("AlphabetChar")]
        public string AlphabetChar { get; set; }
    }

    public class TestData_Activation : TestDataBase
    {
        [XmlElement("Clubcard")]
        public string Clubcard { get; set; }
        [XmlElement("SpecialChars")]
        public string SpecialChars { get; set; }
        [XmlElement("SpaceChar")]
        public string SpaceChar { get; set; }
        [XmlElement("AlphabetChar")]
        public string AlphabetChar { get; set; }
        [XmlElement("Numbers")]
        public string Numbers { get; set; }
        [XmlElement("CSCUsername")]
        public string CSCUsername { get; set; }
        [XmlElement("CSCPassword")]
        public string CSCPassword { get; set; }
        [XmlElement("Email")]
        public string Email { get; set; }
        [XmlElement("Password")]
        public string Password { get; set; }
        [XmlElement("Repassword")]
        public string Repassword { get; set; }
    }
    public class TestData_JoinDetails : TestDataBase
    {    
        #region Duplicate
        [XmlElement("DuplicateName1")]
        public string DuplicateName1 { get; set; }
        [XmlElement("DuplicateEmailAddress")]
        public string DuplicateEmailAddress { get; set; }
        #endregion
        #region CommonFieldAttribute
        [XmlElement("TitleEnglish")]
        public string TitleEnglish { get; set; }
        [XmlElement("Date")]
        public string Date { get; set; }
        [XmlElement("Month")]
        public string Month { get; set; }
        [XmlElement("Year")]
        public string Year { get; set; }
        [XmlElement("Sex")]
        public string Sex { get; set; }
        [XmlElement("MailingAddressLine1")]
        public string MailingAddressLine1 { get; set; }
        [XmlElement("MailingAddressLine2")]
        public string MailingAddressLine2 { get; set; }
        [XmlElement("MailingAddressLine4")]
        public string MailingAddressLine4 { get; set; }
        [XmlElement("MailingAddressLine5")]
        public string MailingAddressLine5 { get; set; }
        [XmlElement("PromotionalCodeStartingToday")]
        public string PromotionalCodeStartingToday { get; set; }
        [XmlElement("PromotionalCodeExpiringToday")]
        public string PromotionalCodeExpiringToday { get; set; }
        [XmlElement("InvalidPromotionalCode")]
        public string InvalidPromotionalCode { get; set; }
        [XmlElement("PromotionalCodeStartingTommorow")]
        public string PromotionalCodeStartingTommorow { get; set; }
        [XmlElement("ExpiredPromotionalCode")]
        public string ExpiredPromotionalCode { get; set; }
       
        #endregion
        #region ValidFieldAttribute
        [XmlElement("Name1")]
        public string Name1 { get; set; }
        [XmlElement("Name2")]
        public string Name2 { get; set; }
        [XmlElement("Name3")]
        public string Name3 { get; set; }
        [XmlElement("EmailAddress")]
        public string EmailAddress { get; set; }
        [XmlElement("EveningPhoneNumber")]
        public string EveningPhoneNumber { get; set; }
        [XmlElement("DayTimePhoneNumber")]
        public string DayTimePhoneNumber { get; set; }
        [XmlElement("MobilePhoneNumber")]
        public string MobilePhoneNumber { get; set; }       
        #endregion
        #region InvalidFieldAttribute
        [XmlElement("InvalidName1")]
        public string InvalidName1 { get; set; }
        [XmlElement("InvalidName2")]
        public string InvalidName2 { get; set; }
        [XmlElement("InvalidName3")]
        public string InvalidName3 { get; set; }
        [XmlElement("InvalidMailingAddressPostCode")]
        public string InvalidMailingAddressPostCode { get; set; }
        [XmlElement("InvalidMailingAddressLine1")]
        public string InvalidMailingAddressLine1 { get; set; }
        [XmlElement("InvalidMailingAddressLine2")]
        public string InvalidMailingAddressLine2 { get; set; }
        [XmlElement("InvalidMailingAddressLine4")]
        public string InvalidMailingAddressLine4 { get; set; }
        [XmlElement("InvalidMailingAddressLine5")]
        public string InvalidMailingAddressLine5 { get; set; }
        [XmlElement("InvalidEmailAddress")]
        public string InvalidEmailAddress { get; set; }
        [XmlElement("InvalidEveningPhoneNumber")]
        public string InvalidEveningPhoneNumber { get; set; }
        [XmlElement("InvalidDayTimePhoneNumber")]
        public string InvalidDayTimePhoneNumber { get; set; }
        [XmlElement("InvalidMobilePhoneNumber")]
        public string InvalidMobilePhoneNumber { get; set; } 
        #endregion
        #region ProfaneFieldAttribute
        [XmlElement("ProfaneName1")]
        public string ProfaneName1 { get; set; }
        [XmlElement("ProfaneName2")]
        public string ProfaneName2 { get; set; }
        [XmlElement("ProfaneName3")]
        public string ProfaneName3 { get; set; }
        [XmlElement("ProfaneHouseName")]
        public string ProfaneHouseName { get; set; }
        [XmlElement("ProfaneMailingAddressLine1")]
        public string ProfaneMailingAddressLine1 { get; set; }
        [XmlElement("ProfaneMailingAddressLine2")]
        public string ProfaneMailingAddressLine2 { get; set; }
        [XmlElement("ProfaneMailingAddressLine3")]
        public string ProfaneMailingAddressLine3 { get; set; }
        [XmlElement("ProfaneMailingAddressLine4")]
        public string ProfaneMailingAddressLine4 { get; set; }
        [XmlElement("ProfaneMailingAddressLine5")]
        public string ProfaneMailingAddressLine5 { get; set; }
        #endregion
       
        [XmlElement("MailingAddressPostCode")]
        public string MailingAddressPostCode { get; set; }
        [XmlElement("PostCodeBtn")]
        public string PostCodeBtn { get; set; }
        [XmlElement("AddressDropDown")]
        public string AddressDropDown { get; set; }
    }

    public class TestData_Voucher : TestDataBase
    {
        [XmlElement("ActiveClubcard")]
        public string ActiveClubcard { get; set; }
        [XmlElement("AviosClubcard")]
        public string AviosClubcard { get; set; }
        [XmlElement("BAAviosClubcard")]
        public string BAAviosClubcard { get; set; }
        [XmlElement("VirginAtlantic")]
        public string VirginAtlantic { get; set; }
        [XmlElement("TopUpClubcard")]
        public string TopUpClubcard { get; set; }
        [XmlElement("BonusClubCard")]
        public string BonusClubCard { get; set; }
        [XmlElement("RedeemedClubCard")]
        public string RedeemedClubCard { get; set; }
        [XmlElement("Password")]
        public string Password { get; set; }
    }
    public class TestData_PersonalDetails : TestDataBase
    {
       
        #region Duplicate
        [XmlElement("DuplicateName1")]
        public string DuplicateName1 { get; set; }
        [XmlElement("DuplicateEmailAddress")]
        public string DuplicateEmailAddress { get; set; }
        #endregion
        #region CommonFieldAttribute
        [XmlElement("TitleEnglish")]
        public string TitleEnglish { get; set; }
        [XmlElement("Race")]
        public string Race { get; set; }
        [XmlElement("Province")]
        public string Province { get; set; }
        [XmlElement("Date")]
        public string Date { get; set; }
        [XmlElement("Month")]
        public string Month { get; set; }
        [XmlElement("Year")]
        public string Year { get; set; }
        [XmlElement("Sex")]
        public string Sex { get; set; }
        [XmlElement("MailingAddressLine1")]
        public string MailingAddressLine1 { get; set; }
        [XmlElement("MailingAddressLine2")]
        public string MailingAddressLine2 { get; set; }
        [XmlElement("MailingAddressLine3")]
        public string MailingAddressLine3 { get; set; }
        [XmlElement("MailingAddressLine4")]
        public string MailingAddressLine4 { get; set; }
        [XmlElement("MailingAddressLine5")]
        public string MailingAddressLine5 { get; set; }
        [XmlElement("MailingAddressLine6")]
        public string MailingAddressLine6 { get; set; }

        #endregion
        #region ValidFieldAttribute
        [XmlElement("Name1")]
        public string Name1 { get; set; }
        [XmlElement("Name2")]
        public string Name2 { get; set; }
        [XmlElement("Name3")]
        public string Name3 { get; set; }
        [XmlElement("EmailAddress")]
        public string EmailAddress { get; set; }
        [XmlElement("EveningPhoneNumber")]
        public string EveningPhoneNumber { get; set; }
        [XmlElement("DayTimePhoneNumber")]
        public string DayTimePhoneNumber { get; set; }
        [XmlElement("MobilePhoneNumber")]
        public string MobilePhoneNumber { get; set; }
        #endregion
        #region InvalidFieldAttribute
        [XmlElement("InvalidName1")]
        public string InvalidName1 { get; set; }
        [XmlElement("InvalidName2")]
        public string InvalidName2 { get; set; }
        [XmlElement("InvalidName3")]
        public string InvalidName3 { get; set; }
        [XmlElement("InvalidMailingAddressPostCode")]
        public string InvalidMailingAddressPostCode { get; set; }
        [XmlElement("InvalidMailingAddressLine1")]
        public string InvalidMailingAddressLine1 { get; set; }
        [XmlElement("InvalidMailingAddressLine2")]
        public string InvalidMailingAddressLine2 { get; set; }
        [XmlElement("InvalidMailingAddressLine3")]
        public string InvalidMailingAddressLine3 { get; set; }
        [XmlElement("InvalidMailingAddressLine4")]
        public string InvalidMailingAddressLine4 { get; set; }
        [XmlElement("InvalidMailingAddressLine5")]
        public string InvalidMailingAddressLine5 { get; set; }
        [XmlElement("InvalidMailingAddressLine6")]
        public string InvalidMailingAddressLine6 { get; set; }
        [XmlElement("InvalidEmailAddress")]
        public string InvalidEmailAddress { get; set; }
        [XmlElement("InvalidEveningPhoneNumber")]
        public string InvalidEveningPhoneNumber { get; set; }
        [XmlElement("InvalidDayTimePhoneNumber")]
        public string InvalidDayTimePhoneNumber { get; set; }
        [XmlElement("InvalidMobilePhoneNumber")]
        public string InvalidMobilePhoneNumber { get; set; }
        #endregion
        #region ProfaneFieldAttribute
        [XmlElement("ProfaneName1")]
        public string ProfaneName1 { get; set; }
        [XmlElement("ProfaneName2")]
        public string ProfaneName2 { get; set; }
        [XmlElement("ProfaneName3")]
        public string ProfaneName3 { get; set; }
        [XmlElement("ProfaneHouseName")]
        public string ProfaneHouseName { get; set; }
        [XmlElement("ProfaneMailingAddressLine1")]
        public string ProfaneMailingAddressLine1 { get; set; }
        [XmlElement("ProfaneMailingAddressLine2")]
        public string ProfaneMailingAddressLine2 { get; set; }
        [XmlElement("ProfaneMailingAddressLine3")]
        public string ProfaneMailingAddressLine3 { get; set; }
        [XmlElement("ProfaneMailingAddressLine4")]
        public string ProfaneMailingAddressLine4 { get; set; }
        [XmlElement("ProfaneMailingAddressLine5")]
        public string ProfaneMailingAddressLine5 { get; set; }
        #endregion

        [XmlElement("MailingAddressPostCode")]
        public string MailingAddressPostCode { get; set; }
        [XmlElement("PostCodeBtn")]
        public string PostCodeBtn { get; set; }
        [XmlElement("AddressDropDown")]
        public string AddressDropDown { get; set; }
        [XmlElement("HouseNoOrName")]
        public string HouseNoOrName { get; set; }
        [XmlElement("ValidPostode")]
        public string ValidPostode { get; set; }
        [XmlElement("InvalidPostcode")]
        public string InvalidPostcode { get; set; }
        [XmlElement("SpacePostcode")]
        public string SpacePostcode { get; set; } 
    }
    public class TestData_XmusSaver : TestDataBase
    {
        [XmlElement("ClubcardNotXmusSaver")]
        public string ClubcardNotXmusSaver { get; set; }
        [XmlElement("XmusSaverClubcard")]
        public string XmusSaverClubcard { get; set; }
        [XmlElement("XmusSaverCardWithTopUpLessThen25")]
        public string XmusSaverCardWithTopUpLessThen25 { get; set; }
        [XmlElement("XmusSaverCardWithTopUpLessThen50")]
        public string XmusSaverCardWithTopUpLessThen50 { get; set; }
        [XmlElement("XmusSaverCardWithTopUpLessThen100")]
        public string XmusSaverCardWithTopUpLessThen100 { get; set; }
        [XmlElement("XmusSaverCardWithTopUpLessThen200")]
        public string XmusSaverCardWithTopUpLessThen200 { get; set; }
        [XmlElement("XmusSaverCardWithTopUpGreaterThen200")]
        public string XmusSaverCardWithTopUpGreaterThen200 { get; set; } 
        [XmlElement("Password")]
        public string Password { get; set; }
        [XmlElement("EmailID")]
        public string EmailID { get; set; }
    }
}
