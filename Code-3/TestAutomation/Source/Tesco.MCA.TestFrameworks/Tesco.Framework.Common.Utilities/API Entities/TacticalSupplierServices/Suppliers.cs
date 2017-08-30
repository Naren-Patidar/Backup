using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Tesco.Framework.Common.Utilities.API_Entities.TacticalSupplierServices
{
    [DataContract]
    public class Suppliers
    {
        private string _gtin;
        private List<Supplier> _suppliers;

        [DataMember]
        public string GTIN
        {
            get { return _gtin; }
            set { _gtin = value; }
        }

        [DataMember(Name = "SuppliersList")]
        public List<Supplier> lstSuppliers
        {
            get { return _suppliers; }
            set { _suppliers = value; }
        }
    }
}
