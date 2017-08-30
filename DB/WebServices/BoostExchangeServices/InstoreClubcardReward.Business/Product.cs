    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

namespace InstoreClubcardReward.Business
{

    [Serializable]
    public class Product : BaseClass
    {
        // product data for display and for creation

        private string _description;
        public string Description
        {
            get
            {
                string newDescription = _description;
                newDescription = newDescription.Replace("<", "");
                newDescription = newDescription.Replace(">", "");
                return newDescription;
            }
            set
            {
                _description = value;
            }
        }

        /// <summary>
        /// The description with the stripped of the contents within the angle brackets
        /// </summary>
        public string StrippedDescription
        {
            get
            {
                string newDescription = Description;
                int startIndex = _description.IndexOf("<");
                int endIndex = _description.IndexOf(">");
                if (startIndex >= 0)
                {
                    newDescription = _description.Substring(0, startIndex-1);
                }
                
                if (endIndex >= 0)
                {
                    newDescription = newDescription + _description.Substring(endIndex+1);
                }

                return newDescription;
            }
        }


        public string ProductCode { get; set; }     // AS Freetime
        public string VendorCode { get; set; }      // "99" etc  
        public int CustomerPrice { get; set; }      // pence in clubcard vouchers
        public int TokenValue { get; set; }         // pence in clubcard vouchers
        public string Country { get; set; }
        public string ValidUntil { get; set; }
        // Enable / display is determined by the stored procedure
        //public DateTime EnableDate { get; set; }    // date used to determine if product is displayed.
        //public DateTime DisableDate { get; set; }   // date that is used to stop displaying the product. 
        // date that is used to override the rolling 3 months corresponds to the last day customer can use token)
        public DateTime UsedByDate { get; set; }    // 

        public string ImageFilename { get; set; }

        public Category Category { get; set; }

        public string ShortDescription { get; set; }

        public string LongDescription { get; set; }

        public ProductType ProductType { get; set; }

        public TokenType TokenType { get; set; }

        // values for printing on the token
        public string TokenTitle { get; set; }
        public string TokenDescription { get; set; }
        public string TokenTermsAndConditions { get; set; }



        /// <summary>
        /// Initializes a new instance of the <see cref="Product"/> class.
        /// </summary>
        public Product()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Product"/> class.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="productcode">The productcode.</param>
        /// <param name="customerPrice">The customer price.</param>
        public Product(string description, string productcode, int customerPrice)
        {
            this.Description = description;
            this.ProductCode = productcode;
            this.CustomerPrice = customerPrice;
        }



        /// <summary>
        /// Initializes a new instance of the <see cref="Product"/> class.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="productcode">The productcode.</param>
        /// <param name="customerPrice">The customer price.</param>
        /// <param name="tokenValue">The token value.</param>
        /// <param name="vendorcode">The vendorcode.</param>
        /// <param name="usedByDate">The used by date.</param>
        /// <param name="imageFilename">The image filename.</param>
        /// <param name="productType">Type of the product.</param>
        /// <param name="shortDescription">The short description.</param>
        /// <param name="longDescription">The long description.</param>
        /// <param name="category">The category.</param>
        /// <param name="tokenType">Type of the token.</param>
        public Product(string description, 
                        string productcode, 
                        int customerPrice, 
                        int tokenValue,
                        string vendorcode,
                        DateTime usedByDate, 
                        string imageFilename,
                        ProductType productType,
                        String shortDescription,
                        String longDescription,
                        Category category, 
                        TokenType tokenType,
                        String tokenTitle,
                        String tokenDescription,
                        String tokenTermsAndConditions)

        {
            this.Description = description;
            this.ProductCode = productcode;
            this.CustomerPrice = customerPrice;
            this.TokenValue = tokenValue;

            this.VendorCode = vendorcode;

            this.UsedByDate = usedByDate;

            this.ImageFilename = imageFilename;

            this.ProductType = productType;

            this.ShortDescription = shortDescription;

            this.LongDescription = longDescription;

            this.Category = category;

            this.TokenType = tokenType;

            this.TokenTitle = tokenTitle;
            this.TokenDescription = tokenDescription;
            this.TokenTermsAndConditions = tokenTermsAndConditions;

        }

        /// <summary>
        /// Gets the category.
        /// </summary>
        /// <param name="categoryId">The category id.</param>
        /// <returns></returns>
        public static Product GetProduct(string productCode)
        {
            ProductCollection products = ProductCollection.GetProducts();

            // use LINQ to get the category which is the first in the collection of categories 
            // which has the same category Id

            if (products.Count == 0)
            {
                return null;
            }
            else
            {
                var chosenProduct = from p in products where p.ProductCode == productCode select p;

                if (chosenProduct.Count() > 0)
                {
                    return chosenProduct.First();
                }
                else
                {
                    return null;
                }
            }

        }

        /// <summary>
        /// Gets the Product.
        /// </summary>
        /// <param name="productCode">The product Code.</param>
        /// <returns></returns>
        /// Added by Dimple to make a WcfCall
        public static Product GetProductWCF(string productCode)
        {
            ProductCollection products = ProductCollection.GetProductsWCF();

            // use LINQ to get the category which is the first in the collection of categories 
            // which has the same category Id

            if (products.Count == 0)
            {
                return null;
            }
            else
            {
                var chosenProduct = from p in products where p.ProductCode == productCode select p;

                if (chosenProduct.Count() > 0)
                {
                    return chosenProduct.First();
                }
                else
                {
                    return null;
                }
            }

        }
        public string getTokenDescription()
        {
            return convertSymbol(TokenDescription);
        }

        public string getTokenTitle()
        {
            return convertSymbol(TokenTitle);
        }

        public string getTokenTermsAndConditions()
        {
            return convertSymbol(TokenTermsAndConditions);
        }


        // data starts off as java script
        // in database converst to strings and symbols seperated by ~
        // eg.... blbla~symbol~blablabla~symbol
        //
        private string convertSymbol(string input)
        {

            const string lb = @"\1B|1lF";
            // 1 line breaks
            const string sp = @"\1B|bC";      // space
            const string ctr = @"\1B|cA";     // Central alignment
            const string s12 = @"\1B|4C\12"; // size of characters
            const string s10 = @"\1B|4C\10"; // size of characters
            const string pnd = @"\u00A3";      // Pound symbol
            // line break and centre
            //string lb_ctr = lb + ctr;
            //string Line = ctr + sp + s12;
            //string Line_next = ctr + sp + s10;

            StringBuilder tokenScript = new StringBuilder();

            // split up the words
            string[] words = input.Split('~');

            foreach (string word in words)
            {
                switch (word)
	            {
                    case "lb":
                        tokenScript.Append(lb);
                        break;
                    case "sp":
                        tokenScript.Append(sp);
                        break;
                    case "ctr":
                        tokenScript.Append(ctr);
                        break;
                    case "s12":
                        tokenScript.Append(s12);
                        break;
                    case "s10":
                        tokenScript.Append(s10);
                        break;
                    case "pnd":
                        tokenScript.Append(pnd);
                        break;
                    case "lb_ctr":
                        tokenScript.Append(lb + ctr);
                        break;
                    case "Line":
                        tokenScript.Append(ctr + sp + s12);
                        break;
                    case "Line_next":
                        tokenScript.Append(ctr +sp + s10);
                        break;
		            default:
                        tokenScript.Append(word);
                    break;
	            }

            }

            return tokenScript.ToString(); 
        
        }


    }



}
