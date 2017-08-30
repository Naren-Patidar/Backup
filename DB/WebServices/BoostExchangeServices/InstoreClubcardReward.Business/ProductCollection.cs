namespace InstoreClubcardReward.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    [Serializable]
    public class ProductCollection : BaseCollection<Product>
    {
        /// For year long exchange
        public const int YEARLONGEX_MONTHS = 6;

        //public bool IsInstoreBooking { get; set; }

        /// <summary>
        ///  get the product collection. from app variable or from database if null
        /// </summary>
        /// <returns></returns>
        public static ProductCollection GetProducts()
        {
            ProductCollection products = new ProductCollection();

            System.Collections.ObjectModel.Collection<Data.SelectProductsAllRow> productRows;

            
                if (System.Web.HttpContext.Current.Application["Products"] != null)
                {
                    productRows = (System.Collections.ObjectModel.Collection<Data.SelectProductsAllRow>)System.Web.HttpContext.Current.Application["Products"];
                }
                else
                {
                    Data.SelectProductsAll selectProductsAll = new Data.SelectProductsAll(ConnectionString);

                    productRows = selectProductsAll.Execute();

                    // Store the product rows in application
                    if (productRows.Count > 0)
                    {
                        System.Web.HttpContext.Current.Application.Add("Products", productRows);
                    }

                }

            if (productRows.Count > 0)
            {
                //var typedProductRows =
                //    from row in productRows
                //    where row.ProductType.Value == (int)productType
                //    // control the order of the collection 
                //    // this reflects stored procedure (and hence display)
                //    orderby row.DisplayOrder, row.ProductCode
                //    select row;


                foreach (Data.SelectProductsAllRow row in productRows)
                {
                    // used by date can be null
                    DateTime usedbydate;
                    if (row.UsedByDate.HasValue)
                    {
                        usedbydate = row.UsedByDate.Value;
                        
                    }
                    else
                    {
                        //Year long run Exchange
                        if (row.UsedByDate == null)
                            usedbydate = DateTime.Today.AddMonths(YEARLONGEX_MONTHS);
                        else
                            usedbydate = new DateTime();
                    }                   


                    products.Items.Add(new Product(row.Description,
                                       row.ProductCode,
                                       row.CustomerPrice.GetValueOrDefault(),
                                       row.TokenValue.GetValueOrDefault(),
                                       row.VendorCode,
                                       usedbydate,
                                       row.ImageFilename,
                                       (ProductType)Enum.Parse(typeof(ProductType), row.ProductType.ToString()),
                                       row.ShortDescription,
                                       row.LongDescription,
                                       Category.GetCategory(row.CategoryId.Value),
                                       (TokenType)Enum.Parse(typeof(TokenType), row.TokenType.ToString()),
                                       row.TokenTitle,
                                       row.TokenDescription,
                                       row.TokenTermsAndConditions
                                       
                                       ));

                }
            }

            return products;

        }


        /// <summary>
        ///  get the product collection. from app variable or from database if null
        /// </summary>
        /// <returns></returns>
        /// Added by Seema to make a WcfCall
        public static ProductCollection GetProductsWCF()
        {
            ProductCollection products = new ProductCollection();

            System.Collections.ObjectModel.Collection<Data.SelectProductsAllRow> productRows;
            Data.SelectProductsAll selectProductsAll = new Data.SelectProductsAll(ConnectionString);
            productRows = selectProductsAll.Execute();


            if (productRows.Count > 0)
            {
                //var typedProductRows =
                //    from row in productRows
                //    where row.ProductType.Value == (int)productType
                //    // control the order of the collection 
                //    // this reflects stored procedure (and hence display)
                //    orderby row.DisplayOrder, row.ProductCode
                //    select row;


                foreach (Data.SelectProductsAllRow row in productRows)
                {
                    // used by date can be null
                    DateTime usedbydate;
                    if (row.UsedByDate.HasValue)
                    {
                        usedbydate = row.UsedByDate.Value;

                    }
                    else
                    {
                        //Year long run Exchange
                        if (row.UsedByDate == null)
                            usedbydate = DateTime.Today.AddMonths(YEARLONGEX_MONTHS);
                        else
                            usedbydate = new DateTime();
                    }


                    products.Items.Add(new Product(row.Description,
                                       row.ProductCode,
                                       row.CustomerPrice.GetValueOrDefault(),
                                       row.TokenValue.GetValueOrDefault(),
                                       row.VendorCode,
                                       usedbydate,
                                       row.ImageFilename,
                                       (ProductType)Enum.Parse(typeof(ProductType), row.ProductType.ToString()),
                                       row.ShortDescription,
                                       row.LongDescription,
                                       Category.GetCategoryWCF(row.CategoryId.Value),
                                       (TokenType)Enum.Parse(typeof(TokenType), row.TokenType.ToString()),
                                       row.TokenTitle,
                                       row.TokenDescription,
                                       row.TokenTermsAndConditions

                                       ));

                }
            }

            return products;

        }
  
        /// <summary>
        /// Gets the index.
        /// </summary>
        /// <param name="productCode">The product code.</param>
        /// <returns></returns>
        public int GetIndex(string productCode)
        {
            for (int index = 0; index < Items.Count; index++)
            {
                if (Items[index].ProductCode == productCode)
                {
                    return index;
                }
            }
            return -1;
        }

        /// get the java string TokenDescription 
        /// for a ProductCode in the collection
        public string GetTokenDescription(string productCode)
        {
            return Product.GetProduct(productCode).getTokenDescription();
            // first item with the productcode.... then get the token description
            //return (Items.First(p => p.ProductCode == productCode)).getTokenDescription();

        }

        /// get the java string TokenDescription 
        /// for a ProductCode in the collection
        public string GetTokenDescriptionWCF(string productCode)
        {
            return Product.GetProductWCF(productCode).getTokenDescription();
            // first item with the productcode.... then get the token description
            //return (Items.First(p => p.ProductCode == productCode)).getTokenDescription();

        }

    }
}
