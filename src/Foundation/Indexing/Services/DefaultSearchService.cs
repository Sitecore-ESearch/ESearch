using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ESearch.Foundation.Indexing.Models;

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
    }
}
