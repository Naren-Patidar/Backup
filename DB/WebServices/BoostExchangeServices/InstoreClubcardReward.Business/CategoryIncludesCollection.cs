using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InstoreClubcardReward.Data;

namespace InstoreClubcardReward.Business
{
    [Serializable]
    public class CategoryIncludesCollection : BaseCollection<CategoryIncludes>
    {
        /// <summary>
        /// Initializes a new instance of the CategoryIncludesCollection class.
        /// </summary>
        public CategoryIncludesCollection()
        {
            
        }

        /// <summary>
        /// Get Category Includes for a category
        /// Uses application variable per category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public static CategoryIncludesCollection GetCategoryIncludes(int categoryId)
        {
            CategoryIncludesCollection categories = new CategoryIncludesCollection();

            System.Collections.ObjectModel.Collection<SelectCategoryIncludeRow> categoryRows = new System.Collections.ObjectModel.Collection<SelectCategoryIncludeRow>();

           
                // use application variable if available
                if (System.Web.HttpContext.Current.Application[string.Format("CategoryIncludes{0}", categoryId)] != null)
                {
                    categoryRows = (System.Collections.ObjectModel.Collection<SelectCategoryIncludeRow>)System.Web.HttpContext.Current.Application[string.Format("CategoryIncludes{0}", categoryId)];
                }
                else
                {
                    // get data from database, store in application variable
                    SelectCategoryInclude selectCategoryIncludes = new SelectCategoryInclude(ConnectionString);

                    selectCategoryIncludes.CategoryId = categoryId;
                    categoryRows = selectCategoryIncludes.Execute();

                    // store the product rows in application
                    if (categoryRows.Count > 0)
                    {
                        System.Web.HttpContext.Current.Application.Add(string.Format("CategoryIncludes{0}", categoryId), categoryRows);

                    }
                }
            

            // load from the categoryRows collection
            foreach (SelectCategoryIncludeRow categoryRow in categoryRows)
            {
                categories.Add(new CategoryIncludes(categoryRow.CategoryId.Value, categoryRow.LineId.Value, categoryRow.Description1, categoryRow.Description2, categoryRow.Description3));
            }

            return categories;
        }

        /// <summary>
        /// Get Category Includes for a category
        /// Uses application variable per category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        /// Added by Seema to make a WcfCall
        public static CategoryIncludesCollection GetCategoryIncludesWCF(int categoryId)
        {
            CategoryIncludesCollection categories = new CategoryIncludesCollection();

            System.Collections.ObjectModel.Collection<SelectCategoryIncludeRow> categoryRows = new System.Collections.ObjectModel.Collection<SelectCategoryIncludeRow>();

                // get data from database
                SelectCategoryInclude selectCategoryIncludes = new SelectCategoryInclude(ConnectionString);

                selectCategoryIncludes.CategoryId = categoryId;
                categoryRows = selectCategoryIncludes.Execute();

            // load from the categoryRows collection
            foreach (SelectCategoryIncludeRow categoryRow in categoryRows)
            {
                categories.Add(new CategoryIncludes(categoryRow.CategoryId.Value, categoryRow.LineId.Value, categoryRow.Description1, categoryRow.Description2, categoryRow.Description3));
            }

            return categories;

        }
    }
}
