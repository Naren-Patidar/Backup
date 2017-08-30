using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Tesco.Framework.Common.Utilities.API_Entities.TacticalSupplierServices
{
    public class PackagingDate
    {
        #region PRIVATE MEMBERS

        private string _month;
        private int _year;

        #endregion

        #region PROPERTIES
        public string Month
        {
            get { return _month; }
            set { _month = value; }
        }

        public int Year 
        { 
            get { return _year; }
            set { _year = value; }
        }
        #endregion
    }
}
