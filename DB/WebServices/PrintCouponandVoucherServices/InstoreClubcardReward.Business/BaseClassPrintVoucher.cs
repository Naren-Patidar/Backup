using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;


namespace InstoreClubcardReward.Business
{
    [Serializable]
    public class BaseClassPrintVoucher
    {
        /// connection string from the applications web config
        protected static string ConnectionString = ConfigurationManager.ConnectionStrings["PrintVouchers_KioskDB"].ConnectionString;//  ConnectionStrings["InstoreReward"];

    }
}
