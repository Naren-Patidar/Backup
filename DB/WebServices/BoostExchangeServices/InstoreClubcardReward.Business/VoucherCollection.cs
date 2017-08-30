namespace InstoreClubcardReward.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using InstoreClubcardReward.NGC.Freetime.AuthorisationGatewayAdapter;
    using InstoreClubcardReward.NGC;
    using System.Configuration;

    [Serializable]
    public class VoucherCollection : BaseCollection<Voucher>
    {
        private const int RewardTokenCustomerPrice = 500;
        private const int RewardTokenValue = 1000;
        public VoucherCollection()
        {
        }
        /// <summary>
        /// Gets the total.
        /// </summary>
        /// <param name="usedVouchersOnly">if set to <c>true</c> [used vouchers only].</param>
        /// <returns></returns>
        public int GetTotal(bool usedVouchersOnly)
        {
            // from ean get the value - not from a call but from form of the ean
            int value = 0;
            foreach (Voucher cv in Items)
            {        
                if (usedVouchersOnly)
                {
                    if (cv.IsUsed)
                    {
                        value += cv.Value;
                    }
                }
                else
                {
                    value += cv.Value;
                }

            }
            return value;
        }


        /// <summary>
        /// Determines whether the ean is contained within the vouchers.
        /// </summary>
        /// <param name="ean">The ean.</param>
        /// <returns>
        /// 	<c>true</c> if the vouchers contain the specified ean; otherwise, <c>false</c>.
        /// </returns>
        public Boolean ContainsVoucher( string ean )
        {
            if (GetVoucherIndex(ean) != -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetVoucherIndex(string ean)
        {
            int foundIndex  = -1;

            // check for duplicate ean
            for (int i = 0; i < Items.Count; i++)
            {
            	if (this[i].Ean == ean)
                {
                    foundIndex = i;
                    break;
                }
            }

            return foundIndex;
        }

        /// <summary>
        /// Gets the reward voucher count.
        /// </summary>
        /// <returns></returns>
        public int GetRewardVoucherCount()
        {
            return this.GetTotal(false) / RewardTokenCustomerPrice;
        }

        /// <summary>
        /// Gets the reward voucher total.
        /// </summary>
        /// <returns></returns>
        public int GetRewardVoucherTotal()
        {
            return GetRewardVoucherCount() * RewardTokenValue;
        }

        /// <summary>
        /// Validates the vouchers.
        /// </summary>
        /// <param name="clubcard">The clubcard.</param>
        /// <param name="agentId">The agent id.</param>
        public void ValidateVouchers(string clubcard, int agentId, string country)
        {
            string response;
            ISmartVoucherRequest[] vRow;

            try 
            {
                // NOTE Channel set for Request instore exchange SR 23/7/10
                vRow = CreateNGCMessage(clubcard, RequestChannel.instoreexchange, country);

                response = CallVoucherRequest(vRow, SmartVoucherServiceRequestType.Validation, agentId, country);


                if (response.Length > 0)
                {
                    SmartVoucherResponseWrapper responseWrapper;

                    responseWrapper = SmartVoucherResponseWrapper.readXML(response);

                    if (responseWrapper != null)
                    {
                        if (responseWrapper.TransactionResponseCode == 91)
                        {
                            throw new Exception("Unable to access NGC");
                        }
                        else
                        {
                            if (responseWrapper.TransactionResponseCode == 0)
                            {
                                UpdateVoucherDetails(responseWrapper);
                            }
                        }


                    }
                  

                }

            }
	        catch (Exception)
            {		
	    	    throw;
	        }
           
        }

        /// <summary>
        /// Creates the NGC message.
        /// </summary>
        /// <param name="clubcard">The clubcard.</param>
        /// <param name="channel">The channel.</param>
        /// <param name="country">The country.</param>
        /// <returns></returns>
        private ISmartVoucherRequest[] CreateNGCMessage(string clubcard, RequestChannel channel, string country)
        {
            SmartVoucherRequestWrapper message = new SmartVoucherRequestWrapper(country);

            ISmartVoucherRequest[] vRow = new ISmartVoucherRequest[0];

            for (int i = 0; i < this.Count; i++)
            {
                Array.Resize(ref vRow, i+1);
                vRow[i] = new ISmartVoucherRequest();

                vRow[i].Ean = this[i].Ean;



                if (!string.IsNullOrEmpty(this[i].Alpha))
                {
                    vRow[i].AlphaNumericID = this[i].Alpha;
                }
                else
                {
                    vRow[i].AlphaNumericID = "";
                }

                if (string.IsNullOrEmpty(clubcard))
                {
                    vRow[i].ClubcardNumber = this[i].Clubcard;
                }
                else
                {
                    vRow[i].ClubcardNumber = clubcard;
                }

                vRow[i].Channel = SmartVoucherRequestWrapperNode.requestchannel((int)channel);
            }

            return vRow;
        }

        /// <summary>
        /// Calls the voucher request.
        /// </summary>
        /// <param name="vRow">The v row.</param>
        /// <param name="messageType">Type of the message.</param>
        /// <param name="agentId">The agent id.</param>
        /// <param name="country">The country.</param>
        /// <returns></returns>
        private String CallVoucherRequest(ISmartVoucherRequest[] vRow, SmartVoucherServiceRequestType messageType, int agentId, String country)
        {
            String response = "";
            if (vRow.Length == 0)
            {
                //Do nothing as no Smart vouchers
            }
            else
            {
                // add the array of requests
                SmartVoucherRequestWrapper message = new SmartVoucherRequestWrapper(vRow, messageType, country);
                try
                {
                    response = message.callVoucherRequest(agentId, country);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return response;
        }

        /// <summary>
        /// Updates the voucher details.
        /// </summary>
        /// <param name="responseObj">The response obj.</param>
        private void UpdateVoucherDetails(SmartVoucherResponseWrapper responseObj)
        {
            foreach (SmartVoucherResponseWrapperNode responseObjNode in responseObj.Responses)
            {
                
                    int index = this.GetVoucherIndex(responseObjNode.VoucherDetails.VoucherNo.ToString());

                    if (index != -1)
                    {
                        // If the voucher wasn't found then set the status to NotFound 
                        if (responseObjNode.ResponseCode == 7)
                        {
                            this[index].Status =  VoucherStatus.NotFound;
                        }
                        // otherwise populate the details
                        else
                        {
                            this[index].Status = (VoucherStatus)responseObjNode.VoucherDetails.Status;
                            this[index].Alpha = responseObjNode.VoucherDetails.AlphaNumericID;
                            this[index].Ean = responseObjNode.VoucherDetails.VoucherNo;
                            this[index].ResponseClubcard = responseObjNode.VoucherDetails.ClubcardNo;
                            this[index].ResponseValue = responseObjNode.VoucherDetails.Value;
                            this[index].Type = (VoucherTypes)responseObjNode.VoucherDetails.Type;

                            this[index].Channel = responseObjNode.VoucherDetails.VoucherUsage.Channel;
                            this[index].UseDateTime = responseObjNode.VoucherDetails.VoucherUsage.DateTime;
                            this[index].StoreNo = responseObjNode.VoucherDetails.VoucherUsage.StoreNo;
                            this[index].VirtualStore = responseObjNode.VoucherDetails.VoucherUsage.VirtualStore;
                            this[index].ExpiryDate = responseObjNode.VoucherDetails.ExpiryDate;

                            // check for an expired voucher (still show as active)
                            if (this[index].Status == VoucherStatus.Active && this[index].ExpiryDate < DateTime.Today)
                            {
                                this[index].Status = VoucherStatus.Expired;
                            }
                        }

                    }


            }
        }

        // save the vouchers for the booking
        public void Save(int BookingId)
        {

            try
            {
                foreach (Voucher voucher in Items)
                {
                    // only save if marked as use voucher
                    if (voucher.IsUsed)  // active voucher  (or redeemed on a cancel)
                    {
                        voucher.Save(BookingId);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BookingException(ErrorTypes.InsertVoucher, "", ex);
            }

        }

        public bool HasUnusableVouchers()
        {
            foreach (Voucher voucher in Items)
            {
                // voucher type set on adding only set to valid type for use
                // non zero checks for setting
                if (voucher.Status != VoucherStatus.Active || 
                    voucher.Type == 0 
                    )
                {
                    return true;
                }
            }
            return false;
        }

        public int GetUnusedTotal()
        {
            int total = 0;

            foreach (Voucher v in this)
            {
                if (!v.IsUsed)
                {
                    total += v.Value;
                }
            }

            return total;
           
        }

        public void UpdateUsedStatus(bool defaultValue)
        {
            for (int i = 0; i <= this.Count-1; i++)
            {
                this[i].IsUsed = defaultValue;
            }
        }




        // check the status of the voucher array 
        /// <summary>
        /// Checks the voucher array status.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public bool CheckVoucherCollectionStatus(VoucherStatus status)
        {
            if (this.Count == 0)
            {
                // no need to test an empty voucher collection
                return false;
            }

            foreach (Voucher cv in this)
            {
                // we're only interested in checking the vouchers which are to be used
                if (cv.IsUsed)
                {
                    if (cv.Status != status)
                    {
                        // did not match
                        return false;
                    }
                }
            }

            return true;
        }



        /// <summary>
        /// Processes the payment. Make public so test can use
        /// </summary>
        /// <returns></returns>
        /// Country code parameter added by seema 
        public void ProcessPayment( int agentid,string country )
        {

            // take the payment (reserve / redeem)
            int ResponseCode;
            try
            {
                // Country code parameter added by seema 
                ResponseCode = VoucherMessage(SmartVoucherServiceRequestType.Redemption, agentid, country);
            }
            catch(Exception ex)
            {
                throw new BookingException(ErrorTypes.ProcessPayment, "Voucher(s) redemption error:", ex);
            }


            if (ResponseCode == 0)
            {
                if (CheckVoucherCollectionStatus(VoucherStatus.Redeemed))
                {

                }
                else
                {
                    // correct or undo?
                    // TODO ############
                    // should only reverse those that are valid - need
                    // to have done a check for the starting status of vouchers and only move 
                    // those back
                    
                    //ResponseCode = -1;
                    throw new BookingException(ErrorTypes.ProcessPayment, "Voucher(s) returned a non-Redeemed status");
                }

            }
            else
            {
                // TBD Undo any reservation - message failed
                
                //ResponseCode = -1;
                throw new BookingException(ErrorTypes.ProcessPayment, "ResponseCode returns was not 0");
            }

            // reponsecode from smart vouchers passed back
            // or -1 if partial voucher processing and needed reversing
            // 0 is success
            //return ResponseCode;
        }


        /// <summary>
        /// Send the vouchers the message.
        /// check / reserve / redeem / unreserve  
        /// </summary>
        /// <param name="MessageType">Type of the message.</param>
        /// <param name="agentid">The agentid.</param>
        /// <returns></returns>
        /// Country code parameter added by seema 
        private int VoucherMessage(
                        SmartVoucherServiceRequestType MessageType, 
                        int agentid,string Country)
        {
            // Country code parameter added by seema 
            string country = Country;

            int responsecode = -1;
            // individual voucher messages recorded against the 
            // voucher

            // create the request array that is used in construction of the request
            // size to the size of voucher array
            ISmartVoucherRequest[] requestarray = new ISmartVoucherRequest[0];
            int i = 0;
            foreach (Voucher cv in this)
            {
                // Check to see whether the voucher is to be used
                if (cv.IsUsed)
                {
                    //If the voucher is to be used then resize the array to include the useable voucher details
                    Array.Resize(ref requestarray, i+1);
                    ISmartVoucherRequest smartrequest = new ISmartVoucherRequest();
                    // ean and clubcard are expect inputs - allow alpha as may be used
                    smartrequest.ClubcardNumber = cv.Clubcard;
                    smartrequest.Ean = cv.Ean;
                    // alpha may or may not be empty (ean used for redeem/reserver/unreserve)
                    if (!string.IsNullOrEmpty(cv.Alpha))
                    {
                        smartrequest.AlphaNumericID = cv.Alpha;
                    }
                    else
                    {
                        smartrequest.AlphaNumericID = "";
                    }
                    // channel for instore changed SR 23/7/10
                    // use available channels .... added new one for CSD smartvoucherrequestwrappernode 
                    
                    if (country == "ROI")
                    {
                        smartrequest.Channel = SmartVoucherRequestWrapperNode.requestchannel((int)RequestChannel.PostalDeals);
                    }
                    else
                    {
                        smartrequest.Channel = SmartVoucherRequestWrapperNode.requestchannel((int)RequestChannel.instoreexchange);    
                    }
                                   
                    // resize array for this voucher and then set
                    //Array.Resize<ISmartVoucherRequest>(ref requestarray, i);
                    requestarray[i] = smartrequest;
                    i++;
                }
            }
            // create the smart voucher request
            // ################## TBD this intialises for Freetime
            // source, branch, till etc needs to be in message
            SmartVoucherRequestWrapper sv = new SmartVoucherRequestWrapper(requestarray, MessageType, country);

            int messagetry = 0;
            int messageAttempLimit = 2; // set to 2 results in two message attempts
            do
            {
                // existing interface call 
                string result = sv.callVoucherRequest(agentid, country);

                if (result == null)
                {
                    // error
                }
                else
                {
                    SmartVoucherResponseWrapper responseobj;
                    responseobj = SmartVoucherResponseWrapper.readXML(result);
                    if (responseobj == null)
                    {
                        // not valid resonse xml
                    }
                    else
                    {

                        // record the response code - output
                        responsecode = responseobj.TransactionResponseCode;
                        if (responsecode == 91)
                        {
                            // standard AG error..freetime throw error
                        }
                        else if (responsecode == 0)
                        {
                            UpdateVoucherDetails(responseobj);
                            // message successful so exit the loop (set to limit)
                            //messagetry = messageAttempLimit;
                            break;
                        }
                    }

                }
                // increment the try count
                messagetry++;
            }
            while (messagetry < messageAttempLimit);

            // message result
            return responsecode;

        }

        /// <summary>
        /// Cancels the payments.
        /// </summary>
        /// <param name="bookingid">The bookingid.</param>
        /// <param name="agentid">The agentid.</param>
        /// <returns></returns>
        /// Country Code parameter added by seema
        public bool CancelPayments(int bookingid, int agentid,string country)
        {

            // reverse the payment

            // takes multiple unreserve (allow 4 for good measure
            int i = 0;


            try
            {
                do
                {
                    if (i == 4)
                    {
                        //return false;
                        throw new BookingException(ErrorTypes.CancelPayment, "Attempted message 4 times and failed");
                    }
                    //Country Code parameter added by seema
                    int ResponseCode = VoucherMessage(SmartVoucherServiceRequestType.Unreservation, agentid,country);
                    i++;

                } while (!CheckVoucherCollectionStatus(VoucherStatus.Active));

                //save the state of the booking
                this.Save(bookingid);
                return true;
            }
            catch (Exception ex)
            {
                throw new BookingException(ErrorTypes.CancelPayment, "", ex);
            }


        }

    }
}
