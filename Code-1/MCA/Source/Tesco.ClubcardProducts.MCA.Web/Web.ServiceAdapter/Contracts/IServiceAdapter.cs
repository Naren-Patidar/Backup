using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service;

namespace Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts
{
    public interface IServiceAdapter
    {
        MCAResponse Get<T>(MCARequest request);
        MCAResponse Set<T>(MCARequest request);
        MCAResponse Delete<T>(MCARequest request);
        MCAResponse Execute(MCARequest request);
    }
}
