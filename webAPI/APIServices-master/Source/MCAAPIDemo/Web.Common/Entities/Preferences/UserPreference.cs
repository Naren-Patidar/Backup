using System;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences
{
    public class UserPreference
    {
        public PreferenceEnum PreferenceID { get; set; }
        //public OptStatus OptedStatus { get; set; }
        public DateTime UpdateDateTime { get { return DateTime.Now; } }
        public string EmailSubject { get; set; }
    }
}
