using Sitecore.ContentSearch;

namespace ESearch.Foundation.Indexing.Services
{
    public interface IIndexResolver
    {
        ISearchIndex Resolve();
    }
}
