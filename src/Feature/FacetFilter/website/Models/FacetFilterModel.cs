using System.Collections.Generic;

namespace ESearch.Feature.FacetFilter.Models
{
    public class FacetFilterModel
    {
        public string ClearLink { get; set; }
        public ICollection<FacetFilterRow> FilterRows { get; set; }
    }

    public class FacetFilterRow
    {
        public string Label { get; set; }
        public string Link { get; set; }
        public int Count { get; set; }
        public bool IsActive { get; set; }
    }
}
