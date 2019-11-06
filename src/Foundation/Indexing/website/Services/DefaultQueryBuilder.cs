using ESearch.Foundation.Indexing.Models;
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
        public NameValueCollection BuildQueryString(SearchQuery query, ISearchSettings settings)
        {
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            if ((query.Offset % settings.PageSize == 0) && query.Limit == settings.PageSize)
            {
                queryString["page"] = (query.Offset / settings.PageSize + 1).ToString();
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
                    ? lowerDate.ToLocalTime().ToString(settings.DateFormat)
                    : betweensCondition.LowerValue;
                var upperValue = DateTime.TryParse(betweensCondition.UpperValue, out var upperDate)
                    ? upperDate.ToLocalTime().ToString(settings.DateFormat)
                    : betweensCondition.UpperValue;

                queryString[betweensCondition.TargetField] = $"{lowerValue}|{upperValue}";
            }

            foreach (var equalsCondition in query.EqualsConditions ?? Enumerable.Empty<EqualsCondition>())
            {
                queryString[equalsCondition.TargetField] = string.IsNullOrEmpty(queryString[equalsCondition.TargetField])
                    ? equalsCondition.Value
                    : $"{queryString[equalsCondition.TargetField]}+{equalsCondition.Value}";
            }

            return queryString;
        }

        public SearchQuery BuildSearchQuery(NameValueCollection queryString, ISearchSettings settings)
        {
            var query = new SearchQuery()
            {
                Scope = settings.Scope,
                TargetTemplates = settings.TargetTemplates.ToArray(),
                ContainsConditions = new List<ContainsCondition>(),
                BetweenConditions = new List<BetweenCondition>(),
                EqualsConditions = new List<EqualsCondition>(),
            };

            query.KeywordCondition = CreateKeywordCondition(queryString["keyword"], settings);
            query.SortConditions = CreateSortConditions(queryString["sort"], settings);
            (query.Offset, query.Limit) = CreatePaginationInfo(queryString["page"], settings);

            foreach (var key in queryString.AllKeys)
            {
                if (key == null || key == "keyword" || key == "sort" || key == "page" || key.StartsWith("sc_"))
                {
                    continue;
                }

                var value = queryString[key];
                if (string.IsNullOrEmpty(value))
                {
                    continue;
                }

                if (value.StartsWith("(") && value.EndsWith(")"))
                {
                    var containsCondition = CreateContainsCondition(key, value, settings);
                    query.ContainsConditions.Add(containsCondition);
                    continue;
                }

                if (value.Contains("|"))
                {
                    var betweenCondition = CreateBetweenCondition(key, value, settings);
                    query.BetweenConditions.Add(betweenCondition);
                    continue;
                }

                var equalsConditions = CreateEqualsConditions(key, value, settings);
                foreach (var equalsCondition in equalsConditions)
                {
                    query.EqualsConditions.Add(equalsCondition);
                }
            }

            return query;
        }

        #region Obsoleted methods
        [Obsolete("This method will be removed in v1.0")]
        public NameValueCollection BuildQueryString(SearchQuery query, Item searchSettings)
        {
            return BuildQueryString(query, new SearchSettings(searchSettings));
        }

        [Obsolete("This method will be removed in v1.0")]
        public SearchQuery BuildSearchQuery(NameValueCollection queryString, Item searchSettings)
        {
            return BuildSearchQuery(queryString, new SearchSettings(searchSettings));
        }
        #endregion

        private KeywordCondition CreateKeywordCondition(string value, ISearchSettings settings)
        {
            var keywordCondition = new KeywordCondition
            {
                TargetFields = settings.KeywordSearchTargets.ToArray(),
                Keywords = value?.Split(new[] { '+' }, StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>(),
            };

            return keywordCondition;
        }

        private ICollection<SortCondition> CreateSortConditions(string value, ISearchSettings settings)
        {
            var sortConditions = new List<SortCondition>();

            foreach (var sortInfo in value?.Split(new[] { '+' }, StringSplitOptions.RemoveEmptyEntries) ?? Enumerable.Empty<string>())
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

        private (int offset, int limit) CreatePaginationInfo(string value, ISearchSettings settings)
        {
            if (settings.PageSize <= 0)
            {
                throw new ArgumentException("PageSize must be positive value.");
            }

            if (string.IsNullOrEmpty(value) || !int.TryParse(value, out var page) || page <= 0)
            {
                return (0, settings.PageSize);
            }

            var offset = (page - 1) * settings.PageSize;
            var limit = settings.PageSize;

            return (offset, limit);
        }

        private ContainsCondition CreateContainsCondition(string key, string value, ISearchSettings settings)
        {
            var containsCondition = new ContainsCondition
            {
                TargetField = key,
                Values = value.TrimStart('(').TrimEnd(')').Split(new[] { '+' }, StringSplitOptions.RemoveEmptyEntries),
            };

            return containsCondition;
        }

        private BetweenCondition CreateBetweenCondition(string key, string value, ISearchSettings settings)
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
                return DateTime.TryParseExact(input, settings.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date)
                    ? date.ToUniversalTime().ToString()
                    : input;
            }
        }

        private ICollection<EqualsCondition> CreateEqualsConditions(string key, string values, ISearchSettings settings)
        {
            return values.Split(new[] { '+' }, StringSplitOptions.RemoveEmptyEntries).Select(value => new EqualsCondition
            {
                TargetField = key,
                Value = value
            }).ToList();
        }
    }
}
