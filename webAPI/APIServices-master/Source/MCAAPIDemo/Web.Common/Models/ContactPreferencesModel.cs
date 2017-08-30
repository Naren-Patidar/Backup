using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences;
using System.Web.Mvc;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Models
{
    public class ContactPreferencesModel
    {
        ContactModel _ContactPreference = new ContactModel();        
        OptInsModel _OptIns = new OptInsModel(); 

        public ContactModel ContactPreference
        {
            get { return _ContactPreference; }
            set { _ContactPreference = value; }
        }

        public OptInsModel OptIns
        {
            get { return _OptIns; }
            set { _OptIns = value; }
        }            
    }

    public class ContactModel 
    {
        long _CustomerID = 0;
        short _PreviousSelectedPreferenceID = 0;
        short _SelectedPreferenceID = 0;
        string _Email = string.Empty;
        string _ConfirmEmail = string.Empty;
        string _Mobile = string.Empty;
        string _ConfirmMobile = string.Empty;
        string _PostalAddress = string.Empty;
        string _Braille = string.Empty;
        bool _IsSaved = false;
        bool _IsValid = false;
        string _ErrorMessage = string.Empty;
        string _InvalidEMail = string.Empty;
        string _InvalidMobile = string.Empty;
        string _CompareEMail = string.Empty;
        string _CompareMobile = string.Empty;
        string _MobilePrefixes = string.Empty;
        int _MobileMinLength = 0;
        int _MobileMaxLength = 0;

        string _ROMobile = string.Empty;
        string _ROEmail = string.Empty;
        string _ROBraille = string.Empty;        

        public long CustomerID
        {
            get { return _CustomerID; }
            set { _CustomerID = value; }
        }

        public string CustomerIDEncr { get; set; }

        public short PreviousSelectedPreferenceID
        {
            get { return _PreviousSelectedPreferenceID; }
            set { _PreviousSelectedPreferenceID = value; }
        }

        public short SelectedPreferenceID
        {
            get { return _SelectedPreferenceID; }
            set { _SelectedPreferenceID = value; }
        }

        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        public string ConfirmEmail
        {
            get { return _ConfirmEmail; }
            set { _ConfirmEmail = value; }
        }

        public string Mobile
        {
            get { return _Mobile; }
            set { _Mobile = value; }
        }

        public string ConfirmMobile
        {
            get { return _ConfirmMobile; }
            set { _ConfirmMobile = value; }
        }

        [AllowHtml]
        public string PostalAddress
        {
            get { return _PostalAddress; }
            set { _PostalAddress = value; }
        }
                
        public string Braille
        {
            get { return _Braille; }
            set { _Braille = value; }
        }

        public bool IsSaved
        {
            get { return _IsSaved; }
            set { _IsSaved = value; }
        }

        public bool IsValid
        {
            get { return _IsValid; }
            set { _IsValid = value; }
        }

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }

        public string InvalidEMail
        {
            get { return _InvalidEMail; }
            set { _InvalidEMail = value; }
        }

        public string InvalidMobile
        {
            get { return _InvalidMobile; }
            set { _InvalidMobile = value; }
        }

        public string CompareEMail
        {
            get { return _CompareEMail; }
            set { _CompareEMail = value; }
        }

        public string CompareMobile
        {
            get { return _CompareMobile; }
            set { _CompareMobile = value; }
        }
        
        public string MobilePrefixes
        {
            get { return _MobilePrefixes; }
            set { _MobilePrefixes = value; }
        }

        public int MobileMinLength
        {
            get { return _MobileMinLength; }
            set { _MobileMinLength = value; }
        }

        public int MobileMaxLength
        {
            get { return _MobileMaxLength; }
            set { _MobileMaxLength = value; }
        }

        public string ROMobile
        {
            get { return _ROMobile; }
            set { _ROMobile = value; }
        }

        public string ROEmail
        {
            get { return _ROEmail; }
            set { _ROEmail = value; }
        }

        public string ROBraille
        {
            get { return _ROBraille; }
            set { _ROBraille = value; }
        }

        public bool IsEMailContactVisible { get; set; }
        public bool IsMobileSMSVisisble { get; set; }
        public bool IsPostContactVisisble { get; set; }
        public bool IsPostLargePrintVisisble { get; set; }

        public bool IsEMailContactOpted { get; set; }
        public bool IsMobileSMSOpted { get; set; }
        public bool IsPostContactOpted { get; set; }
        public bool IsPostLargePrintOpted { get; set; }
        public bool IsBrailleOpted { get; set; }

        public string TrackEmail { get; set; }
        public string TrackMobile { get; set; }
    }

    public class OptInsModel
    {
        List<OptIns> _OptIns = new List<OptIns>();
        List<OptIns> _OptOuts = new List<OptIns>();
        List<PreferencesUIConfig> _PreferencesUIConfig = new List<PreferencesUIConfig>();
        bool _IsOptInBehaviour = false;
        Int64 _CustomerID = 0;
        bool _IsCustomerJoined = true;        
        string _CustomerEmail = string.Empty;
        bool _isSaved = false;
        bool _IsValid = true;

        public List<PreferencesUIConfig> PrefUIConfiguration
        {
            get { return _PreferencesUIConfig; }
            set { _PreferencesUIConfig = value; }
        }

        public string CustomerEmail
        {
            get { return _CustomerEmail; }
            set { _CustomerEmail = value; }
        }

        public List<OptIns> OptIns
        {
            get { return _OptIns; }
            set { _OptIns = value; }
        }

        public List<OptIns> OptOuts
        {
            get { return _OptOuts; }
            set { _OptOuts = value; }
        }

        public bool IsOptInBehaviour
        {
            get { return _IsOptInBehaviour; }
            set { _IsOptInBehaviour = value; }
        }

        public bool IsCustomerJoined
        {
            get { return _IsCustomerJoined; }
            set { _IsCustomerJoined = value; }
        }

        public Int64 CustomerID
        {
            get { return _CustomerID; }
            set { _CustomerID = value; }
        }
        public string CustomerIDEncr { get; set; }

        public bool IsSaved
        {
            get { return _isSaved; }
            set { _isSaved = value; }
        }

        public bool IsValid
        {
            get { return _IsValid; }
            set { _IsValid = value; }
        }
    }

    public class OptIns
    {
        Int16 _PreferenceID = 0;
        bool _IsOpted;
        bool _IsAlreadyOpted;
        bool _IsVisible;

        public bool IsVisible
        {
            get { return _IsVisible; }
            set { _IsVisible = value; }
        }      

        public Int16 PreferenceID
        {
            get { return _PreferenceID; }
            set { _PreferenceID = value; }
        }

        public bool IsOpted
        {
            get { return _IsOpted; }
            set { _IsOpted = value; }
        }

        public bool IsAlreadyOpted
        {
            get { return _IsAlreadyOpted; }
            set { _IsAlreadyOpted = value; }
        } 

        public OptStatus OptStatus
        {
            get 
            {
                return  _IsOpted ? OptStatus.OPTED_IN : OptStatus.OPTED_OUT;
            }
        }
    }

    public class PreferencesUIConfig
    {
        Int16 _PreferenceID = 0;
        bool _IsVisible;

        public Int16 preferenceid
        {
            get { return _PreferenceID; }
            set { _PreferenceID = value; }
        }

        public bool isvisible
        {
            get { return _IsVisible; }
            set { _IsVisible = value; }
        }

        public List<short> dependentprefidsassame { get; set; }

        public List<short> dependentprefidsasopp { get; set; }
    }
}
