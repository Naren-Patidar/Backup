using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.Data;

namespace Tesco.ClubcardProducts.MCA.API.Common.Entities
{
    public class AddressByPostCode
    {
        public AddressByPostCode()
        {

        }

        # region Private Fields

        private string _buildingNumber = string.Empty;
        private string _buildingName = string.Empty;
        private string _street = string.Empty;
        private string _subBuilding = string.Empty;
        private string _organisation = string.Empty;
        private string _locality = string.Empty;
        private string _town = string.Empty;
        private string _country = string.Empty;
        string _gridEast = string.Empty;
        string _gridNorth = string.Empty;
        bool _isBusinessAddress = false;
        bool _isBlockedAddress = false;

        #endregion Private Fields

        #region Public Properties

        public string Street
        {
            get { return _street; }
            set { _street = value; }
        }

        public string BuildingName
        {
            get { return _buildingName; }
            set { _buildingName = value; }
        }

        public string BuildingNumber
        {
            get { return _buildingNumber; }
            set { _buildingNumber = value; }
        }

        public string SubBuilding
        {
            get { return _subBuilding; }
            set { _subBuilding = value; }
        }

        public string Organisation
        {
            get { return _organisation; }
            set { _organisation = value; }
        }

        public string Locality
        {
            get { return _locality; }
            set { _locality = value; }
        }

        public string Town
        {
            get { return _town; }
            set { _town = value; }
        }

        public string Country
        {
            get { return _country; }
            set { _country = value; }
        }

        public string GridEast
        {
            get { return _gridEast; }
            set { _gridEast = value; }
        }

        public string GridNorth
        {
            get { return _gridNorth; }
            set { _gridNorth = value; }
        }

        public bool IsBusinessAddress
        {
            get { return _isBusinessAddress; }
            set { _isBusinessAddress = value; }
        }

        public bool IsBlockedAddress
        {
            get { return _isBlockedAddress; }
            set { _isBlockedAddress = value; }
        }

        #endregion

    }

    public class AddressByPostCodeList : BaseEntity<AddressByPostCodeList>
    {
        public AddressByPostCodeList()
        {
            this._addressByPostCodeList = new List<AddressByPostCode>();
        }

        List<AddressByPostCode> _addressByPostCodeList = null;
        public List<AddressByPostCode> AddressByPostCodeLists
        {
            get { return _addressByPostCodeList; }
            set { _addressByPostCodeList = value; }
        }

        public override void ConvertFromDataset(DataSet ds)
        {
            XDocument xDoc = XDocument.Parse(ds.GetXml());
            this.ConvertFromXml(xDoc.ToString());
        }

        public override void ConvertFromXml(string xml)
        {
            xml = xml.Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
            XDocument xDoc = XDocument.Parse(xml);
            _addressByPostCodeList = (from t in xDoc.Descendants("AddressLiteEntity")
                                      select new AddressByPostCode
                                      {
                                          BuildingNumber = t.Attribute(AdressByPostCodeEnum.BuildingName).Value.TryParse<string>(),
                                          BuildingName = t.Attribute(AdressByPostCodeEnum.BuildingNumber).Value.TryParse<string>(),
                                          Street = t.Attribute(AdressByPostCodeEnum.Street).Value.TryParse<string>(),
                                          SubBuilding = t.Attribute(AdressByPostCodeEnum.SubBuilding).Value.TryParse<string>(),
                                          Organisation = t.Attribute(AdressByPostCodeEnum.Organisation).Value.TryParse<string>(),
                                          Locality = t.Attribute(AdressByPostCodeEnum.Locality).Value.TryParse<string>(),
                                          Town = t.Attribute(AdressByPostCodeEnum.Town).Value.TryParse<string>(),
                                          Country = t.Attribute(AdressByPostCodeEnum.County).Value.TryParse<string>()
                                      }).ToList();
        }
    }
}
