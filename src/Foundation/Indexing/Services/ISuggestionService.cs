using ESearch.Foundation.Indexing.Models;

namespace ESearch.Foundation.Indexing.Services
{
    public interface ISuggestionService
    {
        SuggestionResults GetSuggestions(SuggestionQuery query);
    }
}
