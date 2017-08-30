namespace InstoreClubcardReward.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.ComponentModel;
    using System.Configuration;

    [Serializable]
    public abstract class BaseCollectionPrintVoucher<T> : BindingList<T>
    {

        /// connection string from the applications web config
        protected static string ConnectionString = ConfigurationManager.ConnectionStrings["PrintVouchers_KioskDB"].ConnectionString;//  ConnectionStrings["InstoreRewar
    }
}
