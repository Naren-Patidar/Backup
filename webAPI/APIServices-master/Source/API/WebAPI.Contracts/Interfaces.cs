using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tesco.ClubcardProducts.MCA.API.Contracts
{
    public interface IServiceAdapter
    {
        APIResponse Execute(APIRequest request);
        Dictionary<string, object> GetSupportedOperations();
        string GetName();
    }
}
