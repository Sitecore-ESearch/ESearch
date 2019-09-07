using ESearch.Foundation.Indexing.Models;
using Sitecore.Data.Items;
using System.Collections.Specialized;

namespace ESearch.Foundation.Indexing.Services
{
    public interface IQueryBuilder
    {
        SearchQuery BuildSearchQuery(NameValueCollection queryString, Item searchSettings);
        string BuildQueryString(SearchQuery query);
    }
}
