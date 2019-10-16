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
        SearchBoxModel GetModel();
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

        public SearchBoxModel GetModel()
        {
            var searchSettings = RenderingContext.Current.Rendering.GetItemParameter("Search Settings");
            var queryString = Context.HttpContext.Request.QueryString;
            var searchResults = GetSuggestionResults(queryString["keyword"], searchSettings);
            return new SearchBoxModel()
            {
                Keyword = queryString["keyword"],
                SearchSettingsItemId = searchSettings.ID.ToString(),
                Items = searchResults?.Suggestions?.Select(suggestion => Context.Database.GetItem(suggestion.ItemId)).ToList() ?? new List<Item>()
            };
        }

        public SearchBoxResultModel GetResultModel(SearchBoxModel model)
        {
            var searchSettings = Context.Database.GetItem(ID.Parse(model.SearchSettingsItemId));
            var searchResults = GetSuggestionResults(model.Keyword, searchSettings);
            return new SearchBoxResultModel()
            {
                Items = searchResults?.Suggestions?.Select(suggestion => Context.Database.GetItem(suggestion.ItemId)).ToList() ?? new List<Item>()
            };
        }

        private SuggestionResults GetSuggestionResults(string keyword, Item searchSettings)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return null;
            }
            else
            {
                /**
                 * If it is delimited by single-byte spaces or double-byte spaces, an AND search is performed with the delimited words,
                 * so it is converted to a character string concatenated with “+” so that it can be used in the search interface.
                 */
                var words = keyword.Split(new string[] { " ", "　" }, System.StringSplitOptions.RemoveEmptyEntries);
                var queryString = HttpUtility.ParseQueryString(string.Empty);
                queryString["keyword"] = string.Join("+", words);
                var searchQuery = QueryBuilder.BuildSearchQuery(queryString, searchSettings);
                return SearchService.GetSuggestions(searchQuery);
            }
        }
    }
}
