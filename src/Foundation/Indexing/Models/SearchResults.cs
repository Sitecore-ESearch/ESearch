using Sitecore.Data.Items;
using System.Collections.Generic;

namespace ESearch.Foundation.Indexing.Models
{
    public class SearchResults
    {
        public int TotalCount { get; set; }
        public ICollection<Item> Items { get; set; }
    }
}
