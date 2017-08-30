using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Tesco.Framework.UITesting.Entities
{
    public class TestDataBase
    {
        [XmlAttribute("id")]
        public string Id { get; set; }
    }

    public class TestData_AccountDetails : TestDataBase
    {
        [XmlElement("MainAccount")]
        public MCAUser MainAccount { get; set; }
        [XmlElement("UnusedVoucherAccount")]
        public MCAUser UnusedVoucherAccount { get; set; }
        [XmlElement("NoActiveCouponAccount")]
        public MCAUser NoActiveCouponAccount { get; set; }
        [XmlElement("ActiveCouponAccount")]
        public MCAUser ActiveCouponAccount { get; set; }
        [XmlElement("StandardAccount")]
        public MCAUser StandardAccount { get; set; }
        [XmlElement("PointsAccount")]
        public MCAUser PointsAccount { get; set; }
        [XmlElement("VirginAccount")]
        public MCAUser VirginAccount { get; set; }
        [XmlElement("BlockedAccount")]
        public MCAUser BlockedAccount { get; set; }
        [XmlElement("BoostAccount")]
        public MCAUser BoostAccount { get; set; }
        [XmlElement("MaxOrdersReached")]
        public MCAUser MaxOrdersReached { get; set; }
        
        [XmlElement("HouseholdAccount")]
        public MCAUser HouseholdAccount { get; set; }
        [XmlElement("OnlyStdAccount")]
        public MCAUser OnlyStdAccount { get; set; }
        [XmlElement("ClubcardNonStdTypeN")]
        public MCAUser ClubcardNonStdTypeN { get; set; }
        [XmlElement("ChristmasSaverAccount")]
        public MCAUser ChristmasSaverAccount { get; set; }
        [XmlElement("DietaryPreferenceAccount")]
        public MCAUser DietaryPreferenceAccount { get; set; }
        [XmlElement("AviosAccount")]
        public MCAUser AviosAccount { get; set; }
        [XmlElement("ClubcardNonStdTypeB")]
        public MCAUser ClubcardNonStdTypeB { get; set; }
        [XmlElement("BAAviosPreAccount")]
        public MCAUser BAAviosPreAccount { get; set; }
        [XmlElement("TopUpClubcard")]
        public MCAUser TopUpClubcard { get; set; }        
        [XmlElement("ResetClubcard1")]
        public MCAUser ResetClubcard1 { get; set; }
        [XmlElement("ResetClubcard2")]
        public MCAUser ResetClubcard2 { get; set; }    
    }

    public class TestData_MLS : TestDataBase
    {
        [XmlElement("ClubcardA")]
        public MCAUser TypeAAccount { get; set; }
        [XmlElement("ClubcardB")]
        public MCAUser TypeBAccount { get; set; }
        [XmlElement("ClubcardC")]
        public MCAUser TypeCAccount { get; set; }
        [XmlElement("ClubcardD")]
        public MCAUser TypeDAccount { get; set; }
        [XmlElement("ClubcardE")]
        public MCAUser TypeEAccount { get; set; }
        [XmlElement("ClubcardF")]
        public MCAUser TypeFAccount { get; set; }
        [XmlElement("ClubcardG")]
        public MCAUser ClubcardG { get; set; }
        [XmlElement("ClubcardH")]
        public MCAUser TypeHAccount { get; set; }
        [XmlElement("ClubcardI")]
        public MCAUser TypeIAccount { get; set; }
        [XmlElement("ClubcardJ")]
        public MCAUser TypeJAccount { get; set; }
        [XmlElement("ClubcardK")]
        public MCAUser TypeKAccount { get; set; }
        [XmlElement("ClubcardL")]
        public MCAUser TypeLAccount { get; set; }
        [XmlElement("ClubcardM")]
        public MCAUser TypeMAccount { get; set; }
    }

    public class TestData_HomeSecurity : TestDataBase
    {
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
        [XmlElement("DotcomID")]
        public string DotcomID { get; set; }
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
        [XmlElement("EmailID")]
        public string EmailID { get; set; }
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
        [XmlElement("DuplicateName2")]
        public string DuplicateName2 { get; set; }
        [XmlElement("DuplicateName3")]
        public string DuplicateName3 { get; set; }
        [XmlElement("DuplicateEmailAddress")]
        public string DuplicateEmailAddress { get; set; }
        [XmlElement("DuplicateMailingAddressLine1")]
        public string DuplicateMailingAddressLine1 { get; set; }
        [XmlElement("DuplicatePostCode")]
        public string DuplicatePostCode { get; set; }
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

    public class TestData_PersonalDetails : TestDataBase
    {
        #region Duplicate
        [XmlElement("DuplicateName1")]
        public string DuplicateName1 { get; set; }
        [XmlElement("DuplicateName2")]
        public string DuplicateName2 { get; set; }
        [XmlElement("DuplicateName3")]
        public string DuplicateName3 { get; set; }
        [XmlElement("DuplicateEmailAddress")]
        public string DuplicateEmailAddress { get; set; }
        [XmlElement("DuplicateMailingAddressLine1")]
        public string DuplicateMailingAddressLine1 { get; set; }
        [XmlElement("DuplicatePostCode")]
        public string DuplicatePostCode { get; set; }
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
		[XmlElement("PartialPostcode")]
        public string PartialPostcode { get; set; }
    }

    public class TestData_XmusSaver : TestDataBase
    {
        [XmlElement("ClubcardNotXmusSaver")]
        public MCAUser NoNXmusSaverAccount { get; set; }
        [XmlElement("XmusSaverClubcard")]
        public MCAUser XmusSaverAccount { get; set; }
        [XmlElement("XmusSaverCardWithTopUpLessThen25")]
        public MCAUser XmusSaverCardWithTopUpLessThen25 { get; set; }
        [XmlElement("XmusSaverCardWithTopUpLessThen50")]
        public MCAUser XmusSaverCardWithTopUpLessThen50 { get; set; }
        [XmlElement("XmusSaverCardWithTopUpLessThen100")]
        public MCAUser XmusSaverCardWithTopUpLessThen100 { get; set; }
        [XmlElement("XmusSaverCardWithTopUpLessThen200")]
        public MCAUser XmusSaverCardWithTopUpLessThen200 { get; set; }
        [XmlElement("XmusSaverCardWithTopUpGreaterThen200")]
        public MCAUser XmusSaverCardWithTopUpGreaterThen200 { get; set; }
    }

    public class TestData_HoldingPages : TestDataBase
    {
        [XmlElement("BoostClubcard")]
        public MCAUser BoostAccount { get; set; }
        [XmlElement("XmasSaverClubcard")]
        public MCAUser XmasSaverAccount { get; set; }
        [XmlElement("VoucherClubcard")]
        public MCAUser VoucherAccount { get; set; }
        [XmlElement("CouponsClubcard")]
        public MCAUser CouponsAccount { get; set; }
        [XmlElement("OrderReplacementClubcard")]
        public MCAUser OrderReplacementAccount { get; set; }        
    }

    public class MCAUser
    {
        [XmlElement("Clubcard")]
        public string Clubcard { get; set; }
        [XmlElement("Password")]
        public string Password { get; set; }
        [XmlElement("EmailId")]
        public string EmailID { get; set; }
        [XmlElement("DotcomId")]
        public string DotcomId { get; set; }
    }
}