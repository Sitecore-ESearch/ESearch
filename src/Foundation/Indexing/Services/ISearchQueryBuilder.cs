using ESearch.Foundation.Indexing.Models;
using Sitecore.Data.Items;

namespace ESearch.Foundation.Indexing.Services
{
    public interface ISearchQueryBuilder
    {
        SearchQuery BuildSearchQuery(string queryString, Item searchSettings);
        string BuildQueryString(SearchQuery query);
    }
}
