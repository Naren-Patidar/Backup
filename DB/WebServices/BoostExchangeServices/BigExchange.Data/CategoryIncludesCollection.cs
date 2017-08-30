using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Configuration;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace BigExchange
{
    /// <summary>
    /// Created Date : 08/06/2011
    /// Created By: Dimple Kandoliya
    /// Class Name: CategoryIncludesCollection
    /// </summary>
    [CollectionDataContract]
    public class CategoryIncludesCollection : BindingList<CategoryIncludes>
    {

        /// <summary>
        /// Initializes a new instance of the CategoryIncludesCollection class.
        /// </summary>
        public CategoryIncludesCollection()
        {

        }

    }
}
