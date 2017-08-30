using System;
using System.Data;
using System.Xml.Linq;
using System.Linq;
using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
using System.Collections.Generic;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.Common
{
    [Serializable]
    public class CustomerDisplayName
    {
        public string TitleEnglish { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public string Name3 { get; set; }
    }

}