namespace InstoreClubcardReward.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.ComponentModel;
    using System.Configuration;

    [Serializable]
    public abstract class BaseCollection<T> : BindingList<T>
    {

        /// connection string from the applications web config
        protected static string ConnectionString = ConfigurationManager.ConnectionStrings["InstoreRewardBooking"].ConnectionString;//  ConnectionStrings["InstoreRewar
    }
}
