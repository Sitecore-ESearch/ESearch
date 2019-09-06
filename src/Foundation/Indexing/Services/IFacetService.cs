using ESearch.Foundation.Indexing.Models;

namespace ESearch.Foundation.Indexing.Services
{
    public interface IFacetService
    {
        FacetResults GetFacet(FacetQuery query);
    }
}
