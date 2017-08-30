using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Models
{
    public class XmasSaverVoucherSavedViewModel
    {
        Decimal _totalVoucherSaver = 0;
        List<string> _statementDate = new List<string>();
        List<string> _voucherValue = new List<string>();


        public Decimal TotalVoucherSaver
        {
            get { return _totalVoucherSaver; }
            set { _totalVoucherSaver = value; }
        }

        public List<string> StatementDate
        {
            get { return _statementDate; }
            set { _statementDate = value; }
        }

        public List<string> VoucherValue
        {
            get { return _voucherValue; }
            set { _voucherValue = value; }
        }
    }
}
