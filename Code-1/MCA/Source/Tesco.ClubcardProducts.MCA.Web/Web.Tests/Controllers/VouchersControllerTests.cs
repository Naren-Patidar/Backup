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
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Vouchers;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using System.Threading; 


namespace Web.Tests
{
    [TestFixture]
    public class VouchersControllerTests
    {
        private IVoucherBC _voucherProvider;
        private IPDFGenerator _pdfProvider;
        private VouchersController vouchersController;
        IConfigurationProvider _configProvider = null;
        private ILoggingService _AuditLoggerService;
        private IAccountBC _IAccountBC;
        private IPointsBC _IPointsBC;
        private IXmasSaverBC _IXmasBC;
        [SetUp]
        public void SetUp()
        {
            _voucherProvider = MockRepository.GenerateMock<IVoucherBC>();
            _pdfProvider = MockRepository.GenerateMock<IPDFGenerator>();
            _configProvider = MockRepository.GenerateMock<IConfigurationProvider>();
            _AuditLoggerService = MockRepository.GenerateMock<ILoggingService>();
            _IAccountBC = MockRepository.GenerateMock<IAccountBC>();
            _IPointsBC = MockRepository.GenerateMock<IPointsBC>();
            _IXmasBC = MockRepository.GenerateMock<IXmasSaverBC>();
            // _logger = MockRepository.GenerateMock<ILoggingService>();
            vouchersController = new VouchersController(_voucherProvider, _pdfProvider, _configProvider, _AuditLoggerService, _IAccountBC, _IPointsBC, _IXmasBC);

        }

        [TestCase]
        public void Home_Execution_Successful()
        {
            List<VoucherRewardDetails> voucherRewardDetailsList = new List<VoucherRewardDetails>();
            var firstRewardList = new VoucherRewardDetails
            {
                RewardIssued = 160,
                RewardLeftOver = 0,
                RewardUsed = 160
            };

            var firstRewardModel = new VoucherRewardDetailsOverallSummaryModel(voucherRewardDetailsList);
            //var firstRewardList = new VouchersViewModel
            //{
            //    voucherRewardDetailsOverallSummaryModel = firstRewardModel
            //};
            //_voucherProvider.Stub(x => x.GetVoucherViewDetails(1, 2, "en-GB")).IgnoreArguments().Return(firstRewardList);
            var viewResult = vouchersController.Home() as ViewResult;
            var viewModel = viewResult.Model as VouchersViewModel;
            Assert.AreSame(firstRewardList, viewModel);
        }
        [TestCase]
        public void Voucher_Download_Audit_Successful()
        {
            string strToCompare = MockVoucherData();
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

        private string MockVoucherData()
        {
            LogData logDataAudit = new AuditLogData();
            _AuditLoggerService = new AuditLoggingService();
            string strVoucherInfo = string.Empty;  
            try
            {
                List<VoucherDetails> selectedVouchers = new List<VoucherDetails>();
                VoucherDetails vd = new VoucherDetails();
                vd.VoucherNumberToPrint = "qwerty1234";
                selectedVouchers.Add(vd);
                VoucherDetails vd1 = new VoucherDetails();
                vd1.VoucherNumberToPrint = "qwerty5678";
                selectedVouchers.Add(vd1);
                strVoucherInfo = GetAuditDetailForVouchers(selectedVouchers);               
                logDataAudit.RecordStep(string.Format("Downloaded Vouchers : {0}", strVoucherInfo));
                return strVoucherInfo; 
                
            }
            catch
            {

            }
            finally
            {
                _AuditLoggerService.Submit(logDataAudit);
            }
            return strVoucherInfo; 

        }

        private static string GetAuditDetailForVouchers(List<VoucherDetails> selectedVouchers)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (VoucherDetails v in selectedVouchers)
            {
                if (v.VoucherNumberToPrint.Length > 0)
                {
                    sb.Append(v.VoucherNumberToPrint.ToString().Substring(v.VoucherNumberToPrint.Length - 4));
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
            _voucherProvider = null;

            vouchersController.Dispose();
            vouchersController = null;
        }
    }
}
