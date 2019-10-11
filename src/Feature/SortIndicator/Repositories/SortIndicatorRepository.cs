using Sitecore;
using Sitecore.Mvc.Presentation;
using Sitecore.DependencyInjection;
using ESearch.Feature.SortIndicator.Models;
using ESearch.Foundation.Indexing.Services;
using ESearch.Foundation.SitecoreExtensions.Extensions;

namespace ESearch.Feature.SortIndicator.Repositories
{
    public interface ISortIndicatorRepository
    {
        SortIndicatorModel GetModel();
    }

    public class SortIndicatorRepository : ISortIndicatorRepository
    {
        protected IQueryBuilder QueryBuilder { get; }

        public SortIndicatorRepository()
        {
            QueryBuilder = ServiceLocator.ServiceProvider.GetService(typeof(IQueryBuilder)) as IQueryBuilder;
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
