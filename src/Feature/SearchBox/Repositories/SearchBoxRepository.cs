using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.DependencyInjection;
using Sitecore.Mvc.Presentation;
using System.Web;
using ESearch.Feature.SearchBox.Models;
using ESearch.Foundation.Indexing.Services;
using ESearch.Foundation.SitecoreExtensions.Extensions;

namespace ESearch.Feature.SearchBox.Repositories
{
    public interface ISearchBoxRepository
    {
        SearchBoxModel GetModel();
        SearchBoxResultModel GetResultModel(SearchBoxModel model);
        SearchBoxResultModel GetResultModel(string keyword);
    }

    public class SearchBoxRepository: ISearchBoxRepository
    {
        protected IQueryBuilder QueryBuilder { get; }
        protected ISearchService SearchService { get; }

        public SearchBoxRepository()
        {
            QueryBuilder = ServiceLocator.ServiceProvider.GetService(typeof(IQueryBuilder)) as IQueryBuilder;
            SearchService = ServiceLocator.ServiceProvider.GetService(typeof(ISearchService)) as ISearchService;
        }

        public SearchBoxModel GetModel()
        {
            return new SearchBoxModel()
            {
                SearchSettingsItemId = RenderingContext.Current.Rendering.GetItemParameter("Search Settings").ID.ToString()
            };
        }

        public SearchBoxResultModel GetResultModel(SearchBoxModel model)
        {
            var searchSettings = Context.Database.GetItem(ID.Parse(model.SearchSettingsItemId));
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["keyword"] = model.Keyword;

            var searchQuery = QueryBuilder.BuildSearchQuery(queryString, searchSettings);
            var searchResults = SearchService.GetSuggestions(searchQuery);
            return new SearchBoxResultModel(searchResults.Suggestions);
        }

        public SearchBoxResultModel GetResultModel(string keyword)
        {
            var searchSettings = RenderingContext.Current.Rendering.GetItemParameter("Search Settings");
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["keyword"] = keyword;

            var searchQuery = QueryBuilder.BuildSearchQuery(queryString, searchSettings);
            var searchResults = SearchService.GetSuggestions(searchQuery);
            return new SearchBoxResultModel(searchResults.Suggestions);
        }
    }
}
