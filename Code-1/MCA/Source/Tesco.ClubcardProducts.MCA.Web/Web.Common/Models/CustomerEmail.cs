using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Models
{
    public class CustomerEmail
    {
        string _email = string.Empty;
        string _validEmail = string.Empty;
        string _confirmEmail = string.Empty;
        bool _isValid = true;
        
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public string ValidEmail
        {
            get { return _validEmail; }
            set { _validEmail = value; }
        }

        public string ConfirmEmail
        {
            get { return _confirmEmail; }
            set { _confirmEmail = value; }
        }

        public bool IsValid
        {
            get { return _isValid; }
            set { _isValid = value; }
        }

        public bool HasEmail
        {
            get { return !string.IsNullOrEmpty(_email); }
        }
    }
}
