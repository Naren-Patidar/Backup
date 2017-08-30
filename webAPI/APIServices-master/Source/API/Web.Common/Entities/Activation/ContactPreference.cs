using System;
using System.Runtime.Serialization;

namespace Tesco.ClubcardProducts.MCA.API.Common.Entities.Activation
{
    
    public class ContactPreference 
    {
        private string _wantTescoInfo;
        private string _wantPartnerInfo;
        private string _researchContactable;
        private string _contactable;
        
        public string WantTescoInfo { get { return _wantTescoInfo; } set { _wantTescoInfo = value; } }

        
        public string WantPartnerInfo { get { return _wantPartnerInfo; } set { _wantPartnerInfo = value; } }

        
        public string ResearchContactable { get { return _researchContactable; } set { _researchContactable = value; } }

        
        public string Contactable { get { return _contactable; } set { _contactable = value; } }



    }
}
