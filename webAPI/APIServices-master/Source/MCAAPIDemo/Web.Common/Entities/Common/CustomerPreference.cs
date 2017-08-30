using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common
{
    public class CustomerPreference : BaseEntity<CustomerPreference> , ICloneable
    {
        public string Culture { get; set; }
        public Int64 CustomerID { get; set; }        
        public short CustomerPreferenceType { get; set; }
        public string EmailSubject { get; set; }
        public string IsDeleted { get; set; }
        public OptStatus POptStatus { get; set; }
        public string Status { get; set; }
        public short PreferenceID { get; set; }

        [JsonIgnore]
        [XmlIgnore]
        public CustomerPreference[] Preference { get; set; }

        public string PreferenceDescriptionEng { get; set; }
        public string PreferenceDescriptionLocal { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public string UserID { get; set; }
        public short Sortseq { get; set; }        

        public override void ConvertFromXml(string xml)
        {
            DateTime dtTemp = DateTime.Now;
            XDocument xDoc = XDocument.Parse(xml);
            Culture = xDoc.Element(XName.Get("Culture")).TryParse<string>();
            Preference = (from t in xDoc.Elements("CustomerPreference").Descendants("CustomerPreference")
                     select new CustomerPreference
                     {
                         CustomerID = t.Element("CustomerID").GetValue<Int64>(),
                         EmailSubject = t.Element("EmailSubject").GetValue<string>(),
                         IsDeleted = t.Element("IsDeleted").GetValue<string>(),
                         POptStatus = t.Element("POptStatus").GetValue<OptStatus>(),
                         CustomerPreferenceType = t.Element("CustomerPreferenceType").GetValue<Int16>(),
                         PreferenceDescriptionEng = t.Element("PreferenceDescriptionEng").GetValue<string>(),
                         PreferenceDescriptionLocal = t.Element("PreferenceDescriptionLocal").GetValue<string>(),
                         PreferenceID = t.Element("PreferenceID").GetValue<Int16>(),
                         Sortseq = t.Element("Sortseq").GetValue<Int16>(),
                         UpdateDateTime = t.Element("UpdateDateTime").GetValue<string>().TryParseDate(out dtTemp) ? dtTemp : DateTime.Now
                     }).ToArray();
        }

        #region ICloneable Members

        public object Clone()
        {
            CustomerPreference preference = (CustomerPreference)this.MemberwiseClone();
            return preference;
        }

        #endregion
    }
   
    public enum OptStatus : int
    {
        NOT_SELECTED = 0,
        OPTED_IN = 1,
        OPTED_OUT = 2,
    }

    public enum PreferenceType : int
    {
        NULL = 0,
        ALLERGY = 1,
        DIETARY = 2,
        CONTACT_METHOD = 3,
        PROMOTIONS = 4,
        REWARD = 5,
        PREFERRED_MAILING_ADDRESS = 6,
        MEDICAL = 7,
        COMMUNICATION_LANGUAGE = 8,
        DATA_PROTECTION = 9,
    }
}
