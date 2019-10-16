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
    /// <summary>
    /// Provides methods for retrieving data for use in the search box component.
    /// </summary>
    public interface ISearchBoxRepository
    {
        /// <summary>
        /// Get the data of the search box component.
        /// </summary>
        /// <returns>Data of the search box component.</returns>
        SearchBoxModel GetModel();

        /// <summary>
        /// Get the result data searched by the search box component.
        /// </summary>
        /// <remarks>Called from ajax.</remarks>
        /// <param name="data">Data posted when performing a search in the search box component.</param>
        /// <returns>The search result.</returns>
        SearchBoxResultModel GetResultModel(SearchBoxModel data);
    }

    public class SearchBoxRepository: ISearchBoxRepository
    {
        /// <summary>
        ///  Gets instance of the QueryBuilder.
        /// </summary>
        protected IQueryBuilder QueryBuilder { get; }

        /// <summary>
        /// Gets instance of the SearchService.
        /// </summary>
        protected ISearchService SearchService { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchBoxRepository" /> class.
        /// </summary>
        public SearchBoxRepository()
        {
            QueryBuilder = ServiceLocator.ServiceProvider.GetService(typeof(IQueryBuilder)) as IQueryBuilder;
            SearchService = ServiceLocator.ServiceProvider.GetService(typeof(ISearchService)) as ISearchService;
        }

        /// <summary>
        /// ISearchBoxRepository.GetModel Implementation.
        /// </summary>
        /// <remarks>If “keyword” is included in the query of the request, the search is executed according to the setting of “Search Settings item” set in the rendering parameters.</remarks>
        /// <returns>Data of the search box component.</returns>
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

        /// <summary>
        /// ISearchBoxRepository.GetResultModel Implementation.
        /// </summary>
        /// <remarks>Called from ajax. Use the posted data to get the data for the search result view of the search box component.</remarks>
        /// <param name="data">Data posted when performing a search in the search box component.</param>
        /// <returns>The result of a search.</returns>
        public SearchBoxResultModel GetResultModel(SearchBoxModel data)
        {
            var searchSettings = Context.Database.GetItem(ID.Parse(data.SearchSettingsItemId));
            var searchResults = GetSuggestionResults(data.Keyword, searchSettings);
            return new SearchBoxResultModel()
            {
                Items = searchResults?.Suggestions?.Select(suggestion => Context.Database.GetItem(suggestion.ItemId)).ToList() ?? new List<Item>()
            };
        }

        /// <summary>
        /// Gets suggest search results.
        /// </summary>
        /// <param name="keyword">The search keyword.</param>
        /// <param name="searchSettings">The search setting item specified by rendering parameter.</param>
        /// <returns>The result of suggest search.</returns>
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
