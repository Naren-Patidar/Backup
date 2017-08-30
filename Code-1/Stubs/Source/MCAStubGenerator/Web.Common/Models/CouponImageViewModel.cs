using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Tesco.ClubcardProducts.MCA.Web.Common.Models
{
    /// <summary>
    /// Model for the Coupon Image preview
    /// </summary>
    public class CouponImageViewModel
    {
        #region Private Fields

        string couponDescription = string.Empty;
        string couponImageFile = string.Empty;

        #endregion

        #region Public Properties

        /// <summary>
        /// Property to get or set thte coupon description
        /// </summary>
        public string CouponDescription
        {
            get { return couponDescription; }
            set { couponDescription = value; }
        }

        /// <summary>
        /// Property to get or set the Coupon Image File path
        /// </summary>
        public string CouponImageFile
        {
            get { return couponImageFile; }
            set { couponImageFile = value; }
        }

        #endregion
    }
}
