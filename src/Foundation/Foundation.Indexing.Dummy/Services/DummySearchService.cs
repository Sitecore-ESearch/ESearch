using ESearch.Foundation.Indexing.Models;
using ESearch.Foundation.Indexing.Services;
using Sitecore;
using Sitecore.Data.Items;
using System.Linq;

namespace ESearch.Foundation.Indexing.Dummy.Services
{
    public class DummySearchService : ISearchService
    {
        public SearchResults GetItems(SearchQuery query)
        {
            var dummyItems = Context.Database.GetItem("/sitecore/content/dummy")?.Axes.GetDescendants() ?? Enumerable.Empty<Item>();

            return new SearchResults
            {
                Items = dummyItems.ToList(),
                TotalCount = 238,
            };
        }

        public int GetTotalCount(SearchQuery query)
        {
            return 238;
        }
    }
}
