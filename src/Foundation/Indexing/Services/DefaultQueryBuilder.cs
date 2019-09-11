using ESearch.Foundation.Indexing.Models;
using ESearch.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ESearch.Foundation.Indexing.Services
{
    public class DefaultQueryBuilder : IQueryBuilder
    {
        protected virtual string DateTimeFormat { get; } = "yyyyMMdd";

        public NameValueCollection BuildQueryString(SearchQuery query, Item searchSettings)
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            var pageSize = searchSettings.GetInteger(Templates.SearchSettings.Fields.PageSize);
            if ((query.Offset % pageSize == 0) && query.Limit == pageSize)
            {
                queryString["page"] = (query.Offset / pageSize + 1).ToString();
            }

            queryString["keyword"] = string.Join("+", query.KeywordCondition.Keywords);
            queryString["sort"] = string.Join("+", query.SortConditions.Select(cond => $"{cond.TargetField}:{cond.Direction.ToString().ToLowerInvariant()}"));

            foreach (var containsCondition in query.ContainsConditions ?? Enumerable.Empty<ContainsCondition>())
            {
                queryString[containsCondition.TargetField] = $"({string.Join("+", containsCondition.Values)})";
            }

            foreach (var betweensCondition in query.BetweenConditions ?? Enumerable.Empty<BetweenCondition>())
            {
                var lowerValue = DateTime.TryParse(betweensCondition.LowerValue, out var lowerDate)
                    ? lowerDate.ToString(DateTimeFormat)
                    : betweensCondition.LowerValue;
                var upperValue = DateTime.TryParse(betweensCondition.UpperValue, out var upperDate)
                    ? upperDate.ToString(DateTimeFormat)
                    : betweensCondition.UpperValue;

                queryString[betweensCondition.TargetField] = $"{lowerValue}|{upperValue}";
            }

            foreach (var equalsCondition in query.EqualsConditions ?? Enumerable.Empty<EqualsCondition>())
            {
                queryString[equalsCondition.TargetField] = equalsCondition.Value;
            }

            return queryString;
        }

        public SearchQuery BuildSearchQuery(NameValueCollection queryString, Item searchSettings)
        {
            var query = new SearchQuery()
            {
                Scope = ((ReferenceField)searchSettings.Fields[Templates.SearchSettings.Fields.Scope]).TargetID,
                TargetTemplates = ((MultilistField)searchSettings.Fields[Templates.SearchSettings.Fields.TargetTemplates]).TargetIDs,
                KeywordCondition = new KeywordCondition(),
                SortConditions = new List<SortCondition>(),
                ContainsConditions = new List<ContainsCondition>(),
                BetweenConditions = new List<BetweenCondition>(),
                EqualsConditions = new List<EqualsCondition>(),
            };

            foreach (var key in queryString.AllKeys)
            {
                var value = queryString[key];
                if (string.IsNullOrEmpty(value))
                {
                    continue;
                }

                if (key.Equals("keyword", StringComparison.InvariantCultureIgnoreCase))
                {
                    ApplyKeywordCondition(query, value, searchSettings);
                    continue;
                }

                if (key.Equals("sort", StringComparison.InvariantCultureIgnoreCase))
                {
                    ApplySortConditions(query, value, searchSettings);
                    continue;
                }

                if (key.Equals("page", StringComparison.InvariantCultureIgnoreCase))
                {
                    ApplyPagenationQuery(query, value, searchSettings);
                    continue;
                }

                if (value.StartsWith("(") && value.EndsWith(")"))
                {
                    ApplyContainsCondition(query, key, value, searchSettings);
                    continue;
                }

                if (value.Contains("|"))
                {
                    ApplyContainsCondition(query, key, value, searchSettings);
                    continue;
                }

                ApplyEqualsCondition(query, key, value, searchSettings);
            }

            return query;
        }

        protected virtual void ApplyKeywordCondition(SearchQuery query, string value, Item searchSettings)
        {
            query.KeywordCondition.TargetFields = searchSettings[Templates.SearchSettings.Fields.TargetTemplates].Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            query.KeywordCondition.Keywords = value.Split(new[] { '+' }, StringSplitOptions.RemoveEmptyEntries);
        }

        protected virtual void ApplySortConditions(SearchQuery query, string value, Item searchSettings)
        {
            foreach (var sortInfo in value.Split(new[] { '+' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var fieldAndDirection = sortInfo.Split(':');
                var sortCondition = new SortCondition
                {
                    TargetField = fieldAndDirection[0],
                };

                if (fieldAndDirection.Length < 2)
                {
                    sortCondition.Direction = SortDirection.Asc;
                }
                else if (fieldAndDirection[1].Equals("asc", StringComparison.InvariantCultureIgnoreCase))
                {
                    sortCondition.Direction = SortDirection.Asc;
                }
                else if (fieldAndDirection[1].Equals("desc", StringComparison.InvariantCultureIgnoreCase))
                {
                    sortCondition.Direction = SortDirection.Desc;
                }
                else
                {
                    sortCondition.Direction = SortDirection.Asc;
                }

                query.SortConditions.Add(sortCondition);
            }
        }

        protected virtual void ApplyPagenationQuery(SearchQuery query, string value, Item searchSettings)
        {
            if (!int.TryParse(value, out var page))
            {
                return;
            }

            var pageSize = searchSettings.GetInteger(Templates.SearchSettings.Fields.PageSize) ?? -1;
            if (pageSize <= 0)
            {
                return;
            }

            query.Offset = (page - 1) * pageSize;
            query.Limit = pageSize;

            return;
        }

        protected virtual void ApplyContainsCondition(SearchQuery query, string key, string value, Item searchSettings)
        {
            var containsCondition = new ContainsCondition
            {
                TargetField = key,
                Values = value.TrimStart('(').TrimEnd(')').Split(new[] { '+' }, StringSplitOptions.RemoveEmptyEntries),
            };

            query.ContainsConditions.Add(containsCondition);
        }

        protected virtual void ApplyBetweenCondition(SearchQuery query, string key, string value, Item searchSettings)
        {
            var lowerAndUpper = value.Split('|');
            var lower = lowerAndUpper[0];
            var upper = lowerAndUpper[1];
            var betweenCondition = new BetweenCondition
            {
                TargetField = key,
                LowerValue = Normalize(lower),
                UpperValue = Normalize(upper),
            };

            query.BetweenConditions.Add(betweenCondition);

            string Normalize(string datetimeValue)
            {
                // make datetime value parsable by DateTime.Parse
                return DateTime.TryParseExact(datetimeValue, DateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date)
                    ? date.ToString()
                    : datetimeValue;
            }
        }

        protected virtual void ApplyEqualsCondition(SearchQuery query, string key, string value, Item searchSettings)
        {
            var equalsCondition = new EqualsCondition
            {
                TargetField = key,
                Value = value,
            };

            query.EqualsConditions.Add(equalsCondition);
        }
    }
}
