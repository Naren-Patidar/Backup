using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace BigExchange.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class ClubcardReward : IClubcardReward
    {

        private InstoreClubcardReward.Business.Booking _bizBooking;
        private InstoreClubcardReward.Business.VoucherCollection _bizVoucherColl;

        private InstoreClubcardReward.Business.BookingPrintVoucher _bizBookingPrintVoucher;
        private InstoreClubcardReward.Business.UnusedVoucherCollection _bizUnusedVoucherCollection;


        private void ParseBookingDataToBiz(Booking bookingData)
        {

            //create objects from local business class
            _bizBooking = new InstoreClubcardReward.Business.Booking();

            _bizBooking.BookingId = bookingData.BookingId;
            _bizBooking.StoreId = bookingData.StoreId;
            _bizBooking.UserId = bookingData.UserId;
            _bizBooking.TillId = bookingData.TillId;
            _bizBooking.BookingType = bookingData.BookingType;
            _bizBooking.Clubcard = bookingData.Clubcard;
            _bizBooking.Change = bookingData.Change;
            _bizBooking.PrintNumber = bookingData.PrintNumber;
            _bizBooking.ErrorType = (InstoreClubcardReward.Business.ErrorTypes)bookingData.ErrorType;
            _bizBooking.ReprintReason = bookingData.ReprintReason;
            _bizBooking.TrainingMode = bookingData.TrainingMode;
            _bizBooking.CreateDate = bookingData.CreateDate;
            _bizBooking.Status = bookingData.Status;
            _bizBooking.StatusDate = bookingData.StatusDate;

            //create voucher collection on the booking biz object
            #region Voucher
            foreach (Voucher dataVoucher in bookingData.Vouchers)
            {
                InstoreClubcardReward.Business.Voucher bizVoucher = new InstoreClubcardReward.Business.Voucher();
                bizVoucher.Alpha = dataVoucher.Alpha;
                bizVoucher.Channel = dataVoucher.Channel;
                bizVoucher.Clubcard = dataVoucher.Clubcard;
                bizVoucher.Country = dataVoucher.Country;
                bizVoucher.Ean = dataVoucher.Ean;
                bizVoucher.ExpiryDate = dataVoucher.ExpiryDate;
                bizVoucher.IsUsed = dataVoucher.IsUsed;
                bizVoucher.ResponseClubcard = dataVoucher.ResponseClubcard;
                bizVoucher.ResponseCode = dataVoucher.ResponseCode;
                bizVoucher.ResponseValue = dataVoucher.ResponseValue;
                bizVoucher.Status = (InstoreClubcardReward.Business.VoucherStatus)dataVoucher.Status;
                bizVoucher.StoreNo = dataVoucher.StoreNo;
                bizVoucher.Type = (InstoreClubcardReward.Business.VoucherTypes)dataVoucher.Type;
                bizVoucher.UseDateTime = dataVoucher.UseDateTime;
                bizVoucher.VirtualStore = dataVoucher.VirtualStore;
                bizVoucher.VoucherId = dataVoucher.VoucherId;
                _bizBooking.Vouchers.Add(bizVoucher);
            }
            #endregion


            //create ProductLine collection - which conatins a collection of Tokens
            #region Token

            foreach (ProductLine dataProductLine in bookingData.ProductLines)
            {
                InstoreClubcardReward.Business.ProductLine bizProductLine = new InstoreClubcardReward.Business.ProductLine();
                bizProductLine.ProductLineId = dataProductLine.ProductLineId;
                bizProductLine.ProductNumber = dataProductLine.ProductNumber;

                foreach (Token dataToken in dataProductLine.Tokens)  //null here
                {
                    InstoreClubcardReward.Business.Token bizToken = new InstoreClubcardReward.Business.Token();
                    bizToken.Alpha = dataToken.Alpha;
                    bizToken.CustomerDate = dataToken.CustomerDate;
                    bizToken.EAN = dataToken.EAN;
                    bizToken.EndDate = dataToken.EndDate;
                    bizToken.ProductCode = dataToken.ProductCode;
                    bizToken.ProductLineId = dataToken.ProductLineId;
                    bizToken.ResponseCode = dataToken.ResponseCode;
                    bizToken.SupplierTokenCodeId = dataToken.SupplierTokenCodeId;
                    bizToken.SupplyDate = dataToken.SupplyDate;
                    bizToken.TokenId = dataToken.TokenId;
                    bizToken.TokenValue = dataToken.TokenValue;
                    bizToken.UsedByDate = dataToken.UsedByDate;
                    bizToken.VendorCode = dataToken.VendorCode;
                    bizProductLine.Tokens.Add(bizToken); //add newloy created token to product line collection
                }


                //create product collection on the booking biz object
                //a product has 1 Category object
                #region Product
                InstoreClubcardReward.Business.Product bizProduct = new InstoreClubcardReward.Business.Product();

                #region category
                bizProduct.Category = new InstoreClubcardReward.Business.Category();
                bizProduct.Category.CategoryId = dataProductLine.Product.Category.CategoryId;
                bizProduct.Category.Description = dataProductLine.Product.Category.Description;
                bizProduct.Category.ImageFilename = dataProductLine.Product.Category.ImageFilename;
                //TODO manage the recurrsion, a catagory has a parent which is also a category
                //bizProduct.Category.Parent = dataProductLine.Product.Category.Parent;
                bizProduct.Category.TokenValue = dataProductLine.Product.Category.TokenValue;
                #endregion

                bizProduct.Country = dataProductLine.Product.Country;
                bizProduct.CustomerPrice = dataProductLine.Product.CustomerPrice;
                bizProduct.Description = dataProductLine.Product.Description;
                bizProduct.ImageFilename = dataProductLine.Product.ImageFilename;
                bizProduct.LongDescription = dataProductLine.Product.LongDescription;
                bizProduct.ProductCode = dataProductLine.Product.ProductCode;
                bizProduct.ProductType = (InstoreClubcardReward.Business.ProductType)dataProductLine.Product.ProductType;
                bizProduct.ShortDescription = dataProductLine.Product.ShortDescription;
                bizProduct.TokenDescription = dataProductLine.Product.TokenDescription;
                bizProduct.TokenTermsAndConditions = dataProductLine.Product.TokenTermsAndConditions;
                bizProduct.TokenTitle = dataProductLine.Product.TokenTitle;
                bizProduct.TokenType = (InstoreClubcardReward.Business.TokenType)dataProductLine.Product.TokenType;
                bizProduct.TokenValue = dataProductLine.Product.TokenValue;
                bizProduct.UsedByDate = dataProductLine.Product.UsedByDate;
                bizProduct.ValidUntil = dataProductLine.Product.ValidUntil;
                bizProduct.VendorCode = dataProductLine.Product.VendorCode;
                bizProductLine.Product = bizProduct;

                #endregion
                _bizBooking.ProductLines.Add(bizProductLine);
            }
            #endregion

        }

        private void ParseVoucherDataToBiz(VoucherCollection voucherColl)
        {
            //create objects from local business class
            _bizVoucherColl = new InstoreClubcardReward.Business.VoucherCollection();
            //create voucher collection on the booking biz object
            #region Voucher
            foreach (Voucher dataVoucher in voucherColl)
            {
                InstoreClubcardReward.Business.Voucher bizVoucher = new InstoreClubcardReward.Business.Voucher();
                bizVoucher.Alpha = dataVoucher.Alpha;
                bizVoucher.Channel = dataVoucher.Channel;
                bizVoucher.Clubcard = dataVoucher.Clubcard;
                bizVoucher.Country = dataVoucher.Country;
                bizVoucher.Ean = dataVoucher.Ean;
                bizVoucher.ExpiryDate = dataVoucher.ExpiryDate;
                bizVoucher.IsUsed = dataVoucher.IsUsed;
                bizVoucher.ResponseClubcard = dataVoucher.ResponseClubcard;
                bizVoucher.ResponseCode = dataVoucher.ResponseCode;
                bizVoucher.ResponseValue = dataVoucher.ResponseValue;
                bizVoucher.Status = (InstoreClubcardReward.Business.VoucherStatus)dataVoucher.Status;
                bizVoucher.StoreNo = dataVoucher.StoreNo;
                bizVoucher.Type = (InstoreClubcardReward.Business.VoucherTypes)dataVoucher.Type;
                bizVoucher.UseDateTime = dataVoucher.UseDateTime;
                bizVoucher.VirtualStore = dataVoucher.VirtualStore;
                bizVoucher.VoucherId = dataVoucher.VoucherId;
                _bizVoucherColl.Add(bizVoucher);
            }
            #endregion
        }

        /// <summary>
        /// Returns a collection of products by making a call to the GetProducts() method of InstoreClubcardReward.Business.ProductCollection class
        /// then parses the returned value into an object of BigExchange.ProductCollection and finally returns it.
        /// </summary>
        public ProductCollection GetProducts()
        {
            BigExchange.ProductCollection objBigExProdColl = new ProductCollection();
            try
            {
                InstoreClubcardReward.Business.ProductCollection bizProductCollection = InstoreClubcardReward.Business.ProductCollection.GetProductsWCF();
                foreach (InstoreClubcardReward.Business.Product bizProduct in bizProductCollection)
                {
                    BigExchange.Product objBigExProd = new Product();
                    objBigExProd.Description = bizProduct.Description;
                    objBigExProd.StrippedDescription = bizProduct.StrippedDescription;
                    objBigExProd.ProductCode = bizProduct.ProductCode;
                    objBigExProd.VendorCode = bizProduct.VendorCode;
                    objBigExProd.CustomerPrice = bizProduct.CustomerPrice;
                    objBigExProd.TokenValue = bizProduct.TokenValue;
                    objBigExProd.Country = bizProduct.Country;
                    objBigExProd.ValidUntil = bizProduct.ValidUntil;
                    objBigExProd.UsedByDate = bizProduct.UsedByDate;
                    objBigExProd.ImageFilename = bizProduct.ImageFilename;
                    objBigExProd.ShortDescription = bizProduct.ShortDescription;
                    objBigExProd.LongDescription = bizProduct.LongDescription;
                    objBigExProd.ProductType = (BigExchange.ProductType)bizProduct.ProductType;
                    objBigExProd.TokenType = (BigExchange.TokenType)bizProduct.TokenType;
                    objBigExProd.TokenTitle = bizProduct.TokenTitle;
                    objBigExProd.TokenDescription = bizProduct.TokenDescription;
                    objBigExProd.TokenTermsAndConditions = bizProduct.TokenTermsAndConditions;

                    //Category object
                    objBigExProd.Category = new Category();
                    objBigExProd.Category.CategoryId = bizProduct.Category.CategoryId;
                    objBigExProd.Category.Description = bizProduct.Category.Description;
                    objBigExProd.Category.ImageFilename = bizProduct.Category.ImageFilename;
                    objBigExProd.Category.TokenValue = bizProduct.Category.TokenValue;
                    objBigExProd.Category.Parent = null;

                    objBigExProdColl.Add(objBigExProd);
                }
            }
            catch (Exception ex)
            {
                CustomException objCustEx = new CustomException();
                objCustEx.StatusCode = "Error in Funtion:GetProducts()";
                objCustEx.ErrorMessage = ex.Message;
                throw new FaultException<CustomException>(objCustEx, "Exception occured while getting the list of Products");
            }
            return objBigExProdColl;
        }

        /// <summary>
        /// Returns a collection of categories by making a call to the GetCategories() method of InstoreClubcardReward.Business.CategoryCollection class
        /// then parses the returned value into an object of BigExchange.CategoryCollection and finally returns it.
        /// </summary>
        public CategoryCollection GetCategories()
        {
            BigExchange.CategoryCollection objBigExCategoryColl = new CategoryCollection();
            try
            {
                InstoreClubcardReward.Business.CategoryCollection bizCategoryCollection = InstoreClubcardReward.Business.CategoryCollection.GetCategoriesWCF();
                foreach (InstoreClubcardReward.Business.Category bizCategory in bizCategoryCollection)
                {
                    BigExchange.Category objBigExCategory = new Category();
                    objBigExCategory.CategoryId = bizCategory.CategoryId;
                    objBigExCategory.Description = bizCategory.Description;
                    objBigExCategory.ImageFilename = bizCategory.ImageFilename;
                    objBigExCategory.TokenValue = bizCategory.TokenValue;
                    objBigExCategory.Parent = null;
                    objBigExCategoryColl.Add(objBigExCategory);
                }
                return objBigExCategoryColl;
            }
            catch (Exception ex)
            {
                CustomException objCustEx = new CustomException();
                objCustEx.StatusCode = "Error in Funtion:GetCategories()";
                objCustEx.ErrorMessage = ex.Message;
                throw new FaultException<CustomException>(objCustEx, "Exception occured while getting the list of Categories");
            }
        }

        /// <summary>
        /// Returns a collection of category collection  by making a call to the GetCategoryIncludes() method of InstoreClubcardReward.Business.GetCategoryIncludesCollection class
        /// then parses the returned value into an object of BigExchange.CategoryIncludesCollection and finally returns it.
        /// </summary>
        public CategoryIncludesCollection GetCategoryIncludes(int categoryId)
        {
            BigExchange.CategoryIncludesCollection objBigExCatIncColl = new CategoryIncludesCollection();
            try
            {
                InstoreClubcardReward.Business.CategoryIncludesCollection bizCatIncCollection = InstoreClubcardReward.Business.CategoryIncludesCollection.GetCategoryIncludesWCF(categoryId);
                foreach (InstoreClubcardReward.Business.CategoryIncludes bizCatInc in bizCatIncCollection)
                {
                    BigExchange.CategoryIncludes objBigExCatInc = new CategoryIncludes();
                    objBigExCatInc.CategoryId = bizCatInc.CategoryId;
                    objBigExCatInc.LineId = bizCatInc.LineId;
                    objBigExCatInc.Description1 = bizCatInc.Description1;
                    objBigExCatInc.Description2 = bizCatInc.Description2;
                    objBigExCatInc.Description3 = bizCatInc.Description3;
                    objBigExCatIncColl.Add(objBigExCatInc);
                }
                return objBigExCatIncColl;
            }
            catch (Exception ex)
            {
                CustomException objCustEx = new CustomException();
                objCustEx.StatusCode = "Error in Funtion:CategoryIncludesCollection()";
                objCustEx.ErrorMessage = ex.Message;
                throw new FaultException<CustomException>(objCustEx, "Exception occured while getting the list of CategoryIncludesCollection");
            }
        }

        /// <summary>
        /// Returns a collection of PrintReasons  by making a call to the GetPrintReasons() method of InstoreClubcardReward.Business.PrintReasonCollection class
        /// then parses the returned value into an object of BigExchange.PrintReasonCollection and finally returns it.
        /// </summary>
        public PrintReasonCollection GetPrintReasons()
        {
            BigExchange.PrintReasonCollection objBigExPrntRsnColl = new PrintReasonCollection();
            try
            {
                InstoreClubcardReward.Business.PrintReasonCollection bizPrntRsnCollection = InstoreClubcardReward.Business.PrintReasonCollection.GetPrintReasonsWCF();
                foreach (InstoreClubcardReward.Business.PrintReason bizPrntRsn in bizPrntRsnCollection)
                {
                    BigExchange.PrintReason objBigExPrntRsn = new PrintReason();
                    objBigExPrntRsn.PrintReasonId = bizPrntRsn.PrintReasonId;
                    objBigExPrntRsn.DisplayOrder = bizPrntRsn.DisplayOrder;
                    objBigExPrntRsn.PrintReasonText = bizPrntRsn.PrintReasonText;
                    objBigExPrntRsn.Enabled = bizPrntRsn.Enabled;
                    objBigExPrntRsnColl.Add(objBigExPrntRsn);
                }
                return objBigExPrntRsnColl;
            }
            catch (Exception ex)
            {
                CustomException objCustEx = new CustomException();
                objCustEx.StatusCode = "Error in Funtion:GetPrintReasons()";
                objCustEx.ErrorMessage = ex.Message;
                throw new FaultException<CustomException>(objCustEx, "Exception occured while getting the list of PrintReasons");
            }
        }

        /// <summary>
        /// Validates the given set of vouchers by making a call to SV webservice
        /// </summary>
        public VoucherCollection ValidateVouchers(VoucherCollection voucherColl, string clubcard, int agentId, string country)
        {
            try
            {
                ParseVoucherDataToBiz(voucherColl);
                _bizVoucherColl.ValidateVouchers(clubcard, agentId, country);


                //create objects from local Big Exchange class
                VoucherCollection objBigExVoucherColl = new VoucherCollection();
                //update voucher collection 
                #region Voucher
                foreach (InstoreClubcardReward.Business.Voucher bizVoucher in _bizVoucherColl)
                {
                    BigExchange.Voucher objBigExVoucher = new BigExchange.Voucher();

                    objBigExVoucher.Ean = bizVoucher.Ean;
                    objBigExVoucher.Alpha = bizVoucher.Alpha;
                    objBigExVoucher.Clubcard = bizVoucher.Clubcard;
                    objBigExVoucher.Country = bizVoucher.Country;
                    objBigExVoucher.Status = (BigExchange.VoucherStatus)bizVoucher.Status;
                    objBigExVoucher.Type = (BigExchange.VoucherTypes)bizVoucher.Type;
                    objBigExVoucher.StoreNo = bizVoucher.StoreNo;
                    objBigExVoucher.Channel = bizVoucher.Channel;
                    objBigExVoucher.VirtualStore = bizVoucher.VirtualStore;
                    objBigExVoucher.UseDateTime = bizVoucher.UseDateTime;
                    objBigExVoucher.ExpiryDate = bizVoucher.ExpiryDate;
                    objBigExVoucher.ResponseCode = bizVoucher.ResponseCode;
                    objBigExVoucher.ResponseClubcard = bizVoucher.ResponseClubcard;
                    objBigExVoucher.ResponseValue = bizVoucher.ResponseValue;
                    objBigExVoucher.IsUsed = bizVoucher.IsUsed;
                    objBigExVoucher.VoucherId = bizVoucher.VoucherId;
                    //Read Olny property
                    //objBigExVoucher.Value = bizVoucher.Value;
                    objBigExVoucherColl.Add(objBigExVoucher);
                }
                #endregion

                return objBigExVoucherColl;
            }
            catch (Exception ex)
            {
                CustomException objCustEx = new CustomException();
                objCustEx.StatusCode = "Error in Funtion:ValidateVouchers()";
                objCustEx.ErrorMessage = ex.Message;
                throw new FaultException<CustomException>(objCustEx, "Exception occured while validating the voucher list");
            }
        }

        /// <summary>
        /// Validates the given set of vouchers by making a call to SV webservice
        /// </summary>
        //public Boolean ProcessBooking(Booking bookingData,string country)
        public Booking ProcessBooking(Booking bookingData, string country)
        {
            Boolean lbFlag = false;
            try
            {
                ParseBookingDataToBiz(bookingData);
                lbFlag = _bizBooking.ProcessBooking(country);

                if (lbFlag == false)
                {
                    throw new Exception("ProcessBooking Method Returning a False Flag");
                }

                BigExchange.Booking objBigExBooking = new BigExchange.Booking();
                //ParseBizToBigExBooking();

                objBigExBooking.BookingId = _bizBooking.BookingId;
                objBigExBooking.StoreId = _bizBooking.StoreId;
                objBigExBooking.UserId = _bizBooking.UserId;
                objBigExBooking.TillId = _bizBooking.TillId;
                objBigExBooking.BookingType = _bizBooking.BookingType;

                objBigExBooking.Clubcard = _bizBooking.Clubcard;
                objBigExBooking.Change = _bizBooking.Change;
                objBigExBooking.PrintNumber = _bizBooking.PrintNumber;
                objBigExBooking.ErrorType = (BigExchange.ErrorTypes)_bizBooking.ErrorType;
                objBigExBooking.ReprintReason = _bizBooking.ReprintReason;
                //objBigExBooking.ErrorDescription = _bizBooking.ErrorDescription;  //read only property
                objBigExBooking.TrainingMode = _bizBooking.TrainingMode;

                objBigExBooking.CreateDate = _bizBooking.CreateDate;
                objBigExBooking.Status = _bizBooking.Status;
                objBigExBooking.StatusDate = _bizBooking.StatusDate;
                //objBigExBooking.StatusDescription = _bizBooking.StatusDescription;  //read only property

                //create voucher collection on the booking biz object
                #region Voucher
                foreach (InstoreClubcardReward.Business.Voucher bizVoucher in _bizBooking.Vouchers)
                {
                    BigExchange.Voucher objBigExVoucher = new BigExchange.Voucher();

                    objBigExVoucher.Ean = bizVoucher.Ean;
                    objBigExVoucher.Alpha = bizVoucher.Alpha;
                    objBigExVoucher.Clubcard = bizVoucher.Clubcard;
                    objBigExVoucher.Country = bizVoucher.Country;
                    objBigExVoucher.Status = (BigExchange.VoucherStatus)bizVoucher.Status;
                    objBigExVoucher.Type = (BigExchange.VoucherTypes)bizVoucher.Type;
                    objBigExVoucher.StoreNo = bizVoucher.StoreNo;
                    objBigExVoucher.Channel = bizVoucher.Channel;
                    objBigExVoucher.VirtualStore = bizVoucher.VirtualStore;
                    objBigExVoucher.UseDateTime = bizVoucher.UseDateTime;
                    objBigExVoucher.ExpiryDate = bizVoucher.ExpiryDate;
                    objBigExVoucher.ResponseCode = bizVoucher.ResponseCode;
                    objBigExVoucher.ResponseClubcard = bizVoucher.ResponseClubcard;
                    objBigExVoucher.ResponseValue = bizVoucher.ResponseValue;
                    objBigExVoucher.IsUsed = bizVoucher.IsUsed;
                    objBigExVoucher.VoucherId = bizVoucher.VoucherId;
                    //Read Olny property
                    //objBigExVoucher.Value = bizVoucher.Value;
                    objBigExBooking.Vouchers.Add(objBigExVoucher);
                }
                #endregion

                //create ProductLine collection - which conatins a collection of Tokens
                #region Token

                foreach (InstoreClubcardReward.Business.ProductLine bizProdLine in _bizBooking.ProductLines)
                {
                    //InstoreClubcardReward.Business.ProductLine bizProductLine = new InstoreClubcardReward.Business.ProductLine();
                    BigExchange.ProductLine objBigExProdLine = new BigExchange.ProductLine();
                    objBigExProdLine.ProductLineId = bizProdLine.ProductLineId;
                    objBigExProdLine.ProductNumber = bizProdLine.ProductNumber;

                    objBigExProdLine.Product = new BigExchange.Product();
                    objBigExProdLine.Product.Description = bizProdLine.Product.Description;
                    objBigExProdLine.Product.StrippedDescription = bizProdLine.Product.StrippedDescription;
                    objBigExProdLine.Product.ProductCode = bizProdLine.Product.ProductCode;
                    objBigExProdLine.Product.VendorCode = bizProdLine.Product.VendorCode;
                    objBigExProdLine.Product.CustomerPrice = bizProdLine.Product.CustomerPrice;
                    objBigExProdLine.Product.TokenValue = bizProdLine.Product.TokenValue;
                    objBigExProdLine.Product.Country = bizProdLine.Product.Country;
                    objBigExProdLine.Product.ValidUntil = bizProdLine.Product.ValidUntil;
                    objBigExProdLine.Product.UsedByDate = bizProdLine.Product.UsedByDate;
                    objBigExProdLine.Product.ImageFilename = bizProdLine.Product.ImageFilename;
                    objBigExProdLine.Product.ShortDescription = bizProdLine.Product.ShortDescription;
                    objBigExProdLine.Product.LongDescription = bizProdLine.Product.LongDescription;
                    objBigExProdLine.Product.ProductType = (BigExchange.ProductType)bizProdLine.Product.ProductType;
                    objBigExProdLine.Product.TokenType = (BigExchange.TokenType)bizProdLine.Product.TokenType;
                    objBigExProdLine.Product.TokenTitle = bizProdLine.Product.TokenTitle;
                    objBigExProdLine.Product.TokenDescription = bizProdLine.Product.TokenDescription;
                    objBigExProdLine.Product.TokenTermsAndConditions = bizProdLine.Product.TokenTermsAndConditions;

                    //Category object
                    objBigExProdLine.Product.Category = new Category();
                    objBigExProdLine.Product.Category.CategoryId = bizProdLine.Product.Category.CategoryId;
                    objBigExProdLine.Product.Category.Description = bizProdLine.Product.Category.Description;
                    objBigExProdLine.Product.Category.ImageFilename = bizProdLine.Product.Category.ImageFilename;
                    objBigExProdLine.Product.Category.TokenValue = bizProdLine.Product.Category.TokenValue;
                    objBigExProdLine.Product.Category.Parent = null;

                    //objBigExBooking.Products.Add(objBigExProd);


                    foreach (InstoreClubcardReward.Business.Token bizToken in bizProdLine.Tokens)  //null here
                    {
                        //InstoreClubcardReward.Business.Token bizToken = new InstoreClubcardReward.Business.Token();
                        BigExchange.Token objBigExToken = new BigExchange.Token();

                        objBigExToken.TokenValue = bizToken.TokenValue;
                        objBigExToken.EAN = bizToken.EAN;
                        objBigExToken.Alpha = bizToken.Alpha;
                        objBigExToken.ProductCode = bizToken.ProductCode;
                        objBigExToken.UsedByDate = bizToken.UsedByDate;
                        objBigExToken.VendorCode = bizToken.VendorCode;
                        objBigExToken.SupplyDate = bizToken.SupplyDate;
                        objBigExToken.ProductLineId = bizToken.ProductLineId;
                        objBigExToken.TokenId = bizToken.TokenId;
                        objBigExToken.ResponseCode = bizToken.ResponseCode;
                        objBigExToken.SupplierTokenCodeId = bizToken.SupplierTokenCodeId;
                        objBigExToken.CustomerDate = bizToken.CustomerDate;
                        objBigExToken.EndDate = bizToken.EndDate;
                        objBigExProdLine.Tokens.Add(objBigExToken); //add newloy created token to product line collection
                    }
                    objBigExBooking.ProductLines.Add(objBigExProdLine);
                }
                #endregion

                return objBigExBooking;

                // return lbFlag;
            }
            catch (Exception ex)
            {
                CustomException objCustEx = new CustomException();
                objCustEx.StatusCode = "Error in Funtion:ProcessBooking()";
                objCustEx.ErrorMessage = ex.Message;
                throw new FaultException<CustomException>(objCustEx, "Exception occured while processing the booking details");
            }
        }

        /// <summary>
        /// Saves to BookingError table. On save with a booking 
        /// </summary>
        /// <param name="bookingData"></param>
        /// <param name="error"></param>
        public void SaveToBookingError(Booking bookingData, string error)
        {
            try
            {
                //ParseDataToBiz(bookingData);
                _bizBooking = new InstoreClubcardReward.Business.Booking();
                _bizBooking.BookingId = bookingData.BookingId;
                _bizBooking.SaveToBookingError(error);

            }
            catch (Exception ex)
            {
                CustomException objCustEx = new CustomException();
                objCustEx.StatusCode = "Error in Funtion:SaveToBookingError()";
                objCustEx.ErrorMessage = ex.Message;
                throw new FaultException<CustomException>(objCustEx, "Exception occured while storing errors to BookingError");
            }
        }

        /// <summary>
        /// Saves to Training table. Saves text with
        /// current user id. Used to audit training.000111
        /// </summary>
        /// <param name="locationDescription"></param>
        /// <param name="storeId"></param>
        /// <param name="tillId"></param>
        /// <param name="userId"></param>
        /// <param name="bookingId"></param>
        public void SaveToTraining(string locationDescription, int storeId, int tillId, int userId, int bookingId)
        {
            try
            {
                InstoreClubcardReward.Business.Booking.SaveToTraining(locationDescription, storeId, tillId, userId, bookingId);
            }
            catch (Exception ex)
            {
                CustomException objCustEx = new CustomException();
                objCustEx.StatusCode = "Error in Funtion:SaveToTraining()";
                objCustEx.ErrorMessage = ex.Message;
                throw new FaultException<CustomException>(objCustEx, "Exception occured while storing data into Training table");
            }

        }

        /// <summary>
        /// Gets the bookings for clubcard.
        /// </summary>
        /// <param name="clubcard"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Booking> GetBookingsForClubcard(string clubcard, DateTime startDate, DateTime endDate, int userId)
        {

            List<Booking> objBigExBookingColl = new List<Booking>();

            try
            {

                List<InstoreClubcardReward.Business.Booking> bizBookingCollection = InstoreClubcardReward.Business.Booking.GetBookingsForClubcardWCF(clubcard, startDate, endDate, userId);

                foreach (InstoreClubcardReward.Business.Booking bizBooking in bizBookingCollection)
                {
                    BigExchange.Booking objBigExBooking = new BigExchange.Booking();
                    objBigExBooking.BookingId = bizBooking.BookingId;
                    objBigExBooking.StoreId = bizBooking.StoreId;
                    objBigExBooking.UserId = bizBooking.UserId;
                    objBigExBooking.TillId = bizBooking.TillId;
                    objBigExBooking.BookingType = _bizBooking.BookingType;

                    objBigExBooking.Clubcard = bizBooking.Clubcard;
                    objBigExBooking.Change = _bizBooking.Change;
                    objBigExBooking.PrintNumber = bizBooking.PrintNumber;
                    objBigExBooking.ErrorType = (BigExchange.ErrorTypes)bizBooking.ErrorType;
                    objBigExBooking.ReprintReason = bizBooking.ReprintReason;
                    //objBigExBooking.ErrorDescription = bizBooking.ErrorDescription;  //read only property
                    objBigExBooking.TrainingMode = bizBooking.TrainingMode;

                    objBigExBooking.CreateDate = bizBooking.CreateDate;
                    objBigExBooking.Status = bizBooking.Status;
                    objBigExBooking.StatusDate = bizBooking.StatusDate;
                    //objBigExBooking.StatusDescription = _bizBooking.StatusDescription;  //read only property


                    //create voucher collection on the booking biz object
                    #region Voucher
                    foreach (InstoreClubcardReward.Business.Voucher bizVoucher in bizBooking.Vouchers)
                    {
                        BigExchange.Voucher objBigExVoucher = new BigExchange.Voucher();

                        objBigExVoucher.Ean = bizVoucher.Ean;
                        objBigExVoucher.Alpha = bizVoucher.Alpha;
                        objBigExVoucher.Clubcard = bizVoucher.Clubcard;
                        objBigExVoucher.Country = bizVoucher.Country;
                        objBigExVoucher.Status = (BigExchange.VoucherStatus)bizVoucher.Status;
                        objBigExVoucher.Type = (BigExchange.VoucherTypes)bizVoucher.Type;
                        objBigExVoucher.StoreNo = bizVoucher.StoreNo;
                        objBigExVoucher.Channel = bizVoucher.Channel;
                        objBigExVoucher.VirtualStore = bizVoucher.VirtualStore;
                        objBigExVoucher.UseDateTime = bizVoucher.UseDateTime;
                        objBigExVoucher.ExpiryDate = bizVoucher.ExpiryDate;
                        objBigExVoucher.ResponseCode = bizVoucher.ResponseCode;
                        objBigExVoucher.ResponseClubcard = bizVoucher.ResponseClubcard;
                        objBigExVoucher.ResponseValue = bizVoucher.ResponseValue;
                        objBigExVoucher.IsUsed = bizVoucher.IsUsed;
                        objBigExVoucher.VoucherId = bizVoucher.VoucherId;
                        //Read Olny property
                        //objBigExVoucher.Value = bizVoucher.Value;
                        objBigExBooking.Vouchers.Add(objBigExVoucher);
                    }
                    #endregion

                    //create ProductLine collection - which conatins a collection of Tokens
                    #region Token

                    foreach (InstoreClubcardReward.Business.ProductLine bizProdLine in bizBooking.ProductLines)
                    {
                        //InstoreClubcardReward.Business.ProductLine bizProductLine = new InstoreClubcardReward.Business.ProductLine();
                        BigExchange.ProductLine objBigExProdLine = new BigExchange.ProductLine();
                        objBigExProdLine.ProductLineId = bizProdLine.ProductLineId;
                        objBigExProdLine.ProductNumber = bizProdLine.ProductNumber;

                        objBigExProdLine.Product = new BigExchange.Product();
                        objBigExProdLine.Product.Description = bizProdLine.Product.Description;
                        objBigExProdLine.Product.StrippedDescription = bizProdLine.Product.StrippedDescription;
                        objBigExProdLine.Product.ProductCode = bizProdLine.Product.ProductCode;
                        objBigExProdLine.Product.VendorCode = bizProdLine.Product.VendorCode;
                        objBigExProdLine.Product.CustomerPrice = bizProdLine.Product.CustomerPrice;
                        objBigExProdLine.Product.TokenValue = bizProdLine.Product.TokenValue;
                        objBigExProdLine.Product.Country = bizProdLine.Product.Country;
                        objBigExProdLine.Product.ValidUntil = bizProdLine.Product.ValidUntil;
                        objBigExProdLine.Product.UsedByDate = bizProdLine.Product.UsedByDate;
                        objBigExProdLine.Product.ImageFilename = bizProdLine.Product.ImageFilename;
                        objBigExProdLine.Product.ShortDescription = bizProdLine.Product.ShortDescription;
                        objBigExProdLine.Product.LongDescription = bizProdLine.Product.LongDescription;
                        objBigExProdLine.Product.ProductType = (BigExchange.ProductType)bizProdLine.Product.ProductType;
                        objBigExProdLine.Product.TokenType = (BigExchange.TokenType)bizProdLine.Product.TokenType;
                        objBigExProdLine.Product.TokenTitle = bizProdLine.Product.TokenTitle;
                        objBigExProdLine.Product.TokenDescription = bizProdLine.Product.TokenDescription;
                        objBigExProdLine.Product.TokenTermsAndConditions = bizProdLine.Product.TokenTermsAndConditions;

                        //Category object
                        objBigExProdLine.Product.Category = new Category();
                        objBigExProdLine.Product.Category.CategoryId = bizProdLine.Product.Category.CategoryId;
                        objBigExProdLine.Product.Category.Description = bizProdLine.Product.Category.Description;
                        objBigExProdLine.Product.Category.ImageFilename = bizProdLine.Product.Category.ImageFilename;
                        objBigExProdLine.Product.Category.TokenValue = bizProdLine.Product.Category.TokenValue;
                        objBigExProdLine.Product.Category.Parent = null;

                        //objBigExBooking.Products.Add(objBigExProd);


                        foreach (InstoreClubcardReward.Business.Token bizToken in bizProdLine.Tokens)  //null here
                        {
                            //InstoreClubcardReward.Business.Token bizToken = new InstoreClubcardReward.Business.Token();
                            BigExchange.Token objBigExToken = new BigExchange.Token();

                            objBigExToken.Alpha = bizToken.Alpha;
                            objBigExToken.CustomerDate = bizToken.CustomerDate;
                            objBigExToken.EAN = bizToken.EAN;
                            objBigExToken.EndDate = bizToken.EndDate;
                            objBigExToken.ProductCode = bizToken.ProductCode;
                            objBigExToken.ProductLineId = bizToken.ProductLineId;
                            objBigExToken.ResponseCode = bizToken.ResponseCode;
                            objBigExToken.SupplierTokenCodeId = bizToken.SupplierTokenCodeId;
                            objBigExToken.SupplyDate = bizToken.SupplyDate;
                            objBigExToken.TokenId = bizToken.TokenId;
                            objBigExToken.TokenValue = bizToken.TokenValue;
                            objBigExToken.UsedByDate = bizToken.UsedByDate;
                            objBigExToken.VendorCode = bizToken.VendorCode;
                            objBigExProdLine.Tokens.Add(objBigExToken); //add newloy created token to product line collection
                        }
                        objBigExBooking.ProductLines.Add(objBigExProdLine);
                    }
                    #endregion

                    objBigExBookingColl.Add(objBigExBooking);
                }

                return objBigExBookingColl;
            }
            catch (Exception ex)
            {
                CustomException objCustEx = new CustomException();
                objCustEx.StatusCode = "Error in Funtion:GetBookingsForClubcard()";
                objCustEx.ErrorMessage = ex.Message;
                throw new FaultException<CustomException>(objCustEx, "Exception occured while getting the list of Bookings For Clubcard");
            }

        }

        /// <summary>
        /// Gets the reprint till token script.
        /// </summary>
        /// <param name="bookingData"></param>
        /// <param name="bookingId"></param>
        /// <param name="reprintReason"></param>
        /// <returns></returns>
        public string GetReprintTillTokenScript(Booking bookingData, int bookingId, int? reprintReason)
        {
            try
            {
                ParseBookingDataToBiz(bookingData);
                _bizBooking.BookingId = bookingId;
                return _bizBooking.GetReprintTillTokenScriptWCF(_bizBooking.BookingId, reprintReason);
            }
            catch (Exception ex)
            {
                CustomException objCustEx = new CustomException();
                objCustEx.StatusCode = "Error in Funtion:GetReprintTillTokenScript()";
                objCustEx.ErrorMessage = ex.Message;
                throw new FaultException<CustomException>(objCustEx, "Exception occured while getting the Reprint TokenScript");
            }
        }

        /// <summary>
        /// Gets the till coupon script.
        /// </summary>
        /// <param name="bookingData"></param>
        /// <param name="reprintReason"></param>
        /// <returns></returns>
        public string GetTillTokenScript(Booking bookingData, int? reprintReason)
        {
            try
            {
                ParseBookingDataToBiz(bookingData);
                _bizBooking.Products = InstoreClubcardReward.Business.ProductCollection.GetProductsWCF();
                return _bizBooking.GetTillTokenScriptWCF(reprintReason);
            }
            catch (Exception ex)
            {
                CustomException objCustEx = new CustomException();
                objCustEx.StatusCode = "Error in Funtion:GetTillTokenScript()";
                objCustEx.ErrorMessage = ex.Message;
                throw new FaultException<CustomException>(objCustEx, "Exception occured while getting the Till TokenScript");
            }
        }




        /// <summary>
        /// from the IP address get the parameters for the entry page
        /// returns an empty string if not in Kiosk table
        /// </summary>
        /// <param name="clientIP"></param>
        /// <returns></returns>
        public string KioskEntryPage(string clientIP)
        {
            try
            {
                return InstoreClubcardReward.Business.Kiosk.KioskEntryPageWCF(clientIP);
            }
            catch (Exception ex)
            {
                CustomException objCustEx = new CustomException();
                objCustEx.StatusCode = "Error in Funtion:KioskEntryPage()";
                objCustEx.ErrorMessage = ex.Message;
                throw new FaultException<CustomException>(objCustEx, "Exception occured while getting the URL parameters for the entry page");
            }
        }



        //Below methods for PrintVouchersAtKiosk application
        private void ParseTransPrintVoucherToBiz(BookingPrintVoucher bookingPrintVoucher)
        {
            //create objects from local business class
            _bizBookingPrintVoucher = new InstoreClubcardReward.Business.BookingPrintVoucher();
            _bizBookingPrintVoucher.TransactionID = bookingPrintVoucher.TransactionID;
            _bizBookingPrintVoucher.KioskID = bookingPrintVoucher.KioskID;
            _bizBookingPrintVoucher.Clubcard = bookingPrintVoucher.Clubcard;
            _bizBookingPrintVoucher.PostCode = bookingPrintVoucher.PostCode;
            _bizBookingPrintVoucher.AddressLine1 = bookingPrintVoucher.AddressLine1;
            _bizBookingPrintVoucher.PrintDate = bookingPrintVoucher.PrintDate;
            _bizBookingPrintVoucher.totalActiveVouchers = bookingPrintVoucher.totalActiveVouchers;
            _bizBookingPrintVoucher.FirstName = bookingPrintVoucher.FirstName;
            _bizBookingPrintVoucher.Surname = bookingPrintVoucher.Surname;
            _bizBookingPrintVoucher.StartTime = bookingPrintVoucher.StartTime;
            _bizBookingPrintVoucher.EndTime = bookingPrintVoucher.EndTime;
            _bizBookingPrintVoucher.TranStartTime = bookingPrintVoucher.TranStartTime;
            _bizBookingPrintVoucher.StoreID = bookingPrintVoucher.StoreID;
            _bizBookingPrintVoucher.KioskNo = bookingPrintVoucher.KioskNo;
            _bizBookingPrintVoucher.Status = bookingPrintVoucher.Status;
            _bizBookingPrintVoucher.StatusLoginAttempts = bookingPrintVoucher.StatusLoginAttempts;
            //create voucher collection on the booking biz object
            foreach (UnusedVoucher dataVoucher in bookingPrintVoucher.UnusedVouchers)
            {
                InstoreClubcardReward.Business.UnusedVoucher bizUnusedVoucher = new InstoreClubcardReward.Business.UnusedVoucher();
                bizUnusedVoucher.HouseholdId = dataVoucher.HouseholdId;
                bizUnusedVoucher.PeriodName = dataVoucher.PeriodName;
                bizUnusedVoucher.VoucherValue = dataVoucher.VoucherValue;
                bizUnusedVoucher.VoucherNumber = dataVoucher.VoucherNumber;
                bizUnusedVoucher.OnlineCode = dataVoucher.OnlineCode;
                bizUnusedVoucher.ExpiryDate = dataVoucher.ExpiryDate;
                bizUnusedVoucher.VoucherType = dataVoucher.VoucherType;
                bizUnusedVoucher.Ean = dataVoucher.Ean;
                _bizBookingPrintVoucher.UnusedVouchers.Add(bizUnusedVoucher);
            }

        }
        private BigExchange.BookingPrintVoucher ParseBizToTransPrintVoucher(BigExchange.BookingPrintVoucher bizBookingPrintVoucher)
        {
            //create objects from local business class
            BookingPrintVoucher _bookingPrintVoucher = new BookingPrintVoucher();
            _bookingPrintVoucher.TransactionID = bizBookingPrintVoucher.TransactionID;
            _bookingPrintVoucher.KioskID = bizBookingPrintVoucher.KioskID;
            _bookingPrintVoucher.Clubcard = bizBookingPrintVoucher.Clubcard;
            _bookingPrintVoucher.PostCode = bizBookingPrintVoucher.PostCode;
            _bookingPrintVoucher.AddressLine1 = bizBookingPrintVoucher.AddressLine1;
            _bookingPrintVoucher.PrintDate = bizBookingPrintVoucher.PrintDate;
            _bookingPrintVoucher.totalActiveVouchers = bizBookingPrintVoucher.totalActiveVouchers;
            _bookingPrintVoucher.FirstName = bizBookingPrintVoucher.FirstName;
            _bookingPrintVoucher.Surname = bizBookingPrintVoucher.Surname;
            _bookingPrintVoucher.StartTime = bizBookingPrintVoucher.StartTime;
            _bookingPrintVoucher.EndTime = bizBookingPrintVoucher.EndTime;
            _bookingPrintVoucher.TranStartTime = bizBookingPrintVoucher.TranStartTime;
            _bookingPrintVoucher.StoreID = bizBookingPrintVoucher.StoreID;
            _bookingPrintVoucher.KioskNo = bizBookingPrintVoucher.KioskNo;
            _bookingPrintVoucher.Status = bizBookingPrintVoucher.Status;
            _bookingPrintVoucher.StatusLoginAttempts = bizBookingPrintVoucher.StatusLoginAttempts;
            //create voucher collection on the booking biz object
            foreach (BigExchange.UnusedVoucher bizUnusedVoucher in bizBookingPrintVoucher.UnusedVouchers)
            {
                UnusedVoucher _unusedVoucher = new UnusedVoucher();
                _unusedVoucher.HouseholdId = bizUnusedVoucher.HouseholdId;
                _unusedVoucher.PeriodName = bizUnusedVoucher.PeriodName;
                _unusedVoucher.VoucherValue = bizUnusedVoucher.VoucherValue;
                _unusedVoucher.VoucherNumber = bizUnusedVoucher.VoucherNumber;
                _unusedVoucher.OnlineCode = bizUnusedVoucher.OnlineCode;
                _unusedVoucher.ExpiryDate = bizUnusedVoucher.ExpiryDate;
                _unusedVoucher.VoucherType = bizUnusedVoucher.VoucherType;
                _unusedVoucher.Ean = bizUnusedVoucher.Ean;
                _bookingPrintVoucher.UnusedVouchers.Add(_unusedVoucher);
            }
            return _bookingPrintVoucher;
        }


        /// <summary>
        /// ProcessVoucherBooking
        /// </summary>
        /// <param name="bookingPrintVoucher"></param>
        /// <returns></returns>
        public BookingPrintVoucher ProcessVoucherBooking(BookingPrintVoucher bookingPrintVoucher)
        {
            try
            {
                _bizBookingPrintVoucher = new InstoreClubcardReward.Business.BookingPrintVoucher();
                ParseTransPrintVoucherToBiz(bookingPrintVoucher);
                bookingPrintVoucher.TransactionID = _bizBookingPrintVoucher.ProcessBooking();
                return ParseBizToTransPrintVoucher(bookingPrintVoucher); ;
            }
            catch (Exception ex)
            {
                CustomException objCustEx = new CustomException();
                objCustEx.StatusCode = "Error in Function:ProcessVoucherBooking()";
                objCustEx.ErrorMessage = ex.Message;
                throw new FaultException<CustomException>(objCustEx, "Exception occured in ProcessVoucherBooking");
            }

        }

       
        /// <summary>
        /// from the IP address get the parameters for the entry page
        /// returns an empty string if not in Kiosk table
        /// </summary>
        /// <param name="clientIP"></param>
        /// <returns></returns>
        public string PrintVoucherKioskEntryPage(string clientIP)
        {
            try
            {
                return InstoreClubcardReward.Business.KioskMaster.KioskEntryPageWCF(clientIP);
            }
            catch (Exception ex)
            {
                CustomException objCustEx = new CustomException();
                objCustEx.StatusCode = "Error in Function:PrintVoucherKioskEntryPage()";
                objCustEx.ErrorMessage = ex.Message;
                throw new FaultException<CustomException>(objCustEx, "Exception occured while getting the URL parameters for the entry page");
            }
        }

        /// <summary>
        /// GetUnusedVoucherDetails
        /// </summary>
        /// <param name="clubcard"></param>
        /// <param name="country"></param>
        /// <returns></returns>
        public UnusedVoucherCollection GetUnusedVoucherDetails(string clubcard)
        {
            try
            {
                _bizUnusedVoucherCollection = InstoreClubcardReward.Business.UnusedVoucherCollection.GetUnusedVoucherDetails(clubcard, "UK");
                //ParseTransPrintVoucherToBiz(bookingPrintVoucher);
                //_bizBookingPrintVoucher.UnusedVouchers.GetUnusedVoucherDetails(clubcard,"UK");
                //create objects from local Big Exchange class
                UnusedVoucherCollection objBigExVoucherColl = new UnusedVoucherCollection();
                //update voucher collection 
                foreach (InstoreClubcardReward.Business.UnusedVoucher bizUnusedVoucher in _bizUnusedVoucherCollection)
                {
                    UnusedVoucher _unusedVoucher = new UnusedVoucher();
                    _unusedVoucher.HouseholdId = bizUnusedVoucher.HouseholdId;
                    _unusedVoucher.PeriodName = bizUnusedVoucher.PeriodName;
                    _unusedVoucher.VoucherValue = bizUnusedVoucher.VoucherValue;
                    _unusedVoucher.VoucherNumber = bizUnusedVoucher.VoucherNumber;
                    _unusedVoucher.OnlineCode = bizUnusedVoucher.OnlineCode;
                    _unusedVoucher.ExpiryDate = bizUnusedVoucher.ExpiryDate;
                    _unusedVoucher.VoucherType = bizUnusedVoucher.VoucherType;
                    _unusedVoucher.Ean = bizUnusedVoucher.Ean;
                    objBigExVoucherColl.Add(_unusedVoucher);
                    
                }
                return objBigExVoucherColl;

            }
            catch (Exception ex)
            {
                CustomException objCustEx = new CustomException();
                objCustEx.StatusCode = "Error in Function:GetUnusedVoucherDetails()";
                objCustEx.ErrorMessage = ex.Message;
                throw new FaultException<CustomException>(objCustEx, "Exception occured while calling GetUnusedVoucherDetails Method");
            }
        }


        /// <summary>
        /// Common method for PrintVouchersAtKiosk Application
        /// </summary>
        /// <param name="bookingPrintVoucher"></param>
        /// <param name="error"></param>
        /// <param name="pvm"></param>
        public void Logging(BookingPrintVoucher bookingPrintVoucher, string error, LoggingOperations loggingOperations)
        {
            try
            {
                switch (loggingOperations)
                {
                    case LoggingOperations.InsertLoginAttempts:
                        InsertLoginAttempts(bookingPrintVoucher);
                        break;
                    case LoggingOperations.SaveToTransError:
                        SaveToTransError(bookingPrintVoucher, error);
                        break;
                    case LoggingOperations.SaveUnusedVouchers:
                        SaveUnusedVouchers(bookingPrintVoucher);
                        break;
                    case LoggingOperations.UpdateTranDetailsActiveVoucher:
                        UpdateTranDetailsActiveVoucher(bookingPrintVoucher);
                        break;
                    case LoggingOperations.UpdateTranDetailsPrintDate:
                        UpdateTranDetailsPrintDate(bookingPrintVoucher);
                        break;
                    case LoggingOperations.UpdateTranDetailsStatus:
                        UpdateTranDetailsStatus(bookingPrintVoucher);
                        break;
                    case LoggingOperations.UpdateTranDetailsVerifiedStartTime:
                        UpdateTranDetailsVerifiedStartTime(bookingPrintVoucher);
                        break;
                    case LoggingOperations.UpdateTranDetailsVerifiedTime:
                        UpdateTranDetailsVerifiedTime(bookingPrintVoucher);
                        break;
                }
            }
            catch (FaultException<CustomException> ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Insert data into Login_Attempts table
        /// </summary>
        /// <param name="bookingPrintVoucher"></param>
        private void InsertLoginAttempts(BookingPrintVoucher bookingPrintVoucher)
        {
            try
            {
                _bizBookingPrintVoucher = new InstoreClubcardReward.Business.BookingPrintVoucher();
                ParseTransPrintVoucherToBiz(bookingPrintVoucher);
                _bizBookingPrintVoucher.InsertLoginAttempts();

            }
            catch (Exception ex)
            {
                CustomException objCustEx = new CustomException();
                objCustEx.StatusCode = "Error in Function:InsertLoginAttempts()";
                objCustEx.ErrorMessage = ex.Message;
                throw new FaultException<CustomException>(objCustEx, "Exception occured while adding data in Login Attempts");
            }
        }

        /// <summary>
        /// Saves to Transaction_Error table.
        /// </summary>
        /// <param name="bookingPrintVoucher"></param>
        /// <param name="error"></param>
        private void SaveToTransError(BookingPrintVoucher bookingPrintVoucher, string error)
        {
            try
            {
                //ParseDataToBiz(bookingData);
                _bizBookingPrintVoucher = new InstoreClubcardReward.Business.BookingPrintVoucher();
                _bizBookingPrintVoucher.TransactionID = bookingPrintVoucher.TransactionID;
                _bizBookingPrintVoucher.SaveToTransError(error);

            }
            catch (Exception ex)
            {
                CustomException objCustEx = new CustomException();
                objCustEx.StatusCode = "Error in Function:SaveToTransError()";
                objCustEx.ErrorMessage = ex.Message;
                throw new FaultException<CustomException>(objCustEx, "Exception occured while storing errors to Transaction_Error");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bookingPrintVoucher"></param>
        private void SaveUnusedVouchers(BookingPrintVoucher bookingPrintVoucher)
        {
            try
            {
                //_bizBookingPrintVoucher = new InstoreClubcardReward.Business.BookingPrintVoucher();
                ParseTransPrintVoucherToBiz(bookingPrintVoucher);
                _bizBookingPrintVoucher.UnusedVouchers.Save(bookingPrintVoucher.TransactionID);
            }
            catch (Exception ex)
            {
                CustomException objCustEx = new CustomException();
                objCustEx.StatusCode = "Error in Function:SaveUnusedVouchers()";
                objCustEx.ErrorMessage = ex.Message;
                throw new FaultException<CustomException>(objCustEx, "Exception occured while adding data in Voucher_Details");
            }
        }

        /// <summary>
        /// Update ActiveVoucher of TransactionDetails table
        /// </summary>
        /// <param name="bookingPrintVoucher"></param>
        private void UpdateTranDetailsActiveVoucher(BookingPrintVoucher bookingPrintVoucher)
        {
            try
            {
                //_bizBookingPrintVoucher = new InstoreClubcardReward.Business.BookingPrintVoucher();
                ParseTransPrintVoucherToBiz(bookingPrintVoucher);
                _bizBookingPrintVoucher.UpdateTranDetailsActiveVoucher();
            }
            catch (Exception ex)
            {
                CustomException objCustEx = new CustomException();
                objCustEx.StatusCode = "Error in Function:UpdateTranDetailsActiveVoucher()";
                objCustEx.ErrorMessage = ex.Message;
                throw new FaultException<CustomException>(objCustEx, "Exception occured while updating ActiveVoucher in Transcation_Details");
            }
        }

        /// <summary>
        /// Update PrintDate and Status of TransactionDetails table
        /// </summary>
        /// <param name="bookingPrintVoucher"></param>
        private void UpdateTranDetailsPrintDate(BookingPrintVoucher bookingPrintVoucher)
        {
            try
            {
                //_bizBookingPrintVoucher = new InstoreClubcardReward.Business.BookingPrintVoucher();
                ParseTransPrintVoucherToBiz(bookingPrintVoucher);
                _bizBookingPrintVoucher.UpdateTranDetailsPrintDate();
            }
            catch (Exception ex)
            {
                CustomException objCustEx = new CustomException();
                objCustEx.StatusCode = "Error in Function:UpdateTranDetailsPrintDate()";
                objCustEx.ErrorMessage = ex.Message;
                throw new FaultException<CustomException>(objCustEx, "Exception occured while updating PrintDate in Transcation_Details");
            }
        }

        /// <summary>
        /// Update Status of TransactionDetails table
        /// </summary>
        /// <param name="bookingPrintVoucher"></param>
        private void UpdateTranDetailsStatus(BookingPrintVoucher bookingPrintVoucher)
        {
            try
            {
                //_bizBookingPrintVoucher = new InstoreClubcardReward.Business.BookingPrintVoucher();
                ParseTransPrintVoucherToBiz(bookingPrintVoucher);
                _bizBookingPrintVoucher.UpdateTranDetailsStatus();
            }
            catch (Exception ex)
            {
                CustomException objCustEx = new CustomException();
                objCustEx.StatusCode = "Error in Function:UpdateTranDetailsStatus()";
                objCustEx.ErrorMessage = ex.Message;
                throw new FaultException<CustomException>(objCustEx, "Exception occured while updating Status in Transcation_Details");
            }
        }

        /// <summary>
        /// Update VerifiedStartTime and Status of TransactionDetails table
        /// </summary>
        /// <param name="bookingPrintVoucher"></param>
        private void UpdateTranDetailsVerifiedStartTime(BookingPrintVoucher bookingPrintVoucher)
        {
            try
            {
                //_bizBookingPrintVoucher = new InstoreClubcardReward.Business.BookingPrintVoucher();
                ParseTransPrintVoucherToBiz(bookingPrintVoucher);
                _bizBookingPrintVoucher.UpdateTranDetailsVerifiedStartTime();
            }
            catch (Exception ex)
            {
                CustomException objCustEx = new CustomException();
                objCustEx.StatusCode = "Error in Function:UpdateTranDetailsVerifiedTime()";
                objCustEx.ErrorMessage = ex.Message;
                throw new FaultException<CustomException>(objCustEx, "Exception occured while updating VerifiedTime in Transcation_Details");
            }
        }

        /// <summary>
        /// Update VerifiedTime and Status of TransactionDetails table
        /// </summary>
        /// <param name="bookingPrintVoucher"></param>
        private void UpdateTranDetailsVerifiedTime(BookingPrintVoucher bookingPrintVoucher)
        {
            try
            {
                //_bizBookingPrintVoucher = new InstoreClubcardReward.Business.BookingPrintVoucher();
                ParseTransPrintVoucherToBiz(bookingPrintVoucher);
                _bizBookingPrintVoucher.UpdateTranDetailsVerifiedTime();
            }
            catch (Exception ex)
            {
                CustomException objCustEx = new CustomException();
                objCustEx.StatusCode = "Error in Function:UpdateTranDetailsVerifiedTime()";
                objCustEx.ErrorMessage = ex.Message;
                throw new FaultException<CustomException>(objCustEx, "Exception occured while updating VerifiedTime in Transcation_Details");
            }
        }

        public int ValidateBonusVouchers(string VoucherNumber)
        {
            int IsPresent = 0;
            try
            {
                InstoreClubcardReward.Business.Voucher _bizVoucher = new InstoreClubcardReward.Business.Voucher();
                IsPresent = _bizVoucher.ValidateBonusVouchers(VoucherNumber);

            }
            catch (Exception ex)
            {
                CustomException objCustEx = new CustomException();
                objCustEx.StatusCode = "Error in Function:ValidateBonusVouchers()";
                objCustEx.ErrorMessage = ex.Message;
                throw new FaultException<CustomException>(objCustEx, "Exception occured while Validating Bonus Vouchers");
            }

            return IsPresent;

        }
    }
}
