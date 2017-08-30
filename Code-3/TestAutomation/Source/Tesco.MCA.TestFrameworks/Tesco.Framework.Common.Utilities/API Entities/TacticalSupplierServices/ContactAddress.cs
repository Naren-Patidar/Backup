using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesco.Framework.Common.Utilities.API_Entities.TacticalSupplierServices
{
    public class ContactAddress
    {
        #region PRIVATE MEMBERS

        private string _addressLine1;
        private string _addressLine2;
        private string _addressLine3;
        private string _addressLine4;
        private string _postcode;

        #endregion

        #region PROPERTIES
        public string AddressLine1
        {
            get { return _addressLine1; }
            set { _addressLine1 = value; }
        }

        public string AddressLine2
        {
            get { return _addressLine2; }
            set { _addressLine2 = value; }
        }

        public string AddressLine3
        {
            get { return _addressLine3; }
            set { _addressLine3 = value; }
        }

        public string AddressLine4
        {
            get { return _addressLine4; }
            set { _addressLine4 = value; }
        }

        public string PostCode      
        {
            get { return _postcode; }
            set { _postcode = value; }
        }
        #endregion
    }
}
