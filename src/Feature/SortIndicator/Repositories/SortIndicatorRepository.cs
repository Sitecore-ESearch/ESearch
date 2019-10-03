using ESearch.Feature.SortIndicator.Models;
using ESearch.Foundation.Indexing.Services;
using ESearch.Foundation.SitecoreExtensions.Extensions;
using Sitecore.DependencyInjection;
using Sitecore.Mvc.Presentation;
using Sitecore;

namespace ESearch.Feature.SortIndicator.Repositories
{
    public interface ISortIndicatorRepository
    {
        SortIndicatorModel GetModel();
    }

    public class SortIndicatorRepository : ISortIndicatorRepository
    {
        protected IQueryBuilder QueryBuilder { get; }
        protected ISearchService SearchService { get; }

        public SortIndicatorRepository()
        {
            QueryBuilder = ServiceLocator.ServiceProvider.GetService(typeof(IQueryBuilder)) as IQueryBuilder;
            SearchService = ServiceLocator.ServiceProvider.GetService(typeof(ISearchService)) as ISearchService;
        }

        public SortIndicatorModel GetModel()
        {
            var searchSettings = RenderingContext.Current.Rendering.GetItemParameter("Search Settings");
            var sortSetting = RenderingContext.Current.Rendering.Item;
            var searchQuery = QueryBuilder.BuildSearchQuery(Context.HttpContext.Request.QueryString, searchSettings);

            return new SortIndicatorModel(sortSetting, searchQuery);
        }
    }
}
