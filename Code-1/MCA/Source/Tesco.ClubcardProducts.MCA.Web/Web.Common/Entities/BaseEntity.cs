using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities
{
    [Serializable]
    public class BaseEntity<T> : ComparableEntity<T>
    {
        public virtual void ConvertFromDataset(DataSet ds) { }
        public virtual void ConvertFromDataTable(DataTable ds) { }
        public virtual void ConvertFromXml(string xml) { }
        public virtual void ConvertFromXml(string xml, string parent) { }
        public virtual void ConvertFrom<T>(T obj) { }

        internal override bool AreInstancesEqual(T target)
        {
            return true;
        }
    }
}
