using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Models
{
    public class OptionsAndBenefitsModel
    {
        CustomerPreference _Preference = new CustomerPreference();
        ClubDetails _ChristmasSaversClubDetails = new ClubDetails { PreferenceID = (short)PreferenceEnum.Xmas_Saver };
        ClubDetails _AviosClubDetails = new ClubDetails { ClubID = BusinessConstants.CLUB_AVIOS, PreferenceID = (short)PreferenceEnum.Airmiles_Standard };
        ClubDetails _BAMilesClubDetails = new ClubDetails { ClubID = BusinessConstants.CLUB_BA, PreferenceID = (short)PreferenceEnum.BA_Miles_Standard };
        ClubDetails _VirgnClubDetails = new ClubDetails { ClubID = BusinessConstants.CLUB_VIRGIN, PreferenceID = (short)PreferenceEnum.Virgin_Atlantic };
        short _SelectedPreferenceID = 0;
        short _PreviousSelectedPreferenceID = 0;
        string _CustomerEmail = string.Empty;
        bool _IsSaved = false;
        bool _IsValid = true;

        
        public short SelectedPreferenceID
        {
            get { return _SelectedPreferenceID; }
            set { _SelectedPreferenceID = value; }
        }

        public CustomerPreference Preference
        {
            get { return _Preference; }
            set { _Preference = value; }
        }

        public List<PreferenceEnum> OptedPreferences
        {
            get 
            {
                return Preference.Preference.Where(x => x.POptStatus == OptStatus.OPTED_IN).Select(x => (PreferenceEnum)x.PreferenceID).ToList();
            }
        }

        public short PreviousSelectedPreferenceID
        {
            get { return _PreviousSelectedPreferenceID; }
            set { _PreviousSelectedPreferenceID = value; }
        }

        public string CustomerEmail
        {
            get { return _CustomerEmail; }
            set { _CustomerEmail = value; }
        }

        public ClubDetails ChristmasSaversClubDetails
        {
            get { return _ChristmasSaversClubDetails; }
            set { _ChristmasSaversClubDetails = value; }
        }

        public ClubDetails AviosClubDetails
        {
            get { return _AviosClubDetails; }
            set { _AviosClubDetails = value; }
        }
        public ClubDetails BAMilesClubDetails
        {
            get { return _BAMilesClubDetails; }
            set { _BAMilesClubDetails = value; }
        }

        public ClubDetails VirgnClubDetails
        {
            get { return _VirgnClubDetails; }
            set { _VirgnClubDetails = value; }
        }

        /// <summary>
        /// Flag to check that Model is saved in db successfully
        /// </summary>
        public bool IsSaved
        {
            get { return _IsSaved; }
            set { _IsSaved = value; }
        }

        /// <summary>
        /// Flag to chek if Model has valid input data
        /// </summary>
        public bool IsValid
        {
            get { return _IsValid; }
            set { _IsValid = value; }
        }

        public bool IsXmasSaverVisible { get; set; }
        public bool IsAviosStandardVisisble { get; set; }
        public bool IsAviosPremiumVisisble { get; set; }
        public bool IsVirginMilesVisisble { get; set; }
        public bool IsBAMilesStandardVisisble { get; set; }
        public bool IsBAMilesPremiumVisisble { get; set; }
        public bool IsEmailStatementVisible { get; set; }

        public bool IsXmasSaverOpted { get; set; }
        public bool IsAviosStandardOpted { get; set; }
        public bool IsAviosPremiumOpted { get; set; }
        public bool IsVirginMilesOpted { get; set; }
        public bool IsBAMilesStandardOpted { get; set; }
        public bool IsBAMilesPremiumOpted { get; set; }        
    }
}
