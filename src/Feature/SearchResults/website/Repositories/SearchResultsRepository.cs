using ESearch.Feature.SearchResults.Models;
using ESearch.Foundation.Indexing.Services;
using ESearch.Foundation.SitecoreExtensions.Extensions;
using Sitecore;
using Sitecore.DependencyInjection;
using Sitecore.Mvc.Presentation;

namespace ESearch.Feature.SearchResults.Repositories
{
    public interface ISearchResultsRepository
    {
        SearchResultsModel GetModel();
    }

    public class SearchResultsRepository : ISearchResultsRepository
    {
        protected IQueryBuilder QueryBuilder { get; }
        protected ISearchService SearchService { get; }

        public SearchResultsRepository()
        {
            QueryBuilder = ServiceLocator.ServiceProvider.GetService(typeof(IQueryBuilder)) as IQueryBuilder;
            SearchService = ServiceLocator.ServiceProvider.GetService(typeof(ISearchService)) as ISearchService;
        }

        public SearchResultsModel GetModel()
        {
            var searchSettings = RenderingContext.Current.Rendering.GetItemParameter("Search Settings");
            var searchQuery = QueryBuilder.BuildSearchQuery(Context.HttpContext.Request.QueryString, searchSettings);
            var searchResults = SearchService.SearchItems(searchQuery);

            return new SearchResultsModel
            {
                Items = searchResults.Items,
            };
        }
    }
}
