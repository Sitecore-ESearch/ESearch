using ESearch.Feature.FacetFilter.Models;
using ESearch.Foundation.Indexing.Services;
using ESearch.Foundation.SitecoreExtensions.Extensions;
using Sitecore;
using Sitecore.Data;
using Sitecore.DependencyInjection;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
            var targetField = RenderingContext.Current.Rendering.Parameters["Target Field"];
            if (string.IsNullOrEmpty(targetField))
            {
                return new FacetFilterModel
                {
                    ClearLink = "#",
                    FilterRows = new List<FacetFilterRow>(),
                };
            }

            var rowCount = RenderingContext.Current.Rendering.GetIntegerParameter("Row Count") ?? int.MaxValue;
            var facetResults = SearchService.GetFacets(searchQuery, targetField);
            var facet = facetResults.Facets.FirstOrDefault();
            var filterRows = facet?.FacetValues
                .Where(value => value.Count > 0)
                .OrderByDescending(value => value.Count)
                .Select(value => new FacetFilterRow
                {
                    Label = GetFilterLabel(value.FieldValue),
                    Link = GetFilterLink(facet.FieldName, value.FieldValue),
                    Count = value.Count,
                    IsActive = searchQuery.EqualsConditions.Any(cond => cond.TargetField == facet.FieldName && cond.Value == value.FieldValue)
                })
                .Take(rowCount);

            return new FacetFilterModel
            {
                ClearLink = GetClearLink(facet?.FieldName),
                FilterRows = filterRows?.ToList() ?? new List<FacetFilterRow>(),
            };
        }

        protected virtual string GetFilterLink(string fieldName, string fieldValue)
        {
            var absolutePath = Context.HttpContext.Request.Url.AbsolutePath;
            var query = HttpUtility.ParseQueryString(Context.HttpContext.Request.Url.Query);
            var values = query[fieldName]?.Split(new[] { '+' }, StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();

            query.Remove("page");
            if (!values.Contains(fieldValue))
            {
                query[fieldName] = string.Join("+", values.Append(fieldValue));
            }
            else
            {
                query[fieldName] = string.Join("+", values.Where(val => val != fieldValue));
            }

            return $"{absolutePath}?{query}";
        }

        protected virtual string GetClearLink(string fieldName)
        {
            var absolutePath = Context.HttpContext.Request.Url.AbsolutePath;
            var query = HttpUtility.ParseQueryString(Context.HttpContext.Request.Url.Query);

            query.Remove("page");
            if (!string.IsNullOrEmpty(fieldName))
            {
                query.Remove(fieldName);
            }

            return $"{absolutePath}?{query}";
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
