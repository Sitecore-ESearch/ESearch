using System.Collections.Generic;
using Sitecore.Data.Items;

namespace ESearch.Feature.SearchBox.Models
{
    public class SearchBoxResultModel
    {
        public ICollection<Item> Items { get; set; }

        public SearchBoxResultModel()
        {
        }
    }
}
