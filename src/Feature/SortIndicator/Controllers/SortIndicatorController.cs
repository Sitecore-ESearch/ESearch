using ESearch.Feature.SortIndicator.Repositories;
using Sitecore.DependencyInjection;
using System.Web.Mvc;

namespace ESearch.Feature.SearchResults.Controllers
{
    public class SortIndicatorController : Controller
    {
        private readonly ISortIndicatorRepository _searchIndicatorRepository;

        public SortIndicatorController()
        {
            _searchIndicatorRepository = ServiceLocator.ServiceProvider.GetService(typeof(ISortIndicatorRepository)) as ISortIndicatorRepository;
        }

        public ActionResult Index()
        {
            var model = _searchIndicatorRepository.GetModel();
            return View("/Views/ESearch/SortIndicator.cshtml", model);
        }
    }
}
