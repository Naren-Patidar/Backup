using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Globalization;
using System.Configuration;

namespace PrintVouchersAtKiosk
{
    public class BaseUIPage : System.Web.UI.Page
    {
        #region properties

        public const string SELECTED_CUSTOMERS_INDEX = "SelectedCustomersIndex";

        /// <summary>
        /// Gets or sets the session booking.
        /// </summary>
        /// <value>The session booking.</value>
        public BigExchange.BookingPrintVoucher BookingPrintVoucher
        {
            get
            {
                if (Session["BookingPrintVoucher"] == null)
                {
                    Session["BookingPrintVoucher"] = Helper.CreateBooking("O");
                }

                return (BigExchange.BookingPrintVoucher)Session["BookingPrintVoucher"];

            }
            set
            {
                Session["BookingPrintVoucher"] = value;
            }
        }

        public BigExchange.UnusedVoucherCollection VoucherCollAll
        {
            get
            {
                if (Session["VoucherCollAll"] == null)
                {
                    BigExchange.UnusedVoucherCollection vc = new BigExchange.UnusedVoucherCollection();
                    Session["VoucherCollAll"] = vc;
                }
                return (BigExchange.UnusedVoucherCollection)Session["VoucherCollAll"];

            }
            set
            {
                Session["VoucherCollAll"] = value;
            }
        }

        //Added to put coupon collection in session
        public BigExchange.UnusedCouponCollection CouponCollAll
        {
            get
            {
                if (Session["CouponCollAll"] == null)
                {
                    BigExchange.UnusedCouponCollection CC = new BigExchange.UnusedCouponCollection();
                    Session["CouponCollAll"] = CC;
                }
                return (BigExchange.UnusedCouponCollection)Session["CouponCollAll"];
            }
            set
            {
                Session["CouponCollAll"] = value;
            }
        }

        public List<Int32> SelectedCustomersIndex
        {
            get
            {
                if (ViewState[SELECTED_CUSTOMERS_INDEX] == null)
                {
                    ViewState[SELECTED_CUSTOMERS_INDEX] = new List<Int32>();
                }

                return (List<Int32>)ViewState[SELECTED_CUSTOMERS_INDEX];
            }
        }

        #endregion

        #region Culture setting
        /// <SUMMARY>
        /// Overriding the InitializeCulture method to set the user selected
        /// option in the current thread. Note that this method is called much
        /// earlier in the Page lifecycle and we don't have access to any controls
        /// in this stage, so have to use Form collection.
        /// </SUMMARY>
        protected override void InitializeCulture()
        {
            string culture = ConfigurationManager.AppSettings["CountryCode"];

            if (Session["MyUICulture"] == null && Session["MyCulture"] == null)
            {
                SetCulture(culture, culture);
            }

            if (Session["MyUICulture"] != null && Session["MyCulture"] != null)
            {
                Thread.CurrentThread.CurrentUICulture = (CultureInfo)Session["MyUICulture"];
                Thread.CurrentThread.CurrentCulture = (CultureInfo)Session["MyCulture"];
            }
            base.InitializeCulture();
        }


        /// <Summary>
        /// Sets the current UICulture and CurrentCulture based on
        /// the arguments
        /// </Summary>
        /// <PARAM name="name"></PARAM>
        /// <PARAM name="locale"></PARAM>
        protected void SetCulture(string name, string locale)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(name);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(locale);
            ///<remarks>
            ///Saving the current thread's culture set by the User in the Session
            ///so that it can be used across the pages in the current application.
            ///</remarks>
            Session["MyUICulture"] = Thread.CurrentThread.CurrentUICulture;
            Session["MyCulture"] = Thread.CurrentThread.CurrentCulture;
        }
        #endregion
    }
}