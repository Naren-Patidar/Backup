using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesco.Framework.Common.Utilities.API_Entities.TacticalSupplierServices
{
    public class Customer
    {
        #region PRIVATE MEMBERS

        private string _referenceCustomerId;
        private string _firstName;
        private string _lastName;
        ContactAddress _contactAddress;

        #endregion

        #region PROPERTIES
        public string ReferenceCustomerId
        {
            get { return _referenceCustomerId; }
            set { _referenceCustomerId = value; }
        }

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }

        public ContactAddress ContactAddress
        {
            get { return _contactAddress; }
            set { _contactAddress = value; }
        }
        #endregion
    }
}
