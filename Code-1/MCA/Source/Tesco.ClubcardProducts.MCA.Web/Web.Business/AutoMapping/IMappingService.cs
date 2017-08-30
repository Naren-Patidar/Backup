using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tesco.ClubcardProducts.MCA.Web.Business.BusinessLogics
{
    public interface IMappingService
    {
        TDest Map<TSrc, TDest>(TSrc source) where TDest : class;
    }
}