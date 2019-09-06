using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

        public SearchQuery()
        {
        }

        public SearchQuery(string queryString, Item searchSettings)
        {
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }

    public class KeywordCondition
    {
        public ICollection<string> FieldNames { get; set; }
        public ICollection<string> Keywords { get; set; }
    }

    public class EqualsCondition
    {
        public string FieldName { get; set; }
        public string Value { get; set; }
    }

    public class ContainsCondition
    {
        public string FieldName { get; set; }
        public ICollection<string> Values { get; set; }
    }

    public class BetweenCondition
    {
        public string FieldName { get; set; }
        public string LowerValue { get; set; }
        public string UpperValue { get; set; }
    }
}
