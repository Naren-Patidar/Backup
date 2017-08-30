using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.Identity
{
    public class LegacyLookupReq
    {
        public string client_id { get; set; }
        public string uuid { get; set; }
        public List<string> fields { get; set; }
    }

    public class LegacyLookupRes
    {
        public List<fields> fields { get; set; }
    }

    public class fields
    {
        public string Id { get; set; }
        public string Value { get; set; }
    }
}
