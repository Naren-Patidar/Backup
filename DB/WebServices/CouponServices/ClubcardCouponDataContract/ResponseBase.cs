using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Tesco.Marketing.IT.ClubcardCoupon.DataContract
{
    /// <summary>
    /// This is a general class for the error responses across the club card coupons. Specifically, if any error occurs in the clubcard coupon system,
    /// an instance of this class is created with the relevant details of the context and sent back to the client.
    /// </summary>
    [DataContract]
    public abstract class ResponseBase
    {
        private string _errorLogId;     // Represents the error code used to log any events 
        // into event log/logging repository.  
        // If every system receiving this error also logs against same 
        // id then error can be tracked through the various systems
        // log files
        private string _errorStatusCode;
        private string _errorMessage;

        /// <summary>
        /// Constructor for class ResponseBase
        /// </summary>
        /// <param name="errorLogId">Represents errorLogId</param>
        /// <param name="errorStatusCode">Represents errorStatusCode</param>
        /// <param name="errorMessage">Represents text errorMessage details for the failure of the request</param>
        public ResponseBase(string errorLogId, string errorStatusCode, string errorMessage)
        {
            this._errorLogId = errorLogId;
            this._errorMessage = errorMessage;
            this._errorStatusCode = errorStatusCode;
        }

        /// <summary>
        /// Represents ErrorLogId
        /// </summary>
        [DataMember]
        public string ErrorLogID
        {
            get { return this._errorLogId; }
            set { this._errorLogId = value; }
        }

        /// <summary>
        /// Represents ErrorStatusCode
        /// </summary>
        [DataMember]
        public string ErrorStatusCode
        {
            get { return _errorStatusCode; }
            set { _errorStatusCode = value; }
        }


        /// <summary>
        /// Gets or sets the error message.
        /// </summary>                
        [DataMember]
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }
    }
}