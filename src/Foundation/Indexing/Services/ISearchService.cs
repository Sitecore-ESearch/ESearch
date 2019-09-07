using ESearch.Foundation.Indexing.Models;

namespace ESearch.Foundation.Indexing.Services
{
    public interface ISearchService
    {
        SearchResults SearchItems(SearchQuery query);
        SuggestionResults GetSuggestions(SuggestionQuery query);
        FacetResults GetFacets(FacetQuery query);
        int GetTotalCount(SearchQuery query);
    }
}
