using Sitecore.Data;
using System.Collections.Generic;

namespace ESearch.Foundation.Indexing.Models
{
    public class SuggestionResults
    {
        public ICollection<Suggestion> Suggestions { get; set; }
    }

    public class Suggestion
    {
        public ID ItemId { get; set; }
        public IDictionary<string, string> SuggestedFields { get; set; }
    }
}
