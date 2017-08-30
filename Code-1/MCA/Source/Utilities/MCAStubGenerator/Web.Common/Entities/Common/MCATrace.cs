using System;
using System.Text;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities
{
    /// <summary>
    /// Its mandatory that the customerId is set to MCATrace class static variable before using this class objects.
    /// </summary>
    public class MCATrace : IDisposable
    {
        private string _invoker = string.Empty;
        private  string _customerId = string.Empty;
        private string _context = string.Empty;
        private bool _disposed = false;
     

        /// <summary>
        /// Context need not contain the customer id. As this is anyway going to be part of the logging.
        /// </summary>
        /// <param name="invoker"></param>
        /// <param name="context"></param>
        public MCATrace(string invoker, string context)
        {
            _invoker = invoker == null ? string.Empty : invoker;
            _context = context == null ? string.Empty : context;
            _customerId = MCACookie.Cookie[MCACookieEnum.CustomerID]; // Convert.ToString(CookieUtility.GetTripleDESEncryptedCookieValue(CookieEnum.CustomerID.ToString()));

            string info = "Start:" + _invoker + "\n CustomerID:" + _customerId + "\nContext: " + _context + "\n";

            //NGCTrace.NGCTrace.TraceInfo(info);
            //NGCTrace.NGCTrace.TraceDebug(info);
        }

        public void NoteException(Exception exp, string additionalContext)
        {
            string _additionalContext = additionalContext == null ? string.Empty : additionalContext;

            StringBuilder message = new StringBuilder(_invoker + "\n Customer Id: " + _customerId + "\nContext: " + _context + "\nAdditional Context:" + _additionalContext + "\nError Message:" + exp.ToString());

            //NGCTrace.NGCTrace.TraceCritical("Critical:" + message.ToString() );
            //NGCTrace.NGCTrace.TraceError("Error:" + message.ToString());
            //NGCTrace.NGCTrace.TraceWarning("Warning:" + message.ToString());
            //NGCTrace.NGCTrace.ExeptionHandling(exp);
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called. 
            if (!_disposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources. 
                if (disposing)
                {
                    string info = "End:" + _invoker + "\n CustomerID:" + _customerId + "\n";

                    //NGCTrace.NGCTrace.TraceInfo(info);
                    //NGCTrace.NGCTrace.TraceDebug(info);
                }

                // Note disposing has been done.
                _disposed = true;
            }
        }

        #endregion
    }
}
