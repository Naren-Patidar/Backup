using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesco.Framework.Common.Utilities.API_Entities.TacticalSupplierServices
{
    public class ComplaintResponse
    {
        private string _referencenumber;
        private string _message;
        private Complaint _modelstate;

        public string CaseReferenceNumber
        {
            get { return _referencenumber; }
            set { _referencenumber = value; }
        }

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public Complaint ModelState
        {
            get { return _modelstate; }
            set { _modelstate = value; }
        }
    }
}
