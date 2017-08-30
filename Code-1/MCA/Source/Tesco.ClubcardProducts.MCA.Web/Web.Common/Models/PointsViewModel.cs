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
        public PointsBanner PointsStrip { get; set; }
        
        private bool _isPointEarnedEver;

        public bool IsPointEarnedEver
        {
            get { return _isPointEarnedEver; }
            set { _isPointEarnedEver = value; }
        }
    }
}
