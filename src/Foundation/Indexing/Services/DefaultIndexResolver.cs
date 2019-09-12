using Sitecore;
using Sitecore.ContentSearch;

namespace ESearch.Foundation.Indexing.Services
{
    public class DefaultIndexResolver : IIndexResolver
    {
        public ISearchIndex Resolve()
        {
            var indexable = (SitecoreIndexableItem) Context.Item;
            var indexName = ContentSearchManager.GetContextIndexName(indexable);

            return ContentSearchManager.GetIndex(indexName);
        }
    }
}
