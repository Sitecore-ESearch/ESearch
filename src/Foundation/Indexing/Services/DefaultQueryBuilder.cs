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

            var dateFormat = searchSettings[Templates.SearchSettings.Fields.DateFormat];
            foreach (var betweensCondition in query.BetweenConditions ?? Enumerable.Empty<BetweenCondition>())
            {
                var lowerValue = DateTime.TryParse(betweensCondition.LowerValue, out var lowerDate)
                    ? lowerDate.ToLocalTime().ToString(dateFormat)
                    : betweensCondition.LowerValue;
                var upperValue = DateTime.TryParse(betweensCondition.UpperValue, out var upperDate)
                    ? upperDate.ToLocalTime().ToString(dateFormat)
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
                    query.KeywordCondition = CreateKeywordCondition(value, searchSettings);
                    continue;
                }

                if (key.Equals("sort", StringComparison.InvariantCultureIgnoreCase))
                {
                    query.SortConditions = CreateSortConditions(value, searchSettings);
                    continue;
                }

                if (key.Equals("page", StringComparison.InvariantCultureIgnoreCase))
                {
                    var (offset, limit) = CreatePaginationInfo(value, searchSettings);
                    query.Offset = offset;
                    query.Limit = limit;
                    continue;
                }

                if (value.StartsWith("(") && value.EndsWith(")"))
                {
                    var containsCondition = CreateContainsCondition(key, value, searchSettings);
                    query.ContainsConditions.Add(containsCondition);
                    continue;
                }

                if (value.Contains("|"))
                {
                    var betweenCondition = CreateBetweenCondition(key, value, searchSettings);
                    query.BetweenConditions.Add(betweenCondition);
                    continue;
                }

                var equalsCondition = CreateEqualsCondition(key, value, searchSettings);
                query.EqualsConditions.Add(equalsCondition);
            }

            return query;
        }

        private KeywordCondition CreateKeywordCondition(string value, Item searchSettings)
        {
            var keywordCondition = new KeywordCondition
            {
                TargetFields = searchSettings[Templates.SearchSettings.Fields.KeywordSearchTargets].Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries),
                Keywords = value.Split(new[] { '+' }, StringSplitOptions.RemoveEmptyEntries),
            };

            return keywordCondition;
        }

        private ICollection<SortCondition> CreateSortConditions(string value, Item searchSettings)
        {
            var sortConditions = new List<SortCondition>();

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

                sortConditions.Add(sortCondition);
            }

            return sortConditions;
        }

        private (int offset, int limit) CreatePaginationInfo(string value, Item searchSettings)
        {
            if (!int.TryParse(value, out var page))
            {
                return (0, 0);
            }

            var pageSize = searchSettings.GetInteger(Templates.SearchSettings.Fields.PageSize) ?? -1;
            if (pageSize <= 0)
            {
                return (0, 0);
            }

            var offset = (page - 1) * pageSize;
            var limit = pageSize;

            return (offset, limit);
        }

        private ContainsCondition CreateContainsCondition(string key, string value, Item searchSettings)
        {
            var containsCondition = new ContainsCondition
            {
                TargetField = key,
                Values = value.TrimStart('(').TrimEnd(')').Split(new[] { '+' }, StringSplitOptions.RemoveEmptyEntries),
            };

            return containsCondition;
        }

        private BetweenCondition CreateBetweenCondition(string key, string value, Item searchSettings)
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

            return betweenCondition;

            string Normalize(string input)
            {
                // make datetime value parsable by DateTime.Parse
                var dateFormat = searchSettings[Templates.SearchSettings.Fields.DateFormat];
                return DateTime.TryParseExact(input, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date)
                    ? date.ToUniversalTime().ToString()
                    : input;
            }
        }

        private EqualsCondition CreateEqualsCondition(string key, string value, Item searchSettings)
        {
            var equalsCondition = new EqualsCondition
            {
                TargetField = key,
                Value = value,
            };

            return equalsCondition;
        }
    }
}
