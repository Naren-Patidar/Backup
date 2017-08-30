namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings
{
    public static class CustomerSpecificSecurityConfiguration
    {
        private static DbConfiguration _configuration = null;

        //private CustomerSpecificSecurityConfiguration()
        //{
        //}

        public static DbConfiguration Configuration
        {
            get { return _configuration; }
            set { _configuration = value; }
        }

    }
}
