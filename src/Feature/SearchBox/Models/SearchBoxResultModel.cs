using System.Collections.Generic;
using Sitecore.Data.Items;

namespace ESearch.Feature.SearchBox.Models
{
    /// <summary>
    /// result data searched by the search box component
    /// </summary>
    public class SearchBoxResultModel
    {
        /// <summary>
        /// property for accessing List of search result items
        /// </summary>
        public ICollection<Item> Items { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        public SearchBoxResultModel()
        {
        }
    }
}
