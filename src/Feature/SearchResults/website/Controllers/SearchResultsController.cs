using ESearch.Feature.SearchResults.Repositories;
using Sitecore.DependencyInjection;
using System.Web.Mvc;

namespace ESearch.Feature.SearchResults.Controllers
{
    public class SearchResultsController : Controller
    {
        private readonly ISearchResultsRepository _searchResultsRepository;

        public SearchResultsController()
        {
            _searchResultsRepository = ServiceLocator.ServiceProvider.GetService(typeof(ISearchResultsRepository)) as ISearchResultsRepository;
        }

        public ActionResult Index()
        {
            var model = _searchResultsRepository.GetModel();
            return View("/Views/ESearch/SearchResults.cshtml", model);
        }
    }
}
