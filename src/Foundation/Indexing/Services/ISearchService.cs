using ESearch.Foundation.Indexing.Models;

namespace ESearch.Foundation.Indexing.Services
{
    public interface ISearchService
    {
        SearchResults SearchItems(SearchQuery query);
        SuggestionResults GetSuggestions(SearchQuery query, params string[] targetFields);
        FacetResults GetFacets(SearchQuery query, params string[] targetFields);
        int GetTotalCount(SearchQuery query);
    }
}
