using System.Runtime.Serialization;

namespace InstoreClubcardReward.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class ProductLine : BaseClass
    {
        /// the basic product information.... from database
        public Product Product;

        /// user input
        public int ProductNumber { get; set; }
        /// database saving  input (only know once product lines saved)
        public int ProductLineId { get; set; }

        public TokenCollection Tokens { get; set; }


        /// cost of a line in CCV pence
        public int Cost()
        {
            return (this.Product.CustomerPrice * this.ProductNumber);
        }

        /// cost of a line as token Value pence
        public int Value()
        {
            return (this.Product.TokenValue * this.ProductNumber);
        }


       /// <summary>
        /// Initializes a new instance of the ProductLine class.
        /// </summary>
        public ProductLine()
        {
            Tokens = new TokenCollection();
        }

        /// <summary>
        /// Initializes a new instance of the ProductLine class.
        /// </summary>
        /// <param name="product"></param>
        public ProductLine(Product product, int productNumber)
        {
            Product = product;
            ProductNumber = productNumber;
        }

        /// <summary>
        /// Saves the specified bookingid.
        /// </summary>
        /// <param name="bookingid">The bookingid.</param>
        /// <param name="productlineid">The productlineid.</param>
        public void Save(int bookingid, int productlineid)
        {

            // only save if productnumber is not zero
            if (ProductNumber != 0)
            {
                // record the productlineid used for the saving - used later for saving tokens
                this.ProductLineId = productlineid;

                InstoreClubcardReward.Data.InsertProductLine ipl = new InstoreClubcardReward.Data.InsertProductLine(ConnectionString);
                ipl.BookingId = bookingid;
                ipl.ProductLineId = this.ProductLineId;
                ipl.ProductCode = this.Product.ProductCode;
                ipl.ProductNumber = this.ProductNumber;

                ipl.Execute();

            }

        }

        /// <summary>
        /// Creates the tokens.
        /// </summary>
        /// <param name="bookingId">The booking id.</param>
        /// <param name="clubcard">The clubcard.</param>
        /// <param name="productLine">The product line.</param>
        /// <param name="userId">The user id.</param>
        /// Country Code parameter added by seema
        public void CreateTokens(int bookingId, string clubcard, ProductLine productLine, int userId,string country)
        {
            // create the token collection based on 
            InitialiseTokens(productLine.Product.TokenType);
            // Country Code parameter added by seema
            Tokens.CreateTokens(bookingId, clubcard, productLine, userId, country);
        }

        /// <summary>
        /// Initialises the tokens.
        /// </summary>
        /// <param name="tokenType">Type of the token.</param>
        private void InitialiseTokens(TokenType tokenType)
        {
            switch (tokenType)
            {
                case TokenType.Direct:
                    Tokens = new DotcomTokenCollection();
                    break;
                case TokenType.Online:
                    Tokens = new DotcomTokenCollection();
                    break;
                case TokenType.Token:
                    Tokens = new TokenCollection();
                    break;
            }
        }
    }
}
