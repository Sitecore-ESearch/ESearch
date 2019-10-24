using ESearch.Feature.SearchBox.Models;
using ESearch.Foundation.Indexing.Models;
using ESearch.Foundation.Indexing.Services;
using ESearch.Foundation.SitecoreExtensions.Extensions;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.DependencyInjection;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESearch.Feature.SearchBox.Repositories
{
    public interface ISearchBoxRepository
    {
        SearchBoxModel GetModel();
        SearchBoxResultModel GetResultModel(SearchBoxModel data);
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

        public SearchBoxResultModel GetResultModel(SearchBoxModel data)
        {
            var searchSettings = Context.Database.GetItem(ID.Parse(data.SearchSettingsItemId));
            var searchResults = GetSuggestionResults(data.Keyword, searchSettings);
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
