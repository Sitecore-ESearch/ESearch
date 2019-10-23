using Sitecore.Data;
using System.Collections.Generic;

namespace ESearch.Foundation.Indexing.Models
{
    public class SearchQuery
    {
        public ID Scope { get; set; }

        public ICollection<ID> TargetTemplates { get; set; }

        public int Offset { get; set; }

        public int Limit { get; set; }

        public KeywordCondition KeywordCondition { get; set; }

        public ICollection<EqualsCondition> EqualsConditions { get; set; }

        public ICollection<ContainsCondition> ContainsConditions { get; set; }

        public ICollection<BetweenCondition> BetweenConditions { get; set; }

        public ICollection<SortCondition> SortConditions { get; set; }
    }

    public class KeywordCondition
    {
        public ICollection<string> TargetFields { get; set; }
        public ICollection<string> Keywords { get; set; }
    }

    public class EqualsCondition
    {
        public string TargetField { get; set; }
        public string Value { get; set; }
    }

    public class ContainsCondition
    {
        public string TargetField { get; set; }
        public ICollection<string> Values { get; set; }
    }

    public class BetweenCondition
    {
        public string TargetField { get; set; }

        /// <summary>
        /// A lower value of the target range.
        /// If the value is parsable as DateTime, it treats as UTC time.
        /// </summary>
        public string LowerValue { get; set; }

        /// <summary>
        /// A upper value of the target range.
        /// If the value is parsable as DateTime, it treats as UTC time.
        /// </summary>
        public string UpperValue { get; set; }
    }

    public class SortCondition
    {
        public string TargetField { get; set; }
        public SortDirection Direction { get; set; }
    }

    public enum SortDirection
    {
        Asc,
        Desc
    }
}
