﻿using System;
using System.Data;
using System.Xml.Linq;
using System.Linq;
using Tesco.ClubcardProducts.MCA.API.Common.Utilities;
using System.Collections.Generic;

namespace Tesco.ClubcardProducts.MCA.API.Common.Entities.Boost
{
    public class RewardAndToken
    {
        public List<Reward> Rewards { get; set; }
        public List<Token> Tokens { get; set; }
    }  
}
