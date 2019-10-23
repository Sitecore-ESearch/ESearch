using System.Web.Mvc;
using Sitecore.DependencyInjection;
using ESearch.Feature.SortIndicator.Repositories;

namespace ESearch.Feature.SortIndicator.Controllers
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
