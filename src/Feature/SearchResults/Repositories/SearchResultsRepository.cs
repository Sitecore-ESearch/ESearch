using ESearch.Feature.SearchResults.Models;
using ESearch.Foundation.Indexing.Services;
using ESearch.Foundation.SitecoreExtensions.Extensions;
using Sitecore;
using Sitecore.DependencyInjection;
using Sitecore.Mvc.Presentation;
using System;

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

            // クエリ文字列 => SearchQuery
            var searchQuery = QueryBuilder.BuildSearchQuery(Context.HttpContext.Request.QueryString, searchSettings);

            // SearchQuery => クエリ文字列
            var queryString = QueryBuilder.BuildQueryString(searchQuery, searchSettings);

            // アイテム
            var searchResults = SearchService.SearchItems(searchQuery);

            // 総ヒット数（SearchItemsより高速）
            var totalCount = SearchService.GetTotalCount(searchQuery);

            // サジェスト
            var suggestionResults = SearchService.GetSuggestions(searchQuery);

            // ファセット
            var facetResults = SearchService.GetFacets(searchQuery, "field1", "field2");

            throw new NotImplementedException();
        }
    }
}
