using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace Tesco.Framework.Common.Utilities.Entity
{
    public class SMTPDetails
    {
        #region Private Members
        private string _smtpHost;
        private int _smtpPort;
        private bool _isSSLEnabled;
        private SmtpDeliveryMethod _deliveryMethod;
        private string _domain;
        private string _username;
        private string _password;
        #endregion

        #region Properties
        public string SmtpHost 
        {
            get { return _smtpHost;}
            set { _smtpHost = value; } 
        }

        public int SmtpPort 
        { 
            get { return _smtpPort;}
            set { _smtpPort = value; } 
        }

        public bool IsSSLEnabled
        { 
            get { return _isSSLEnabled; }
            set { _isSSLEnabled = value; } 
        }

        public SmtpDeliveryMethod DeliveryMethod
        {
            get { return _deliveryMethod; }
            set { _deliveryMethod = value;}
        }

        public string SmtpDomain
        {
            get { return _domain; }
            set { _domain = value; }
        }

        public string SmtpUserName
        {
            get { return _username; }
            set { _username = value; }
        }

        public string SmtpPassword
        {
            get { return _username; }
            set { _username = value; }
        }
        #endregion
    }
}
