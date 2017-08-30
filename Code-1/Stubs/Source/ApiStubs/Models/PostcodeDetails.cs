using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdentityApiStub.Models
{
    public class PostcodeDetails
    {
        public string id { get; set; }
        public List<aLine> addressLines { get; set; }
        public string postcode { get; set; }
        public string postTown { get; set; }
        public royalMailSpecificDetails royalMailSpecificDetails { get; set; }
     }

    public class aLine
    {
        public int lineNumber { get; set; }
        public string value { get; set; }

    }
    public class royalMailSpecificDetails
    {
        public string legacyV2AddressId { get; set; }
        public string addressKey { get; set; }
        public string postcodeType { get; set; }
        public string organisationKey { get; set; }
        public string udprn { get; set; }
        public string umrrn { get; set; }
        public string dependentLocality { get; set; }
        public string doubleDependentLocality { get; set; }
        public string thoroughfareName { get; set; }
        public string thoroughfareDescriptor { get; set; }
        public string thoroughfareApprovedAbbreviation { get; set; }
        public string dependentThoroughfareName { get; set; }
        public string dependentThoroughfareDescriptor { get; set; }
        public string dependentThoroughfareApprovedAbbreviation { get; set; }
        public string buildingNumber { get; set; }
        public string buildingName { get; set; }
        public string subBuildingName { get; set; }
        public string organisationName { get; set; }
        public string departmentName { get; set; }
        public string poBoxNumber { get; set; }
        public int numberOfHouseHolds { get; set; }
        public string multiOccupancyCountOfOwningDP { get; set; }
        public string smallUserOrganisationIndicator { get; set; }
        public string concatenationIndicator { get; set; }
        public string deliveryPointSuffix { get; set; }
    }


}