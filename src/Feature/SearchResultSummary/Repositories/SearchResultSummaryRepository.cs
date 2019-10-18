using ESearch.Feature.SearchResultSummary.Models;
using ESearch.Foundation.Indexing.Services;
using ESearch.Foundation.SitecoreExtensions.Extensions;
using Sitecore;
using Sitecore.DependencyInjection;
using Sitecore.Mvc.Presentation;

namespace ESearch.Feature.SearchResultSummary.Repositories
{
    public interface ISearchResultSummaryRepository
    {
        SearchResultSummaryModel GetModel();
    }
    public class SearchResultSummaryRepository : ISearchResultSummaryRepository
    {
        protected IQueryBuilder QueryBuilder { get; }
        protected ISearchService SearchService { get; }

        public SearchResultSummaryRepository()
        {
            QueryBuilder = ServiceLocator.ServiceProvider.GetService(typeof(IQueryBuilder)) as IQueryBuilder;
            SearchService = ServiceLocator.ServiceProvider.GetService(typeof(ISearchService)) as ISearchService;
        }

        public SearchResultSummaryModel GetModel()
        {
            var searchSettings = RenderingContext.Current.Rendering.GetItemParameter("Search Settings");

            var searchQuery = QueryBuilder.BuildSearchQuery(Context.HttpContext.Request.QueryString, searchSettings);
            var totalCount = SearchService.GetTotalCount(searchQuery);

            return new SearchResultSummaryModel(searchQuery, searchSettings, totalCount);
        }
    }
}
