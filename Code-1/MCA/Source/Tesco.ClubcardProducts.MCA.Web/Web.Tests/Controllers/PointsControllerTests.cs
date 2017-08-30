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

namespace Web.Tests
{
    [TestFixture]
    public class PointsControllerTests
    {
        private PointsController pointsController;
        private IConfigurationProvider _configurationMock;
        private IPointsBC _pointsProvider;

        [SetUp]
        public void SetUp()
        {
            _pointsProvider = MockRepository.GenerateMock<IPointsBC>();
            _configurationMock = MockRepository.GenerateMock<IConfigurationProvider>();
            pointsController = new PointsController(_pointsProvider, _configurationMock);
        }

        [TestCase]
        public void Index_Execution_Successful()
        {
            var viewResult = pointsController.Index() as ViewResult;
            Assert.IsNull(viewResult.Model);
        }

        [TestCase]
        public void Home_Execution_Successful()
        {
            PointsViewModel pointsViewModel = new PointsViewModel();
            List<Offer> offersList = new List<Offer>();
            var offer = new Offer
            {
                Period = "Current",
                EndDateTime = DateTime.Now,
                StartDateTime = DateTime.Now,
                PointsBalanceQty = "20",
                Vouchers="80"
            };
            offersList.Add(offer);
            offer = new Offer
            {
                Period = "Aug 2015",
                EndDateTime = DateTime.Now,
                StartDateTime = DateTime.Now,
                PointsBalanceQty = "20",
                Vouchers = "80"
            };
            offersList.Add(offer);
            offer = new Offer
            {
                Period = "May 2015",
                EndDateTime = DateTime.Now,
                StartDateTime = DateTime.Now,
                PointsBalanceQty = "20",
                Vouchers = "80"
            };
            offersList.Add(offer);
            pointsViewModel.Offers = offersList;
            pointsViewModel.IsPointEarnedEver = true;            
            _pointsProvider.Stub(x => x.GetPointsViewdetails(1, "en-GB")).IgnoreArguments().Return(pointsViewModel);
            _configurationMock.Stub(x => x.GetStringAppSetting(AppConfigEnum.Culture)).Return("en-GB");
            _configurationMock.Stub(x => x.GetStringAppSetting(AppConfigEnum.DisplayDateFormat)).Return("dd/mm/yyyy");
            _configurationMock.Stub(x => x.GetBoolAppSetting(AppConfigEnum.DisableCurrencyDecimal)).Return(false);
            var viewResult = pointsController.Home() as ViewResult;
            Assert.AreEqual(3, pointsViewModel.Offers.Count);
        }

        [TearDown]
        public void TestCleanup()
        {
            pointsController.Dispose();
            pointsController = null;
            _pointsProvider = null;
        }
    }
}
