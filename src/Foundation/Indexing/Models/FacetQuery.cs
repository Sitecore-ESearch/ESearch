using System.Collections.Generic;

namespace ESearch.Foundation.Indexing.Models
{
    public class FacetQuery
    {
        public ICollection<string> FacetFields { get; set; }
        public SearchQuery SearchQuery { get; set; }
    }
}
