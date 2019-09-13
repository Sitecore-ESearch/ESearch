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
            if (!string.IsNullOrEmpty(indexName))
            {
                return ContentSearchManager.GetIndex(indexName);
            }

            var dbName = Context.Database.Name.ToLowerInvariant();
            return ContentSearchManager.GetIndex($"sitecore_{dbName}_index");
        }
    }
}
