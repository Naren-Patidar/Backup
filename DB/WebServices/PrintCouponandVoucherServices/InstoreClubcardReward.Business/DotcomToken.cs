namespace InstoreClubcardReward.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using InstoreClubcardReward.Data;

    [Serializable]
    public class DotcomToken : Token
    {

        public DotcomToken(int productlineid)
            : base(productlineid)
        {
        }

        public bool GetSupplierTokenCode()
        {
            UpdateSupplierTokenCodeNext1 tc = new UpdateSupplierTokenCodeNext1(ConnectionString);
            tc.SupplierCode = this.ProductCode;
            tc.TokenId = this.TokenId;
            tc.CustomerDate = DateTime.Parse(this.GetCustomerDate());

           

            // execute
            try
            {
                tc.Execute();
            }
            catch(Exception ex)  
            {
                throw new BookingException(ErrorTypes.CreateDotComToken, "No Voucher returned",ex);            
            }

            if (!tc.SupplierTokenCode.IsNull)
            {
                this.Alpha = (string)tc.SupplierTokenCode;
                return true;
            }
            else
            {
                throw new BookingException( ErrorTypes.CreateDotComToken,"No Voucher returned");
            }
            return false;
        }

    }
}
