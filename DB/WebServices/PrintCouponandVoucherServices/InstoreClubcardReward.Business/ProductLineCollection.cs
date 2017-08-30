using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using InstoreClubcardReward.Data;
using System.Collections.ObjectModel;

namespace InstoreClubcardReward.Business
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ProductLineCollection : BaseCollection<ProductLine>
    {
        /// <summary>
        /// Adds the specified product line.
        /// </summary>
        /// <param name="productLine">The product line.</param>
        public new void Add(ProductLine productLine)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Product.ProductCode == productLine.Product.ProductCode)
                {
                    Items[i].ProductNumber = productLine.ProductNumber;
                    return;
                }
            }
            Items.Add(productLine);
        }
        /// <summary>
        /// Total up the cost for the product array
        /// </summary>
        /// <returns></returns>
        public int GetTotal()
        {
            int total = 0;
            foreach (ProductLine productline in Items)
            {
                total += productline.Cost();
            }
            return total;
        }

        /// <summary>
        /// Total up the token value for the product array
        /// </summary>
        /// <returns></returns>
        public int GetTotalTokenValue()
        {
            int total = 0;
            foreach (ProductLine productline in Items)
            {
                total += productline.Value();
            }
            return total;
        }


        /// <summary>
        /// Resets the product numbers back to zero.
        /// </summary>
        public void ResetProductNumbers()
        {
            foreach (ProductLine productline in Items)
            {
                productline.ProductNumber = 0;
            }
        }

        
        /// <summary>
        /// Gets the product number total for the collection
        /// </summary>
        /// <returns></returns>
        public int GetProductNumberTotal()
        {
            int total = 0;
            foreach (ProductLine productline in Items)
            {
                total += productline.ProductNumber;
            }
            return total;
        }

        /// <summary>
        /// Gets the products the number for product.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <returns>the total product number for the given product.</returns>
        public int GetproductNumberForProduct(Product product)
        {
            // get the group of products which match the product (based on product code)
            var products = from p in Items where p.Product.ProductCode == product.ProductCode
                           select p;

            // sum the product number and return this
            return products.Sum(p => p.ProductNumber);
        }



        public void RemoveProduct(Product product)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].Product.ProductCode == product.ProductCode)
                {
                    Items.RemoveAt(i);
                }
            }
        }


        /// <summary>
        /// save the product lines for the booking
        /// </summary>
        /// <param name="BookingId">The booking id.</param>
        public void Save(int BookingId)
        {

            // insert records for the collection
            int productLineId = 0;  // product line id managed from collection
            try
            {
                foreach (ProductLine productline in Items)

                {
                    //  only save if productnumber is not zero    
                    if (productline.ProductNumber != 0)
                    {
                        productline.Save(BookingId, productLineId);
                        productLineId++;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new BookingException(ErrorTypes.InsertProductLine, "", ex);
            }

        }



        public static ProductLineCollection GetProductLines(int bookingId, int userid)
        {
            Collection<SelectProductLinesByBookingIdRow> productLines = null;
            try
            {
                SelectProductLinesByBookingId productLinesByBookingId = new SelectProductLinesByBookingId(ConnectionString);
                productLinesByBookingId.BookingId = bookingId;
                productLines = productLinesByBookingId.Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving products", ex);
            }

            ProductLineCollection pc = new ProductLineCollection();

            try
            {
                foreach (SelectProductLinesByBookingIdRow productLine in productLines)
                {
                    ProductLine pl = new ProductLine();

                    int bookingid = (int)productLine.BookingId;
                    pl.ProductLineId = (int)productLine.ProductLineId;
                    pl.ProductNumber = (int)productLine.ProductNumber;

                    pl.Tokens = TokenCollection.GetTokens(bookingid, userid);
                    pl.Product = Product.GetProduct(productLine.ProductCode.ToString());
                    pc.Add(pl);

                }

                return pc;
            }
            catch (Exception ex)
            {
                throw new Exception("Error populating product lines", ex);
            }
        }

        /// Added by Dimple to make a WcfCall
        public static ProductLineCollection GetProductLinesWCF(int bookingId, int userid)
        {
            Collection<SelectProductLinesByBookingIdRow> productLines = null;
            try
            {
                SelectProductLinesByBookingId productLinesByBookingId = new SelectProductLinesByBookingId(ConnectionString);
                productLinesByBookingId.BookingId = bookingId;
                productLines = productLinesByBookingId.Execute();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving products", ex);
            }

            ProductLineCollection pc = new ProductLineCollection();

            try
            {
                foreach (SelectProductLinesByBookingIdRow productLine in productLines)
                {
                    ProductLine pl = new ProductLine();

                    int bookingid = (int)productLine.BookingId;
                    pl.ProductLineId = (int)productLine.ProductLineId;
                    pl.ProductNumber = (int)productLine.ProductNumber;

                    pl.Tokens = TokenCollection.GetTokens(bookingid, userid);
                    pl.Product = Product.GetProductWCF(productLine.ProductCode.ToString());
                    pc.Add(pl);

                }

                return pc;
            }
            catch (Exception ex)
            {
                throw new Exception("Error populating product lines", ex);
            }
        }

    }
}
