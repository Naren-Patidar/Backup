namespace APITestClient.Helper
{
    public class MCACookie
    {
        private static MCACookie _instance = new MCACookie();

        private MCACookie()
        {
        }

        /// <summary>
        /// To be readonly
        /// </summary>
        public static MCACookie Cookie
        {
            get { return _instance; }
        }

        public string this[MCACookieEnum cookieName]
        {
            get
            {
                return CookieUtility.GetTripleDESEncryptedCookieValue(cookieName.ToString());
            }
        }

        public void Remove(MCACookieEnum cookieName)
        {
            CookieUtility.DeleteTripleDESEncryptedCookie(cookieName.ToString());
        }

        //public void SetCookie(MCACookieEnum cookieName, string value)
        //{
        //    CookieUtility.SetTripleDESEncryptedCookie(cookieName.ToString(), value);
        //}

        /// <summary>
        /// Change Details  : Add, this is added during security page Refactoring to add the cookie value.
        /// Changed By      : Swaraj Kumar Patra
        /// Reviewed By     : Srinivasa RaoPelluri (On 25/09/2014)
        /// Code Checked In : By Swaraj Patra (On 26/09/2014)
        /// Team Name       : 41_Digital_Clubcard_MCA (Vikings)
        /// </summary>
    
        public void Add(MCACookieEnum name, string value)
        {
            CookieUtility.DeleteTripleDESEncryptedCookie(name.ToString());
            CookieUtility.SetTripleDESEncryptedCookie(name.ToString(), value);
        }
    }
}
