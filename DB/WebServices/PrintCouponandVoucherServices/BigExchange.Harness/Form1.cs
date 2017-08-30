using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BigExchange.Harness.serviceClubcardReward;
using ProductLineCollection = BigExchange.Harness.serviceClubcardReward.ProductLineCollection;
using VoucherCollection = BigExchange.Harness.serviceClubcardReward.VoucherCollection;
using System.Configuration;
using System.ServiceModel;

namespace BigExchange.Harness
{
    public partial class Form1 : Form
    {
        serviceClubcardReward.ClubcardRewardClient bookingClient = new serviceClubcardReward.ClubcardRewardClient();
        
        public Form1()
        {
            InitializeComponent();
        }

        

        private void btnGetBalance_Click(object sender, EventArgs e)
        {
            //in order to test GetBookingBalance on the service we need
            //to pass a booking that contains a voucher with value and a productLine
            //with a cost. Also need to pass populate any other objects that will get passed in service layer
            //, otherwise we'll receive an object null exception


            serviceClubcardReward.Voucher voucher = new serviceClubcardReward.Voucher();
            voucher.Ean = "1234567890123456789012";

            serviceClubcardReward.Category category =new Category();
            category.CategoryId = 1;
            category.Description = "random category description";
            category.ImageFilename = "image filename goes here";
            category.TokenValue = 1;
            category.Parent = null; //TODO not sure how this should work

            serviceClubcardReward.Token token = new Token();
            token.Alpha = "";
            token.CustomerDate = DateTime.Now;
            token.EAN = "1234567890123456789012";
            token.EndDate = DateTime.Now;
            token.ProductCode = "D0099";
            token.ProductLineId = 1;
            token.ResponseCode = 1;
            token.SupplierTokenCodeId = 1;
            token.SupplyDate = DateTime.Now;
            token.TokenId = 1;
            token.TokenValue = 1;
            token.UsedByDate = DateTime.Now;
            token.VendorCode = 1;


            serviceClubcardReward.ProductLine productLine = new serviceClubcardReward.ProductLine();
            productLine.ProductNumber = 1;
            productLine.ProductLineId = 1;
            productLine.Product = new Product();
            productLine.Product.CustomerPrice = 2;
            productLine.ProductNumber = 2;
            productLine.Product.ProductType = ProductType.Instore;
            productLine.Product.TokenType = TokenType.Token;
            productLine.Product.VendorCode ="84";
            productLine.Product.Category = category;
           // productLine.Cost = 100;
            
            
            //BookingData give access to service data contract
            serviceClubcardReward.Booking bookingData = new serviceClubcardReward.Booking() ;
            bookingData.Vouchers = new VoucherCollection();
            bookingData.Vouchers.Add(voucher);

           // bookingData.Tokens = new TokenCollection();
          //  bookingData.Tokens.Add(token);

            bookingData.ProductLines = new ProductLineCollection();
            //productLine contains a collection of tokens
          //  productLine.Tokens = bookingData.Tokens;
            bookingData.ProductLines.Add(productLine);

            //add the product from the ProductLine to the Products collection
            bookingData.Products = new ProductCollection();
            bookingData.Products.Add(productLine.Product);

            //BookingClient gives access to the service methods
           // this.txtResult.Text = bookingClient.GetBookingBalance(bookingData).ToString();
           
        }

        private void btnBalanceLocalRef_Click(object sender, EventArgs e)
        {
            //InstoreClubcardReward.Business.Booking booking = new Booking();


            //InstoreClubcardReward.Business.Voucher voucher = new InstoreClubcardReward.Business.Voucher();
            //InstoreClubcardReward.Business.ProductLine productLine = new InstoreClubcardReward.Business.ProductLine();

            //productLine.Product.CustomerPrice = 2;
            //productLine.ProductNumber = 2;
            
            //voucher.Ean = "1234567890";

            //this.txtResult.Text = booking.GetBookingBalance().ToString();

        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            try
            {
                serviceClubcardReward.ProductCollection objProdColl = new serviceClubcardReward.ProductCollection();
                objProdColl = bookingClient.GetProducts();
            }
            catch (FaultException<serviceClubcardReward.CustomException> ex)
            {
                //Process the Exception
            }
            catch (System.ServiceModel.CommunicationException ex)
            {
            }

        }

        private void btnCategories_Click(object sender, EventArgs e)
        {
            try
            {
                serviceClubcardReward.CategoryCollection objCategoryColl = new serviceClubcardReward.CategoryCollection();
                objCategoryColl = bookingClient.GetCategories();
            }
            catch (FaultException<serviceClubcardReward.CustomException> ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (System.ServiceModel.CommunicationException ex)
            {
            }
        }

        private void btnCatInc_Click(object sender, EventArgs e)
        {

            try
            {
                serviceClubcardReward.CategoryIncludesCollection objCatIncColl = new serviceClubcardReward.CategoryIncludesCollection();
                objCatIncColl = bookingClient.GetCategoryIncludes(22);
            }
            catch (FaultException<serviceClubcardReward.CustomException> ex)
            {
                //Process the Exception
            }
            catch (System.ServiceModel.CommunicationException ex)
            {
            }

        }

        private void btnPrintReason_Click(object sender, EventArgs e)
        {

            try
            {
                serviceClubcardReward.PrintReasonCollection objCatIncColl = new serviceClubcardReward.PrintReasonCollection();
                objCatIncColl = bookingClient.GetPrintReasons();
            }
            catch (FaultException<serviceClubcardReward.CustomException> ex)
            {
                //Process the Exception
            }
            catch (System.ServiceModel.CommunicationException ex)
            {
            }
        }

        private void btnValidateVchr_Click(object sender, EventArgs e)
        {
            try
            {

                serviceClubcardReward.VoucherCollection objVoucherColl = new serviceClubcardReward.VoucherCollection();

                serviceClubcardReward.Voucher voucher = new serviceClubcardReward.Voucher();
                //voucher.Ean = "9611211025929056345842";
                //voucher.Clubcard = "6340049020452739";
                voucher.Ean = "9611211016387594519215";
                voucher.Clubcard = "6340081000244596";
                //voucher.Alpha = "AB7WT67Y6KBR";
                voucher.Channel = "20";
                objVoucherColl.Add(voucher);
                objVoucherColl = bookingClient.ValidateVouchers(objVoucherColl, "6340081000244596", 1, "UK");
    
            }
            catch (FaultException<serviceClubcardReward.CustomException> ex)
            {
                //Process the Exception
            }
            catch (System.ServiceModel.CommunicationException ex)
            {
            }
        }

        private void btnProcessBooking_Click(object sender, EventArgs e)
        {
            try
            {

                serviceClubcardReward.Voucher voucher = new serviceClubcardReward.Voucher();
                voucher.Ean = "9611211021198911065003";
                voucher.Clubcard = "634004024044289116";
                voucher.IsUsed = true;
                voucher.Alpha = "AYR9336D84TC";
                voucher.Type = VoucherTypes.Tesco;

                serviceClubcardReward.Category category = new Category();
                category.CategoryId = 32;
                category.Description = "Finest Food and Finest Soft Drinks";
                category.ImageFilename = "image filename goes here";
                category.TokenValue = 1;
                category.Parent = null; //TODO not sure how this should work

                serviceClubcardReward.Token token = new Token();
                token.Alpha = "";
                token.CustomerDate = DateTime.Now;
                token.EAN = "9611211021198911065003";
                token.EndDate = DateTime.Now;
                token.ProductCode = "D0099";
                token.ProductLineId = 1;
                token.ResponseCode = 1;
                token.SupplierTokenCodeId = 1;
                token.SupplyDate = DateTime.Now;
                token.TokenId = 1;
                token.TokenValue = 1;
                token.UsedByDate = DateTime.Now;
                token.VendorCode = 1;


                serviceClubcardReward.ProductLine productLine = new serviceClubcardReward.ProductLine();
                productLine.ProductNumber = 1;
                productLine.ProductLineId = 1;
                productLine.Product = new Product();
                productLine.Product.CustomerPrice = 2;
                productLine.ProductNumber = 2;
                productLine.Product.ProductCode = "D0099";
                productLine.Product.VendorCode = "92";
                productLine.Product.ProductType = ProductType.Instore;
                productLine.Product.TokenType = TokenType.Token;
                productLine.Product.Category = category;
                productLine.Product.TokenValue = 2000;
                productLine.Product.Description = "Description";
                productLine.Product.StrippedDescription = "StrippedDescription";
                // productLine.Cost = 100;

              

                //BookingData give access to service data contract
                serviceClubcardReward.Booking bookingData = new serviceClubcardReward.Booking();
                bookingData.Vouchers = new VoucherCollection();
                bookingData.Vouchers.Add(voucher);

                TokenCollection tokens = new TokenCollection();
                tokens.Add(token);

                bookingData.ProductLines = new ProductLineCollection();
                //productLine contains a collection of tokens
                productLine.Tokens = tokens;
                bookingData.ProductLines.Add(productLine);

                //add the product from the ProductLine to the Products collection
                bookingData.Products = new ProductCollection();
                bookingData.Products.Add(productLine.Product);
                bookingData.StoreId = 2661;
                bookingData.UserId = 1;
                bookingData.TillId = 1;
                bookingData.Clubcard = "634004024044289124";

                //BookingClient gives access to the service methods
               //Boolean lbFlag= bookingClient.ProcessBooking(bookingData,"UK");
                serviceClubcardReward.Booking rtnBookingData = new serviceClubcardReward.Booking();
                rtnBookingData = bookingClient.ProcessBooking(bookingData, "UK");
           
            }
            catch (FaultException<serviceClubcardReward.CustomException> ex)
            {
                //Process the Exception
            }
            catch (System.ServiceModel.CommunicationException ex)
            {
            }
        }

        private void btnBookingErr_Click(object sender, EventArgs e)
        {
            try
            {
                serviceClubcardReward.Booking bookingData = new serviceClubcardReward.Booking();
                bookingData.BookingId = 5968667;
                bookingClient.SaveToBookingError(bookingData, "Error Testing");
            }
            catch (FaultException<serviceClubcardReward.CustomException> ex)
            {
                //Process the Exception
            }
            catch (System.ServiceModel.CommunicationException ex)
            {
            }

        }

        private void btnSavetotraining_Click(object sender, EventArgs e)
        {
            try
            {
                serviceClubcardReward.Booking bookingData = new serviceClubcardReward.Booking();
                bookingData.StoreId = 2661;
                bookingData.TillId = 1;
                bookingData.UserId = 1;
                bookingData.BookingId = 5968667;
                bookingClient.SaveToTraining("Training Testing", bookingData.StoreId, bookingData.TillId, bookingData.UserId, bookingData.BookingId);
            }
            catch (FaultException<serviceClubcardReward.CustomException> ex)
            {
                //Process the Exception
            }
            catch (System.ServiceModel.CommunicationException ex)
            {
            }

        }

        private void btnGetBookingsForCC_Click(object sender, EventArgs e)
        {
            try
            {
                serviceClubcardReward.Booking bookingData = new serviceClubcardReward.Booking();
                List<serviceClubcardReward.Booking> objBigExBookingColl;
                DateTime startDate = new DateTime(2011, 5, 1);
                DateTime endDate = DateTime.Now;

                bookingData.Clubcard = "634004024070325214";
                objBigExBookingColl = bookingClient.GetBookingsForClubcard(bookingData.Clubcard, startDate, endDate, 0);
            }
            catch (FaultException<serviceClubcardReward.CustomException> ex)
            {
                //Process the Exception
            }
            catch (System.ServiceModel.CommunicationException ex)
            {
            }
        }

        private void btnRePrintTokenScript_Click(object sender, EventArgs e)
        {
            try
            {
                serviceClubcardReward.Voucher voucher = new serviceClubcardReward.Voucher();
                voucher.Ean = "9611211021198911065003";
                voucher.Clubcard = "634004024044289124";
                voucher.IsUsed = true;
                voucher.Alpha = "AXFY76X3YTF6";
                voucher.Type = VoucherTypes.Tesco;

                serviceClubcardReward.Category category = new Category();
                category.CategoryId = 1;
                category.Description = "random category description";
                category.ImageFilename = "image filename goes here";
                category.TokenValue = 1;
                category.Parent = null; //TODO not sure how this should work

                serviceClubcardReward.Token token = new Token();
                token.Alpha = "";
                token.CustomerDate = DateTime.Now;
                token.EAN = "9611211021198911065003";
                token.EndDate = DateTime.Now;
                token.ProductCode = "D0099";
                token.ProductLineId = 1;
                token.ResponseCode = 1;
                token.SupplierTokenCodeId = 1;
                token.SupplyDate = DateTime.Now;
                token.TokenId = 7488888;
                token.TokenValue = 1;
                token.UsedByDate = DateTime.Now;
                token.VendorCode = 1;

                serviceClubcardReward.ProductLine productLine = new serviceClubcardReward.ProductLine();
                productLine.ProductNumber = 1;
                productLine.ProductLineId = 0;
                productLine.Product = new Product();
                productLine.Product.CustomerPrice = 2;
                //productLine.ProductNumber = 2;
                productLine.Product.ProductCode = "D0099";
                productLine.Product.VendorCode = "80";
                productLine.Product.ProductType = ProductType.Instore;
                productLine.Product.TokenType = TokenType.Token;
                productLine.Product.Category = category;
                productLine.Product.TokenValue = 1000;
                // productLine.Cost = 100;


                //BookingData give access to service data contract
                serviceClubcardReward.Booking bookingData = new serviceClubcardReward.Booking();
                bookingData.Vouchers = new VoucherCollection();
                bookingData.Vouchers.Add(voucher);

                TokenCollection tokens = new TokenCollection();
                tokens.Add(token);

                bookingData.ProductLines = new ProductLineCollection();
                //productLine contains a collection of tokens
                productLine.Tokens = tokens;
                bookingData.ProductLines.Add(productLine);

                //add the product from the ProductLine to the Products collection
                bookingData.Products = new ProductCollection();
                bookingData.Products.Add(productLine.Product);
                bookingData.StoreId = 2661;
                bookingData.UserId = 1;
                bookingData.TillId = 1;
                bookingData.Clubcard = "634004024044289124";

                bookingData.BookingId = 5968671;
                string result;
                bookingData.BookingId = 5968671;
                result = bookingClient.GetReprintTillTokenScript(bookingData, bookingData.BookingId, bookingData.ReprintReason);
            }
            catch (FaultException<serviceClubcardReward.CustomException> ex)
            {
                //Process the Exception
            }
            catch (System.ServiceModel.CommunicationException ex)
            {
            }
        }

        private void btnTillTokenScript_Click(object sender, EventArgs e)
        {
            try
            {
                serviceClubcardReward.Voucher voucher = new serviceClubcardReward.Voucher();
                voucher.Ean = "9611211052429151413310";
                voucher.Clubcard = "634004022026341616";
                voucher.IsUsed = true;
                voucher.Alpha = "ACBCXQDD8QYC";
                voucher.Type = VoucherTypes.Tesco;

                serviceClubcardReward.Category category = new Category();
                category.CategoryId = 33;
                category.Description = "random category description";
                category.ImageFilename = "image filename goes here";
                category.TokenValue = 1;
                category.Parent = null; //TODO not sure how this should work

                serviceClubcardReward.Token token = new Token();
                token.Alpha = "";
                token.CustomerDate = DateTime.Now;
                token.EAN = "9682150021236087442178";
                token.EndDate = DateTime.Now;
                token.ProductCode = "D0099";
                token.ProductLineId = 1;
                token.ResponseCode = 1;
                token.SupplierTokenCodeId = 1;
                token.SupplyDate = DateTime.Now;
                token.TokenId = 7488888;
                token.TokenValue = 1;
                token.UsedByDate = DateTime.Now;
                token.VendorCode = 1;

                serviceClubcardReward.ProductLine productLine = new serviceClubcardReward.ProductLine();
                productLine.ProductNumber = 1;
                productLine.ProductLineId = 0;
                productLine.Product = new Product();
                productLine.Product.CustomerPrice = 2;
                //productLine.ProductNumber = 2;
                productLine.Product.ProductCode = "D0099";
                productLine.Product.VendorCode = "80";
                productLine.Product.ProductType = ProductType.Instore;
                productLine.Product.TokenType = TokenType.Token;
                productLine.Product.Category = category;
                productLine.Product.TokenValue = 1000;
                // productLine.Cost = 100;

   
                //BookingData give access to service data contract
                serviceClubcardReward.Booking bookingData = new serviceClubcardReward.Booking();
                bookingData.Vouchers = new VoucherCollection();
                bookingData.Vouchers.Add(voucher);

                TokenCollection tokens = new TokenCollection();
                tokens.Add(token);

                bookingData.ProductLines = new ProductLineCollection();
                //productLine contains a collection of tokens
                productLine.Tokens = tokens;
                bookingData.ProductLines.Add(productLine);
                
                //add the product from the ProductLine to the Products collection
                bookingData.Products = new ProductCollection();
                bookingData.Products.Add(productLine.Product);
                bookingData.StoreId = 2661;
                bookingData.UserId = 1;
                bookingData.TillId = 1;
                bookingData.Clubcard = "634004022026341616";

                bookingData.BookingId = 6213268;
                //bookingData.ReprintReason = 1;
                string result;
                result = bookingClient.GetTillTokenScript(bookingData, null);
            }
            catch (FaultException<serviceClubcardReward.CustomException> ex)
            {
                //Process the Exception
            }
            catch (System.ServiceModel.CommunicationException ex)
            {
            }
        }

        private void btnKioskURl_Click(object sender, EventArgs e)
        {
            string clientIp = txtResult.Text;
            string getURL;
            getURL = bookingClient.KioskEntryPage(clientIp);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //serviceClubcardReward.UnusedVoucherCollection vouchers = new serviceClubcardReward.UnusedVoucherCollection();
                serviceClubcardReward.UnusedVoucher unUsedVoucher = new serviceClubcardReward.UnusedVoucher();
                //voucher.Ean = "9611211021198911065003";
                //voucher.Clubcard = "634004024044289116";
                //voucher.IsUsed = true;
                //voucher.Alpha = "AYR9336D84TC";
                //voucher.Type = VoucherTypes.Tesco;

                //BookingData give access to service data contract
                serviceClubcardReward.BookingPrintVoucher bookingData = new serviceClubcardReward.BookingPrintVoucher();
                bookingData.UnusedVouchers = new UnusedVoucherCollection();
                unUsedVoucher.Ean = "9611211021198911065003";
                //unUsedVoucher.ExpiryDate = "634004024044289116";
                unUsedVoucher.HouseholdId = "123123";
                unUsedVoucher.OnlineCode = "AYR9336D84TC";
                unUsedVoucher.PeriodName = "PeriodName";
                unUsedVoucher.VoucherNumber = "198911065003";
                unUsedVoucher.VoucherValue = 12;
                unUsedVoucher.VoucherType = 1;

                bookingData.UnusedVouchers.Add(unUsedVoucher);

                bookingData.TransactionID = 45;
                bookingData.StoreID = 1;
                bookingData.KioskNo = 1;
                bookingData.KioskID = 2;
                bookingData.Clubcard = "6340010061388695";
                bookingData.PostCode = "MK45 1JF";
                bookingData.AddressLine1 = "12 BOONS PLACE";
                bookingData.Status = 1;
                bookingData.totalActiveVouchers = 10;
                //BookingClient gives access to the service methods
                //Boolean lbFlag= bookingClient.ProcessBooking(bookingData,"UK");
                //serviceClubcardReward.BookingPrintVoucher rtnBookingData = new serviceClubcardReward.BookingPrintVoucher();
                int rtnVal;
                string rtnStr;
                //rtnStr= bookingClient.PrintVoucherKioskEntryPage("127.0.0.1");

               string s=  bookingClient.UpdateTranDetailsStatus1(bookingData);
                //bookingClient.UpdateTranDetailsActiveVoucher(bookingData);
                //bookingClient.SaveToTransError(bookingData,"Testing Error");
                //rtnVal = bookingClient.SaveToBookingTable(bookingData);
                
                //MessageBox.Show(rtnVal.ToString() + " " + bookingData.TransactionID.ToString());
                MessageBox.Show(bookingData.Status.ToString());
            }
            catch (FaultException<serviceClubcardReward.CustomException> ex)
            {
                //Process the Exception
            }
            catch (System.ServiceModel.CommunicationException ex)
            {
            }
        }
        private void btnKioskPrintVoucher_Click(object sender, EventArgs e)
        {
            try
            {
                string rtnStr;
                rtnStr= bookingClient.PrintVoucherKioskEntryPage("127.0.0.1");

                MessageBox.Show(rtnStr);
            }
            catch (FaultException<serviceClubcardReward.CustomException> ex)
            {
                //Process the Exception
            }
            catch (System.ServiceModel.CommunicationException ex)
            {
            }
        }

     

    }
}
