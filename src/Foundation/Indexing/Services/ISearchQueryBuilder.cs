using ESearch.Foundation.Indexing.Models;
using Sitecore.Data.Items;

namespace ESearch.Foundation.Indexing.Services
{
    public interface ISearchQueryBuilder
    {
        SearchQuery Build(string queryString, Item searchSettings);
    }
}
