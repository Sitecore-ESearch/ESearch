using System.Collections.Generic;

namespace ESearch.Foundation.Indexing.Models
{
    public class FacetResults
    {
        public ICollection<Facet> Facets { get; set; }
    }

    public class Facet
    {
        public string FieldName { get; set; }
        public ICollection<FacetValue> FacetValues { get; set; }
    }

    public class FacetValue
    {
        public string FieldValue { get; set; }
        public int Count { get; set; }
    }
}
