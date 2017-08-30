using System;
using System.Runtime.Serialization;

namespace Tesco.com.IntegrationServices.Messages
{
    [Serializable]
    [DataContract]
    public abstract class ResponseBase
    {
        private string _errorLogId;     // Represents the error code used to log any events 
        private string _errorStatusCode;
        private string _errorMessage;

        public ResponseBase(string errorLogId, string errorStatusCode, string errorMessage)
        {
            this._errorLogId = errorLogId;
            this._errorMessage = errorMessage;
            this._errorStatusCode = errorStatusCode;
        }

        [DataMember]
        public string ErrorLogID
        {
            get { return this._errorLogId; }
            set { this._errorLogId = value; }
        }

        [DataMember]
        public string ErrorStatusCode
        {
            get { return _errorStatusCode; }
            set { _errorStatusCode = value; }
        }

        [DataMember]
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }
    }
}
