using ESearch.Feature.SearchResultSummary.Repositories;
using Sitecore.DependencyInjection;
using System.Web.Mvc;

namespace ESearch.Feature.SearchResultSummary.Controllers
{
    public class SearchResultSummaryController : Controller
    {
        private readonly ISearchResultSummaryRepository _searchResultSummaryRepository;

        public SearchResultSummaryController()
        {
            var type = typeof(ISearchResultSummaryRepository);
            _searchResultSummaryRepository = ServiceLocator.ServiceProvider.GetService(typeof(ISearchResultSummaryRepository)) as ISearchResultSummaryRepository;
        }

        public ActionResult Index()
        {
            var model = _searchResultSummaryRepository.GetModel();
            return View("/Views/ESearch/SearchResultSummary.cshtml", model);
        }
    }
}
