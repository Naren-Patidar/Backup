using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Reflection;

namespace Tesco.Com.Marketing.Kiosk.JoinAtKiosk
{
    public class CommonClassForJoin
    {

        public CommonClassForJoin()
        {
            this.InitDefaults();
        }

       

        [DefaultValue("")]
        public static string CurrentCulture
        {
            get;
            set;
        }

        /// <summary>
        /// name Config Details
        /// </summary>
        public static int Name1MaxLength { get; set; }
        public static int Name1MinLength { get; set; }
        public static int Name2MaxLength { get; set; }
        public static int Name2MinLength { get; set; }
        public static int Name3MaxLength { get; set; }
        public static int Name3MinLength { get; set; }
        public static bool Name3Required { get; set; }
        public static bool Name2Required { get; set; }
        public static bool Name1Required { get; set; }
        public static bool AddressLine1 { get; set; }
        public static bool AddressLine2 { get; set; }
        public static bool AddressLine3 { get; set; }
        public static bool AddressLine4 { get; set; }
        public static bool AddressLine5 { get; set; }

        [DefaultValue("")]
        public static string Name1RegExp { get; set; }
        [DefaultValue("")]
        public static string Name2RegExp { get; set; }
        [DefaultValue("")]
        public static string Name3RegExp { get; set; }

        /// <summary>
        /// address Config Details
        /// </summary>
        [DefaultValue("")]
        public static string PostcodeRegExp1 { get; set; }
        [DefaultValue("")]
        public static string PostcodeRegExp2 { get; set; }
        public static bool IsProfinityCheckNeeded { get; set; }
        public static bool TitleRequired { get; set; }
        [DefaultValue("")]
        public static string MailingAddressRegExp { get; set; }
        public static bool AddressPostCodeReq { get; set; }
        public static bool AddressLine1Req { get; set; }
        public static bool AddressLine2Req { get; set; }
        public static bool AddressLine3Req { get; set; }
        public static bool AddressLine4Req { get; set; }
        public static bool AddressLine5Req { get; set; }
        public static bool PostcodeReq { get; set; }
        public static int AddressLine1MaxLength { get; set; }
        public static int AddressLine1MinLength { get; set; }
        public static int AddressLine2MaxLength { get; set; }
        public static int AddressLine2MinLength { get; set; }
        public static int AddressLine3MaxLength { get; set; }
        public static int AddressLine3MinLength { get; set; }
        public static int AddressLine4MaxLength { get; set; }
        public static int AddressLine4MinLength { get; set; }
        public static int AddressLine5MaxLength { get; set; }
        public static int AddressLine5MinLength { get; set; }
        public static int PostcodeMaxLength { get; set; }
        public static int PostcodeMinLength { get; set; }
      

        /// <summary>
        /// Moboile, daytime and evening no config details
        /// </summary>
        public static int DaytimeNumberMaxLength { get; set; }
        public static int DaytimeNumberMinLength { get; set; }
        public static int MobileNumberMaxLength { get; set; }
        public static int MobileNumberMinLength { get; set; }
        public static int EveningNumberMaxLength { get; set; }
        public static int EveningNumberMinLength { get; set; }
        public static int EmailMaxLength { get; set; }
        public static int EmailMinLength { get; set; }
        public static bool EmailRequired { get; set; }
        public static bool DaytimeNumberRequired { get; set; }
        public static bool MobileNumberRequired { get; set; }
        public static bool EveningNumberRequired { get; set; }
        [DefaultValue("")]
        public static string EmailRegExp { get; set; }
        [DefaultValue("")]
        public static string PhoneRegExp { get; set; }
        [DefaultValue("")]
        public static string DaytimePhoneFormat { get; set; }
        [DefaultValue("")]
        public static string MobilePhoneFormat { get; set; }
       
        /// <summary>
        /// Language, Passport, SSN
        /// </summary>
        public static bool PassportRequired { get; set; }
        [DefaultValue("")]
        public static string PassportRegExp { get; set; }
        public static bool SSNRequired { get; set; }
        [DefaultValue("")]
        public static string SSNRegExp { get; set; }
        public static int PassportMaxLength { get; set; }
        public static int SSNMaxLength { get; set; }
        public static int PassportMinLength { get; set; }
        public static int SSNMinLength { get; set; }
        public static bool LanguageReq { get; set; }

        /// <summary>
        /// Promotionalcode
        /// </summary>
        public static int PromotionalCodeMaxLength { get; set; }
        public static int PromotionalCodeMinLength { get; set; }
        public static bool PromotionalCodeReq { get; set; }
        
        [DefaultValue("")]
        public static string JoinRouteCodeForKiosk { get; set; }

        public static bool RaceRequired { get; set; }

        public static bool DOBReq { get; set; }
        public static bool HHAge1Req { get; set; }
        public static bool HHAge2Req { get; set; }
        public static bool HHAge3Req { get; set; }
        public static bool HHAge4Req { get; set; }
        public static bool HHAge5Req { get; set; }

        public static bool DietryPrefReq { get; set; }
        public static bool ContactPrefReq { get; set; }

        [DefaultValue("")]
        public static string ProfinityCheckFields { get; set; }

    }


    public static class ExtensionsClass
    {
        /// <summary>
        /// Inititializes default values of automatic properties
        /// </summary>
        /// <param name="o">The o.</param>
        public static void InitDefaults(this object o)
        {
            PropertyInfo[] props = o.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);

            for (int i = 0; i < props.Length; i++)
            {
                PropertyInfo prop = props[i];

                if (prop.GetCustomAttributes(true).Length > 0)
                {
                    object[] defaultValueAttribute = prop.GetCustomAttributes(typeof(DefaultValueAttribute), true);

                    if (defaultValueAttribute != null)
                    {
                        DefaultValueAttribute dva = defaultValueAttribute[0] as DefaultValueAttribute;

                        if (dva != null)
                            prop.SetValue(o, dva.Value, null);
                    }
                }
            }
        }
    }
}