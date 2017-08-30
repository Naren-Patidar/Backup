﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.225
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Tesco.ClubcardProducts.MCA.API.ServiceAdapter.PreferenceServices {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="PreferenceType", Namespace="http://schemas.datacontract.org/2004/07/Tesco.com.ClubcardOnline.Entities")]
    public enum PreferenceType : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        NULL = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ALLERGY = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        DIETARY = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        CONTACT_METHOD = 3,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        PROMOTIONS = 4,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        REWARD = 5,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        PREFERRED_MAILING_ADDRESS = 6,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        MEDICAL = 7,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        COMMUNICATION_LANGUAGE = 8,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        DATA_PROTECTION = 9,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CustomerPreference", Namespace="http://schemas.datacontract.org/2004/07/Tesco.com.ClubcardOnline.Entities")]
    [System.SerializableAttribute()]
    public partial class CustomerPreference : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CultureField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private long CustomerIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private short CustomerPreferenceTypeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string EmailSubjectField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IsDeletedField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Tesco.ClubcardProducts.MCA.API.ServiceAdapter.PreferenceServices.OptStatus POptStatusField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Tesco.ClubcardProducts.MCA.API.ServiceAdapter.PreferenceServices.CustomerPreference[] PreferenceField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PreferenceDescriptionEngField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PreferenceDescriptionLocalField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private short PreferenceIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PreferenceOptStatusField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Tesco.ClubcardProducts.MCA.API.ServiceAdapter.PreferenceServices.CustomerPreference[] PreferenceTypeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private short SortseqField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime UpdateDateTimeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string UserIDField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Culture {
            get {
                return this.CultureField;
            }
            set {
                if ((object.ReferenceEquals(this.CultureField, value) != true)) {
                    this.CultureField = value;
                    this.RaisePropertyChanged("Culture");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public long CustomerID {
            get {
                return this.CustomerIDField;
            }
            set {
                if ((this.CustomerIDField.Equals(value) != true)) {
                    this.CustomerIDField = value;
                    this.RaisePropertyChanged("CustomerID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public short CustomerPreferenceType {
            get {
                return this.CustomerPreferenceTypeField;
            }
            set {
                if ((this.CustomerPreferenceTypeField.Equals(value) != true)) {
                    this.CustomerPreferenceTypeField = value;
                    this.RaisePropertyChanged("CustomerPreferenceType");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string EmailSubject {
            get {
                return this.EmailSubjectField;
            }
            set {
                if ((object.ReferenceEquals(this.EmailSubjectField, value) != true)) {
                    this.EmailSubjectField = value;
                    this.RaisePropertyChanged("EmailSubject");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string IsDeleted {
            get {
                return this.IsDeletedField;
            }
            set {
                if ((object.ReferenceEquals(this.IsDeletedField, value) != true)) {
                    this.IsDeletedField = value;
                    this.RaisePropertyChanged("IsDeleted");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Tesco.ClubcardProducts.MCA.API.ServiceAdapter.PreferenceServices.OptStatus POptStatus {
            get {
                return this.POptStatusField;
            }
            set {
                if ((this.POptStatusField.Equals(value) != true)) {
                    this.POptStatusField = value;
                    this.RaisePropertyChanged("POptStatus");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Tesco.ClubcardProducts.MCA.API.ServiceAdapter.PreferenceServices.CustomerPreference[] Preference {
            get {
                return this.PreferenceField;
            }
            set {
                if ((object.ReferenceEquals(this.PreferenceField, value) != true)) {
                    this.PreferenceField = value;
                    this.RaisePropertyChanged("Preference");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PreferenceDescriptionEng {
            get {
                return this.PreferenceDescriptionEngField;
            }
            set {
                if ((object.ReferenceEquals(this.PreferenceDescriptionEngField, value) != true)) {
                    this.PreferenceDescriptionEngField = value;
                    this.RaisePropertyChanged("PreferenceDescriptionEng");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PreferenceDescriptionLocal {
            get {
                return this.PreferenceDescriptionLocalField;
            }
            set {
                if ((object.ReferenceEquals(this.PreferenceDescriptionLocalField, value) != true)) {
                    this.PreferenceDescriptionLocalField = value;
                    this.RaisePropertyChanged("PreferenceDescriptionLocal");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public short PreferenceID {
            get {
                return this.PreferenceIDField;
            }
            set {
                if ((this.PreferenceIDField.Equals(value) != true)) {
                    this.PreferenceIDField = value;
                    this.RaisePropertyChanged("PreferenceID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string PreferenceOptStatus {
            get {
                return this.PreferenceOptStatusField;
            }
            set {
                if ((object.ReferenceEquals(this.PreferenceOptStatusField, value) != true)) {
                    this.PreferenceOptStatusField = value;
                    this.RaisePropertyChanged("PreferenceOptStatus");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Tesco.ClubcardProducts.MCA.API.ServiceAdapter.PreferenceServices.CustomerPreference[] PreferenceType {
            get {
                return this.PreferenceTypeField;
            }
            set {
                if ((object.ReferenceEquals(this.PreferenceTypeField, value) != true)) {
                    this.PreferenceTypeField = value;
                    this.RaisePropertyChanged("PreferenceType");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public short Sortseq {
            get {
                return this.SortseqField;
            }
            set {
                if ((this.SortseqField.Equals(value) != true)) {
                    this.SortseqField = value;
                    this.RaisePropertyChanged("Sortseq");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime UpdateDateTime {
            get {
                return this.UpdateDateTimeField;
            }
            set {
                if ((this.UpdateDateTimeField.Equals(value) != true)) {
                    this.UpdateDateTimeField = value;
                    this.RaisePropertyChanged("UpdateDateTime");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string UserID {
            get {
                return this.UserIDField;
            }
            set {
                if ((object.ReferenceEquals(this.UserIDField, value) != true)) {
                    this.UserIDField = value;
                    this.RaisePropertyChanged("UserID");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="OptStatus", Namespace="http://schemas.datacontract.org/2004/07/Tesco.com.ClubcardOnline.Entities")]
    public enum OptStatus : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        NOT_SELECTED = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        OPTED_IN = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        OPTED_OUT = 2,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ClubDetails", Namespace="http://schemas.datacontract.org/2004/07/Tesco.com.ClubcardOnline.Entities")]
    [System.SerializableAttribute()]
    public partial class ClubDetails : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ChangedBirthDateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private short ClubIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Tesco.ClubcardProducts.MCA.API.ServiceAdapter.PreferenceServices.ClubDetails[] ClubInformationField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CultureField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Tesco.ClubcardProducts.MCA.API.ServiceAdapter.PreferenceServices.ClubDetails[] DOBDetailsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DateOfBirthField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IsDeletedField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime JoinDateField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private short MediaField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MediaDescField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private Tesco.ClubcardProducts.MCA.API.ServiceAdapter.PreferenceServices.ClubDetails[] MediaDetailsField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string MembershipIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string UserIDField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ChangedBirthDate {
            get {
                return this.ChangedBirthDateField;
            }
            set {
                if ((object.ReferenceEquals(this.ChangedBirthDateField, value) != true)) {
                    this.ChangedBirthDateField = value;
                    this.RaisePropertyChanged("ChangedBirthDate");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public short ClubID {
            get {
                return this.ClubIDField;
            }
            set {
                if ((this.ClubIDField.Equals(value) != true)) {
                    this.ClubIDField = value;
                    this.RaisePropertyChanged("ClubID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Tesco.ClubcardProducts.MCA.API.ServiceAdapter.PreferenceServices.ClubDetails[] ClubInformation {
            get {
                return this.ClubInformationField;
            }
            set {
                if ((object.ReferenceEquals(this.ClubInformationField, value) != true)) {
                    this.ClubInformationField = value;
                    this.RaisePropertyChanged("ClubInformation");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Culture {
            get {
                return this.CultureField;
            }
            set {
                if ((object.ReferenceEquals(this.CultureField, value) != true)) {
                    this.CultureField = value;
                    this.RaisePropertyChanged("Culture");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Tesco.ClubcardProducts.MCA.API.ServiceAdapter.PreferenceServices.ClubDetails[] DOBDetails {
            get {
                return this.DOBDetailsField;
            }
            set {
                if ((object.ReferenceEquals(this.DOBDetailsField, value) != true)) {
                    this.DOBDetailsField = value;
                    this.RaisePropertyChanged("DOBDetails");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DateOfBirth {
            get {
                return this.DateOfBirthField;
            }
            set {
                if ((object.ReferenceEquals(this.DateOfBirthField, value) != true)) {
                    this.DateOfBirthField = value;
                    this.RaisePropertyChanged("DateOfBirth");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string IsDeleted {
            get {
                return this.IsDeletedField;
            }
            set {
                if ((object.ReferenceEquals(this.IsDeletedField, value) != true)) {
                    this.IsDeletedField = value;
                    this.RaisePropertyChanged("IsDeleted");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime JoinDate {
            get {
                return this.JoinDateField;
            }
            set {
                if ((this.JoinDateField.Equals(value) != true)) {
                    this.JoinDateField = value;
                    this.RaisePropertyChanged("JoinDate");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public short Media {
            get {
                return this.MediaField;
            }
            set {
                if ((this.MediaField.Equals(value) != true)) {
                    this.MediaField = value;
                    this.RaisePropertyChanged("Media");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string MediaDesc {
            get {
                return this.MediaDescField;
            }
            set {
                if ((object.ReferenceEquals(this.MediaDescField, value) != true)) {
                    this.MediaDescField = value;
                    this.RaisePropertyChanged("MediaDesc");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public Tesco.ClubcardProducts.MCA.API.ServiceAdapter.PreferenceServices.ClubDetails[] MediaDetails {
            get {
                return this.MediaDetailsField;
            }
            set {
                if ((object.ReferenceEquals(this.MediaDetailsField, value) != true)) {
                    this.MediaDetailsField = value;
                    this.RaisePropertyChanged("MediaDetails");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string MembershipID {
            get {
                return this.MembershipIDField;
            }
            set {
                if ((object.ReferenceEquals(this.MembershipIDField, value) != true)) {
                    this.MembershipIDField = value;
                    this.RaisePropertyChanged("MembershipID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string UserID {
            get {
                return this.UserIDField;
            }
            set {
                if ((object.ReferenceEquals(this.UserIDField, value) != true)) {
                    this.UserIDField = value;
                    this.RaisePropertyChanged("UserID");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CustomerDetails", Namespace="http://schemas.datacontract.org/2004/07/Tesco.com.ClubcardOnline.Entities")]
    [System.SerializableAttribute()]
    public partial class CustomerDetails : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CardNumberField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string EmailIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FirstnameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string[] PreferencelistField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SurnameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string TitleField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CardNumber {
            get {
                return this.CardNumberField;
            }
            set {
                if ((object.ReferenceEquals(this.CardNumberField, value) != true)) {
                    this.CardNumberField = value;
                    this.RaisePropertyChanged("CardNumber");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string EmailId {
            get {
                return this.EmailIdField;
            }
            set {
                if ((object.ReferenceEquals(this.EmailIdField, value) != true)) {
                    this.EmailIdField = value;
                    this.RaisePropertyChanged("EmailId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Firstname {
            get {
                return this.FirstnameField;
            }
            set {
                if ((object.ReferenceEquals(this.FirstnameField, value) != true)) {
                    this.FirstnameField = value;
                    this.RaisePropertyChanged("Firstname");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] Preferencelist {
            get {
                return this.PreferencelistField;
            }
            set {
                if ((object.ReferenceEquals(this.PreferencelistField, value) != true)) {
                    this.PreferencelistField = value;
                    this.RaisePropertyChanged("Preferencelist");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Surname {
            get {
                return this.SurnameField;
            }
            set {
                if ((object.ReferenceEquals(this.SurnameField, value) != true)) {
                    this.SurnameField = value;
                    this.RaisePropertyChanged("Surname");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Title {
            get {
                return this.TitleField;
            }
            set {
                if ((object.ReferenceEquals(this.TitleField, value) != true)) {
                    this.TitleField = value;
                    this.RaisePropertyChanged("Title");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="PreferenceServices.IPreferenceService")]
    public interface IPreferenceService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPreferenceService/ViewCustomerPreference", ReplyAction="http://tempuri.org/IPreferenceService/ViewCustomerPreferenceResponse")]
        Tesco.ClubcardProducts.MCA.API.ServiceAdapter.PreferenceServices.CustomerPreference ViewCustomerPreference(long customerID, Tesco.ClubcardProducts.MCA.API.ServiceAdapter.PreferenceServices.PreferenceType PreferenceType, bool optionalPreference);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPreferenceService/ViewClubDetails", ReplyAction="http://tempuri.org/IPreferenceService/ViewClubDetailsResponse")]
        Tesco.ClubcardProducts.MCA.API.ServiceAdapter.PreferenceServices.ClubDetails ViewClubDetails(long customerID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPreferenceService/MaintainCustomerPreference", ReplyAction="http://tempuri.org/IPreferenceService/MaintainCustomerPreferenceResponse")]
        void MaintainCustomerPreference(long customerID, Tesco.ClubcardProducts.MCA.API.ServiceAdapter.PreferenceServices.CustomerPreference customerPreference, Tesco.ClubcardProducts.MCA.API.ServiceAdapter.PreferenceServices.CustomerDetails customerDetails);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPreferenceService/MaintainClubDetails", ReplyAction="http://tempuri.org/IPreferenceService/MaintainClubDetailsResponse")]
        void MaintainClubDetails(long customerID, Tesco.ClubcardProducts.MCA.API.ServiceAdapter.PreferenceServices.ClubDetails cluDetails, string sEmailDTo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPreferenceService/SendEmailToCustomers", ReplyAction="http://tempuri.org/IPreferenceService/SendEmailToCustomersResponse")]
        bool SendEmailToCustomers(Tesco.ClubcardProducts.MCA.API.ServiceAdapter.PreferenceServices.CustomerPreference CustomerPreference, Tesco.ClubcardProducts.MCA.API.ServiceAdapter.PreferenceServices.CustomerDetails customerDetails);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPreferenceService/SendEmailNoticeToCustomers", ReplyAction="http://tempuri.org/IPreferenceService/SendEmailNoticeToCustomersResponse")]
        bool SendEmailNoticeToCustomers(long customerID, Tesco.ClubcardProducts.MCA.API.ServiceAdapter.PreferenceServices.CustomerPreference customerPreference, Tesco.ClubcardProducts.MCA.API.ServiceAdapter.PreferenceServices.CustomerDetails customerDetails, string Pagedetails, string trackxml);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IPreferenceServiceChannel : Tesco.ClubcardProducts.MCA.API.ServiceAdapter.PreferenceServices.IPreferenceService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class PreferenceServiceClient : System.ServiceModel.ClientBase<Tesco.ClubcardProducts.MCA.API.ServiceAdapter.PreferenceServices.IPreferenceService>, Tesco.ClubcardProducts.MCA.API.ServiceAdapter.PreferenceServices.IPreferenceService {
        
        public PreferenceServiceClient() {
        }
        
        public PreferenceServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public PreferenceServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PreferenceServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PreferenceServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Tesco.ClubcardProducts.MCA.API.ServiceAdapter.PreferenceServices.CustomerPreference ViewCustomerPreference(long customerID, Tesco.ClubcardProducts.MCA.API.ServiceAdapter.PreferenceServices.PreferenceType PreferenceType, bool optionalPreference) {
            return base.Channel.ViewCustomerPreference(customerID, PreferenceType, optionalPreference);
        }
        
        public Tesco.ClubcardProducts.MCA.API.ServiceAdapter.PreferenceServices.ClubDetails ViewClubDetails(long customerID) {
            return base.Channel.ViewClubDetails(customerID);
        }
        
        public void MaintainCustomerPreference(long customerID, Tesco.ClubcardProducts.MCA.API.ServiceAdapter.PreferenceServices.CustomerPreference customerPreference, Tesco.ClubcardProducts.MCA.API.ServiceAdapter.PreferenceServices.CustomerDetails customerDetails) {
            base.Channel.MaintainCustomerPreference(customerID, customerPreference, customerDetails);
        }
        
        public void MaintainClubDetails(long customerID, Tesco.ClubcardProducts.MCA.API.ServiceAdapter.PreferenceServices.ClubDetails cluDetails, string sEmailDTo) {
            base.Channel.MaintainClubDetails(customerID, cluDetails, sEmailDTo);
        }
        
        public bool SendEmailToCustomers(Tesco.ClubcardProducts.MCA.API.ServiceAdapter.PreferenceServices.CustomerPreference CustomerPreference, Tesco.ClubcardProducts.MCA.API.ServiceAdapter.PreferenceServices.CustomerDetails customerDetails) {
            return base.Channel.SendEmailToCustomers(CustomerPreference, customerDetails);
        }
        
        public bool SendEmailNoticeToCustomers(long customerID, Tesco.ClubcardProducts.MCA.API.ServiceAdapter.PreferenceServices.CustomerPreference customerPreference, Tesco.ClubcardProducts.MCA.API.ServiceAdapter.PreferenceServices.CustomerDetails customerDetails, string Pagedetails, string trackxml) {
            return base.Channel.SendEmailNoticeToCustomers(customerID, customerPreference, customerDetails, Pagedetails, trackxml);
        }
    }
}
