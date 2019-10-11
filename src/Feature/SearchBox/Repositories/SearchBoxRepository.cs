using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.DependencyInjection;
using Sitecore.Mvc.Presentation;
using System.Web;
using ESearch.Feature.SearchBox.Models;
using ESearch.Foundation.Indexing.Services;
using ESearch.Foundation.SitecoreExtensions.Extensions;
using System.Linq;
using ESearch.Foundation.Indexing.Models;
using System.Collections.Generic;

namespace ESearch.Feature.SearchBox.Repositories
{
    public interface ISearchBoxRepository
    {
        SearchBoxModel GetModel(string keyword);
        SearchBoxResultModel GetResultModel(SearchBoxModel model);
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

        public SearchBoxModel GetModel(string keyword)
        {
            var searchSettings = RenderingContext.Current.Rendering.GetItemParameter("Search Settings");
            var searchResults = new SuggestionResults();
            if (!string.IsNullOrEmpty(keyword))
            {
                var queryString = HttpUtility.ParseQueryString(string.Empty);
                queryString["keyword"] = keyword;
                var searchQuery = QueryBuilder.BuildSearchQuery(queryString, searchSettings);
                searchResults = SearchService.GetSuggestions(searchQuery);
            }

            return new SearchBoxModel()
            {
                Keyword = keyword,
                SearchSettingsItemId = searchSettings.ID.ToString(),
                Items = searchResults?.Suggestions?.Select(suggestion => Context.Database.GetItem(suggestion.ItemId)).ToList() ?? new List<Item>()
            };
        }

        public SearchBoxResultModel GetResultModel(SearchBoxModel model)
        {
            var searchSettings = Context.Database.GetItem(ID.Parse(model.SearchSettingsItemId));
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["keyword"] = model.Keyword;

            var searchQuery = QueryBuilder.BuildSearchQuery(queryString, searchSettings);
            var searchResults = SearchService.GetSuggestions(searchQuery);
            return new SearchBoxResultModel()
            {
                Items = searchResults.Suggestions?.Select(suggestion => Context.Database.GetItem(suggestion.ItemId)).ToList() ?? new List<Item>()
            };
        }
    }
}
