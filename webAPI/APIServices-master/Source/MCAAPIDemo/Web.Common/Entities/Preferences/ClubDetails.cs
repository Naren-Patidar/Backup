using System;
using System.Collections.Generic;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.Preferences
{
    [Serializable]
    public class ClubDetails
    {
        private string _dateOfBirth = string.Empty;
        private string _originalDateOfBirth = string.Empty;
        private int _mediaID;
        private int _clubID;
        private string _membershipID = string.Empty;
        private string _isDeleted = string.Empty;
        List<ClubDetails> _ClubInformation = new List<ClubDetails>();
        List<ClubDetails> _MediaDetails = new List<ClubDetails>();
        List<ClubDetails> _DOBDetails = new List<ClubDetails>();
        bool _isSelected = false;
        short _PreferenceID = 0;
        string _UserID;
        DateTime _JoinDate;


        public short PreferenceID
        {
            get { return _PreferenceID; }
            set { _PreferenceID = value; }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; }
        }

        public List<ClubDetails> ClubInformation
        {
            get { return _ClubInformation; }
            set { _ClubInformation = value; }
        }

        public List<ClubDetails> MediaDetails
        {
            get { return _MediaDetails; }
            set { _MediaDetails = value; }
        }


        public List<ClubDetails> DOBDetails
        {
            get { return _DOBDetails; }
            set { _DOBDetails = value; }
        }

        public string IsDeleted
        {
            get { return _isDeleted; }
            set { _isDeleted = value; }
        }
        public string MembershipID
        {
            get { return _membershipID; }
            set { _membershipID = value; }
        }
        public int ClubID
        {
            get { return _clubID; }
            set { _clubID = value; }
        }

        public int MediaID
        {
            get { return _mediaID; }
            set { _mediaID = value; }
        }
        public string OriginalDateOfBirth
        {
            get { return _originalDateOfBirth; }
            set { _originalDateOfBirth = value; }
        }
        public string DateOfBirth
        {
            get { return _dateOfBirth; }
            set { _dateOfBirth = value; }
        }

        public string UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }


        public DateTime JoinDate
        {
            get { return _JoinDate; }
            set { _JoinDate = value; }
        }
    }

    public class Media
    {
        private int _mediaID;
        private string _mediaDescription;
        public int MediaID
        {
            get { return _mediaID; }
            set { _mediaID = value; }
        }
        public string MediaDescription
        {
            get { return _mediaDescription; }
            set { _mediaDescription = value; }
        }
    }
}
