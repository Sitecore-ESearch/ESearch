using ESearch.Foundation.Indexing.Models;
using ESearch.Foundation.Indexing.Services;

namespace ESearch.Foundation.Indexing.Dummy.Services
{
    public class DummyFacetService : IFacetService
    {
        public FacetResults GetFacet(FacetQuery query)
        {
            return new FacetResults
            {
                FieldName = query.FacetField,
                FacetValues = new []
                {
                    new Sitecore.ContentSearch.Linq.FacetValue("hiking", 84),
                    new Sitecore.ContentSearch.Linq.FacetValue("climbing", 53),
                    new Sitecore.ContentSearch.Linq.FacetValue("cycling", 51),
                    new Sitecore.ContentSearch.Linq.FacetValue("swimming", 40),
                    new Sitecore.ContentSearch.Linq.FacetValue("others", 10),
                }
            };
        }
    }
}
