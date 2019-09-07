using ESearch.Foundation.Indexing.Models;
using Sitecore.Data.Items;
using System;

namespace ESearch.Foundation.Indexing.Services
{
    public class DefaultSearchQueryBuilder : ISearchQueryBuilder
    {
        public string BuildQueryString(SearchQuery query)
        {
            throw new NotImplementedException();
        }

        public SearchQuery BuildSearchQuery(string queryString, Item searchSettings)
        {
            throw new NotImplementedException();
        }
    }
}
