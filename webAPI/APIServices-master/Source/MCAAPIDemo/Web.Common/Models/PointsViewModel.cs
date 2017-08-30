using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Models
{
    public class PointsViewModel
    {
        public List<Offer> Offers {get; set;}

        private string _optedPreference;
        private bool _isPointEarnedEver;

        public bool IsPointEarnedEver
        {
            get { return _isPointEarnedEver; }
            set { _isPointEarnedEver = value; }
        }

        public string OptedPreference
        {
            get { return _optedPreference; }
            set { _optedPreference = value; }
        }
    }
}
