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

namespace Web.Tests
{
    [TestFixture]
    public class HomeControllerTests
    {
        private HomeController homeController;

        [SetUp]
        public void SetUp()
        {
            homeController = new HomeController();
        }

        [TestCase]
        public void Index_Execution_Successful()
        {
           // var viewResult = homeController.Home() as ViewResult;
           // Assert.IsNull(viewResult.Model);
        }

        [TearDown]
        public void TestCleanup()
        {
            homeController.Dispose();
            homeController = null;
        }
    }
}
