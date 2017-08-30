using System.Collections;
using InstoreClubcardReward.Data;
using System.Collections.ObjectModel;

namespace InstoreClubcardReward.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Configuration;

    /// <summary>
    /// 
    /// </summary>
    /// 
    [Serializable]
    public class TokenCollection : BaseCollection<Token>
    {
        public TokenCollection()
        {
        }


        /// <summary>
        /// Creates the tokens.
        /// </summary>
        /// <param name="bookingid">The bookingid.</param>
        /// <param name="clubcard">The clubcard.</param>
        /// <param name="productLine">The product line.</param>
        /// <param name="agentid">The agentid.</param>
        /// <returns></returns>
        /// Country Code parameter Added by seema
        public virtual int CreateTokens(int bookingid, string clubcard, ProductLine productLine, int agentid,string country)
        {
            try
            {

                // this creates the token
                // payment will have been taken. The produst required saved.
                // The inputs required are the clubcard and the productlines
                // constant for the token create message
                const int RequestType = 1;

                // Code lifted from Print.cDoubleUp.CreateSmartToken

                int responsecode = -1;
                // individual voucher messages recorded against the 
                // voucher
                InstoreClubcardReward.NGC.cDoubleUpRequest request = new InstoreClubcardReward.NGC.cDoubleUpRequest();
                // node array - start off with a value
                InstoreClubcardReward.NGC.cDoubleUpRequestNode[] RequestNode = new InstoreClubcardReward.NGC.cDoubleUpRequestNode[0];
                // node to add
                InstoreClubcardReward.NGC.cDoubleUpRequestNode RequestNodeItem;

                
                // set up the tokens in the TokenCollection
                // set up the message for creating the token
                int j = 0; // token counter
                int k = 0; // productline counter


                    // create a node for each voucher required
                    for (int i = 0; i < productLine.ProductNumber; i++)
                    {

                        // sets the productline id from the outer counter j
                        Token token = new Token(k);
                        // record some parameters
                        token.ProductCode = productLine.Product.ProductCode;
                        token.TokenValue = productLine.Product.TokenValue; // tokenValue from product data

                        token.ProductLineId = productLine.ProductLineId;

                        // save the token, also creates the TokenId used for double up token
                        token.SaveToToken(bookingid);
                        this.Items.Add(token);
                        // record vendor code in token object
                        token.VendorCode = int.Parse(productLine.Product.VendorCode);
                        // record the product used by date
                        token.UsedByDate = productLine.Product.UsedByDate;


                        // build the request node
                        RequestNodeItem = new InstoreClubcardReward.NGC.cDoubleUpRequestNode();
                        RequestNodeItem.ClubcardNo = clubcard;
                        RequestNodeItem.RequestType = RequestType;
                        RequestNodeItem.ExpiryDate = Items[j].GetExpiryDate();  // rolling 3 months (read from data?)
                        // customer value ---- Freetime is a product parameter, fixed for thisformat 10.0
                        RequestNodeItem.Value = Items[j].GetTokenCustomerValue().ToString("##.0");    //##### does it need formating more?         

                        RequestNodeItem.VendorCode = token.VendorCode;                                                /// 
                        // resize the array (one more than zero based index)
                        Array.Resize<InstoreClubcardReward.NGC.cDoubleUpRequestNode>(ref RequestNode, j + 1);
                        RequestNode[j] = RequestNodeItem;

                        // overall counter (keep going for each inner loop)
                        j++;
                    }
                    //increment productline number
                    k++;

                // add the contructed array of request nodes
                request.RequestNodes = RequestNode;

                // call the request
                // TBD This is for freetime.... agentid, saving messages to database
                //Country Code parameter Added by seema
                string response = request.callService(agentid, country);


                // examine response
                InstoreClubcardReward.NGC.cDoubleUpResponse responseobj = ParseDUResponse(response, RequestType);

                bool CreateNodeProblem = false;  // set when responsecode on a node is not 0 

                if (responseobj == null)
                {
                }
                else
                {
                    // record response ######## FREETIME DB
                    responseobj.Save();

                    // record the repsonse code.... used for output
                    responsecode = responseobj.TransactionResponseCode;


                    // main message ok.... so can look at the nodes
                    if (responseobj.TransactionResponseCode == 0)
                    {

                        if (responseobj.ResponseNodes == null)
                        {
                            // error
                        }
                        else
                        {
                            // loop through the response nodes
                            int i = 0;
                            foreach (InstoreClubcardReward.NGC.cDoubleUpResponseNode responsenode in responseobj.ResponseNodes)
                            {
                                // record the result in the couponarray
                                Items[i].ResponseCode = (int)responsenode.ResponseCode;

                                // potentially only record when responsecode is zero
                                // TBD ######### make sure 0 is set and not just happens
                                if (responsenode.ResponseCode != 0)
                                {
                                    // quick flag rather than having to go through 
                                    // CreateNodeProblem = true;

                                    if (responsenode.ResponseCode == 27)
                                    {
                                        throw new BookingException(ErrorTypes.CreateTokenForClubcard,
                                               string.Format("Response code {0} returned from Double Up Webservice", responsenode.ResponseCode));
                                    }
                                    else
                                    {
                                        throw new BookingException(ErrorTypes.CreateToken,
                                                string.Format("Response code {0} returned from Double Up Webservice", responsenode.ResponseCode));
                                    }

                                }
                                else
                                {
                                    // only record if token created
                                    Items[i].Alpha = responsenode.AlphaNumericID;
                                    Items[i].EAN = responsenode.VoucherNo;

                                    // save to the suppliertokencode
                                    Items[i].SaveToSupplierTokenCode();
                                }
                                i++;
                            }

                        }

                        //if (CreateNodeProblem)
                        //{
                        //    // a problem
                        //    // TBD ##############
                        //    // 27 is fixable with another clubcard
                        //    // 29 normally worth doing another message - fixed 10/3/9

                        //    // cancel reverse if necessary
                        //}

                        // message result
                    }
                }

                return responsecode;
            }
            catch (BookingException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new BookingException(ErrorTypes.CreateToken, ex.Message, ex);
            }
        }


        // from vb cDoubleUp.parseDUResponse
        /// <summary>
        /// Parses the DU response.
        /// </summary>
        /// <param name="strResponse">The STR response.</param>
        /// <param name="RequestType">Type of the request.</param>
        /// <returns></returns>
        private InstoreClubcardReward.NGC.cDoubleUpResponse ParseDUResponse(String strResponse, int RequestType)
        {
            InstoreClubcardReward.NGC.cDoubleUpResponse responseobj = null;
            try
            {
                // if response is nothing then IL has not provided a response eg could not get to IL
                if (strResponse == null)
                {
                }
                else
                {
                    // examine the response
                    // deserialize to the response object
                    responseobj = InstoreClubcardReward.NGC.cDoubleUpResponse.readXML(strResponse, RequestType);
                    if (responseobj == null)
                        throw new BookingException(ErrorTypes.InvalidTokenResponse, "parseDUResponse: response object is nothing");
                    else
                        if (responseobj.TransactionResponseCode != 0)
                            //Throw new exception
                            throw new BookingException(ErrorTypes.InvalidTokenResponse,"Error from IL - response code: " + responseobj.TransactionResponseCode.ToString());
                        else
                            responseobj.Save();
                }
            }
            catch (Exception ex)
            {
                throw new BookingException(ErrorTypes.InvalidTokenResponse, "", ex);
            }
            return responseobj;

        }

        public virtual bool CancelTokens(int bookingid, string clubcard, int agentid)
        {
            const int RequestType = 2;  // Cancel

            int responsecode = -1;
            // individual voucher messages recorded against the 
            // voucher
            InstoreClubcardReward.NGC.cDoubleUpRequest request = new InstoreClubcardReward.NGC.cDoubleUpRequest();
            // node array - start off with a value
            InstoreClubcardReward.NGC.cDoubleUpRequestNode[] RequestNode = new InstoreClubcardReward.NGC.cDoubleUpRequestNode[0];
            // node to add
            InstoreClubcardReward.NGC.cDoubleUpRequestNode RequestNodeItem;


            int tokenRequests=0;
            // build the request
            foreach (Token t in Items)
            {

                if (!string.IsNullOrEmpty(t.EAN))
                {
                    // build the request node
                    RequestNodeItem = new InstoreClubcardReward.NGC.cDoubleUpRequestNode();
                    RequestNodeItem.RequestType = RequestType;
                    // customer value ---- Freetime is a product parameter, fixed for thisformat 10.0
                    //RequestNodeItem.ClubcardNo = clubcard;
                    RequestNodeItem.Value = t.GetTokenCustomerValue().ToString("##.0");    //##### does it need formating more?         
                    RequestNodeItem.VoucherNo = t.EAN;
                    //RequestNodeItem.VendorCode = t.VendorCode;  
                    // resize the array (one more than zero based index)
                    Array.Resize<InstoreClubcardReward.NGC.cDoubleUpRequestNode>(ref RequestNode, tokenRequests + 1);
                    RequestNode[tokenRequests] = RequestNodeItem;

                    tokenRequests++;
                }
            }

            if (tokenRequests > 0)
            {
                // add the contructed array of request nodes
                request.RequestNodes = RequestNode;


                // call the request
                // TBD This is for freetime.... agentid, saving messages to database
                string response = request.callService(agentid, ConfigurationManager.AppSettings["Country"]);


                // examine response
                InstoreClubcardReward.NGC.cDoubleUpResponse responseobj = ParseDUResponse(response, RequestType);

                bool CreateNodeProblem = false;  // set when responsecode on a node is not 0 

                if (responseobj == null)
                {
                }
                else
                {
                    // record response ######## FREETIME DB
                    responseobj.Save();

                    // record the repsonse code.... used for output
                    responsecode = responseobj.TransactionResponseCode;


                    // main message ok.... so can look at the nodes
                    if (responseobj.TransactionResponseCode == 0)
                    {
                        if (responseobj.ResponseNodes == null)
                        {
                            // error
                        }
                        else
                        {
                            // loop through the response nodes
                            int i = 0;
                            foreach (InstoreClubcardReward.NGC.cDoubleUpResponseNode responsenode in responseobj.ResponseNodes)
                            {
                                // record the result in the couponarray
                                Items[i].ResponseCode = (int)responsenode.ResponseCode;

                                // potentially only record when responsecode is zero
                                // TBD ######### make sure 0 is set and not just happens
                                if (responsenode.ResponseCode != 0)
                                {
                                    // quick flag rather than having to go through 
                                    CreateNodeProblem = true;
                                }
                                else
                                {
                                    // successful cancellation 
                                    // TODO Record ####################
                                }
                                i++;
                            }

                        }
                    }
                }
            }
            
            // TODO Sort what happens on failure
            return true;
        }





        /// <summary>
        /// Gets the tokens.
        /// </summary>
        /// <param name="bookingid">The bookingid.</param>
        /// <returns></returns>
        public static TokenCollection GetTokens(int bookingid, int userId)
        {

            Collection<Data.SelectSupplierTokenCodesByBookingIdRow> stcr = null;
            try
            {
                SelectSupplierTokenCodesByBookingId stc = new SelectSupplierTokenCodesByBookingId(ConnectionString);
                stc.BookingId = bookingid;
                stc.UserId = userId;
                stcr = stc.Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving tokens", ex);
            }

            TokenCollection tokens = new TokenCollection();
            try
            {
                foreach (SelectSupplierTokenCodesByBookingIdRow token in stcr)
                {
                    tokens.Items.Add(new
                        Token((string)token.SupplierTokenId,
                                (string)token.SupplierTokenCode,
                                (string)token.SupplierCode,
                                (DateTime)token.SupplyDate,
                                (DateTime)token.CustomerDate,
                                (DateTime)token.EndDate,
                                (int)token.ProductLineId,
                                (int)token.TokenId));
                }

                return tokens;
            }
            catch (Exception ex)
            {
                throw new Exception("Error populating token", ex);
            }

        }




    }
}
