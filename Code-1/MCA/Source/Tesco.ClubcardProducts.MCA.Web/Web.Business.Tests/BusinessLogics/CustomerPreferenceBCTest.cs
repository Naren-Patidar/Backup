using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
using Rhino.Mocks;
using Tesco.ClubcardProducts.MCA.Web.Business.BusinessLogics;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.ClubcardCouponServices;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Ecoupon;
using Tesco.ClubcardProducts.MCA.Web.Common.Logger;
using Customer = Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.CustomerService;
using System.Data;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.PreferenceServices;
using Tesco.ClubcardProducts.MCA.Web.Common.Models;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences;

namespace Web.Business.Tests.BusinessLogics
{
    [TestFixture]
    public class CustomerPreferenceBCTest
    {
        IPreferenceService _preferenceServiceClient;
        PreferenceServiceAdapter _preferenceServiceAdapter;
        private ILoggingService _Logger;
        private IConfigurationProvider _Config;
        CustomerPreferenceBC preferenceBC;


        [SetUp]
        public void SetUp()
        {
            _preferenceServiceClient = MockRepository.GenerateMock<IPreferenceService>();
            _Logger = MockRepository.GenerateMock<ILoggingService>();
            _Config = MockRepository.GenerateMock<IConfigurationProvider>();

            _preferenceServiceAdapter = new PreferenceServiceAdapter(_preferenceServiceClient, _Logger);
            preferenceBC = new CustomerPreferenceBC(_preferenceServiceAdapter, _Config, _Logger);
        }

        [TestCase]
        public void Validate_GetClubDetails()
        {
            Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.PreferenceServices.ClubDetails clubs = GetServiceClubDetails();
            _preferenceServiceClient.Stub(x => x.ViewClubDetails(0)).IgnoreArguments().Return(clubs);
            Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences.ClubDetails expectedClubs = GetExpectedClubDetails();
            Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences.ClubDetails actualClubs = preferenceBC.GetClubDetails(0);

            Assert.IsTrue(actualClubs.ClubInformation.SequenceEqual(expectedClubs.ClubInformation));
        }        

        [TestCase]
        public void Validate_GetSelectedStatementPreference()
        { 
            OptionsAndBenefitsModel model = GetValidModel(PreferenceEnum.Xmas_Saver);
            Int16 actualID = preferenceBC.GetSelectedStatementPreference(model.Preference.Preference.ToList());
            Assert.IsTrue(((short)PreferenceEnum.Xmas_Saver).Equals(actualID));
        }

        [TestCase]
        public void Validate_UpdateCustomerPreferences_Xmas_Saver()
        {
            _preferenceServiceClient.Stub(x => x.MaintainClubDetails(0, new Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.PreferenceServices.ClubDetails(), string.Empty)).IgnoreArguments();
            _preferenceServiceClient.Stub(x => x.MaintainCustomerPreference(0, new Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.PreferenceServices.CustomerPreference(), new CustomerDetails())).IgnoreArguments();

            OptionsAndBenefitsModel model = GetValidModel(PreferenceEnum.Xmas_Saver);
            AccountDetails account = GetAccountDetails();
            bool res = preferenceBC.UpdateCustomerPreferences(model, account, "123");

            Assert.IsTrue(res);
        }

        [TestCase]
        public void Validate_UpdateCustomerPreferences_Airmiles_Standard()
        {
            _preferenceServiceClient.Stub(x => x.MaintainClubDetails(0, new Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.PreferenceServices.ClubDetails(), string.Empty)).IgnoreArguments();
            _preferenceServiceClient.Stub(x => x.MaintainCustomerPreference(0, new Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.PreferenceServices.CustomerPreference(), new CustomerDetails())).IgnoreArguments();

            OptionsAndBenefitsModel model = GetValidModel(PreferenceEnum.Airmiles_Standard);
            AccountDetails account = GetAccountDetails();
            bool res = preferenceBC.UpdateCustomerPreferences(model, account, "123");

            Assert.IsTrue(res);
        }

        [TestCase]
        public void Validate_UpdateCustomerPreferences_Virgin_Atlantic()
        {
            _preferenceServiceClient.Stub(x => x.MaintainClubDetails(0, new Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.PreferenceServices.ClubDetails(), string.Empty)).IgnoreArguments();
            _preferenceServiceClient.Stub(x => x.MaintainCustomerPreference(0, new Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.PreferenceServices.CustomerPreference(), new CustomerDetails())).IgnoreArguments();

            OptionsAndBenefitsModel model = GetValidModel(PreferenceEnum.Virgin_Atlantic);
            AccountDetails account = GetAccountDetails();
            bool res = preferenceBC.UpdateCustomerPreferences(model, account, "123");

            Assert.IsTrue(res);
        }

        [TestCase]
        public void Validate_UpdateCustomerPreferences_BA_Miles_Standard()
        {
            _preferenceServiceClient.Stub(x => x.MaintainClubDetails(0, new Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.PreferenceServices.ClubDetails(), string.Empty)).IgnoreArguments();
            _preferenceServiceClient.Stub(x => x.MaintainCustomerPreference(0, new Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.PreferenceServices.CustomerPreference(), new CustomerDetails())).IgnoreArguments();

            OptionsAndBenefitsModel model = GetValidModel(PreferenceEnum.BA_Miles_Standard);
            AccountDetails account = GetAccountDetails();
            bool res = preferenceBC.UpdateCustomerPreferences(model, account, "123");

            Assert.IsTrue(res);
        }
        

        [TestCase]
        public void Validate_UpdateClubDetails()
        { 
            _preferenceServiceClient.Stub(x => x.MaintainClubDetails(0, new Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.PreferenceServices.ClubDetails(),string.Empty)).IgnoreArguments();
            OptionsAndBenefitsModel model = GetValidModel(PreferenceEnum.Xmas_Saver);
            Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences.ClubDetails clubs = new Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences.ClubDetails();
            clubs.ClubInformation.Add(model.AviosClubDetails);
            clubs.ClubInformation.Add(model.VirgnClubDetails);
            clubs.ClubInformation.Add(model.BAMilesClubDetails);

            bool res = preferenceBC.UpdateClubDetails(0, clubs, string.Empty);
            Assert.IsTrue(res);
        }

        [TestCase]
        public void Validate_ValidateCustomerPreferences()
        {
            List<bool> expectedresults = new List<bool> { true, false, false, true, false, false };
            List<bool> actualResults = new List<bool>();
            List<PreferenceEnum> preferences = new List<PreferenceEnum> { PreferenceEnum.Virgin_Atlantic, PreferenceEnum.BA_Miles_Standard };
            OptionsAndBenefitsModel model;
            bool res;
            foreach (PreferenceEnum preference in preferences)
            {
                model = GetValidModel(preference);
                res = preferenceBC.ValidateCustomerPreferences(model);
                actualResults.Add(res);
                model = GetInValidModel_AlphaNumeric(preference);
                res = preferenceBC.ValidateCustomerPreferences(model);
                actualResults.Add(res);
                model = GetInValidModel_WronLength(preference);
                res = preferenceBC.ValidateCustomerPreferences(model);
                actualResults.Add(res);
            }
            Assert.IsTrue(actualResults.SequenceEqual(expectedresults));
        }

        [TestCase]
        public void Validate_GetTabId()
        {
            List<Int32> expectedIds = new List<int> { 0, 1, 1, 2, 3, 3 };
            List<Int32> actualIds = new List<int>();
            List<PreferenceEnum> preferences = new List<PreferenceEnum> { PreferenceEnum.Xmas_Saver, PreferenceEnum.Airmiles_Standard, PreferenceEnum.Airmiles_Premium,
                                                                           PreferenceEnum.Virgin_Atlantic, PreferenceEnum.BA_Miles_Standard, PreferenceEnum.BA_Miles_Premium};
            foreach (PreferenceEnum preference in preferences)
            {
                OptionsAndBenefitsModel model = GetValidModel(preference);
                int id = preferenceBC.GetTabId(model);
                actualIds.Add(id);
            }

            Assert.IsTrue(actualIds.SequenceEqual(expectedIds));
        }


        #region Private Methods

        private OptionsAndBenefitsModel GetValidModel(PreferenceEnum selected)
        {
            OptionsAndBenefitsModel model = new OptionsAndBenefitsModel();
            model.Preference = new Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common.CustomerPreference();
            model.Preference.Preference = GetCustomerPreferences().ToArray();
            model.Preference.Preference.ToList().Find(p => p.PreferenceID == (short)selected).POptStatus = Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common.OptStatus.OPTED_IN;
            model.SelectedPreferenceID = (Int16)selected;
            switch (selected)
            {
                case PreferenceEnum.Virgin_Atlantic:
                    model.VirgnClubDetails = new Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences.ClubDetails { ClubID = (Int16)selected, MembershipID = "01234567891" , IsDeleted = "N", JoinDate = DateTime.Now };                    
                    break;
                case PreferenceEnum.BA_Miles_Standard:
                    model.VirgnClubDetails = new Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences.ClubDetails { ClubID = (Int16)selected, MembershipID = "01234567", IsDeleted = "N", JoinDate = DateTime.Now };
                    break;
                case PreferenceEnum.Airmiles_Standard:
                    model.AviosClubDetails = new Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences.ClubDetails { ClubID = (Int16)selected, IsDeleted = "N", JoinDate = DateTime.Now };
                    break;
                case PreferenceEnum.Xmas_Saver:
                    model.VirgnClubDetails = new Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences.ClubDetails { ClubID = (Int16)selected, IsDeleted = "N", JoinDate = DateTime.Now  };
                    break;
            }
            return model;
        }

        private OptionsAndBenefitsModel GetInValidModel_AlphaNumeric(PreferenceEnum selected)
        {
            OptionsAndBenefitsModel model = new OptionsAndBenefitsModel();
            model.Preference = new Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common.CustomerPreference();
            model.Preference.Preference = GetCustomerPreferences().ToArray();
            model.Preference.Preference.ToList().Find(p => p.PreferenceID == (short)selected).POptStatus = Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common.OptStatus.OPTED_IN;           
            model.SelectedPreferenceID = (Int16)selected;
            switch (selected)
            {
                case PreferenceEnum.Virgin_Atlantic:
                    model.VirgnClubDetails = new Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences.ClubDetails { ClubID = (Int16)selected, MembershipID = "012abc67891" };
                    break;
                case PreferenceEnum.BA_Miles_Standard:
                    model.VirgnClubDetails = new Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences.ClubDetails { ClubID = (Int16)selected, MembershipID = "01xyz567" };
                    break;
                case PreferenceEnum.Airmiles_Standard:
                    model.AviosClubDetails = new Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences.ClubDetails { ClubID = (Int16)selected, IsDeleted = "N", JoinDate = DateTime.Now };
                    break;
                case PreferenceEnum.Xmas_Saver:
                    model.VirgnClubDetails = new Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences.ClubDetails { ClubID = (Int16)selected, IsDeleted = "N", JoinDate = DateTime.Now };
                    break;
            }
            return model;
        }

        private OptionsAndBenefitsModel GetInValidModel_WronLength(PreferenceEnum selected)
        {
            OptionsAndBenefitsModel model = new OptionsAndBenefitsModel();
            model.Preference = new Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common.CustomerPreference();
            model.Preference.Preference = GetCustomerPreferences().ToArray();
            model.Preference.Preference.ToList().Find(p => p.PreferenceID == (short)selected).POptStatus = Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common.OptStatus.OPTED_IN;           
            model.SelectedPreferenceID = (Int16)selected;
            switch (selected)
            {
                case PreferenceEnum.Virgin_Atlantic:
                    model.VirgnClubDetails = new Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences.ClubDetails { ClubID = (Int16)selected, MembershipID = "012345891" };
                    break;
                case PreferenceEnum.BA_Miles_Standard:
                    model.VirgnClubDetails = new Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences.ClubDetails { ClubID = (Int16)selected, MembershipID = "02347" };
                    break;
                case PreferenceEnum.Airmiles_Standard:
                    model.AviosClubDetails = new Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences.ClubDetails { ClubID = (Int16)selected, IsDeleted = "N", JoinDate = DateTime.Now };
                    break;
                case PreferenceEnum.Xmas_Saver:
                    model.VirgnClubDetails = new Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences.ClubDetails { ClubID = (Int16)selected, IsDeleted = "N", JoinDate = DateTime.Now };
                    break;
            }
            return model;
        }

        private AccountDetails GetAccountDetails()
        {
            AccountDetails account = new AccountDetails();
            account.TitleEnglish = "Mr";
            account.Name1 = "Goel";
            account.Name3 = "Abhishek";            
            return account;
        }

        private Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.PreferenceServices.ClubDetails GetServiceClubDetails()
        {
            Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.PreferenceServices.ClubDetails clubs = new Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.PreferenceServices.ClubDetails();
            clubs.ClubInformation = new Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.PreferenceServices.ClubDetails[10];
            clubs.ClubInformation[0] = new Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.PreferenceServices.ClubDetails { ClubID = BusinessConstants.CLUB_BA, IsDeleted = "Y", JoinDate = DateTime.Now };
            clubs.ClubInformation[1] = new Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.PreferenceServices.ClubDetails { ClubID = BusinessConstants.CLUB_BA, IsDeleted = "Y", JoinDate = DateTime.Now };
            clubs.ClubInformation[2] = new Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.PreferenceServices.ClubDetails { ClubID = BusinessConstants.CLUB_BA, IsDeleted = "Y", JoinDate = DateTime.Now };
            clubs.ClubInformation[3] = new Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.PreferenceServices.ClubDetails { ClubID = BusinessConstants.CLUB_VIRGIN, IsDeleted = "Y", JoinDate = DateTime.Now };
            clubs.ClubInformation[4] = new Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.PreferenceServices.ClubDetails { ClubID = BusinessConstants.CLUB_VIRGIN, IsDeleted = "Y", JoinDate = DateTime.Now };
            clubs.ClubInformation[5] = new Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.PreferenceServices.ClubDetails { ClubID = BusinessConstants.CLUB_BA, IsDeleted = "Y", JoinDate = DateTime.Now };
            clubs.ClubInformation[6] = new Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.PreferenceServices.ClubDetails { ClubID = BusinessConstants.CLUB_BA, IsDeleted = "Y", JoinDate = DateTime.Now };
            clubs.ClubInformation[7] = new Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.PreferenceServices.ClubDetails { ClubID = BusinessConstants.CLUB_BA, IsDeleted = "Y", JoinDate = DateTime.Now };
            clubs.ClubInformation[8] = new Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.PreferenceServices.ClubDetails { ClubID = BusinessConstants.CLUB_BA, IsDeleted = "Y", JoinDate = DateTime.Now };
            clubs.ClubInformation[9] = new Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.PreferenceServices.ClubDetails { ClubID = BusinessConstants.CLUB_BA, IsDeleted = "N", JoinDate = DateTime.Now, MembershipID = "123456" };
            return clubs;
        }

        private Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences.ClubDetails GetExpectedClubDetails()
        {
            Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences.ClubDetails clubs = new Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences.ClubDetails();
            clubs.ClubInformation = new List<Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences.ClubDetails>();
            clubs.ClubInformation.Add(new Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences.ClubDetails { ClubID = BusinessConstants.CLUB_BA, IsDeleted = "Y", JoinDate = DateTime.Now });
            clubs.ClubInformation.Add(new Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences.ClubDetails { ClubID = BusinessConstants.CLUB_BA, IsDeleted = "Y", JoinDate = DateTime.Now });
            clubs.ClubInformation.Add(new Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences.ClubDetails { ClubID = BusinessConstants.CLUB_BA, IsDeleted = "Y", JoinDate = DateTime.Now });
            clubs.ClubInformation.Add(new Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences.ClubDetails { ClubID = BusinessConstants.CLUB_VIRGIN, IsDeleted = "Y", JoinDate = DateTime.Now });
            clubs.ClubInformation.Add(new Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences.ClubDetails { ClubID = BusinessConstants.CLUB_VIRGIN, IsDeleted = "Y", JoinDate = DateTime.Now });
            clubs.ClubInformation.Add(new Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences.ClubDetails { ClubID = BusinessConstants.CLUB_BA, IsDeleted = "Y", JoinDate = DateTime.Now });
            clubs.ClubInformation.Add(new Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences.ClubDetails { ClubID = BusinessConstants.CLUB_BA, IsDeleted = "Y", JoinDate = DateTime.Now });
            clubs.ClubInformation.Add(new Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences.ClubDetails { ClubID = BusinessConstants.CLUB_BA, IsDeleted = "Y", JoinDate = DateTime.Now });
            clubs.ClubInformation.Add(new Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences.ClubDetails { ClubID = BusinessConstants.CLUB_BA, IsDeleted = "Y", JoinDate = DateTime.Now });
            clubs.ClubInformation.Add(new Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences.ClubDetails { ClubID = BusinessConstants.CLUB_BA, IsDeleted = "Y", JoinDate = DateTime.Now });
            clubs.ClubInformation.Add(new Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences.ClubDetails { ClubID = BusinessConstants.CLUB_BA, IsDeleted = "N", JoinDate = DateTime.Now, MembershipID = "123456" });
            return clubs;
        }

        private List<Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common.CustomerPreference> GetCustomerPreferences()
        {
            List<Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common.CustomerPreference> preferences = new List<Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common.CustomerPreference>();
            preferences.Add(new Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common.CustomerPreference { PreferenceID = (short)PreferenceEnum.Xmas_Saver, IsDeleted = "N" });
            preferences.Add(new Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common.CustomerPreference { PreferenceID = (short)PreferenceEnum.Virgin_Atlantic, IsDeleted = "N" });
            preferences.Add(new Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common.CustomerPreference { PreferenceID = (short)PreferenceEnum.Airmiles_Standard, IsDeleted = "N" });
            preferences.Add(new Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common.CustomerPreference { PreferenceID = (short)PreferenceEnum.Airmiles_Premium, IsDeleted = "N" });
            preferences.Add(new Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common.CustomerPreference { PreferenceID = (short)PreferenceEnum.BA_Miles_Premium, IsDeleted = "N" });
            preferences.Add(new Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common.CustomerPreference { PreferenceID = (short)PreferenceEnum.BA_Miles_Standard, IsDeleted = "N" });
            return preferences;
        }

        #endregion


    }
}
