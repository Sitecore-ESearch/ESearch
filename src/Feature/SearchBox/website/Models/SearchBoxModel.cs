using Sitecore.Data.Items;
using System.Collections.Generic;

namespace ESearch.Feature.SearchBox.Models
{
    public class SearchBoxModel
    {
        public string Keyword { get; set; }
        public string SearchSettingsItemId { get; set; }
        public ICollection<Item> Items { get; set; }

        public SearchBoxModel()
        {
        }        
    }
}
