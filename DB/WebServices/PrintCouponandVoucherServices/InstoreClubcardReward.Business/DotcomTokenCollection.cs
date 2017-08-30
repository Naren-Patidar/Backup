using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace InstoreClubcardReward.Business
{
    [Serializable]
    public class DotcomTokenCollection : TokenCollection
    {
        // reference product constructor
        public DotcomTokenCollection() : base() { }

        //Country Code parameter added by seema
        public override int CreateTokens(int bookingid, string clubcard, ProductLine productLine, int agentid,string country)
        {
            // use responsecode - not using webservice
            int responsecode = 0;
            
            // set up the tokens in the TokenCollection
            // set up the message for creating the token
            int j = 0; // token counter
            int k = 0; // productline counter
            
                // create a node for each voucher required
                for (int i = 0; i < productLine.ProductNumber; i++)
                {

                    // sets the productline id from the outer counter j
                    DotcomToken token = new DotcomToken(k);
                    // record some parameters
                    token.ProductCode = productLine.Product.ProductCode;
                    token.TokenValue = productLine.Product.TokenValue; // tokenValue from product data

                    token.ProductLineId = productLine.ProductLineId;

                    // record the product used by date
                    token.UsedByDate = productLine.Product.UsedByDate;

                    // save the token, also creates the TokenId used for double up token
                    token.SaveToToken(bookingid);
                    this.Items.Add(token);

                    // if false returned when creating mark as a problem
                    if (!token.GetSupplierTokenCode())
                    {
                        //Exception thrown in  GetSUpplierTokenCode
                        responsecode = 1; // still mark as a problem
                    }


                    // overall counter (keep going for each inner loop)
                    j++;
                }
                //increment productline number
                k++;

            return responsecode;
        }

        public override bool CancelTokens(int bookingid, string clubcard, int agentid)
        {
            // dotcom tokens do not have any functions to cancel
            // this is a batch process outside of the application
            return true;
        }

    }
}