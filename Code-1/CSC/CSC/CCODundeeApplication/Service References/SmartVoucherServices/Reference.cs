﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18063
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CCODundeeApplication.SmartVoucherServices {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ResponseBase", Namespace="http://schemas.datacontract.org/2004/07/Tesco.com.IntegrationServices.Messages")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CCODundeeApplication.SmartVoucherServices.GetUnusedVoucherDtlsRsp))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CCODundeeApplication.SmartVoucherServices.GetUsedVoucherDtlsRsp))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CCODundeeApplication.SmartVoucherServices.GetVoucherValHHRsp))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CCODundeeApplication.SmartVoucherServices.GetRewardDtlsMilesRsp))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CCODundeeApplication.SmartVoucherServices.GetVoucherValAllCPSRsp))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(CCODundeeApplication.SmartVoucherServices.GetRewardDtlsRsp))]
    public partial class ResponseBase : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ErrorLogIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ErrorMessageField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ErrorStatusCodeField;
        
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
        public string ErrorLogID {
            get {
                return this.ErrorLogIDField;
            }
            set {
                if ((object.ReferenceEquals(this.ErrorLogIDField, value) != true)) {
                    this.ErrorLogIDField = value;
                    this.RaisePropertyChanged("ErrorLogID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ErrorMessage {
            get {
                return this.ErrorMessageField;
            }
            set {
                if ((object.ReferenceEquals(this.ErrorMessageField, value) != true)) {
                    this.ErrorMessageField = value;
                    this.RaisePropertyChanged("ErrorMessage");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ErrorStatusCode {
            get {
                return this.ErrorStatusCodeField;
            }
            set {
                if ((object.ReferenceEquals(this.ErrorStatusCodeField, value) != true)) {
                    this.ErrorStatusCodeField = value;
                    this.RaisePropertyChanged("ErrorStatusCode");
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
    [System.Runtime.Serialization.DataContractAttribute(Name="GetUnusedVoucherDtlsRsp", Namespace="http://schemas.datacontract.org/2004/07/Tesco.com.IntegrationServices.Messages")]
    [System.SerializableAttribute()]
    public partial class GetUnusedVoucherDtlsRsp : CCODundeeApplication.SmartVoucherServices.ResponseBase {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Data.DataSet dsResponseField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Data.DataSet dsResponse {
            get {
                return this.dsResponseField;
            }
            set {
                if ((object.ReferenceEquals(this.dsResponseField, value) != true)) {
                    this.dsResponseField = value;
                    this.RaisePropertyChanged("dsResponse");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="GetUsedVoucherDtlsRsp", Namespace="http://schemas.datacontract.org/2004/07/Tesco.com.IntegrationServices.Messages")]
    [System.SerializableAttribute()]
    public partial class GetUsedVoucherDtlsRsp : CCODundeeApplication.SmartVoucherServices.ResponseBase {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Data.DataSet dsResponseField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Data.DataSet dsResponse {
            get {
                return this.dsResponseField;
            }
            set {
                if ((object.ReferenceEquals(this.dsResponseField, value) != true)) {
                    this.dsResponseField = value;
                    this.RaisePropertyChanged("dsResponse");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="GetVoucherValHHRsp", Namespace="http://schemas.datacontract.org/2004/07/Tesco.com.IntegrationServices.Messages")]
    [System.SerializableAttribute()]
    public partial class GetVoucherValHHRsp : CCODundeeApplication.SmartVoucherServices.ResponseBase {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Data.DataSet dsResponseField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Data.DataSet dsResponse {
            get {
                return this.dsResponseField;
            }
            set {
                if ((object.ReferenceEquals(this.dsResponseField, value) != true)) {
                    this.dsResponseField = value;
                    this.RaisePropertyChanged("dsResponse");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="GetRewardDtlsMilesRsp", Namespace="http://schemas.datacontract.org/2004/07/Tesco.com.IntegrationServices.Messages")]
    [System.SerializableAttribute()]
    public partial class GetRewardDtlsMilesRsp : CCODundeeApplication.SmartVoucherServices.ResponseBase {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Data.DataSet dsResponseField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Data.DataSet dsResponse {
            get {
                return this.dsResponseField;
            }
            set {
                if ((object.ReferenceEquals(this.dsResponseField, value) != true)) {
                    this.dsResponseField = value;
                    this.RaisePropertyChanged("dsResponse");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="GetVoucherValAllCPSRsp", Namespace="http://schemas.datacontract.org/2004/07/Tesco.com.IntegrationServices.Messages")]
    [System.SerializableAttribute()]
    public partial class GetVoucherValAllCPSRsp : CCODundeeApplication.SmartVoucherServices.ResponseBase {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Data.DataSet dsResponseField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Data.DataSet dsResponse {
            get {
                return this.dsResponseField;
            }
            set {
                if ((object.ReferenceEquals(this.dsResponseField, value) != true)) {
                    this.dsResponseField = value;
                    this.RaisePropertyChanged("dsResponse");
                }
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="GetRewardDtlsRsp", Namespace="http://schemas.datacontract.org/2004/07/Tesco.com.IntegrationServices.Messages")]
    [System.SerializableAttribute()]
    public partial class GetRewardDtlsRsp : CCODundeeApplication.SmartVoucherServices.ResponseBase {
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Data.DataSet dsResponseField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Data.DataSet dsResponse {
            get {
                return this.dsResponseField;
            }
            set {
                if ((object.ReferenceEquals(this.dsResponseField, value) != true)) {
                    this.dsResponseField = value;
                    this.RaisePropertyChanged("dsResponse");
                }
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://tesco.com/clubcardonline/datacontract/2010/01", ConfigurationName="SmartVoucherServices.ISmartVoucherServices")]
    public interface ISmartVoucherServices {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tesco.com/clubcardonline/datacontract/2010/01/ISmartVoucherServices/GetRew" +
            "ardDtls", ReplyAction="http://tesco.com/clubcardonline/datacontract/2010/01/ISmartVoucherServices/GetRew" +
            "ardDtlsResponse")]
        CCODundeeApplication.SmartVoucherServices.GetRewardDtlsRsp GetRewardDtls(string ClubcardNumber);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tesco.com/clubcardonline/datacontract/2010/01/ISmartVoucherServices/GetUnu" +
            "sedVoucherDtls", ReplyAction="http://tesco.com/clubcardonline/datacontract/2010/01/ISmartVoucherServices/GetUnu" +
            "sedVoucherDtlsResponse")]
        CCODundeeApplication.SmartVoucherServices.GetUnusedVoucherDtlsRsp GetUnusedVoucherDtls(string ClubcardNumber);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tesco.com/clubcardonline/datacontract/2010/01/ISmartVoucherServices/GetUse" +
            "dVoucherDtls", ReplyAction="http://tesco.com/clubcardonline/datacontract/2010/01/ISmartVoucherServices/GetUse" +
            "dVoucherDtlsResponse")]
        CCODundeeApplication.SmartVoucherServices.GetUsedVoucherDtlsRsp GetUsedVoucherDtls(string ClubcardNumber);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tesco.com/clubcardonline/datacontract/2010/01/ISmartVoucherServices/GetVou" +
            "cherValHH", ReplyAction="http://tesco.com/clubcardonline/datacontract/2010/01/ISmartVoucherServices/GetVou" +
            "cherValHHResponse")]
        CCODundeeApplication.SmartVoucherServices.GetVoucherValHHRsp GetVoucherValHH(string Household_ID, string CPStartDate, string CPEndDate);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tesco.com/clubcardonline/datacontract/2010/01/ISmartVoucherServices/GetRew" +
            "ardDtlsMiles", ReplyAction="http://tesco.com/clubcardonline/datacontract/2010/01/ISmartVoucherServices/GetRew" +
            "ardDtlsMilesResponse")]
        CCODundeeApplication.SmartVoucherServices.GetRewardDtlsMilesRsp GetRewardDtlsMiles(string ClubcardNumber, int ReasonCode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tesco.com/clubcardonline/datacontract/2010/01/ISmartVoucherServices/GetVou" +
            "cherValCPS", ReplyAction="http://tesco.com/clubcardonline/datacontract/2010/01/ISmartVoucherServices/GetVou" +
            "cherValCPSResponse")]
        CCODundeeApplication.SmartVoucherServices.GetVoucherValAllCPSRsp GetVoucherValCPS(string Clubcard_Number, string CPStartDate, string CPEndDate);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ISmartVoucherServicesChannel : CCODundeeApplication.SmartVoucherServices.ISmartVoucherServices, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SmartVoucherServicesClient : System.ServiceModel.ClientBase<CCODundeeApplication.SmartVoucherServices.ISmartVoucherServices>, CCODundeeApplication.SmartVoucherServices.ISmartVoucherServices {
        
        public SmartVoucherServicesClient() {
        }
        
        public SmartVoucherServicesClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SmartVoucherServicesClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SmartVoucherServicesClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SmartVoucherServicesClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public CCODundeeApplication.SmartVoucherServices.GetRewardDtlsRsp GetRewardDtls(string ClubcardNumber) {
            return base.Channel.GetRewardDtls(ClubcardNumber);
        }
        
        public CCODundeeApplication.SmartVoucherServices.GetUnusedVoucherDtlsRsp GetUnusedVoucherDtls(string ClubcardNumber) {
            return base.Channel.GetUnusedVoucherDtls(ClubcardNumber);
        }
        
        public CCODundeeApplication.SmartVoucherServices.GetUsedVoucherDtlsRsp GetUsedVoucherDtls(string ClubcardNumber) {
            return base.Channel.GetUsedVoucherDtls(ClubcardNumber);
        }
        
        public CCODundeeApplication.SmartVoucherServices.GetVoucherValHHRsp GetVoucherValHH(string Household_ID, string CPStartDate, string CPEndDate) {
            return base.Channel.GetVoucherValHH(Household_ID, CPStartDate, CPEndDate);
        }
        
        public CCODundeeApplication.SmartVoucherServices.GetRewardDtlsMilesRsp GetRewardDtlsMiles(string ClubcardNumber, int ReasonCode) {
            return base.Channel.GetRewardDtlsMiles(ClubcardNumber, ReasonCode);
        }
        
        public CCODundeeApplication.SmartVoucherServices.GetVoucherValAllCPSRsp GetVoucherValCPS(string Clubcard_Number, string CPStartDate, string CPEndDate) {
            return base.Channel.GetVoucherValCPS(Clubcard_Number, CPStartDate, CPEndDate);
        }
    }
}
