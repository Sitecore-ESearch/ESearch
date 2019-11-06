using ESearch.Foundation.Indexing.Models;
using Sitecore.Data.Items;
using System;
using System.Collections.Specialized;

namespace ESearch.Foundation.Indexing.Services
{
    public interface IQueryBuilder
    {
        SearchQuery BuildSearchQuery(NameValueCollection queryString, ISearchSettings settings);
        NameValueCollection BuildQueryString(SearchQuery query, ISearchSettings settings);

        #region Obsoleted methods
        [Obsolete("This method will be removed in v1.0")]
        SearchQuery BuildSearchQuery(NameValueCollection queryString, Item searchSettings);
        [Obsolete("This method will be removed in v1.0")]
        NameValueCollection BuildQueryString(SearchQuery query, Item searchSettings);
        #endregion
    }
}
