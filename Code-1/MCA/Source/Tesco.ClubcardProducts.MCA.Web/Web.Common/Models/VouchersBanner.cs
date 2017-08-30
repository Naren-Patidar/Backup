using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Models
{
    public class VouchersBanner
    {
        Int32 _OptedScheme;
        string _VoucherValue = string.Empty;
        bool _IsSecurityCheckDone = false;

        public Int32 OptedScheme
        {
            get { return _OptedScheme; }
            set { _OptedScheme = value; }
        }

        public string VoucherValue
        {
            get { return _VoucherValue; }
            set { _VoucherValue = value; }
        }

        public bool IsSecurityCheckDone
        {
            get { return _IsSecurityCheckDone; }
            set { _IsSecurityCheckDone = value; }
        }
    }
    
}
