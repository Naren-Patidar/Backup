namespace InstoreClubcardReward.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using InstoreClubcardReward.NGC.Freetime.AuthorisationGatewayAdapter;
    using InstoreClubcardReward.NGC;
    using System.Configuration;
    using System.Data;


    [Serializable]
    public class UnusedVoucherCollection : BaseCollectionPrintVoucher<UnusedVoucher>
    {
        public UnusedVoucherCollection()
        {
        }
        public static UnusedVoucherCollection GetUnusedVoucherDetails(string clubcard, string country)
        {
            try
            {
                callClubcardOnlineGetUnusedVoucherDetails getUnusedVoucherDetail = new callClubcardOnlineGetUnusedVoucherDetails();
                DataSet ds = new DataSet();
                GetUnusedVoucherDtlsRsp response = new GetUnusedVoucherDtlsRsp();
                UnusedVoucherCollection vouchers = new UnusedVoucherCollection();
                UnusedVoucher voucher;


                response = getUnusedVoucherDetail.GetUnusedVoucherDtls(clubcard, country);
                ds = response.dsResponse;

                int cnt;
                cnt = ds.Tables[0].Rows.Count;

                if (cnt > 0)
                {
                    for (int i = 0; i < cnt; i++)
                    {
                        voucher = new UnusedVoucher();
                        voucher.HouseholdId = ds.Tables[0].Rows[i][0].ToString();
                        voucher.PeriodName = ds.Tables[0].Rows[i][1].ToString();
                        voucher.VoucherValue = Convert.ToDecimal(ds.Tables[0].Rows[i][2].ToString());
                        voucher.VoucherNumber = ds.Tables[0].Rows[i][3].ToString();
                        voucher.OnlineCode = ds.Tables[0].Rows[i][4].ToString();
                        voucher.ExpiryDate = DateTime.ParseExact(ds.Tables[0].Rows[i][5].ToString(), "dd/MM/yy", null);
                        //voucher.Expiry_Date = Convert.ToDateTime(ds.Tables[0].Rows[i][5].ToString());
                        voucher.VoucherType = Convert.ToInt32(ds.Tables[0].Rows[i][6].ToString());
                        voucher.Ean = ds.Tables[0].Rows[i][7].ToString();
                        vouchers.Add(voucher);
                    }
                }

                //return response1;
                return vouchers;
            }
            catch (Exception ex)
            {
                BookingPrintVoucher bpv = new BookingPrintVoucher();
                bpv.SaveToTransError("UnusedVoucherCollection - GetUnusedVoucherDetails() - Error during Smart Voucher Web Service call " + ex.ToString());
                //BookingPrintVoucher.SaveToTransError("VoucherDetails PageLoad() - Error during Smart Voucher Web Service call " + ex.ToString());
                throw ex;
            }


        }

        // save the vouchers for the booking
        public void Save(int TransactionID)
        {

            try
            {
                foreach (UnusedVoucher voucher in Items)
                {
                    voucher.Save(TransactionID);
                }
            }
            catch (Exception ex)
            {
                throw ex; // new BookingException(ErrorTypes.InsertVoucher, "", ex);
            }

        }
        /// <summary>
        /// Gets the total.
        /// </summary>
        /// <param name="usedVouchersOnly">if set to <c>true</c> [used vouchers only].</param>
        /// <returns></returns>
        public Decimal GetTotal()
        {
            // from ean get the value - not from a call but from form of the ean
            Decimal value = 0;
            foreach (UnusedVoucher cv in Items)
            {
                value += cv.VoucherValue;

            }
            return value;
        }

    }
}
