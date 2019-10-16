using System.Collections.Generic;
using Sitecore.Data.Items;

namespace ESearch.Feature.SearchBox.Models
{
    public class SearchBoxResultModel
    {
        /// <summary>
        ///Gets or sets list of search result items.
        /// </summary>
        public ICollection<Item> Items { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchBoxResultModel" /> class.
        /// </summary>
        public SearchBoxResultModel()
        {
        }
    }
}
