using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts
{
    internal class DummyLocatorServiceClient : ILocatorSvcSDA
    {
        public DummyLocatorServiceClient()
        {

        }

        #region ILocatorSvcSDA Members

        public string FindAddressesByPostcode(string applicationId, string postcode)
        {
            throw new NotImplementedException();
        }

        public string GetAddressesByPostCode(string postcode, string houseNameNumber)
        {
            throw new NotImplementedException();
        }

        public string GetAddressListForPostcode(string postcode)
        {
            throw new NotImplementedException();
        }

        public string GetAreaDetails(string postcode)
        {
            throw new NotImplementedException();
        }

        public string GetGridRefByPostCode(string postcode)
        {
            throw new NotImplementedException();
        }

        public string GetAddressExceptions(string postcode, string houseNumber, string houseName, string streetName)
        {
            throw new NotImplementedException();
        }

        public string GetAddressForLocation(string postcode, string county, string addressLine)
        {
            throw new NotImplementedException();
        }

        public string GetAddressesByPostCodeHouseNumber(IsAddressBlockedEntity[] isAddressBlocked)
        {
            throw new NotImplementedException();
        }

        public string GetGeoCoordinateByPostCode(string postcode)
        {
            throw new NotImplementedException();
        }

        public string FindAddressLite(string postcode, string county, string addressLine)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
