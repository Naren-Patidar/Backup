using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InstoreClubcardReward.Data;

namespace InstoreClubcardReward.Business
{
    [Serializable]
    public class CategoryIncludes : BaseClass
    {

        public int CategoryId { get; set; }
        public int LineId { get; set; }

        public string Description1 { get; set; }
        public string Description2 { get; set; }
        public string Description3 { get; set; }

        /// <summary>
        /// Initializes a new instance of the CategoryIncludes class.
        /// </summary>
        public CategoryIncludes()
        {
            
        }


        /// <summary>
        /// Initializes a new instance of the CategoryIncludes class.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="lineId"></param>
        /// <param name="description1"></param>
        /// <param name="description2"></param>
        /// <param name="description3"></param>
        public CategoryIncludes(int categoryId, int lineId, string description1, string description2, string description3)
        {

            CategoryId = categoryId;
            LineId = lineId;
            Description1 = description1;
            Description2 = description2;
            Description3 = description3;

        }
    }
}
