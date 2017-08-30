using System;
using System.Runtime.Serialization;

namespace Tesco.com.ClubcardOnline.WebService.Messages
{
    [Serializable]
    [DataContract]
    public abstract class ResponseBase
    {
        // Too generic and would be difficult to handle multiple return types:
        //private bool _boolResult;
        //private string _result2;
        //private string _result3;
        private string _errorLogId;     // Represents the error code used to log any events 
                                        // into event log/logging repository.  
                                        // If every system receiving this error also logs against same 
                                        // id then error can be tracked through the various systems
                                        // log files
        private string _errorStatusCode;
        private string _errorMessage;

        public ResponseBase(string errorLogId, string errorStatusCode, string errorMessage)
        {
            this._errorLogId = errorLogId;
            this._errorMessage = errorMessage;
            this._errorStatusCode = errorStatusCode;
        }

        //[DataMember]
        //public bool Result { get { return _boolResult; } set { _boolResult = value; } }

        //[DataMember]
        //public string Result2 { get { return _result2; } set { _result2 = value; } }

        //[DataMember]
        //public string Result3 { get { return _result3; } set { _result3 = value; } }

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
