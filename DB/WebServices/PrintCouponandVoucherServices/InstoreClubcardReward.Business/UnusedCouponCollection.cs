using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InstoreClubcardReward.NGC.Freetime.AuthorisationGatewayAdapter;
using InstoreClubcardReward.NGC;
using System.Configuration;
using System.Data;

namespace InstoreClubcardReward.Business
{
    [Serializable]
    public class UnusedCouponCollection : BaseCollectionPrintVoucher<UnusedCoupon>
    {
         public UnusedCouponCollection()
        {
        }

         public void Save(int TransactionID)
         {

             try
             {
                 foreach (UnusedCoupon coupon in Items)
                 {
                     coupon.Save(TransactionID);
                 }
             }
             catch (Exception ex)
             {
                 throw ex; // new BookingException(ErrorTypes.InsertVoucher, "", ex);
             }

         }
    }
}
