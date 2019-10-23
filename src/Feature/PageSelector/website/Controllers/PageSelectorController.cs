using ESearch.Feature.PageSelector.Repositories;
using Sitecore.DependencyInjection;
using System.Web.Mvc;

namespace ESearch.Feature.PageSelector.Controllers
{
    public class PageSelectorController : Controller
    {
        private readonly IPageSelectorRepository _pageSelectorRepository;

        public PageSelectorController()
        {
            _pageSelectorRepository = ServiceLocator.ServiceProvider.GetService(typeof(IPageSelectorRepository)) as IPageSelectorRepository;
        }

        public ActionResult Index()
        {
            var model = _pageSelectorRepository.GetModel();
            return View("/Views/ESearch/PageSelector.cshtml", model);
        }
    }
}
