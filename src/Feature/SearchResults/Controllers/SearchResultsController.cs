using ESearch.Foundation.Indexing.Services;
using Sitecore.DependencyInjection;
using System.Web.Mvc;

namespace ESearch.Feature.SearchResults.Controllers
{
    public class SearchResultsController : Controller
    {
        private readonly IQueryBuilder _queryBuilder;
        private readonly ISearchService _searchService;

        public SearchResultsController()
        {
            _queryBuilder = ServiceLocator.ServiceProvider.GetService(typeof(IQueryBuilder)) as IQueryBuilder;
            _searchService = ServiceLocator.ServiceProvider.GetService(typeof(ISearchService)) as ISearchService;
        }

        public ActionResult Index()
        {
            var query = _queryBuilder.BuildSearchQuery(Request.QueryString, null);
            var qs = _queryBuilder.BuildQueryString(query).ToString();
            var searchResults = _searchService.SearchItems(query);
            var facetResults = _searchService.GetFacets(query, "myfield");
            var suggestionResults = _searchService.GetSuggestions(query);
            var totalCount = _searchService.GetTotalCount(query);

            return new EmptyResult();
        }
    }
}
