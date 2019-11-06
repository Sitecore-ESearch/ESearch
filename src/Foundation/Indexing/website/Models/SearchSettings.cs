using ESearch.Foundation.SitecoreExtensions.Extensions;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;

namespace ESearch.Foundation.Indexing.Models
{
    public interface ISearchSettings
    {
        string DateFormat { get; }
        IReadOnlyCollection<string> KeywordSearchTargets { get; }
        int PageSize { get; }
        ID Scope { get; }
        IReadOnlyCollection<ID> TargetTemplates { get; }
    }

    public class SearchSettings : ISearchSettings
    {
        public string DateFormat { get; }
        public IReadOnlyCollection<string> KeywordSearchTargets { get; }
        public int PageSize { get; }
        public ID Scope { get; }
        public IReadOnlyCollection<ID> TargetTemplates { get; }

        public SearchSettings(Item item)
        {
            Assert.IsNotNull(item, string.Empty);

            DateFormat = item[Templates.SearchSettings.Fields.DateFormat];
            KeywordSearchTargets = item[Templates.SearchSettings.Fields.KeywordSearchTargets].Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            PageSize = item.GetInteger(Templates.SearchSettings.Fields.PageSize) ?? -1;
            Scope = ID.Parse(item[Templates.SearchSettings.Fields.Scope], ItemIDs.ContentRoot);
            TargetTemplates = ((MultilistField)item.Fields[Templates.SearchSettings.Fields.TargetTemplates]).TargetIDs;
        }
    }
}
