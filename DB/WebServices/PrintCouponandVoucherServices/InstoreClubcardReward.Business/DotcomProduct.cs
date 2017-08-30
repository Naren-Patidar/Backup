
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
using System.Runtime.Serialization;

    namespace InstoreClubcardReward.Business
    {
        [Serializable]
        public class DotcomProduct : Product
        {
            // reference product constructor
            public DotcomProduct(string description, 
                    string productcode, 
                    int unitcost, 
                    int tokenvalue, 
                    string coupontype, 
                    DateTime usedbydate, 
                    string imageFilename, 
                    ProductType productType, 
                    string shortDescription, 
                    string LongDescription, 
                    Category category, 
                    TokenType tokenType,
                    String tokenTitle,
                    String tokenDescription,
                    String tokenTermsAndConditions)
                : base(description, productcode, unitcost, tokenvalue, coupontype, usedbydate, imageFilename, productType, shortDescription, LongDescription, category, tokenType, tokenTitle, tokenDescription, tokenTermsAndConditions)

            {
            }

            /// <summary>
            /// Initializes a new instance of the DotcomProduct class.
            /// </summary>
            public DotcomProduct()
            {
            }

        }
    }

