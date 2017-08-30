using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.DBConfiguration;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using System.Web.Routing;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using Microsoft.Practices.ServiceLocation;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Validator
{
    public class Validators : ValidationAttribute
    {
        private string _viewpath;
        IConfigurationProvider _configProvider;
        string pCulture =string.Empty;
        string TescoVisaHigh =string.Empty;
        string TescoVisaLow =string.Empty;
        string TescoPlatinumHigh = string.Empty;
        string TescoPlatinumLow =string.Empty;
        string MastercardHigh =string.Empty;
        string MastercardLow =string.Empty;
        string MastercardClubcardHigh=string.Empty;
        string MastercardClubcardLow=string.Empty;
        string MastercardBonusHigh =string.Empty;
        string MastercardBonusLow =string.Empty;
        string VisaBonusHigh =string.Empty;
        string VisaBonusLow =string.Empty;
        string VisaClubcardHigh =string.Empty;
        string VisaClubcardLow =string.Empty;
        string PlatinumMastercardHigh=string.Empty;
        string PlatinumMastercardLow=string.Empty;
        bool IsJoin = false;
        bool IsPersonalDetails = false;
        

        public Validators()
        {
            RouteData currentRoute = HttpContext.Current.Request.RequestContext.RouteData;
            string ctrl = currentRoute.GetRequiredString("controller");
            string act = currentRoute.GetRequiredString("action");
            //Set Property to pick  Join specific configuration
            IsJoin = !(ctrl.ToLower().Equals("join")) ? false : true;
            IsPersonalDetails = !(ctrl.ToLower().Equals("personaldetails")) ? false : true;
            if(IsJoin)
                _viewpath = "~/Views/Shared/_PersonalDetails.cshtml";
            else if (IsPersonalDetails)
                _viewpath = "~/Views/Shared/_PersonalDetails.cshtml";
            else
                _viewpath = string.Format("~/Views/{0}/{1}.cshtml", ctrl, act);
            _configProvider = ServiceLocator.Current.GetInstance<IConfigurationProvider>();
            pCulture = _configProvider.GetStringAppSetting(AppConfigEnum.Culture);
            TescoVisaHigh = _configProvider.GetStringAppSetting(AppConfigEnum.Tesco_Visa_High);
            TescoVisaLow = _configProvider.GetStringAppSetting(AppConfigEnum.Tesco_Visa_Low);
            TescoPlatinumHigh = _configProvider.GetStringAppSetting(AppConfigEnum.Tesco_Platinum_High);
            TescoPlatinumLow = _configProvider.GetStringAppSetting(AppConfigEnum.Tesco_Platinum_Low);
            MastercardHigh = _configProvider.GetStringAppSetting(AppConfigEnum.Mastercard_High);
            MastercardLow = _configProvider.GetStringAppSetting(AppConfigEnum.Mastercard_Low);
            MastercardClubcardHigh = _configProvider.GetStringAppSetting(AppConfigEnum.Mastercard_Clubcard_High);
            MastercardClubcardLow = _configProvider.GetStringAppSetting(AppConfigEnum.Mastercard_Clubcard_Low);
            MastercardBonusHigh = _configProvider.GetStringAppSetting(AppConfigEnum.Mastercard_Bonus_High);
            MastercardBonusLow = _configProvider.GetStringAppSetting(AppConfigEnum.Mastercard_Bonus_Low);
            VisaBonusHigh = _configProvider.GetStringAppSetting(AppConfigEnum.Visa_Bonus_High);
            VisaBonusLow = _configProvider.GetStringAppSetting(AppConfigEnum.Visa_Bonus_Low);
            VisaClubcardHigh = _configProvider.GetStringAppSetting(AppConfigEnum.Visa_Clubcard_High);
            VisaClubcardLow = _configProvider.GetStringAppSetting(AppConfigEnum.Visa_Clubcard_Low);
            PlatinumMastercardHigh = _configProvider.GetStringAppSetting(AppConfigEnum.Platinum_Mastercard_High);
            PlatinumMastercardLow = _configProvider.GetStringAppSetting(AppConfigEnum.Platinum_Mastercard_Low);
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            ValidationResult vr=null;
            string moduleName=String.Empty;
            string displayName = string.Empty;
            if (!string.IsNullOrEmpty(context.DisplayName) && context.DisplayName.Contains("_"))
            {
                moduleName = context.DisplayName.Split('_')[0].ToString();
                displayName = context.DisplayName.Split('_')[1].ToString();
            }
            else
                displayName = context.DisplayName;

            switch (moduleName)
            {

                case "Activation":
                    
                    vr=ActivationRequired(value,displayName);
                      if (value != null)
                        {
                            if (displayName.ToString().Equals("MobilePhoneNumber"))
                            {
                                if (vr == null)
                                vr = prefixValidation(displayName, value);
                            }
                            else
                            {
                                if (vr == null)
                                    vr = RegexCheck(displayName, value);
                                if (vr == null)
                                    vr = LengthValidation(displayName, value);
                                if (displayName == "ClubcardNumber" && value.ToString() != "0")
                                {
                                    if (vr == null)
                                        vr = ClubcardValidation(displayName, value);
                                    break;
                                }
                            }
                           
                        }
                    
                    break;

                case "PersonalDetails":

                    if (!ValidatorUtility.IsNotVisible(displayName))
                    {
                        vr = PersonalDetailsRequired(value, displayName);

                        if (!string.IsNullOrEmpty(Convert.ToString(value).Trim()))
                        {
                            if (displayName.ToString().ToLower().Equals("mobilephonenumber")||displayName.ToString().ToLower().Equals("daytimephonenumber")||displayName.ToString().ToLower().Equals("eveningphonenumber"))
                            {
                                vr = prefixValidation(displayName, value);
                            }
                            else
                            {
                                     vr = RegexCheck(displayName, value);
                                if (vr == null)
                                    vr = LengthValidation(displayName, value);
                            }
                        }
                    }
                    break;

            }
            return vr;
        }

        private ValidationResult ActivationRequired(object value, string DisplayName)
        {
            ValidationResult vr = null;
            if (ValidatorUtility.IsActivationRequired(DisplayName))
            {
               
                    if (value == null || string.IsNullOrEmpty(value.ToString().Trim())||value.ToString()=="0")
                    {

                        vr = new ValidationResult(HttpContext.GetLocalResourceObject(_viewpath, DisplayName, System.Globalization.CultureInfo.CurrentCulture).ToString() );
                       
                    }
                
            }

            return vr;
        }

        private ValidationResult PersonalDetailsRequired(object value, string DisplayName)
        {
            ValidationResult vr = null;

            if (ValidatorUtility.IsRequiredField(DisplayName))
            {
                if (value == null)
                {
                    vr = new ValidationResult(HttpContext.GetLocalResourceObject(_viewpath, DisplayName, System.Globalization.CultureInfo.CurrentCulture).ToString());
                }
                else
                {
                    vr = RegexCheck(DisplayName, value);
                }
            }
            return vr;
        }
        
        private ValidationResult RegexCheck(string DisplayName, object actualValue)
        {
            ValidationResult vr = null;
            string value = actualValue != null ? actualValue.ToString().Trim() : String.Empty;
            bool isValid = false;
            string configValue1 = string.Empty;
            string configValue2 = string.Empty;
            ValidatorUtility.GetFormat(DisplayName, out configValue1, out configValue2);
            if (!string.IsNullOrEmpty(configValue1) && string.IsNullOrEmpty(configValue2))
            {
                isValid = RegexUtility.IsRegexMatch(value.ToString(), configValue1, false, false);
                if (!isValid)
                {
                    vr = new ValidationResult(HttpContext.GetLocalResourceObject(_viewpath, DisplayName,
                        System.Globalization.CultureInfo.CurrentCulture).ToString());
                }
            }
            else if (string.IsNullOrEmpty(configValue1) && !string.IsNullOrEmpty(configValue2))
            {
                isValid = RegexUtility.IsRegexMatch(value.ToString(), configValue2, false, false);
                if (!isValid)
                {
                    vr = new ValidationResult(HttpContext.GetLocalResourceObject(_viewpath, DisplayName,
                        System.Globalization.CultureInfo.CurrentCulture).ToString());
                }
            }
            else if (!string.IsNullOrEmpty(configValue1) && !string.IsNullOrEmpty(configValue2))
            {
               if (!RegexUtility.IsRegexMatch(value.ToString(), configValue1, false, false) &&
                    !RegexUtility.IsRegexMatch(value.ToString(), configValue2, false, false))
                {
                    vr = new ValidationResult(HttpContext.GetLocalResourceObject(_viewpath, DisplayName,
                        System.Globalization.CultureInfo.CurrentCulture).ToString());
                }

            }


            return vr;
        }

        private ValidationResult LengthValidation(string DisplayName, object value)
        {
            ValidationResult vr = null;
            bool isValid = false;
            string configValue1 = string.Empty;
            string configValue2 = string.Empty;
            ValidatorUtility.GetLength(DisplayName, out configValue1, out configValue2);

            if (!string.IsNullOrEmpty(configValue1) && string.IsNullOrEmpty(configValue2))
            {
                if (value != null && value.ToString().Length >= configValue1.TryParse<Int64>())
                    isValid = true;
                if (!isValid)
                {
                    vr = new ValidationResult(HttpContext.GetLocalResourceObject(_viewpath, "Len_" + DisplayName,
                        System.Globalization.CultureInfo.CurrentCulture).ToString());
                }
            }
            else if (value != null && string.IsNullOrEmpty(configValue1) && !string.IsNullOrEmpty(configValue2))
            {
                if (value.ToString().Length <= configValue2.TryParse<Int64>())
                    isValid = true;
                if (!isValid)
                {
                    vr = new ValidationResult(HttpContext.GetLocalResourceObject(_viewpath, "Len_" + DisplayName,
                        System.Globalization.CultureInfo.CurrentCulture).ToString());
                }
            }
            else if (value != null && !string.IsNullOrEmpty(configValue1) && !string.IsNullOrEmpty(configValue2))
            {

                if (value.ToString().Length >= configValue1.TryParse<Int64>() && value.ToString().Length <= configValue2.TryParse<Int64>())
                    isValid = true;
                if (!isValid)
                {
                    vr = new ValidationResult(HttpContext.GetLocalResourceObject(_viewpath, "Len_" + DisplayName,
                         System.Globalization.CultureInfo.CurrentCulture).ToString());
                }

            }


            return vr;
        }

        private ValidationResult prefixValidation(string DisplayName, object value)
        {
            ValidationResult vr = null;
            //bool isValid = false;
            //bool bErrorFlag = true;
            string configValue1 = string.Empty;
            string configValue2 = string.Empty;
            string phoneNumber=value!=null ?value.ToString():String.Empty;
            ValidatorUtility.GetPrefix(DisplayName, out configValue1, out configValue2);

            if (DisplayName.ToLower().Equals("mobilephonenumber") )
            {
                if (!string.IsNullOrEmpty(phoneNumber))//If the configured data has more than one value(comma seperated)
                {
                    if (configValue1.Contains(','))
                    {
                        string[] mobPrefixes = configValue1.Split(',');
                        bool flgMobPrefix = false;

                        for (int i = 0; i < mobPrefixes.Length; i++)
                        {
                            if (!(phoneNumber.Trim().Length < mobPrefixes[i].Trim().Length))
                            {
                                if (phoneNumber.Trim().Substring(0, mobPrefixes[i].Trim().Length) == mobPrefixes[i].ToString())
                                {
                                    flgMobPrefix = true;
                                    break;
                                }
                            }
                        }
                        if (!flgMobPrefix)
                        {

                            vr = new ValidationResult(HttpContext.GetLocalResourceObject(_viewpath, "MobilePhoneNumber",
                         System.Globalization.CultureInfo.CurrentCulture).ToString());
                        }
                        else if (phoneNumber.Trim().Substring(0, 2) == configValue2)
                        {
                            string regConfigValue1 = string.Empty;
                            string regConfigValue2 = string.Empty;
                            ValidatorUtility.GetFormat(DisplayName, out regConfigValue1, out regConfigValue2);
                            if (!RegexUtility.IsRegexMatch(phoneNumber.Trim(), regConfigValue1, false, false))
                            {
                                vr = new ValidationResult(HttpContext.GetLocalResourceObject(_viewpath, "MobilePhoneNumber",
                         System.Globalization.CultureInfo.CurrentCulture).ToString());
                            }
                        }
                        else
                        {
                            string regConfigValue1 = string.Empty;
                            string regConfigValue2 = string.Empty;
                            ValidatorUtility.GetFormat(DisplayName, out regConfigValue1, out regConfigValue2);
                            if (!RegexUtility.IsRegexMatch(phoneNumber.Trim(), regConfigValue2, false, false))
                            {
                                vr = new ValidationResult(HttpContext.GetLocalResourceObject(_viewpath, "Landline",
                                        System.Globalization.CultureInfo.CurrentCulture).ToString());
                               // bErrorFlag = false;
                            }
                        }
                         

                    }
                    else if (phoneNumber.Trim().Substring(0, configValue1.Trim().Length) != configValue1)
                    {

                        vr = new ValidationResult(HttpContext.GetLocalResourceObject(_viewpath, "MobilePhoneNumber",
                                        System.Globalization.CultureInfo.CurrentCulture).ToString());

                    }
                    else
                    {
                        string regConfigValue1 = string.Empty;
                        string regConfigValue2 = string.Empty;
                        ValidatorUtility.GetFormat(DisplayName, out regConfigValue1, out regConfigValue2);
                        if (!RegexUtility.IsRegexMatch(phoneNumber.Trim(), regConfigValue1, false, false))
                        {
                            vr = new ValidationResult(HttpContext.GetLocalResourceObject(_viewpath, "MobilePhoneNumber",
                     System.Globalization.CultureInfo.CurrentCulture).ToString());
                        }
 
                    }

                 if(vr==null) 
                    vr = LengthValidation(DisplayName, value);
                
                }
            }
            if (DisplayName.ToLower().Equals("daytimephonenumber") )
            {
                if (value != null)//If the configured data has more than one value(comma seperated)
                {
                    if (!string.IsNullOrEmpty(configValue1))
                    {
                        string[] dayPrefixes = configValue1.Split(',');
                        bool flgMobPrefix = false;

                        for (int i = 0; i < dayPrefixes.Length; i++)
                        {
                            if (!(phoneNumber.Trim().Length < dayPrefixes[i].Trim().Length))
                            {
                                if (phoneNumber.Trim().Substring(0, dayPrefixes[i].Trim().Length) == dayPrefixes[i].ToString())
                                {
                                    flgMobPrefix = true;
                                    break;
                                }
                            }
                        }

                        if (!flgMobPrefix)
                        {
                            vr = new ValidationResult(HttpContext.GetLocalResourceObject(_viewpath, DisplayName, System.Globalization.CultureInfo.CurrentCulture).ToString());
                        }
                    }
                    //else
                    //{
                    if (vr == null)
                        vr = RegexCheck(DisplayName, value);
                    if(vr==null)
                        vr = LengthValidation(DisplayName, value);


                    //}
                }



            }
            if (DisplayName.ToLower().Equals("eveningphonenumber") )
            {
                if (value != null)//If the configured data has more than one value(comma seperated)
                {
                    if (!string.IsNullOrEmpty(configValue1) )
                    {
                        string[] dayPrefixes = configValue1.Split(',');
                        bool flgMobPrefix = false;

                        for (int i = 0; i < dayPrefixes.Length; i++)
                        {
                            if (!(phoneNumber.Trim().Length < dayPrefixes[i].Trim().Length))
                            {
                                if (phoneNumber.Trim().Substring(0, dayPrefixes[i].Trim().Length) == dayPrefixes[i].ToString())
                                {
                                    flgMobPrefix = true;
                                    break;
                                }
                            }
                        }

                        if (!flgMobPrefix)
                        {
                            vr = new ValidationResult(HttpContext.GetLocalResourceObject(_viewpath, DisplayName, System.Globalization.CultureInfo.CurrentCulture).ToString());
                        }
                    }
                    //else
                    //{
                        if (vr == null)
                            vr = RegexCheck(DisplayName, value);
                        if (vr == null)
                            vr = LengthValidation(DisplayName, value);


                    //}
                }



            }
            return vr;
        }

         private ValidationResult ClubcardValidation(string DisplayName, object value)
        {
            ValidationResult vr = null;
            string ClubcardNumber = value.ToString();

            if (ClubcardNumber.ToString().Trim().Length > 8) // To check whether user has entered credit card No
            {
                if ((long.Parse(ClubcardNumber.Substring(0, 8).Trim()) >= long.Parse(TescoVisaHigh.Trim()) && long.Parse(ClubcardNumber.Substring(0, 8).Trim()) <= long.Parse(TescoVisaLow.Trim())) || (long.Parse(ClubcardNumber.Substring(0, 8).Trim()) >= long.Parse(TescoPlatinumHigh.Trim()) && long.Parse(ClubcardNumber.Substring(0, 8).Trim()) <= long.Parse(TescoPlatinumLow.Trim())) ||
                (long.Parse(ClubcardNumber.Substring(0, 8).Trim()) >= long.Parse(MastercardHigh.Trim()) && long.Parse(ClubcardNumber.Substring(0, 8).Trim()) <= long.Parse(MastercardLow.Trim())) || (long.Parse(ClubcardNumber.Substring(0, 8).Trim()) >= long.Parse(MastercardClubcardHigh.Trim()) && long.Parse(ClubcardNumber.Substring(0, 8).Trim()) <= long.Parse(MastercardClubcardLow.Trim())) ||
                (long.Parse(ClubcardNumber.Substring(0, 8).Trim()) >= long.Parse(MastercardBonusHigh.Trim()) && long.Parse(ClubcardNumber.Substring(0, 8).Trim()) <= long.Parse(MastercardBonusLow.Trim())) || (long.Parse(ClubcardNumber.Substring(0, 8).Trim()) >= long.Parse(VisaBonusHigh.Trim()) && long.Parse(ClubcardNumber.Substring(0, 8).Trim()) <= long.Parse(VisaBonusLow.Trim())) ||
                (long.Parse(ClubcardNumber.Substring(0, 8).Trim()) >= long.Parse(VisaClubcardHigh.Trim()) && long.Parse(ClubcardNumber.Substring(0, 8).Trim()) <= long.Parse(VisaClubcardLow.Trim())) || (long.Parse(ClubcardNumber.Substring(0, 8).Trim()) >= long.Parse(PlatinumMastercardHigh.Trim()) && long.Parse(ClubcardNumber.Substring(0, 8).Trim()) <= long.Parse(PlatinumMastercardLow.Trim())))
                {
                         vr = new ValidationResult(HttpContext.GetLocalResourceObject(_viewpath, "CreditCard_" + DisplayName,
                         System.Globalization.CultureInfo.CurrentCulture).ToString());
                }

            }


            return vr;
        }
    }       

         
}
