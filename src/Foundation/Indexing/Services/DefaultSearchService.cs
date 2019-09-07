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

        public SuggestionResults GetSuggestions(SuggestionQuery query)
        {
            throw new NotImplementedException();
        }

        public FacetResults GetFacets(FacetQuery query)
        {
            throw new NotImplementedException();
        }
    }
}
