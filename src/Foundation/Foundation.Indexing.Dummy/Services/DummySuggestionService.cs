using ESearch.Foundation.Indexing.Models;
using ESearch.Foundation.Indexing.Services;
using System.Collections.Generic;
using System.Linq;

namespace ESearch.Foundation.Indexing.Dummy.Services
{
    public class DummySuggestionService : ISuggestionService
    {
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
    }
}
