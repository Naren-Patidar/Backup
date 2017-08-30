using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tesco.ClubcardProducts.MCA.Web.Business.Contracts;
using Microsoft.Practices.ServiceLocation;
using Tesco.ClubcardProducts.MCA.Web.MVCAttributes;
using Tesco.ClubcardProducts.MCA.Web.Common.Models;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences;

namespace Tesco.ClubcardProducts.MCA.Web.Controllers
{
    public class ChristmasSaversController : BaseController
    {
        IXmasSaverBC _xmasSaverProvider;

        #region Constructors

        /// <summary>
        /// Default parameter less constructor
        /// </summary>
        public ChristmasSaversController()
        {
            _xmasSaverProvider = ServiceLocator.Current.GetInstance<IXmasSaverBC>();
        }

        /// <summary>
        /// Constructor with all required business interfaces as parameter
        /// </summary>
        /// <param name="xmasSaverProvider"></param>
        public ChristmasSaversController(IXmasSaverBC _IxmasSaverBC)
        {
            this._xmasSaverProvider = _IxmasSaverBC;
        }

        #endregion

        [HttpGet]
        [AuthorizeUser(Order = 1)]
        [SecurityCheck(Order = 2)]
        [PageAuthorization(Order = 3)]
        public ActionResult Home()
        {
            XmasSaverViewModel xmasSaverViewModel = new XmasSaverViewModel();

            string sCustomerID = base.CustomerId;

            if (string.IsNullOrEmpty(sCustomerID))
            {
                return View("~/Views/Home/Home.cshtml");
            }
            else
            {
                if (_xmasSaverProvider.CheckCustomerIsXmasClubMember(sCustomerID, CurrentCulture))
                {
                    xmasSaverViewModel = _xmasSaverProvider.GetXmasSaverViewModel(CustomerId, CurrentCulture);
                    return View(xmasSaverViewModel);
                }
                else
                {
                    return RedirectToAction("VoucherSchemes", "AccountManagement");
                }
            }
        }
    }
}
