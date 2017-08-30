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
    public class JoinViewModel
    {
        public JoinViewModel()
        { }
        public bool showIsCaseSensistiveText { get; set; }
        public CustomerFamilyMasterData CustomerFamilyMasterData { get; set; }
        public List<BTClubPreference> PersonalDetailsPreferences { get; set; }
        public List<string> FamilyMemberDob { get; set; }
        public Dictionary<string, string> Genders
        {
            get
            {
                Dictionary<string, string> radio = new Dictionary<string, string>();
                radio.Add(HttpContext.GetLocalResourceObject("~/Views/PersonalDetails/Home.cshtml", "lclMale.Text", System.Globalization.CultureInfo.CurrentCulture).ToString(), "M");
                radio.Add(HttpContext.GetLocalResourceObject("~/Views/PersonalDetails/Home.cshtml", "lclFemale.Text", System.Globalization.CultureInfo.CurrentCulture).ToString(), "F");
                return radio;
            }
            set { }
        }
        public Dictionary<string, string> Titles
        {
            get
            {
                Dictionary<string, string> titles = new Dictionary<string, string>();
                titles.Add("Mr", "Mr");
                titles.Add("Mrs", "Mrs");
                titles.Add("Miss", "Miss");
                titles.Add("Ms", "Ms");
                return titles;
            }
            set { }
        }
        public AddressDetails AddressDetails { get; set; }

        public JoinViewModel(CustomerFamilyMasterData customerFamilyMasterData)
        {
            this.CustomerFamilyMasterData = customerFamilyMasterData;
        }
        public string DayOfBirth { get; set; }
        public string MonthOfBirth { get; set; }
        public string YearOfBirth { get; set; }
        public string HiddenMessage { get; set; }
        //public string HiddenSurname { get; set; }
        public bool FindAddressClicked { get; set; }
        public bool GroupAddressAvailable { get; set; }
        public string hdnTrackAddress { get; set; }
        public string HiddenFirstName
        {
            get;
            set;
        }
        public string HiddenSurname
        {
            get;
            set;
        }

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

        public string HiddenDay 
        { 
           get;
           set;
        }

        public string HiddenMonth
        { 
            get;
           set;
        }

        public string HiddenYear 
        { 
             get;
             set;
        }

        public string HiddenSex
        {
            get;
            set;
        }


        public string HiddenAddressLine1
        {
            get;
            set;
        }
        public string HiddenAddressLine2
        {
            get;
            set;
        }
        public string HiddenAddressLine3
        {
            get;
            set;
        }
        public string HiddenAddressLine4
        {
            get;
            set;
        }
        public string HiddenAddressLine5
        {
            get;
            set;
        }
        public string HiddenAddressLine6
        {
            get;
            set;
        }
        public string HiddenEmail
        {
            get;
            set;
        }
        public string HiddenPostcode
        {
            get;
            set;
        }
        public string HiddenEveningPhoneNumber
        {
            get;
            set;
        }
        public string HiddenDayPhoneNumber
        {
            get;
            set;
        }
        public string HiddenSSN
        {
            get;
            set;
        }
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
        public string HiddenAllergyPreferences { get; set; }
        public string HiddenDietaryPreferences { get; set; }
        public string HiddenDietary { get; set; }
        public string HiddenAllergy { get; set; }
        public string HiddenSendEmailForDietary { get; set; }
        public string HiddenSendEmailForAllergy { get; set; }
        
    }
}
