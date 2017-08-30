using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Tesco.com.ClubcardOnline.Entities
{
    [Serializable]
    [DataContract]
    public class CustomerPreference
    {
        public short preferenceID;

        public string _userID;

        public long _customerID;

        public List<CustomerPreference> _customerPreference;

        public List<CustomerPreference> _preferenceType;

        public DateTime updateDateTime;

        public short _customerPreferenceType;

        public string _preferenceDescriptionEng;

        public string _preferenceDescriptionLocal;

        public string _isDeleted;

        public string _preferenceOptStatus;

        public short _sortseq;

        public OptStatus _optStatus;

        public string _pCulture;

        public string _emailSubject;

        /// <summary>
        ///  Culture
        /// </summary>
        [DataMember]
        public string Culture
        {
            get { return this._pCulture; }
            set { this._pCulture = value; }
        }

        /// <summary>
        ///  pOptStatus
        /// </summary>
        [DataMember]
        public OptStatus POptStatus
        {
            get { return this._optStatus; }
            set { this._optStatus = value; }
        }

        /// <summary>
        ///  Sortseq
        /// </summary>
        [DataMember]
        public short Sortseq
        {
            get { return this._sortseq; }
            set { this._sortseq = value; }
        }

        /// <summary>
        ///  PreferenceOptStatus
        /// </summary>
        [DataMember]
        public string PreferenceOptStatus
        {
            get { return this._preferenceOptStatus; }
            set { this._preferenceOptStatus = value; }
        }

        /// <summary>
        ///  PreferenceDescriptionEng
        /// </summary>
        [DataMember]
        public string PreferenceDescriptionEng
        {
            get { return this._preferenceDescriptionEng; }
            set { this._preferenceDescriptionEng = value; }
        }

        /// <summary>
        ///  IsDeleted
        /// </summary>
        [DataMember]
        public string IsDeleted
        {
            get { return this._isDeleted; }
            set { this._isDeleted = value; }
        }

        /// <summary>
        ///  PreferenceDescriptionLocal
        /// </summary>
        [DataMember]
        public string PreferenceDescriptionLocal
        {
            get { return this._preferenceDescriptionLocal; }
            set { this._preferenceDescriptionLocal = value; }
        }

        /// <summary>
        ///  PreferenceID
        /// </summary>
        [DataMember]
        public Int16 PreferenceID
        {
            get { return this.preferenceID; }
            set { this.preferenceID = value; }
        }

        [DataMember]
        public long CustomerID
        {
            get { return _customerID; }
            set { _customerID = value; }
        }

        [DataMember]
        public string UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }

        [DataMember]
        public DateTime UpdateDateTime
        {
            get { return updateDateTime; }
            set { updateDateTime = value; }
        }

        [DataMember]
        public List<CustomerPreference> Preference
        {
            get { return _customerPreference; }
            set { _customerPreference = value; }
        }

        [DataMember]
        public List<CustomerPreference> PreferenceType
        {
            get { return _preferenceType; }
            set { _preferenceType = value; }
        }

        [DataMember]
        public short CustomerPreferenceType
        {
            get { return _customerPreferenceType; }
            set { _customerPreferenceType = value; }
        }

        [DataMember]
        public string EmailSubject
        {
            get { return _emailSubject; }
            set { _emailSubject = value; }
        }
    }
}
