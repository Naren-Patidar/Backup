namespace Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Xml.Serialization;
    using System.Xml;
    using System.Configuration;
    using Tesco.ClubcardProducts.MCA.Web.ServiceAdapter.Contracts;
    using Tesco.ClubcardProducts.MCA.Web.Common.Entities;
    using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Settings;
    using Tesco.ClubcardProducts.MCA.Web.Common.Utilities;
    using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Vouchers;
    using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Ecoupon;
    using Tesco.ClubcardProducts.MCA.Web.Common.Entities.Boost;

    public class PopulateVD
    {
        static string dateformat = AppConfiguration.Settings[AppConfigEnum.DisplayDateFormat];
        public static List<VoucherDetails> getVouchers(List<VoucherDetails> voucherDetailsList, List<CustomerMLS_PDF_DownloadDetails> customerDetailsList)
        {
            string voucherDetailsListXml = SerializerUtility<List<VoucherDetails>>.GetSerializedString(voucherDetailsList);
            string customerDetailsListXml = SerializerUtility<List<CustomerMLS_PDF_DownloadDetails>>.GetSerializedString(customerDetailsList);

            MCATrace trace = new MCATrace("Adapters.PopulateVD.getVouchers", " voucherDetailsList:" + voucherDetailsListXml + " customerDetailsList:" + customerDetailsListXml);

            try
            {
                string strCurrencySymbol = string.Empty;
                string strCurrencyAllignment = string.Empty;
                string culture = string.Empty;

                CustomerMLS_PDF_DownloadDetails customerDetails = new CustomerMLS_PDF_DownloadDetails();
                if (customerDetailsList != null)
                {
                    customerDetails = customerDetailsList[0];
                }


                List<VoucherDetails> list = new List<VoucherDetails>();
                if (voucherDetailsList != null) // _ds.Tables.Count != 0)
                {
                    int _rowSelected = -1;
                    //if (ConfigurationManager.AppSettings["CultureDefaultloc"] == "en-IE")
                    if (culture == "en-IE")
                    {
                        //foreach (DataRow dr in _ds.Tables[0].Rows)
                        foreach (VoucherDetails obj in voucherDetailsList)
                        {
                            _rowSelected++;
                            VoucherDetails g__initLocal0 = new VoucherDetails
                            {

                                VoucherNumber = obj.VoucherNumber,//  _ds.Tables[0].Rows[_rowSelected][3].ToString(),
                                AlphaCode = obj.AlphaCode, //  _ds.Tables[0].Rows[_rowSelected][4].ToString(),
                                //ClubcardNumber = _custDetail.Tables[0].Rows[0]["ClubcardID"].ToString(),
                                //ExpiryDate = Convert.ToDateTime(obj.ExpiryDate).ToString(dateformat),//Convert.ToDateTime(_ds.Tables[0].Rows[_rowSelected][5].ToString()).ToString("dd/MM/yyyy"),


                                //Value = strCurrencySymbol + _ds.Tables[0].Rows[_rowSelected][2].ToString(),
                                Value = obj.Value, //  _ds.Tables[0].Rows[_rowSelected][2].ToString(),

                                //CustomerName = _customerName,
                                VoucherType = obj.VoucherType, //  _ds.Tables[0].Rows[_rowSelected][6].ToString(),
                                VoucherNumberToPrint = obj.VoucherNumberToPrint, //  _ds.Tables[0].Rows[_rowSelected][7].ToString(),

                                //New fields added for CR
                                PrimaryCustomerFirstname = customerDetails.PrimaryCustName1, //   _custDetail.Tables[0].Rows[0]["PrimaryCustName1"].ToString(),
                                PrimaryCustomerMiddlename = customerDetails.PrimaryCustName2, //   _custDetail.Tables[0].Rows[0]["PrimaryCustName2"].ToString(),
                                PrimaryCustomerLastname = customerDetails.PrimaryCustName3, //   _custDetail.Tables[0].Rows[0]["PrimaryCustName3"].ToString(),
                                PrimaryCustomerCardnumber = customerDetails.PrimaryClubcardId, //   _custDetail.Tables[0].Rows[0]["PrimaryClubcardID"].ToString(),
                                AssociateCustomerFirstname = customerDetails.AssociateCustName1,//   _custDetail.Tables[0].Rows[0]["AssociateCustName1"].ToString(),
                                AssociateCustomerMiddlename = customerDetails.AssociateCustName2, //   _custDetail.Tables[0].Rows[0]["AssociateCustName2"].ToString(),
                                AssociateCustomerLastname = customerDetails.AssociateCustName3, //   _custDetail.Tables[0].Rows[0]["AssociateCustName3"].ToString(),
                                AssociateCustomerCardnumber = customerDetails.AssociateClubcardId //   _custDetail.Tables[0].Rows[0]["AssociateClubcardID"].ToString()
                            };
                            list.Add(g__initLocal0);
                        }
                    }
                    else
                    {
                        //foreach (DataRow dr in _ds.Tables[0].Rows)
                        foreach (VoucherDetails obj in voucherDetailsList)
                        {
                            _rowSelected++;
                            VoucherDetails g__initLocal0 = new VoucherDetails
                            {

                                VoucherNumber = obj.VoucherNumber,// _ds.Tables[0].Rows[_rowSelected][3].ToString(),
                                AlphaCode = obj.AlphaCode, //  _ds.Tables[0].Rows[_rowSelected][4].ToString(),
                                //ClubcardNumber = _custDetail.Tables[0].Rows[0]["ClubcardID"].ToString(),
                                //ExpiryDate = Convert.ToDateTime(obj.ExpiryDate).ToString(dateformat),
                                //Convert.ToDateTime(_ds.Tables[0].Rows[_rowSelected][5].ToString()).ToString("dd/MM/yyyy"),
                                //Value = strCurrencySymbol + _ds.Tables[0].Rows[_rowSelected][2].ToString(),
                                Value = obj.Value,//  _ds.Tables[0].Rows[_rowSelected][2].ToString(),
                                //CustomerName = _customerName,
                                VoucherType = obj.VoucherType, //  _ds.Tables[0].Rows[_rowSelected][6].ToString(),
                                VoucherNumberToPrint = obj.VoucherNumberToPrint, //  _ds.Tables[0].Rows[_rowSelected][7].ToString(),

                                //New fields added for CR
                                PrimaryCustomerFirstname = customerDetails.PrimaryCustName1, //   _custDetail.Tables[0].Rows[0]["PrimaryCustName1"].ToString(),
                                PrimaryCustomerMiddlename = customerDetails.PrimaryCustName2, //   _custDetail.Tables[0].Rows[0]["PrimaryCustName2"].ToString(),
                                PrimaryCustomerLastname = customerDetails.PrimaryCustName3, //   _custDetail.Tables[0].Rows[0]["PrimaryCustName3"].ToString(),
                                PrimaryCustomerCardnumber = customerDetails.PrimaryClubcardId, //   _custDetail.Tables[0].Rows[0]["PrimaryClubcardID"].ToString(),
                                AssociateCustomerFirstname = customerDetails.AssociateCustName1, //   _custDetail.Tables[0].Rows[0]["AssociateCustName1"].ToString(),
                                AssociateCustomerMiddlename = customerDetails.AssociateCustName2, //   _custDetail.Tables[0].Rows[0]["AssociateCustName2"].ToString(),
                                AssociateCustomerLastname = customerDetails.AssociateCustName3, //   _custDetail.Tables[0].Rows[0]["AssociateCustName3"].ToString(),
                                AssociateCustomerCardnumber = customerDetails.AssociateClubcardId //   _custDetail.Tables[0].Rows[0]["AssociateClubcardID"].ToString()
                            };
                            list.Add(g__initLocal0);
                        }
                    }
                }

                return list;
            }
            catch (Exception exp)
            {
                trace.NoteException(exp, string.Empty);
                throw;
            }
            finally
            {
                trace.Dispose();
            }
        }
        public static List<CouponDetails> getCoupons(List<CouponDetails> couponDetailsList, List<CustomerMLS_PDF_DownloadDetails> customerDetailsList)
        {
            string voucherDetailsListXml = SerializerUtility<List<CouponDetails>>.GetSerializedString(couponDetailsList);
            string customerDetailsListXml = SerializerUtility<List<CustomerMLS_PDF_DownloadDetails>>.GetSerializedString(customerDetailsList);

            MCATrace trace = new MCATrace("Adapters.PopulateVD.getVouchers", " voucherDetailsList:" + voucherDetailsListXml + " customerDetailsList:" + customerDetailsListXml);

            try
            {
                List<CouponDetails> list = new List<CouponDetails>();

                CustomerMLS_PDF_DownloadDetails customerDetails = new CustomerMLS_PDF_DownloadDetails();
                if (customerDetailsList != null)
                {
                    customerDetails = customerDetailsList[0];
                }


                if (couponDetailsList != null) //(_dsCoupon.Tables.Count != 0)
                {
                    int _rowSelected = -1;
                    foreach (CouponDetails obj in couponDetailsList)//   (DataRow dr in _dsCoupon.Tables[0].Rows)
                    {
                        _rowSelected++;
                        CouponDetails g__initLocal0 = new CouponDetails
                        {
                            BarcodeNumber = obj.BarcodeNumber, //  _dsCoupon.Tables[0].Rows[_rowSelected]["SmartBarcode"].ToString(),
                            ImageName = obj.ImageName,//  _dsCoupon.Tables[0].Rows[_rowSelected]["FullImageName"].ToString(),

                            //Customer details
                            PrimaryCustomerFirstName = customerDetails.PrimaryCustName1, //   _dsCustDetails.Tables[0].Rows[0]["PrimaryCustName1"].ToString(),
                            PrimaryCustomerMiddleName = customerDetails.PrimaryCustName2, //   _dsCustDetails.Tables[0].Rows[0]["PrimaryCustName2"].ToString(),
                            PrimaryCustomerLastName = customerDetails.PrimaryCustName3, //   _dsCustDetails.Tables[0].Rows[0]["PrimaryCustName3"].ToString(),
                            PrimaryClubcardnumber = customerDetails.PrimaryClubcardId, //   _dsCustDetails.Tables[0].Rows[0]["PrimaryClubcardID"].ToString(),
                            AssociateCustomerFirstName = customerDetails.AssociateCustName1, //  _dsCustDetails.Tables[0].Rows[0]["AssociateCustName1"].ToString(),
                            AssociateCustomerMiddleName = customerDetails.AssociateCustName2, //   _dsCustDetails.Tables[0].Rows[0]["AssociateCustName2"].ToString(),
                            AssociateCustomerLastName = customerDetails.AssociateCustName3, //   _dsCustDetails.Tables[0].Rows[0]["AssociateCustName3"].ToString(),
                            AssociateClubcardNumber = customerDetails.AssociateClubcardId //   _dsCustDetails.Tables[0].Rows[0]["AssociateClubcardID"].ToString()
                        };
                        list.Add(g__initLocal0);
                    }
                }
                return list;
            }
            catch (Exception exp)
            {
                trace.NoteException(exp, string.Empty);
                throw;
            }
            finally
            {
                trace.Dispose();
            }
        }

       public static List<TokenDetails> getTokens(DataSet ds)
       {
           MCATrace trace = new MCATrace("Adapters.PopulateVD.getTokens", " ds:" + ds.GetXml() );

           try
           {
               string strCurrencySymbol = string.Empty;

               List<TokenDetails> list = new List<TokenDetails>();
               if (ds.Tables.Count != 0)
               {
                   int _rowSelected = -1;
                   foreach (DataRow dr in ds.Tables[0].Rows)
                   {
                       _rowSelected++;
                       TokenDetails g__initLocal0 = new TokenDetails
                       {
                           TokenId = Convert.ToInt64(ds.Tables[0].Rows[_rowSelected][1].ToString()),
                           TokenCode = ds.Tables[0].Rows[_rowSelected][5].ToString(),
                           ExpiryDate = Convert.ToDateTime(ds.Tables[0].Rows[_rowSelected][6].ToString()).ToString("dd-MM-yyyy"),
                           Value = strCurrencySymbol + ds.Tables[0].Rows[_rowSelected][8].ToString(),
                           //TokenValue  = _ds.Tables[0].Rows[_rowSelected][3].ToString(),
                           QualifySpend = ds.Tables[0].Rows[_rowSelected][9].ToString(),
                           Includes = ds.Tables[0].Rows[_rowSelected][11].ToString(),
                           Excludes = ds.Tables[0].Rows[_rowSelected][12].ToString(),
                           TermsAndCondition = ds.Tables[0].Rows[_rowSelected][13].ToString()
                       };
                       list.Add(g__initLocal0);
                   }
               }
               return list;
           }
           catch (Exception exp)
           {
               trace.NoteException(exp, string.Empty);
               throw;
           }
           finally
           {
               trace.Dispose();
           }
       }

       public static List<TokenDetails> getTokens(List<Token> lstToken)
       {
           string lstTokenXml = SerializerUtility<List<Token>>.GetSerializedString(lstToken);

           MCATrace trace = new MCATrace("Adapters.PopulateVD.getTokens", " lstToken:" + lstTokenXml);

           try
           {
               //string strCurrencySymbol = Resources.GenerateVouchersPDF.ResourceManager.GetString("lblstrCurrencySymbol").ToString();
               string strCurrencySymbol = string.Empty;

               List<TokenDetails> list = new List<TokenDetails>();
               if (lstToken.Count != 0)
               {

                   foreach (Token token in lstToken)
                   {
                       TokenDetails g__initLocal0 = new TokenDetails
                       {
                           TokenId = token.TokenID,
                           TokenCode = token.SupplierTokenCode,
                           ExpiryDate = Convert.ToDateTime(token.ValidUntil).ToString("dd-MM-yyyy"),
                           Value = strCurrencySymbol + token.TokenValue,
                           //TokenValue  = _ds.Tables[0].Rows[_rowSelected][3].ToString(),
                           QualifySpend = token.QualifyingSpend,
                           Includes = token.Includes,
                           Excludes = token.Excludes,
                           TermsAndCondition = token.TermsAndConditions
                       };
                       list.Add(g__initLocal0);
                   }
               }
               return list;
           }
           catch (Exception exp)
           {
               trace.NoteException(exp, string.Empty);
               throw;
           }
           finally
           {
               trace.Dispose();
           }
       }
    }
}

