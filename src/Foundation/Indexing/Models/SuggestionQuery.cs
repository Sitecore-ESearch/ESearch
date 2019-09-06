using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESearch.Foundation.Indexing.Models
{
    public class SuggestionQuery
    {
        public ICollection<string> Keywords { get; set; }
        public ICollection<ID> TargetTemplates { get; set; }
        public ICollection<string> TargetFields { get; set; }
        public int Limit { get; set; }
    }
}
