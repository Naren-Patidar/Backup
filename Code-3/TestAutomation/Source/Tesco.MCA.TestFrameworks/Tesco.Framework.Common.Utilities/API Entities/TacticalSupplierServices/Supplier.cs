using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesco.Framework.Common.Utilities.API_Entities.TacticalSupplierServices
{
    public class Supplier
    {
        #region PRIVATE MEMBERS

        private int _accountNumber;
        private int _accountDivisionNumber;
        private string _name;
        private string _comment;
        private string _status;

        #endregion

        #region PROPERTIES

        public int AccountNumber
        {
            get { return _accountNumber; }
            set { _accountNumber = value; }
        }

        public int AccountDivisionNumber
        {
            get { return _accountDivisionNumber; }
            set { _accountDivisionNumber = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Comment
        {
            get { return _comment; }
            set { _comment = value; }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        #endregion

    }
}
