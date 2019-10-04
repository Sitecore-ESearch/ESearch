using System.Collections.Generic;
using Sitecore.Data.Items;
using ESearch.Foundation.Indexing.Models;
using System.Linq;

namespace ESearch.Feature.SearchBox.Models
{
    public class SearchBoxResultModel
    {
        public ICollection<Item> Items { get; set; }

        public SearchBoxResultModel(ICollection<Suggestion> suggestions)
        {
            Items = suggestions.Select(suggestion => Sitecore.Context.Database.GetItem(suggestion.ItemId)).ToList();
        }
    }
}
