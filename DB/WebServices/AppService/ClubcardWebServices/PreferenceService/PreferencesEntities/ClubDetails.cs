using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Tesco.com.ClubcardOnline.Entities
{
    [Serializable]
    [DataContract]
    public class ClubDetails
    {
        #region Fields

        /// <summary>
        /// <clubdetails>
        /// </summary>
        public List<ClubDetails> _clubDetails;

        /// <summary>
        /// <clubdetails>
        /// </summary>
        public List<ClubDetails> _mediaDetails;

        /// <summary>
        /// <clubdetails>
        /// </summary>
        public List<ClubDetails> _dOBDetails;

        /// <summary>
        /// ExpectedActualBirthDate
        /// </summary>
        public string ExpectedActualBirthDate;

        /// <summary>
        /// ChangedBirthDate
        /// </summary>
        public string _changedBirthDate;

        /// <summary>
        /// MediaID
        /// </summary>
        public short _mediaID;

        /// <summary>
        /// CLubID
        /// </summary>
        public short _clubID;

        /// <summary>
        /// ISDeleted
        /// </summary>
        public string _isdeleted;

        /// <summary>
        /// MediaDesc
        /// </summary>
        public string _mediaDesc;

        /// <summary>
        /// MembershipID
        /// </summary>
        public string _membershipID;

        public string _userID;

        public string _pCulture;

        public DateTime _joinDate;

        #endregion

        #region Properties

        // <summary>
        ///  Culture
        /// </summary>
        [DataMember]
        public DateTime JoinDate
        {
            get { return this._joinDate; }
            set { this._joinDate = value; }
        }

        // <summary>
        ///  Culture
        /// </summary>
        [DataMember]
        public string Culture
        {
            get { return this._pCulture; }
            set { this._pCulture = value; }
        }

        // <summary>
        ///  UserID
        /// </summary>
        [DataMember]
        public string UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }

        /// <summary>
        ///  MembershipDesc
        /// </summary>
        [DataMember]
        public string MediaDesc
        {
            get { return this._mediaDesc; }
            set { this._mediaDesc = value; }
        }

        /// <summary>
        ///  MembershipID
        /// </summary>
        [DataMember]
        public string MembershipID
        {
            get { return this._membershipID; }
            set { this._membershipID = value; }
        }

        /// <summary>
        ///  ClubID
        /// </summary>
        [DataMember]
        public string IsDeleted
        {
            get { return this._isdeleted; }
            set { this._isdeleted = value; }
        }

        /// <summary>
        ///  ClubID
        /// </summary>
        [DataMember]
        public short ClubID
        {
            get { return this._clubID; }
            set { this._clubID = value; }
        }

        /// <summary>
        ///  DateOfBirth
        /// </summary>
        [DataMember]
        public string DateOfBirth
        {
            get { return this.ExpectedActualBirthDate; }
            set { this.ExpectedActualBirthDate = value; }
        }

        /// <summary>
        ///  ExpectedActualBirthDate
        /// </summary>
        [DataMember]
        public string ChangedBirthDate
        {
            get { return this._changedBirthDate; }
            set { this._changedBirthDate = value; }
        }


        /// <summary>
        ///  MediaID
        /// </summary>
        [DataMember]
        public short Media
        {
            get { return this._mediaID; }
            set { this._mediaID = value; }
        }

        ///<summary>
        /// ClubDetail <CustomerPreference>
        /// </summary>
        [DataMember]
        public List<ClubDetails> ClubInformation
        {
            get { return _clubDetails; }
            set { _clubDetails = value; }
        }

         ///<summary>
        /// MediaDetails <CustomerPreference>
        /// </summary>
        [DataMember]
        public List<ClubDetails> MediaDetails
        {
            get { return _mediaDetails; }
            set { _mediaDetails = value; }
        }

        
         ///<summary>
        /// DOBDetails <CustomerPreference>
        /// </summary>
        [DataMember]
        public List<ClubDetails> DOBDetails
        {
            get { return _dOBDetails; }
            set { _dOBDetails = value; }
        }

        
        #endregion
    }
}
