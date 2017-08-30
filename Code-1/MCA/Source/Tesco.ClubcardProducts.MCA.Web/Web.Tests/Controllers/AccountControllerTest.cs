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
    public class AccountControllerTest
    {
       
        private IPDFGenerator _pdfProvider;
        private AccountController accountController;
        IConfigurationProvider _configProvider = null;
        private ILoggingService _AuditLoggerService;
        private IAccountBC _IAccountBC;

        [SetUp]
        public void SetUp()
        {
            _pdfProvider = MockRepository.GenerateMock<IPDFGenerator>();
            _configProvider = MockRepository.GenerateMock<IConfigurationProvider>();
            _AuditLoggerService = MockRepository.GenerateMock<ILoggingService>();
            _IAccountBC = MockRepository.GenerateMock<IAccountBC>();
            // _logger = MockRepository.GenerateMock<ILoggingService>();
            accountController = new AccountController(_IAccountBC, _AuditLoggerService, _configProvider);
        }

        [TestCase]
        public void SecurityDetails_Audit_Successful()
        {
            string strToCompare = StubAccountSecurityData();

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
        private string StubAccountSecurityData()
        {
            string strToCompare = string.Empty;
            LogData logDataAudit = new AuditLogData();
            _AuditLoggerService = new AuditLoggingService();
            try
            {
                strToCompare = String.Format("False|{0}",
                               new
                               {
                                   FirstDigit = String.Format("Key:{0},Value:{1}", "11th", 5),
                                   SeondDigit = String.Format("Key:{0},Value:{1}", "13th", 9),
                                   ThirdDigit = String.Format("Key:{0},Value:{1}", "14th", 2),
                                   AttemptCount = 3,
                                   IsBlocked = "No"
                               }.JsonText());
                logDataAudit.RecordStep(String.Format("False|{0}", strToCompare));
               
            }
            catch
            { }
            finally
            {
                _AuditLoggerService.Submit(logDataAudit);
            }
            return strToCompare; 
        
        }

        [TearDown]
        public void TestCleanup()
        {
            
            accountController.Dispose();
            accountController = null;
        }

    }
}
