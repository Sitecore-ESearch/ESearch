using Sitecore;
using Sitecore.ContentSearch;

namespace ESearch.Foundation.Indexing.Services
{
    public class DefaultIndexResolver : IIndexResolver
    {
        public ISearchIndex Resolve()
        {
            var databaseName = Context.Database.Name.ToLowerInvariant();
            var indexName = $"sitecore_{databaseName}_index";

            return ContentSearchManager.GetIndex(indexName);
        }
    }
}
