using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESearch.Foundation.Indexing.Models
{
    public class SuggestionResults
    {
        public ICollection<Suggestion> Suggestions { get; set; }
    }

    public class Suggestion
    {
        public IDictionary<string, string> SuggestedFields { get; set; }
    }
}
