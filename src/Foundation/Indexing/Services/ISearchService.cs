using ESearch.Foundation.Indexing.Models;

namespace ESearch.Foundation.Indexing.Services
{
    public interface ISearchService
    {
        SearchResults GetItems(SearchQuery query);
        int GetTotalCount(SearchQuery query);
    }
}
