using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InstoreClubcardReward.Data;

namespace InstoreClubcardReward.Business
{
    [Serializable]
    public class CategoryCollection : BaseCollection<Category>
    {
        /// <summary>
        /// Initializes a new instance of the CategoryCollection class.
        /// </summary>
        public CategoryCollection()
        {
            
        }

        /// <summary>
        /// Gets the categories(default 0 - all)
        /// </summary>
        /// <returns></returns>
        public static CategoryCollection GetCategories()
        {
            return GetCategories(0);
        }

        /// <summary>
        /// Gets the categories(default 0 - all)
        /// </summary>
        /// <returns></returns>
        /// Added by Seema to make a WcfCall
        public static CategoryCollection GetCategoriesWCF()
        {
            return GetCategoriesWCF(0);
        }



        /// <summary>
        /// Gets the categories.
        /// </summary>
        /// <param name="value">get categories that match the tokenvalue (0 - all)</param>
        /// <returns></returns>
        public static CategoryCollection GetCategories(int? tokenValue)
        {
            CategoryCollection categories = new CategoryCollection();

            System.Collections.ObjectModel.Collection<SelectCategoriesAllRow> categoryRows = new System.Collections.ObjectModel.Collection<SelectCategoriesAllRow>();
            
                if (System.Web.HttpContext.Current.Application["Categories"] != null)
                {
                    categoryRows = (System.Collections.ObjectModel.Collection<SelectCategoriesAllRow>)System.Web.HttpContext.Current.Application["Categories"];
                }
                else
                {
                    SelectCategoriesAll selectCategoriesAll = new SelectCategoriesAll(ConnectionString);

                    categoryRows = selectCategoriesAll.Execute();

                    // store the product rows in application
                    if (categoryRows.Count > 0)
                    {
                        System.Web.HttpContext.Current.Application.Add("Categories", categoryRows);

                    }
                }
         
            foreach (SelectCategoriesAllRow categoryRow in categoryRows)
            {
                //0 - all, 
                if (tokenValue == 0 || categoryRow.TokenValue == tokenValue)
                {
                    categories.Add(new Category(categoryRow.CategoryId.Value, categoryRow.Description, categoryRow.ImageFilename, null, categoryRow.TokenValue));
                }
            }

            return categories;

        }



        /// <summary>
        /// Gets the categories.
        /// </summary>
        /// <param name="value">get categories that match the tokenvalue (0 - all)</param>
        /// <returns></returns>
        /// Added by Seema to make a WcfCall
        public static CategoryCollection GetCategoriesWCF(int? tokenValue)
        {
            CategoryCollection categories = new CategoryCollection();

            System.Collections.ObjectModel.Collection<SelectCategoriesAllRow> categoryRows = new System.Collections.ObjectModel.Collection<SelectCategoriesAllRow>();

            SelectCategoriesAll selectCategoriesAll = new SelectCategoriesAll(ConnectionString);

            categoryRows = selectCategoriesAll.Execute();

            foreach (SelectCategoriesAllRow categoryRow in categoryRows)
            {
                //0 - all, 
                if (tokenValue == 0 || categoryRow.TokenValue == tokenValue)
                {
                    categories.Add(new Category(categoryRow.CategoryId.Value, categoryRow.Description, categoryRow.ImageFilename, null, categoryRow.TokenValue));
                }
            }
            return categories;

        }
        


    }
}
