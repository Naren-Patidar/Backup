using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service
{
    public class MCAResponse
    {
        Dictionary<string, object> _outParameters = new Dictionary<string, object>();

        public object Data { get; set; }
        public bool Status { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public Exception ErrorException { get; set; }
        public string OperationName { get; set; }
        public List<ErrorData> Errors { get; set; }
        public Dictionary<string, object> OutParameters
        {
            get { return _outParameters; }
            set { _outParameters = value; }
        }
    }

    public class ErrorData
    {
        public string ElementName { get; set; }
        public string Details { get; set; }
    }
}
