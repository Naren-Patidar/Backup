using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.RestClient.Contracts;
using Tesco.ClubcardProducts.MCA.Web.RestClient;

namespace Tesco.Framework.UITesting.Services
{
    public class AddressAPIAdapter : BaseAdaptor
    {
        private readonly IRestProxies _restClientManager = null;
        private string _APIUrl = String.Empty;

        public AddressAPIAdapter(string url)
        {
            this._APIUrl = url;
            this._restClientManager = new RestProxies();
        }

        public bool IsValidPostCode(string p)
        {
            try
            {
                return this.SearchPostcodes(p).Count > 0;
            }
            catch
            {
                return false;
            }
        }

        public List<string> SearchPostcodes(string p)
        {
            List<string> res = new List<string>();

            var postCodes = this._restClientManager.RestGet<List<PostCode>>(String.Format("{0}?postcode={1}", this._APIUrl, p), "");
            if (postCodes != null)
            {
                return postCodes.Select(pc => pc.postcode).ToList<string>();
            }

            return new List<string>();
        }

        public List<AddressSummary> SearchAddresesByPostcode(string p)
        {
            //p = "sw128tp";
            return this._restClientManager.RestGet<List<AddressSummary>>(String.Format("{0}/{1}/addresses?providerSpecific=true", this._APIUrl, p), "");
        }
    }

    public class PostCode
    {
        public string postcode { get; set; }
        public string description { get; set; }
        public string postTown { get; set; }
    }

    public class AddressSummary
    {
        public string id { get; set; }
        public List<AddressLineEntry> addressLines { get; set; }
        public string postcode { get; set; }
        public string postTown { get; set; }
        public royalMailSpecificDetails royalMailSpecificDetails { get; set; }
    }

    public class AddressLineEntry
    {
        public int lineNumber { get; set; }
        public string value { get; set; }
    }

    public class royalMailSpecificDetails
    {
        public string dependentLocality { get; set; } //Locality
        public string thoroughfareName { get; set; } //Street
        public string thoroughfareDescriptor { get; set; }
    }
}
