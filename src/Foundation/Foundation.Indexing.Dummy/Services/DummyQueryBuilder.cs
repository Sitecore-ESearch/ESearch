using ESearch.Foundation.Indexing.Models;
using ESearch.Foundation.Indexing.Services;
using Sitecore;
using Sitecore.Data.Items;
using System.Collections.Specialized;
using System.Web;

namespace ESearch.Foundation.Indexing.Dummy.Services
{
    public class DummyQueryBuilder : IQueryBuilder
    {
        public NameValueCollection BuildQueryString(SearchQuery query)
        {
            return HttpUtility.ParseQueryString("keyword=everest&tags=(beginner)&category=climbing&price=100|250&sort=date:desc");
        }

        public SearchQuery BuildSearchQuery(NameValueCollection queryString, Item searchSettings)
        {
            return new SearchQuery()
            {
                Scope = ItemIDs.ContentRoot,
                TargetTemplates = new[]
                {
                    TemplateIDs.UnversionedFile, TemplateIDs.MediaFolder
                },
                ContainsConditions = new[]
                {
                    new ContainsCondition { FieldName = "tags", Values = new[] { "beginner" } },
                },
                EqualsConditions = new[]
                {
                    new EqualsCondition { FieldName = "category", Value = "climbing" }
                },
                BetweenConditions = new[]
                {
                    new BetweenCondition { FieldName = "price", LowerValue = "100", UpperValue = "250" },
                },
                KeywordCondition = new KeywordCondition
                {
                    FieldNames = new[] { "title" },
                    Keywords = new [] { "everest" },
                },
                SortConditions = new[]
                {
                    new SortCondition { FieldName = "date", Direction = SortDirection.Desc }
                },
            };
        }
    }
}
