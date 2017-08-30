using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Service;
using Tesco.ClubcardProducts.MCA.Web.Common.ResponseRecorder;

namespace Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts
{
    public interface IServiceAdapter
    {
        MCAResponse Get(MCARequest request);
        MCAResponse Set<T>(MCARequest request);
        MCAResponse Delete<T>(MCARequest request);
        MCAResponse Execute(MCARequest request);
        Recorder GetRecorder();
    }
}
