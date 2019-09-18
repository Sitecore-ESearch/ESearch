using System.Collections.Generic;
using ESearch.Foundation.Indexing.Models;
using Sitecore.Data.Items;

namespace ESearch.Feature.SearchResultSummary.Models
{
    public class SearchResultSummaryModel
    {
        #region Property
        public Item ScopeItem { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; }

        public string TargetTemplates { get; set; }

        public string TargetFields { get; set; }

        public string Keywords { get; set; }

        public ICollection<EqualsCondition> EqualsConditions { get; set; }

        public ICollection<ContainsCondition> ContainsConditions { get; set; }
        #endregion

        #region Constructor
        public SearchResultSummaryModel(SearchQuery searchQuery)
        {
            ScopeItem = Sitecore.Context.Database.GetItem(searchQuery.Scope);

            var targetTemplateItemNames = new List<string>();
            foreach (var template in searchQuery.TargetTemplates)
            {
                targetTemplateItemNames.Add(Sitecore.Context.Database.GetItem(template).Name);
            }
            TargetTemplates = string.Join(" , ", targetTemplateItemNames);

            TargetFields = string.Join(" , ", searchQuery.KeywordCondition.TargetFields);
            Keywords = string.Join(" , ", searchQuery.KeywordCondition.Keywords);
            var count = searchQuery.EqualsConditions.Count;

            EqualsConditions = searchQuery.EqualsConditions;
            ContainsConditions = searchQuery.ContainsConditions;

        }
        #endregion

    }
}
