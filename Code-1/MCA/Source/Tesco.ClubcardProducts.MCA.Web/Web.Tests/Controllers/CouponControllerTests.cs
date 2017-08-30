using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rhino.Mocks;
using Tesco.ClubcardProducts.MCA.Web.Business.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Controllers;
using Tesco.ClubcardProducts.MCA.Web.Common.Models;
using System.Web.Mvc;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
using Tesco.ClubcardProducts.MCA.Web;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Ecoupon;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common;
using System.IO;
using System.Threading;  

namespace Web.Tests
{
    [TestFixture]
    public class CouponControllerTests
    {
        private ICouponBC couponProvider;
        private IPDFGenerator _pdfProvider;
        private CouponsController  couponController;
        IConfigurationProvider _configProvider = null;
        private ILoggingService _AuditLoggerService;
        private IAccountBC _IAccountBC;

        [SetUp]
        public void SetUp()
        {
            couponProvider = MockRepository.GenerateMock<ICouponBC>();
            _pdfProvider = MockRepository.GenerateMock<IPDFGenerator>();
            _configProvider = MockRepository.GenerateMock<IConfigurationProvider>();
            _AuditLoggerService = MockRepository.GenerateMock<ILoggingService>();
            _IAccountBC = MockRepository.GenerateMock<IAccountBC>();
            // _logger = MockRepository.GenerateMock<ILoggingService>();
            couponController = new CouponsController(_IAccountBC, couponProvider, _pdfProvider, _AuditLoggerService, _configProvider);
            
        }

        [TestCase]
        public void Coupon_Download_Audit_Successful()
        {
            string strToCompare = MockCouponData();
            //--${basedir}/../../../../Trace/MCA.Audit.${shortdate}.log
            string strBasedir = AppDomain.CurrentDomain.BaseDirectory.ToString();
            string shortdate = DateTime.Now.ToString(@"yyyy-MM-dd");
            string strRawFileName = "{0}/../../../../../../UnitTest//MCA.Audit.{1}.log";
            string strFile = string.Format(strRawFileName, strBasedir, shortdate);
            bool b = false;
            for (int i = 0; i < 5; i++)
            {
                if (System.IO.File.Exists(strFile))
                {
                    List<string> lst = System.IO.File.ReadLines(strFile).ToList();
                    if (lst.Count > 2)
                    {
                        b = lst[lst.Count - 2].ToString().Contains(strToCompare);
                    }
                }
                if (b)
                {
                    break;
                }
                else
                {
                    Thread.Sleep(500);
                    continue;
                }
            }

            Assert.AreEqual(true, b);
        }

        private string MockCouponData()
        {
            LogData logDataAudit = new AuditLogData();
            _AuditLoggerService = new AuditLoggingService();
            string strToValidateInfo = string.Empty;
            try
            {
                List<CouponDetails> selectedCoupon = new List<CouponDetails>();
                CouponDetails cd = new CouponDetails();
                cd.CouponDescription = "Coupon1";
                selectedCoupon.Add(cd); 
                CouponDetails cd1 = new CouponDetails();
                cd1.CouponDescription = "Coupon2";
                selectedCoupon.Add(cd1);
                strToValidateInfo = GetAuditDetailForCoupons(selectedCoupon);
                logDataAudit.RecordStep(string.Format("Downloaded Coupon : {0}", strToValidateInfo));
                return strToValidateInfo;

            }
            catch
            {

            }
            finally
            {
                _AuditLoggerService.Submit(logDataAudit);
            }
            return strToValidateInfo;

        }

        private static string GetAuditDetailForCoupons(List<CouponDetails> availableCoupons)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (CouponDetails v in availableCoupons)
            {
                if (v.CouponDescription.Length > 0)
                {
                    sb.Append(v.CouponDescription.ToString());
                    sb.Append(", ");
                }
            }

            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 2, 2);
            }
            return sb.ToString();
        }
        [TearDown]
        public void TestCleanup()
        {

            couponController.Dispose();
            couponController = null;
        }

    }
}
