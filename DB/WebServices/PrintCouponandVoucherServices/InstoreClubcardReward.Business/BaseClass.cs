using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;


namespace InstoreClubcardReward.Business
{
    [Serializable]
    public class BaseClass
    {
        /// connection string from the applications web config
        protected static string ConnectionString = ConfigurationManager.ConnectionStrings["InstoreRewardBooking"].ConnectionString;//  ConnectionStrings["InstoreReward"];

    }
}
