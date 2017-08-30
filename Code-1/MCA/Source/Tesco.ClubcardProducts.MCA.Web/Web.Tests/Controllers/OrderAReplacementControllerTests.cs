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
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.OrderReplacement;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Common;
using System.IO;
using System.Threading;

namespace Web.Tests
{
    [TestFixture]
    public class OrderAReplacementControllerTests
    {
        private OrderAReplacementController replacementController;
        private IOrderAReplacementBC _orderAReplacementProvider;
        private ILoggingService _AuditLoggerService;
        private IAccountBC _IAccountBC;
        private IConfigurationProvider _IConfigurationProvider;

        [SetUp]
        public void SetUp()
        {
            _orderAReplacementProvider = MockRepository.GenerateMock<IOrderAReplacementBC>();
            _AuditLoggerService = MockRepository.GenerateMock<ILoggingService>();
            _IAccountBC = MockRepository.GenerateMock<IAccountBC>();
            _IConfigurationProvider = MockRepository.GenerateMock<IConfigurationProvider>();
            replacementController = new OrderAReplacementController(_orderAReplacementProvider, _AuditLoggerService, _IAccountBC, _IConfigurationProvider);
        }

        [TestCase]
        public void Home_Get_Execution_Successful()
        {
            OrderAReplacementModel orderReplacementModel = new OrderAReplacementModel();
            orderReplacementModel.OrderReplacementModel.ClubcardNumber = 1;
            orderReplacementModel.divStandardNonStandard = false;
            orderReplacementModel.errorMsg = false;
            orderReplacementModel.IsInProcess = false;
            orderReplacementModel.IsMaxOrdersReached = false;
            //orderReplacementModel.IsNewOrder = true;
            orderReplacementModel.IsNonStandardCard = false;
            orderReplacementModel.OrderReplacementModel.ClubcardNumber = 1;
            orderReplacementModel.OrderReplacementModel.CustomerIdEncrypt = CryptoUtility.EncryptTripleDES("1");
            orderReplacementModel.OrderReplacementModel.Reason = null;
            orderReplacementModel.OrderReplacementModel.RequestType = OrderReplacementTypeEnum.NewCardAndKeyFOB;


            _orderAReplacementProvider.Stub(x => x.GetOrderAReplacementModel(1,"en-GB")).IgnoreArguments().Return(orderReplacementModel);

            var viewResult = replacementController.Home() as ViewResult;
            Assert.AreEqual(5, orderReplacementModel.Reasons.Count);
            Assert.AreEqual(null, orderReplacementModel.OrderReplacementModel.Reason);
        }

        [TestCase]
        public void Home_Post_Execution_Successful()
        {
            OrderAReplacementModel orderReplacementModel = new OrderAReplacementModel();
            orderReplacementModel.OrderReplacementModel.ClubcardNumber= 1;
            orderReplacementModel.divStandardNonStandard = false;
            orderReplacementModel.errorMsg = false;
            orderReplacementModel.IsInProcess = false;
            orderReplacementModel.IsMaxOrdersReached = false;
            //orderReplacementModel.IsNewOrder = true;
            orderReplacementModel.IsNonStandardCard = false;
            orderReplacementModel.OrderReplacementModel.ClubcardNumber = 1;
            orderReplacementModel.OrderReplacementModel.CustomerIdEncrypt = CryptoUtility.EncryptTripleDES("1");
            orderReplacementModel.OrderReplacementModel.Reason = "L";
            orderReplacementModel.OrderReplacementModel.RequestType = OrderReplacementTypeEnum.NewCardAndKeyFOB;


            _orderAReplacementProvider.Stub(x => x.GetOrderAReplacementModel(1,"en-GB")).IgnoreArguments().Return(orderReplacementModel);

            var viewResult = replacementController.Home(orderReplacementModel) as ViewResult;
            Assert.AreEqual(true, orderReplacementModel.IsInProcess);
        }

        [TestCase]
        public void OrderAReplacement_Audit_Successful()
        {
            string strToCompare = StubAndLogOrderAReplacementData();
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

        private string StubAndLogOrderAReplacementData()
        {
            LogData logDataAudit = new AuditLogData();
            _AuditLoggerService = new AuditLoggingService();
            string strToValidate = string.Empty;  
            try
            {
                OrderReplacementModel orderRModel = new OrderReplacementModel();
                orderRModel.ClubcardNumber = 98765432014567;
                orderRModel.Reason = "Damage";
                strToValidate = string.Format("Order replacement - {0}::{1}", orderRModel.Reason, orderRModel.ClubcardNumber);
                logDataAudit.RecordStep(strToValidate);
            }
            catch
            { }
            finally
            {
                _AuditLoggerService.Submit(logDataAudit);
            }
            return strToValidate; 
        }

        [TearDown]
        public void TestCleanup()
        {
            replacementController.Dispose();
            replacementController = null;
            _orderAReplacementProvider= null;
        }

    }
}
