using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdentityApiStub.Models
{
    public class AddressBook
    {

        public string addressIdentifier { get; set; }
        public string addressLookupIdentifier { get; set; }
        public addressLines addressLines { get; set; }
        public string alias { get; set; }
    }

    public class addressLines
    {        
        public string id { get; set; }
        public string departmentName { get; set; }
        public string organisationName { get; set; }
        public string subBuildingName { get; set; }
        public string postalBox { get; set; }
        public string buildingName { get; set; }
        public string buildingNumber { get; set; }
        public string secondaryRoad { get; set; }
        public string primaryRoad { get; set; }
        public string secondaryArea { get; set; }
        public string primaryArea { get; set; }
        public string town { get; set; }
        public string postcode { get; set; }
        public string postcodeType { get; set; }
        public string smallUserOrganisationIndicator { get; set; }
        public string concatenationIndicator { get; set; }
        public lines lines { get; set; }

    }

    public class lines
    {
        public string lineNumber { get; set; }
        public string text { get; set; }

    }
}