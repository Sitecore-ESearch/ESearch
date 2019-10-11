using System.Collections.Generic;
using ESearch.Foundation.SitecoreExtensions.Extensions;
using ESearch.Foundation.Indexing;
using ESearch.Foundation.Indexing.Models;
using Sitecore;
using Sitecore.Data.Items;
using System;
using System.Globalization;
using System.Linq;

namespace ESearch.Feature.SearchResultSummary.Models
{
    public class SearchResultSummaryModel
    {
        #region Property

        public string Keywords { get; set; }

        public SearchQuery SearchQuery { get; set; }

        public ICollection<BetweenCondition> BetweenConditions { get; set; }

        #endregion

        #region Constructor
        public SearchResultSummaryModel(SearchQuery searchQuery, Item searchSettings)
        {
            SearchQuery = searchQuery;

            Keywords = string.Join(", ", SearchQuery.KeywordCondition.Keywords);

            BetweenConditions = SearchQuery.BetweenConditions;

            var dateFormat = searchSettings[Templates.SearchSettings.Fields.DateFormat];
            
            foreach (var betweensCondition in BetweenConditions ?? Enumerable.Empty<BetweenCondition>())
            {
                betweensCondition.LowerValue = DateTime.TryParse(betweensCondition.LowerValue, out var lowerDate)
                    ? lowerDate.ToLocalTime().ToString(dateFormat)
                    : betweensCondition.LowerValue;
                betweensCondition.UpperValue = DateTime.TryParse(betweensCondition.UpperValue, out var upperDate)
                    ? upperDate.ToLocalTime().ToString(dateFormat)
                    : betweensCondition.UpperValue;
            }
        }
        #endregion

    }
}
