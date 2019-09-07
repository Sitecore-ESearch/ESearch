using ESearch.Foundation.Indexing.Models;
using ESearch.Foundation.Indexing.Services;
using Sitecore;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Linq;

namespace ESearch.Foundation.Indexing.Dummy.Services
{
    public class DummySearchService : ISearchService
    {
        public SearchResults SearchItems(SearchQuery query)
        {
            var dummyItems = Context.Database.GetItem("/sitecore/content/dummy")?.Axes.GetDescendants() ?? Enumerable.Empty<Item>();

            return new SearchResults
            {
                Items = dummyItems.ToList(),
                TotalCount = 238,
            };
        }

        public int GetTotalCount(SearchQuery query)
        {
            return 238;
        }

        public SuggestionResults GetSuggestions(SuggestionQuery query)
        {
            var suggestions = new List<Suggestion>();
            for (var i = 0; i < query.Limit; i++)
            {
                var fields = query.TargetFields.ToDictionary(
                    field => field,
                    field => "Lorem ipsum dolor sit <em>amet</em>, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut <em>labore</em> et dolore magna aliqua.");
                suggestions.Add(new Suggestion
                {
                    SuggestedFields = fields
                });
            }

            return new SuggestionResults
            {
                Suggestions = suggestions,
            };
        }

        public FacetResults GetFacets(FacetQuery query)
        {
            return new FacetResults
            {
                Facets = query.FacetFields.Select(field => new Facet
                {
                    FieldName = field,
                    FacetValues = new[]
                    {
                        new FacetValue { FieldValue = "hiking", Count = 84 },
                        new FacetValue { FieldValue ="climbing", Count = 53 },
                        new FacetValue { FieldValue = "cycling", Count = 51 },
                        new FacetValue { FieldValue = "swimming", Count = 40 },
                        new FacetValue { FieldValue = "others", Count = 10 },
                    }
                }).ToList(),
            };
        }
    }
}
