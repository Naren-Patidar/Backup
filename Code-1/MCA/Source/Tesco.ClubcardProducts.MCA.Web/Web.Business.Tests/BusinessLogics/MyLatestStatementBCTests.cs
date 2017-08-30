using NUnit.Framework;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Tesco.ClubcardProducts.MCA.Web.Business.Contracts;
using Rhino.Mocks;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Tesco.ClubcardProducts.MCA.Web.Business.BusinessLogics;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using System.Collections.Generic;
using System;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Points;
using Tesco.ClubcardProducts.MCA.Web.Common.Models;

namespace Web.Business.Tests
{
    [TestFixture]
    public class MyLatestStatementBCTests
    {
        protected IServiceAdapter _clubcardServiceAdapter;
        private IPDFGenerator _generatePDF;
        private IConfigurationProvider _configProvider = null;
        private IAccountBC _accountProvider;
        private IPointsBC _pointsProvider;
        private ILoggingService _logger;
        private MyLatestStatementBC myLatestStatementBC;

        [SetUp]
        public void Setup()
        {
            _configProvider = MockRepository.GenerateMock<IConfigurationProvider>();
            _accountProvider = MockRepository.GenerateMock<IAccountBC>();
            _pointsProvider = MockRepository.GenerateMock<IPointsBC>();
            _clubcardServiceAdapter = MockRepository.GenerateMock<IServiceAdapter>();
            _logger = MockRepository.GenerateMock<ILoggingService>();

            myLatestStatementBC = new MyLatestStatementBC(
                                        _accountProvider, 
                                        _pointsProvider, 
                                        _clubcardServiceAdapter,
                                        _logger, 
                                        _configProvider);
        }

        [TestCase]
        public void GetMLSViewDetails_Execution_Successful()
        {
            List<Offer> offersList = new List<Offer>();
            var offer = new Offer
            {
                Period = "Current",
                EndDateTime = new DateTime(2016, 09, 23),
                StartDateTime = new DateTime(2015, 05, 08)
            };
            offersList.Add(offer);
            offer = new Offer
            {
                Period = "May 2015",
                Id = 21,
                EndDateTime = new DateTime(2015, 01, 23),
                StartDateTime = new DateTime(2015, 05, 07)
            };
            offersList.Add(offer);
            offer = new Offer
            {
                Period = "February 2015",
                EndDateTime = new DateTime(2015, 01, 22),
                StartDateTime = new DateTime(2014, 10, 17)
            };
            offersList.Add(offer);
            _pointsProvider.Stub(x => x.GetOffersForCustomer(1, "en-GB")).IgnoreArguments().Return(offersList);
            _configProvider.Stub(x => x.GetStringAppSetting(AppConfigEnum.DisplayDateFormat)).Return("dd/MM/yyyy");

            DbConfiguration dbConfig = getMLSDbConfigurations();
            _accountProvider.Stub(x => x.GetDBConfigurations(new List<DbConfigurationTypeEnum>() { DbConfigurationTypeEnum.Holding_dates }, "en-GB")).IgnoreArguments().Return(dbConfig);
            DbConfiguration dbPointsConfig = getPointsDbConfiguration();
            _accountProvider.Stub(x => x.GetDBConfigurations(new List<DbConfigurationTypeEnum>() { DbConfigurationTypeEnum.Holding_dates }, "en-GB")).IgnoreArguments().Return(dbPointsConfig);

            List<PointsSummary> pointsSummaryList = new List<PointsSummary>();
            PointsSummary pointsSummary = new PointsSummary
            {
                BonusVouchers = "0.00",
                CustomerType = "A",
                EndDateTime = "07/05/2015",
                MainClubcardID = "634004024007890751",
                NoCoupons = "0",
                PointSummaryDescEnglish = "Reward",
                RewardMilesRate = null,
                Salutation = "Dear Clubcard Customer",
                StatementType = null,
                StatementVideo = null,
                TopUpVouchers = "0.00",
                TotalPoints = "200",
                TotalReward = "2.00",
                TotalRewardMiles = "0"
            };

            foreach (string pointsTypeLiteral in Enum.GetNames(typeof(PointsTypeEnum)))
            {
                PointsTypeEnum pointsType = (PointsTypeEnum)Enum.Parse(typeof(PointsTypeEnum), pointsTypeLiteral);

                pointsSummary.pointsBreakup.Add(pointsType, 233);
            }

            foreach (string rewardsTypeLiteral in Enum.GetNames(typeof(RewardsTypeEnum)))
            {
                RewardsTypeEnum pointsType = (RewardsTypeEnum)Enum.Parse(typeof(RewardsTypeEnum), rewardsTypeLiteral);
                pointsSummary.rewardsBreakup.Add(pointsType, 23);
            }
            pointsSummaryList.Add(pointsSummary);

            MyLatestStatementViewModel myLatestStatementViewModel = new MyLatestStatementViewModel();
            myLatestStatementViewModel.prevOfferId = 21;
            myLatestStatementViewModel.pointsPrevEndDate = new DateTime(2015, 01, 23);
            myLatestStatementViewModel.pointsPrevStartDate = new DateTime(2015, 05, 07);
            myLatestStatementViewModel.isHoldingPageEnabled = false;
            myLatestStatementViewModel.pointsSummary = pointsSummaryList[0];
            myLatestStatementViewModel.dMiles = 0;
            myLatestStatementViewModel.voucherPerMileFifty = "50";


            _pointsProvider.Stub(x => x.GetPointsSummary(1, 2, "en-GB")).IgnoreArguments().Return(pointsSummaryList[0]);
            MyLatestStatementViewModel mlsModel = myLatestStatementBC.GetMLSViewDetails(1);
            
            Assert.AreEqual(myLatestStatementViewModel, mlsModel);
        }

        private DbConfiguration getMLSDbConfigurations()
        {
            DbConfiguration dbConfiguration = new DbConfiguration();
            DbConfigurationItem dbConfigItem = new DbConfigurationItem();
            dbConfigItem.ConfigurationName = DbConfigurationItemNames.MyLatestStatementDates;
            dbConfigItem.ConfigurationValue1 = "Jan 15 2016 12:00AM";
            dbConfigItem.ConfigurationValue2 = "Feb 1 2016 11:59PM";
            dbConfigItem.ConfigurationType = DbConfigurationTypeEnum.Holding_dates;

            List<DbConfigurationItem> lstDbConfigurationItems = new List<DbConfigurationItem>();
            lstDbConfigurationItems.Add(dbConfigItem);
            dbConfiguration.ConfigurationItems = lstDbConfigurationItems;

            return dbConfiguration;
        }

        private DbConfiguration getPointsDbConfiguration()
        {
            DbConfiguration dbConfigurationPoints = new DbConfiguration();
            DbConfigurationItem dbConfigPointsItem = new DbConfigurationItem();
            dbConfigPointsItem.ConfigurationName = DbConfigurationItemNames.PtsSummaryDates;
            dbConfigPointsItem.ConfigurationValue1 = DateTime.Now.AddDays(-1).ToString();
            dbConfigPointsItem.ConfigurationValue2 = DateTime.Now.AddDays(-4).ToString();
            dbConfigPointsItem.ConfigurationType = DbConfigurationTypeEnum.Holding_dates;
            List<DbConfigurationItem> lstDbConfigurationItems = new List<DbConfigurationItem>();
            lstDbConfigurationItems.Add(dbConfigPointsItem);
            dbConfigurationPoints.ConfigurationItems = lstDbConfigurationItems;

            return dbConfigurationPoints;
        }
    }
}