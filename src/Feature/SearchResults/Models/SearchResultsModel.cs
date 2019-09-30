using Sitecore.Data.Items;
using System.Collections.Generic;

namespace ESearch.Feature.SearchResults.Models
{
    public class SearchResultsModel
    {
        public ICollection<Item> Items { get; set; }
        public string CssClass { get; set; }
        public string ItemCssClass { get; set; }
    }
}
