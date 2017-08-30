using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.ResponseRecorder;

namespace Tesco.ClubcardProducts.MCAStubGenerator.ServiceInvoker
{
    public class APIs
    {
        public List<ServiceDefinition> apis { get; set; }
    }

    public class ServiceDefinition
    {
        public string servicetype { get; set; }
        public string proxy { get; set; }
        public List<MethodDefinition> methods;
    }

    public class MethodDefinition
    {
        public string name { get; set; }
        public ResponseType responseformat { get; set; }
        public string opname { get; set; }
    }

    public class EndPointDefinition
    {
        bool _IsSelected = true;
        public MethodDefinition MethodDef { get; set; }

        public string ServiceType { get; set; }
        public string MethodName { get; set; }        
        public string ProxyType { get; set; }
        public bool IsSelected
        {
            get { return this._IsSelected; }
            set { this._IsSelected = value; }
        }
    }

    public class ExecutableEndPoint
    {
        public int ID { get; set; }
        public long customerID { get; set; }
        public long clubcardNumber { get; set; }
        public int ReasonCode { get; set; }
        public EndPointDefinition EndPointDef { get; set; }
        public Status CurrentStatus { get; set; }
        public string ResultDirectory { get; set; }
        public DateTime? Started { get; set; }
        public DateTime? Ended { get; set; }
        public string FailureReason { get; set; }
    }

    public enum Status
    {
        ReadyToGo,
        NowProcessing,
        Failed,
        Succeeded
    }

    public class StringEventArgs : EventArgs
    {
        public string Message { get; set; }

        public StringEventArgs(string message)
        {
            // TODO: Complete member initialization
            this.Message = message;
        }
    }
}

