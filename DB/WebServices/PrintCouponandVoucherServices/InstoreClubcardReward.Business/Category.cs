using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InstoreClubcardReward.Data;

namespace InstoreClubcardReward.Business
{
    [Serializable]
    public class Category : BaseClass
    {
        private const int INT_Constant = 0;

        /// <summary>
        /// Initializes a new instance of the Category class.
        /// </summary>

        public Category()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the Category class.
        /// </summary>
        /// <param name="categoryId"></param>
        public Category(int categoryId, string description, string imageFilename, Category parent, int? tokenValue)
        {
            Description = description;
            ImageFilename = imageFilename;
            Parent = parent;
            CategoryId = categoryId;
            TokenValue = tokenValue;
        }

        public int CategoryId { get; set; }

        public string Description { get; set; }

        public string ImageFilename { get; set; }

        public Category Parent { get; set; }

        public int? TokenValue { get; set; } // token value in pence (max value for products in category, or null if no product)
      


        /// <summary>
        /// Gets the category.
        /// </summary>
        /// <param name="categoryId">The category id.</param>
        /// <returns></returns>
        public static Category GetCategory(int categoryId)
        {
            CategoryCollection categories = CategoryCollection.GetCategories();
            
            // use LINQ to get the category which is the first in the collection of categories 
            // which has the same category Id

            if (categories.Count == 0)
            {
                return null;
            }
            else
            {
                var chosenCategories = from c in categories where c.CategoryId == categoryId select c;

                if (chosenCategories.Count() > 0)
                {
                    return chosenCategories.First();
                }
                else
                {
                    return null;
                }
            }

        }

        /// <summary>
        /// Gets the category.
        /// </summary>
        /// <param name="categoryId">The category id.</param>
        /// <returns></returns>
        /// Added by Seema to make a WcfCall
        public static Category GetCategoryWCF(int categoryId)
        {
            CategoryCollection categories = CategoryCollection.GetCategoriesWCF();

            // use LINQ to get the category which is the first in the collection of categories 
            // which has the same category Id

            if (categories.Count == 0)
            {
                return null;
            }
            else
            {
                var chosenCategories = from c in categories where c.CategoryId == categoryId select c;

                if (chosenCategories.Count() > 0)
                {
                    return chosenCategories.First();
                }
                else
                {
                    return null;
                }
            }

        }

    }

}
