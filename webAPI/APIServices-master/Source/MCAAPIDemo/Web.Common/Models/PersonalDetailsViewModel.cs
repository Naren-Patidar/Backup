using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using System.Web.Mvc;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.CustomerDetails;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using System.Web;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Models
{
    public class PersonalDetailsViewModel
    {
        public PersonalDetailsViewModel()
        { }
        public PersonalDetailsViewModel(CustomerFamilyMasterData customerFamilyMasterData)
        {
            this.CustomerFamilyMasterData = customerFamilyMasterData;
        }

        #region Join Properties
        public bool captchaValidationFlag { get; set; }
        public bool noShowCaseSensistiveText { get; set; }
        public bool IsConfirmation { get; set; }
        public string PromotionCode
        {
            get;
            set;
        }
        public string ErrorPromotion
        {
            get;
            set;
        }
        public string Clubcard
        {
            get;
            set;
        }
        public string ErrorDB
        {
            get;
            set;
        }
        public bool IsJoinEnabled
        {
            get;
            set;
        }
        public bool IsJoinPage
        {
            get;
            set;
        }
        public string RedirectUrl
        {
            get;
            set;
        }



        #endregion Join Properties

        OptInsModel _OptIns = new OptInsModel();
        public CustomerFamilyMasterData CustomerFamilyMasterData { get; set; }
        public List<BTClubPreference> PersonalDetailsPreferences { get; set; }
        public List<string> FamilyMemberDob { get; set; }
        public Dictionary<string, string> Genders
        {
            get
            {
                Dictionary<string, string> radio = new Dictionary<string, string>();
                radio.Add(HttpContext.GetLocalResourceObject("~/Views/Shared/_PersonalDetails.cshtml", "lclMale.Text", System.Globalization.CultureInfo.CurrentCulture).ToString(), "M");
                radio.Add(HttpContext.GetLocalResourceObject("~/Views/Shared/_PersonalDetails.cshtml", "lclFemale.Text", System.Globalization.CultureInfo.CurrentCulture).ToString(), "F");
                return radio;
            }
            set { }
        }
        public Dictionary<string, string> Titles
        {
            get
            {
                Dictionary<string, string> titles = new Dictionary<string, string>();
                titles.Add(HttpContext.GetLocalResourceObject("~/Views/Shared/_PersonalDetails.cshtml", "lclTitle1.Text",
                    System.Globalization.CultureInfo.CurrentCulture).ToString(), HttpContext.GetLocalResourceObject("~/Views/Shared/_PersonalDetails.cshtml", "lclTitle1.Text",
                    System.Globalization.CultureInfo.CurrentCulture).ToString());
                                titles.Add(HttpContext.GetLocalResourceObject("~/Views/Shared/_PersonalDetails.cshtml", "lclTitle2.Text",
                    System.Globalization.CultureInfo.CurrentCulture).ToString(), HttpContext.GetLocalResourceObject("~/Views/Shared/_PersonalDetails.cshtml", "lclTitle2.Text",
                    System.Globalization.CultureInfo.CurrentCulture).ToString());
                                titles.Add(HttpContext.GetLocalResourceObject("~/Views/Shared/_PersonalDetails.cshtml", "lclTitle3.Text",
                    System.Globalization.CultureInfo.CurrentCulture).ToString(), HttpContext.GetLocalResourceObject("~/Views/Shared/_PersonalDetails.cshtml", "lclTitle3.Text",
                    System.Globalization.CultureInfo.CurrentCulture).ToString());
                                titles.Add(HttpContext.GetLocalResourceObject("~/Views/Shared/_PersonalDetails.cshtml", "lclTitle4.Text",
                    System.Globalization.CultureInfo.CurrentCulture).ToString(), HttpContext.GetLocalResourceObject("~/Views/Shared/_PersonalDetails.cshtml", "lclTitle4.Text",
                    System.Globalization.CultureInfo.CurrentCulture).ToString());
                return titles;
            }
            set { }
        }
        public AddressDetails AddressDetails { get; set; }

        public string DayOfBirth { get; set; }
        public string MonthOfBirth { get; set; }
        public string YearOfBirth { get; set; }
        public string HiddenMessage { get; set; }
        //public string HiddenSurname { get; set; }
        public bool FindAddressClicked { get; set; }
        public bool GroupAddressAvailable { get; set; }
        public string hdnTrackAddress { get; set; }
        [AllowHtml]
        public string HiddenPromotionCode { get; set; }
        [AllowHtml]
        public string HiddenFirstName
        {
            get;
            set;
        }
        [AllowHtml]
        public string HiddenSurname
        {
            get;
            set;
        }
        [AllowHtml]
        public string HiddenMidName
        {
            get;
            set;
        }
        
        public string HiddenDOB
        {
            get;
            set;
        }
        
        public string HiddenSex
        {
            get;
            set;
        }
        [AllowHtml]
        public string HiddenAddressLine1
        {
            get;
            set;
        }
        [AllowHtml]
        public string HiddenAddressLine2
        {
            get;
            set;
        }
        [AllowHtml]
        public string HiddenAddressLine3
        {
            get;
            set;
        }
        [AllowHtml]
        public string HiddenAddressLine4
        {
            get;
            set;
        }
        [AllowHtml]
        public string HiddenAddressLine5
        {
            get;
            set;
        }
        [AllowHtml]
        public string HiddenAddressLine6
        {
            get;
            set;
        }
        [AllowHtml]
        public string HiddenEmail
        {
            get;
            set;
        }
        [AllowHtml]
        public string HiddenPostcode
        {
            get;
            set;
        }
        [AllowHtml]
        public string HiddenEveningPhoneNumber
        {
            get;
            set;
        }
        [AllowHtml]
        public string HiddenDayPhoneNumber
        {
            get;
            set;
        }
        [AllowHtml]
        public string HiddenSSN
        {
            get;
            set;
        }
        [AllowHtml]
        public string HiddenMobileNumber
        {
            get;
            set;
        }
        
        public string HiddenRaceID
        {
            get;
            set;
        }
        [AllowHtml]
        public string HiddenPassportNo
        {
            get;
            set;
        }
        
        public string HiddenISOLanguageCode
        {
            get;
            set;
        }
        [AllowHtml]
        public string HiddenSentEmail
        {
            get;
            set;
        }
       
        public string HiddenEmailPref
        {
            get;
            set;
        }
       
        public string HiddenMobilePref
        {
            get;
            set;
        }
       
        public string HiddenPostPref
        {
            get;
            set;
        }
        public OptInsModel OptIns
        {
            get { return _OptIns; }
            set { _OptIns = value; }
        }
        public string HiddenAllergyPreferences { get; set; }
        public string HiddenDietaryPreferences { get; set; }
        public string HiddenDietary { get; set; }
        public string HiddenAllergy { get; set; }
        public string HiddenSendEmailForDietary { get; set; }
        public string HiddenSendEmailForAllergy { get; set; }
        public bool OptLegalPolicy1 { get; set; }
        public bool OptLegalPolicy2 { get; set; }
        public bool IsLegalPolicyError { get; set; }

    }
}
