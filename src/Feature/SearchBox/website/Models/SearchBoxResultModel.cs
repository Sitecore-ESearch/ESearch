using Sitecore.Data.Items;
using System.Collections.Generic;

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
