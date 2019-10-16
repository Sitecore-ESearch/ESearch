using System.Collections.Generic;
using Sitecore.Data.Items;

namespace ESearch.Feature.SearchBox.Models
{
    public class SearchBoxModel
    {
        /// <summary>
        /// Gets or sets  search keywords.
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// Gets or sets item ID of the search setting item set in the rendering parameter.
        /// </summary>
        public string SearchSettingsItemId { get; set; }

        /// <summary>
        /// Gets or sets  list of search result items.
        /// </summary>
        public ICollection<Item> Items { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchBoxModel" /> class.
        /// </summary>
        public SearchBoxModel()
        {
        }        
    }
}
