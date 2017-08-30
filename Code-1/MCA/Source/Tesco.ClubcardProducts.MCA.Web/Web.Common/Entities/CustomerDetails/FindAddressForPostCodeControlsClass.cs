using System.Collections;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.CustomerDetails
{
    public class FindAddressForPostCodeControlsClass 
    {
        #region Private Fields

        private ArrayList _fullAddresses;
        private bool _isStreetAvailable = true;
        private bool _isLocalityAvailable = false;
        private bool _isCountyAvailable = false;
        private string _defaultTown;
        private string _defaultStreet;
        private string _defaultLocality;
        private string _defaultCounty;
        private int _tableCountInDataSet;

      
        #endregion

        #region Properties


        public int TableCountInDataSet
        {
            get { return _tableCountInDataSet; }
            set { _tableCountInDataSet = value; }
        }
        public bool IsStreetAvailable
        {
            get { return _isStreetAvailable; }
            set { _isStreetAvailable = value; }
        }
        public string DefaultTown
        {
            get { return _defaultTown; }
            set { _defaultTown = value; }
        }
        public bool IsLocalityAvailable
        {
            get { return _isLocalityAvailable; }
            set { _isLocalityAvailable = value; }
        }
        public string DefaultLocality
        {
            get { return _defaultLocality; }
            set { _defaultLocality = value; }
        }
        

        public bool IsCountyAvailable 
        {
            get { return _isCountyAvailable; }
            set { _isCountyAvailable = value; }
        }
       

        public string DefaultCounty
        {
            get { return _defaultCounty; }
            set { _defaultCounty = value; }
        }

        public ArrayList FullAddresses
        {
            get { return _fullAddresses; }
            set { _fullAddresses = value; }
        }
        public string DefaultStreet
        {
            get { return _defaultStreet; }
            set { _defaultStreet = value; }
        }
       
        #endregion

    }
}
