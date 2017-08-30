using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service
{
    [Serializable]
    public class MCARequest
    {
        Dictionary<string, object> _parameters = new Dictionary<string, object>();

        public Dictionary<string, object> Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }
    }
}
