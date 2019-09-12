using ESearch.Foundation.Indexing.Models;
using Sitecore;
using Sitecore.ContentSearch.Linq;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;
using Sitecore.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using FacetResults = ESearch.Foundation.Indexing.Models.FacetResults;

namespace ESearch.Foundation.Indexing.Services
{
    public class DefaultSearchService<TResult> : ISearchService where TResult : SearchResultItem
    {
        protected IIndexResolver IndexResolver { get; }

        public DefaultSearchService()
        {
            IndexResolver = ServiceLocator.ServiceProvider.GetService(typeof(IIndexResolver)) as IIndexResolver;
        }

        public SearchResults SearchItems(SearchQuery query)
        {
            var index = IndexResolver.Resolve();
            using (var context = index.CreateSearchContext())
            {
                var queryable = context.GetQueryable<TResult>();

                queryable = ApplyFilterConditions(queryable, query);
                queryable = ApplySortConditions(queryable, query.SortConditions);
                queryable = ApplyPagination(queryable, query.Limit, query.Offset);

                var results = queryable.GetResults();
                return new SearchResults
                {
                    Items = results.Hits.Select(hit => hit.Document.GetItem()).ToList(),
                    TotalCount = results.TotalSearchResults,
                };
            }
        }

        public int GetTotalCount(SearchQuery query)
        {
            var index = IndexResolver.Resolve();
            using (var context = index.CreateSearchContext())
            {
                var queryable = context.GetQueryable<TResult>();

                queryable = ApplyFilterConditions(queryable, query);

                return queryable.Count();
            }
        }

        public SuggestionResults GetSuggestions(SearchQuery query)
        {
            var index = IndexResolver.Resolve();
            using (var context = index.CreateSearchContext())
            {
                var queryable = context.GetQueryable<TResult>();

                queryable = ApplyFilterConditions(queryable, query);
                queryable = ApplyPagination(queryable, query.Limit, query.Offset);

                return new SuggestionResults
                {
                    Suggestions = queryable.GetResults().Hits
                    .Select(hit => ExtractSuggestionFields(hit.Document.Fields, query.KeywordCondition.TargetFields))
                    .Select(fields => new Suggestion { SuggestedFields = fields })
                    .ToList()
                };
            }
        }

        public FacetResults GetFacets(SearchQuery query, params string[] targetFields)
        {
            var index = IndexResolver.Resolve();
            using (var context = index.CreateSearchContext())
            {
                var queryable = context.GetQueryable<TResult>();

                queryable = ApplyFilterConditions(queryable, query);
                queryable = ApplyFacets(queryable, targetFields);

                return new FacetResults()
                {
                    Facets = queryable.GetFacets().Categories
                    .Select(category => new Facet()
                    {
                        FieldName = category.Name,
                        FacetValues = category.Values.Select(value => new Models.FacetValue(value.Name, value.AggregateCount)).ToList()
                    })
                    .ToList()
                };
            }
        }

        protected IQueryable<TResult> ApplyFilterConditions(IQueryable<TResult> queryable, SearchQuery query)
        {
            var scope = ID.IsNullOrEmpty(query.Scope) ? ItemIDs.ContentRoot : query.Scope;
            var templatePred = query.TargetTemplates.Aggregate(
                PredicateBuilder.False<TResult>(),
                (pred, templateId) => pred.Or(item => item.TemplateId == templateId));

            queryable = queryable.Filter(item => item.Paths.Contains(scope)).Filter(templatePred);
            queryable = ApplyKeywordsCondition(queryable, query.KeywordCondition);
            queryable = ApplyContainsConditions(queryable, query.ContainsConditions);
            queryable = ApplyBetweenConditions(queryable, query.BetweenConditions);
            queryable = ApplyEqualsConditions(queryable, query.EqualsConditions);

            return queryable;
        }

        protected virtual IQueryable<TResult> ApplyKeywordsCondition(IQueryable<TResult> queryable, KeywordCondition condition)
        {
            if (condition == null ||
                condition.TargetFields == null ||
                condition.Keywords == null ||
                !condition.TargetFields.Any() ||
                !condition.Keywords.Any())
            {
                return queryable;
            }

            var keywordsPred = PredicateBuilder.False<TResult>();

            foreach (var targetField in condition.TargetFields)
            {
                var fieldContainsKeywords = condition.Keywords.Aggregate(
                    PredicateBuilder.True<TResult>(),
                    (pred, keyword) => pred.And(item => item[targetField].Contains(keyword)));

                keywordsPred = keywordsPred.Or(fieldContainsKeywords);
            }

            return queryable.Where(keywordsPred);
        }

        protected virtual IQueryable<TResult> ApplyContainsConditions(IQueryable<TResult> queryable, ICollection<ContainsCondition> conditions)
        {
            if (conditions == null || !conditions.Any())
            {
                return queryable;
            }

            foreach (var condition in conditions)
            {
                if (condition.Values == null || !condition.Values.Any())
                {
                    continue;
                }

                var containsPred = condition.Values.Aggregate(
                    PredicateBuilder.True<TResult>(),
                    (pred, value) => pred.And(item => item[condition.TargetField].Contains(value)));

                queryable = queryable.Where(containsPred);
            }

            return queryable;
        }

        protected virtual IQueryable<TResult> ApplyBetweenConditions(IQueryable<TResult> queryable, ICollection<BetweenCondition> conditions)
        {
            if (conditions == null || !conditions.Any())
            {
                return queryable;
            }

            foreach (var condition in conditions)
            {
                var lower = EnsureDateFormat(condition.LowerValue);
                var upper = EnsureDateFormat(condition.UpperValue);
                queryable = queryable.Where(item => item[condition.TargetField].Between(lower, upper, Inclusion.Both));
            }

            return queryable;

            string EnsureDateFormat(string dateString)
            {
                var dateFormat = IndexResolver.Resolve().Configuration.DefaultDateFormat;
                return DateTime.TryParse(dateString, out var date) ? date.ToString(dateFormat) : dateString;
            }
        }

        protected virtual IQueryable<TResult> ApplyEqualsConditions(IQueryable<TResult> queryable, ICollection<EqualsCondition> conditions)
        {
            if (conditions == null || !conditions.Any())
            {
                return queryable;
            }

            foreach (var condition in conditions)
            {
                queryable = queryable.Where(item => item[condition.TargetField] == condition.Value);
            }

            return queryable;
        }

        protected virtual IQueryable<TResult> ApplySortConditions(IQueryable<TResult> queryable, ICollection<SortCondition> conditions)
        {
            if (conditions == null || !conditions.Any())
            {
                return queryable;
            }

            foreach (var condition in conditions)
            {
                switch (condition.Direction)
                {
                    case SortDirection.Asc:
                        queryable = condition == conditions.First()
                            ? queryable.OrderBy(item => item[condition.TargetField])
                            : ((IOrderedQueryable<TResult>)queryable).ThenBy(item => item[condition.TargetField]);
                        break;
                    case SortDirection.Desc:
                        queryable = condition == conditions.First()
                            ? queryable.OrderByDescending(item => item[condition.TargetField])
                            : ((IOrderedQueryable<TResult>)queryable).ThenByDescending(item => item[condition.TargetField]);
                        break;
                }
            }

            return queryable;
        }

        protected virtual IQueryable<TResult> ApplyPagination(IQueryable<TResult> queryable, int limit, int offset)
        {
            if (limit <= 0 || offset < 0)
            {
                return queryable;
            }

            return queryable.Skip(offset).Take(limit);
        }

        protected virtual IQueryable<TResult> ApplyFacets(IQueryable<TResult> queryable, ICollection<string> fieldNames)
        {
            return fieldNames.Aggregate(queryable, (acc, fieldName) => acc.FacetOn(item => item[fieldName]));
        }

        protected IDictionary<string, string> ExtractSuggestionFields(IDictionary<string, object> fields, ICollection<string> targetFields)
        {
            var translator = IndexResolver.Resolve().FieldNameTranslator;

            return targetFields
                .OrderByDescending(field => field.Length)
                .ToDictionary(fieldName => fieldName, GetFieldValue);

            string GetFieldValue(string fieldName)
            {
                var nameWithoutType = translator.GetIndexFieldName(fieldName);
                var nameWithType = translator.GetIndexFieldName(fieldName, typeof(string));
                var fieldKey = fields.Keys.FirstOrDefault(key => key == nameWithoutType || key == nameWithType);
                return string.IsNullOrEmpty(fieldKey) ? string.Empty : fields[fieldKey].ToString();
            }
        }
    }
}
