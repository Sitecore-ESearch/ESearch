using ESearch.Feature.FacetFilter.Models;
using ESearch.Foundation.Indexing.Services;
using ESearch.Foundation.SitecoreExtensions.Extensions;
using Sitecore;
using Sitecore.Data;
using Sitecore.DependencyInjection;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ESearch.Feature.FacetFilter.Repositories
{
    public interface IFacetFilterRepository
    {
        FacetFilterModel GetModel();
    }

    public class FacetFilterRepository : IFacetFilterRepository
    {
        protected IQueryBuilder QueryBuilder { get; }
        protected ISearchService SearchService { get; }

        public FacetFilterRepository()
        {
            QueryBuilder = ServiceLocator.ServiceProvider.GetService(typeof(IQueryBuilder)) as IQueryBuilder;
            SearchService = ServiceLocator.ServiceProvider.GetService(typeof(ISearchService)) as ISearchService;
        }

        public FacetFilterModel GetModel()
        {
            var searchSettings = RenderingContext.Current.Rendering.GetItemParameter("Search Settings");
            var searchQuery = QueryBuilder.BuildSearchQuery(Context.HttpContext.Request.QueryString, searchSettings);
            var facetKey = RenderingContext.Current.Rendering.Item[Templates.FacetFilter.Fields.FilterKey];
            var rowCount = RenderingContext.Current.Rendering.Item.GetInteger(Templates.FacetFilter.Fields.RowCount);

            var facetResults = SearchService.GetFacets(searchQuery, facetKey);
            var filterRows = facetResults.Facets
                .FirstOrDefault()?
                .FacetValues
                .OrderByDescending(value => value.Count)
                .Select(value => new FacetFilterRow
                {
                    Label = GetFilterLabel(value.FieldValue),
                    FilterKey = value.FieldValue,
                    Count = value.Count
                })
                .Take(rowCount ?? int.MaxValue);

            return new FacetFilterModel
            {
                FilterRows = filterRows?.ToList() ?? new List<FacetFilterRow>(),
            };
        }

        protected virtual string GetFilterLabel(string filterKey)
        {
            if (ID.TryParse(filterKey, out var id))
            {
                var item = Context.Database.GetItem(id);
                return item?.DisplayName ?? filterKey;
            }

            if (ShortID.TryParse(filterKey, out var shortId))
            {
                var item = Context.Database.GetItem(shortId.ToID());
                return item?.DisplayName ?? filterKey;
            }

            return filterKey;
        }
    }
}
