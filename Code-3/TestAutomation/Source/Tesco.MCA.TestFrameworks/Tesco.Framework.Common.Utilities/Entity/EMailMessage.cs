using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace Tesco.Framework.Common.Utilities.Entity
{
    public class EMailMessage
    {
        #region Private Members
        private string _mailFrom;
        private List<MailAddress> _mailTo;
        private string _subject;
        private string _body;
        private bool _isBodyHtml;
        #endregion

        #region Properties
        public string MailFrom
        {
            get { return _mailFrom; }
            set { _mailFrom = value; }
        }

        public List<MailAddress> MailTo
        {
            get { return _mailTo; }
            set { _mailTo = value; }
        }

        public string Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }

        public string Body
        {
            get { return _body; }
            set { _body = value; }
        }

        public bool IsBodyHtml
        {
            get { return _isBodyHtml; }
            set { _isBodyHtml = value; }
        }
        #endregion
    }
}
