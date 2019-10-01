using ESearch.Feature.PageSelector.Models;
using ESearch.Foundation.Indexing.Services;
using ESearch.Foundation.SitecoreExtensions.Extensions;
using Sitecore;
using Sitecore.DependencyInjection;
using Sitecore.Mvc.Presentation;
using IndexingTemplates = ESearch.Foundation.Indexing.Templates;

namespace ESearch.Feature.PageSelector.Repositories
{
    public interface IPageSelectorRepository
    {
        PageSelectorModel GetModel();
    }

    public class PageSelectorRepository : IPageSelectorRepository
    {
        protected IQueryBuilder QueryBuilder { get; }
        protected ISearchService SearchService { get; }
        
        public PageSelectorRepository()
        {
            QueryBuilder = ServiceLocator.ServiceProvider.GetService(typeof(IQueryBuilder)) as IQueryBuilder;
            SearchService = ServiceLocator.ServiceProvider.GetService(typeof(ISearchService)) as ISearchService;
        }

        public PageSelectorModel GetModel()
        {
            var queryString = Context.HttpContext.Request.QueryString;
            var searchSettings = RenderingContext.Current.Rendering.GetItemParameter("Search Settings");
            var searchQuery = QueryBuilder.BuildSearchQuery(queryString, searchSettings);

            var url = Context.HttpContext.Request.Url.OriginalString;
            var pageSize = searchSettings.GetInteger(IndexingTemplates.SearchSettings.Fields.PageSize) ?? 20;
            var totalCount = SearchService.GetTotalCount(searchQuery);
            var selectorSize = RenderingContext.Current.Rendering.GetIntegerParameter("Selector Size") ?? 2;
            return new PageSelectorModel(url, pageSize, totalCount, selectorSize);
        }
    }
}
