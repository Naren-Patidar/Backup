using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdentityApiStub.Models
{
    public class Address
    {
        //{"id":"EA0E6B4E-37BA-E511-A257-BE2A82EC07D3","departmentName":null,"organisationName":null,"subBuildingName":null,"postalBox":null,"buildingName":null,"buildingNumber":"73"
        //,"secondaryRoad":null,"primaryRoad":"West Hill","secondaryArea":null,"primaryArea":null,"town":"HITCHIN","postcode":"SG5 2HY","postcodeType":2,
        //"smallUserOrganisationIndicator":false,"concatenationIndicator":"","lines":[{"lineNumber":1,"text":"73 West Hill"}],"udprnkey":"","isBusinessAddress":false}
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
        public PostcodeType postcodeType { get; set; }
        public bool smallUserOrganisationIndicator { get; set; }
        public string concatenationIndicator { get; set; }
        public List<AddressLine> lines { get; set; }
        public string udprnkey { get; set; }
        public bool isBusinessAddress { get; set; }
    }

    public class AddressLine
    {
        public int lineNumber { get; set; }
        public string text { get; set; }
    }

    public enum PostcodeType
    {
        SmallUser = 0,
        LargeUser,
        Unknown
    }
}