using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESearch.Foundation.Indexing.Models
{
    public class FacetQuery
    {
        public string FacetField { get; set; }
        public SearchQuery SearchQuery { get; set; }
    }
}
