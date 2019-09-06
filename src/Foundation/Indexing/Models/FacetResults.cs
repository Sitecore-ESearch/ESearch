using Sitecore.ContentSearch.Linq;
using System.Collections.Generic;

namespace ESearch.Foundation.Indexing.Models
{
    public class FacetResults
    {
        public string FieldName { get; set; }
        public ICollection<FacetValue> FacetValues { get; set; }
    }
}
