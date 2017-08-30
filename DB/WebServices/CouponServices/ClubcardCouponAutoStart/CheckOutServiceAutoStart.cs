using System;
using System.Collections.Generic;
using System.Web.Hosting;
using Tesco.Marketing.IT.ClubcardCoupon.CheckOutService;
using Tesco.Marketing.IT.ClubcardCoupon.DataContract;

namespace Tesco.ClubcardCoupon.AutoStart
{
    public class CheckOutServiceAutoStart : IProcessHostPreloadClient
    {
        public void Preload(string[] parameters)
        {
            try
            {
                ClubcardCouponCheckOutService obj = new ClubcardCouponCheckOutService();
                List<CheckOutRequest> aaa = new List<CheckOutRequest>();
                CheckOutRequest req = new CheckOutRequest();
                req.SessionId = "IISAutoStart";
                req.CashierNumber = "1";
                req.SmartBarcodeNumber = "0";
                req.TxnTimeStamp = DateTime.Today;
                req.StoreNumber = 2;
                req.TillType = 1;
                req.TillId = 1;
                req.RedemptionChannel = CouponCheckOutChannel.Storeline;
                req.IsOfflineTransaction = true;
                req.IsReversal = false;
                aaa.Add(req);
                obj.ProcessCheckOut(aaa);
            }
            catch { }
        }
    }
}
