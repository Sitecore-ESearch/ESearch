using ESearch.Foundation.Indexing.Models;
using System;

namespace ESearch.Foundation.Indexing.Services
{
    public class DefaultSearchService : ISearchService
    {
        public SearchResults SearchItems(SearchQuery query)
        {
            throw new NotImplementedException();
        }

        public int GetTotalCount(SearchQuery query)
        {
            throw new NotImplementedException();
        }

        public SuggestionResults GetSuggestions(SearchQuery query)
        {
            throw new NotImplementedException();
        }

        public FacetResults GetFacets(SearchQuery query, params string[] targetFields)
        {
            throw new NotImplementedException();
        }
    }
}
