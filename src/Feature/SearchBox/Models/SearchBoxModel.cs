using System.Collections.Generic;
using Sitecore.Data.Items;

namespace ESearch.Feature.SearchBox.Models
{
    /// <summary>
    /// data of the search box component
    /// </summary>
    public class SearchBoxModel
    {
        /// <summary>
        /// property for accessing search keywords
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// property for accessing Item ID of the search setting item set in the rendering parameter
        /// </summary>
        public string SearchSettingsItemId { get; set; }

        /// <summary>
        /// property for accessing List of search result items
        /// </summary>
        public ICollection<Item> Items { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        public SearchBoxModel()
        {
        }        
    }
}
