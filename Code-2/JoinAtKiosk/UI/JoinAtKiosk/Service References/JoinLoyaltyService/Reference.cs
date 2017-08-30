﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Tesco.Com.Marketing.Kiosk.JoinAtKiosk.JoinLoyaltyService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="JoinLoyaltyService.IJoinLoyaltyService")]
    public interface IJoinLoyaltyService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJoinLoyaltyService/AccountCreate", ReplyAction="http://tempuri.org/IJoinLoyaltyService/AccountCreateResponse")]
        string AccountCreate(long dotcomCustomerID, string objectXml, string source, string culture);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJoinLoyaltyService/SendJoinConfirmationEmail", ReplyAction="http://tempuri.org/IJoinLoyaltyService/SendJoinConfirmationEmailResponse")]
        bool SendJoinConfirmationEmail(string strTo, string title, string custName, long clubcardID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IJoinLoyaltyService/AccountDuplicateCheck", ReplyAction="http://tempuri.org/IJoinLoyaltyService/AccountDuplicateCheckResponse")]
        bool AccountDuplicateCheck(out string resultXml, string inputXml);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IJoinLoyaltyServiceChannel : Tesco.Com.Marketing.Kiosk.JoinAtKiosk.JoinLoyaltyService.IJoinLoyaltyService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class JoinLoyaltyServiceClient : System.ServiceModel.ClientBase<Tesco.Com.Marketing.Kiosk.JoinAtKiosk.JoinLoyaltyService.IJoinLoyaltyService>, Tesco.Com.Marketing.Kiosk.JoinAtKiosk.JoinLoyaltyService.IJoinLoyaltyService {
        
        public JoinLoyaltyServiceClient() {
        }
        
        public JoinLoyaltyServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public JoinLoyaltyServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public JoinLoyaltyServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public JoinLoyaltyServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string AccountCreate(long dotcomCustomerID, string objectXml, string source, string culture) {
            return base.Channel.AccountCreate(dotcomCustomerID, objectXml, source, culture);
        }
        
        public bool SendJoinConfirmationEmail(string strTo, string title, string custName, long clubcardID) {
            return base.Channel.SendJoinConfirmationEmail(strTo, title, custName, clubcardID);
        }
        
        public bool AccountDuplicateCheck(out string resultXml, string inputXml) {
            return base.Channel.AccountDuplicateCheck(out resultXml, inputXml);
        }
    }
}
