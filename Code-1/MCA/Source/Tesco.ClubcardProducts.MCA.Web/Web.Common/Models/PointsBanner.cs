using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.ConfigurationProvider;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.DBConfiguration;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Models
{
    public class PointsBanner
    {
        PointsBannerStage stage = PointsBannerStage.None;
        CustomerPointsRange _PreviousPointsRange = CustomerPointsRange.None;

        
        DbConfigurationItem pointsDates;
        double NoOfDaysLeftForConversionMesg;
        double _warningDays = 0;
        double _CurrentPoints = 0;
        double _CurrentPointTarget = 0;
        double _CurrentPointStartTarget = 0;        
        string _CurrentVoucherTarget = string.Empty;
        string _CutoffEnd = string.Empty;
        string _PreviousPoints = string.Empty;        
        string _PreviousVouchers = string.Empty;
        string _PreviousCollectionStart = string.Empty;
        string _PreviousCollectionEnd = string.Empty;
        string _CurrentCollectionStart = string.Empty;
        string _CurrentCollectionEnd = string.Empty;        
        string _statementMonth = string.Empty;
        bool _IsClickable = false;
        int _optedSchemeID = 0;

        public VoucherSchemeName OptedVoucherScheme { get; set; }
        public bool HidePointsConversionStrip { get; set; }
                
        public PointsBanner(DbConfigurationItem _pointsDates, double _NoOfDaysLeftForConversionMesg)
        {
            this.pointsDates = _pointsDates;
            this.NoOfDaysLeftForConversionMesg = _NoOfDaysLeftForConversionMesg;
            stage = DetermineMailingStage();
        }

        public CustomerPointsRange PreviousPointsRange
        {
            get { return _PreviousPointsRange; }
            set { _PreviousPointsRange = value; }
        }

        public string PreviousCollectionStart
        {
            get { return _PreviousCollectionStart; }
            set { _PreviousCollectionStart = value; }
        }

        public string PreviousCollectionEnd
        {
            get { return _PreviousCollectionEnd; }
            set { _PreviousCollectionEnd = value; }
        }

        public string CurrentCollectionStart
        {
            get { return _CurrentCollectionStart; }
            set { _CurrentCollectionStart = value; }
        }

        public string CurrentCollectionEnd
        {
            get { return _CurrentCollectionEnd; }
            set { _CurrentCollectionEnd = value; }
        }

        public string PreviousPoints
        {
            get { return _PreviousPoints; }
            set { _PreviousPoints = value; }
        }

        public string PreviousVouchers
        {
            get { return _PreviousVouchers; }
            set { _PreviousVouchers = value; }
        }

        public PointsBannerStage Stage
        {
            get { return stage; }
            set { stage = value; }
        }

        public double WarningDays
        {
            get{ return _warningDays; }
            set { _warningDays = value; }
        }

        public double CurrentPoints
        {
            get { return _CurrentPoints; }
            set { _CurrentPoints = value; }
        }

        public double CurrentPointStartTarget
        {
            get { return _CurrentPointStartTarget; }
            set { _CurrentPointStartTarget = value; }
        }

        public double CurrentPointTarget
        {
            get { return _CurrentPointTarget; }
            set { _CurrentPointTarget = value; }
        }

        public string CurrentVoucherTarget
        {
            get { return _CurrentVoucherTarget; }
            set { _CurrentVoucherTarget = value; }
        }

        public string CutoffEnd
        {
            get { return _CutoffEnd; }
            set { _CutoffEnd = value; }
        }

        public string StatementMonth
        {
            get { return _statementMonth; }
            set { _statementMonth = value; }
        }


        public bool IsClickable
        {
            get { return _IsClickable; }
            set { _IsClickable = value; }
        }

        public int OptedSchemeID
        {
            get { return _optedSchemeID; }
            set { _optedSchemeID = value; }
        }

        public string OptedPreference { get; set; }

        PointsBannerStage DetermineMailingStage()
        {
            PointsBannerStage _stage = PointsBannerStage.None;
            
            DateTime cutoffStart, cutoffEnd, today = System.DateTime.Now;

            if (pointsDates.ConfigurationValue1.TryParseDate(out cutoffStart)
                && pointsDates.ConfigurationValue2.TryParseDate(out cutoffEnd))
            {
                if ((cutoffStart - today).TotalDays > NoOfDaysLeftForConversionMesg)
                {
                    _stage = PointsBannerStage.PrePointsCutoff;
                }
                else if ((cutoffStart - today).TotalDays > 0 && (cutoffStart - today).TotalDays <= NoOfDaysLeftForConversionMesg)
                {
                    _stage = PointsBannerStage.PrePointsCutoffWarning;
                }
                else if ((cutoffStart - today).TotalDays < 0 && (cutoffEnd - today).TotalDays >= 0)
                {
                    _stage = PointsBannerStage.PointsCutoff;
                }
                else if ((cutoffStart - today).TotalDays < 0 && (cutoffEnd - today).TotalDays < 0)
                {
                    _stage = PointsBannerStage.PostPointsCutoff;
                }
                _warningDays = (cutoffStart - today).TotalDays;
                _CutoffEnd = cutoffEnd.AddDays(1).FormatDateDDthMMM();
            }
            return _stage;
        }                
    }

    public enum PointsBannerStage
    {
        None,
        PrePointsCutoff,
        PrePointsCutoffWarning,
        PointsCutoff,
        PostPointsCutoff
    }

    public enum CustomerPointsRange
    {
        None,        
        ZeroPoints,
        InSufficientPoints,
        SufficientPoints
    }
}
