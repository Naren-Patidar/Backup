using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Models
{
    public class PersonalDetailsPreferences
    {
        PreferenceEnum preferenceType { get; set; }
        bool optedStatus { get; set; }
    }
}
