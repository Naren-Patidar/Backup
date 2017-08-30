using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.OrderReplacement;
using System.Collections;
using System.Web;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Models
{
    public class OrderAReplacementModel
    {
        public OrderReplacementModel OrderReplacementModel { get; set; }
        public Dictionary<string, string> Reasons
        {
            get
            {
                Dictionary<string, string> reasons = new Dictionary<string, string>();
                reasons.Add("L", HttpContext.GetLocalResourceObject("~/Views/AccountManagement/OrderANewCard.cshtml", "lclKeyfobResource1.Text", System.Globalization.CultureInfo.CurrentCulture).ToString());
                reasons.Add("D", HttpContext.GetLocalResourceObject("~/Views/AccountManagement/OrderANewCard.cshtml", "lcldmgedResource1.Text", System.Globalization.CultureInfo.CurrentCulture).ToString());
                reasons.Add("S", HttpContext.GetLocalResourceObject("~/Views/AccountManagement/OrderANewCard.cshtml", "lclStolenResource1.Text", System.Globalization.CultureInfo.CurrentCulture).ToString());
                reasons.Add("M", HttpContext.GetLocalResourceObject("~/Views/AccountManagement/OrderANewCard.cshtml", "lclHHkeyResource1.Text", System.Globalization.CultureInfo.CurrentCulture).ToString());
                reasons.Add("O", HttpContext.GetLocalResourceObject("~/Views/AccountManagement/OrderANewCard.cshtml", "lclothrResource1.Text", System.Globalization.CultureInfo.CurrentCulture).ToString());
                return reasons;
            }
        }
        public bool IsInProcess { get; set; }
        public bool IsNonStandardCard { get; set; }
        public bool IsMaxOrdersReached { get; set; }
        public bool divStandardNonStandard { get; set; }
        public bool errorMsg { get; set; }
    }
}
