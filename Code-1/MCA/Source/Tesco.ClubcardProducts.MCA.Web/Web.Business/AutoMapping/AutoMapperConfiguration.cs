using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;

namespace Tesco.ClubcardProducts.MCA.Web.Business.BusinessLogics
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            // specify the mapping profile
            Mapper.Initialize(x => x.AddProfile<ViewModelProfile>());
        }
    }
}
