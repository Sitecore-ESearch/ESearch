using ESearch.Feature.SearchBox.Models;
using ESearch.Feature.SearchBox.Repositories;
using Sitecore.DependencyInjection;
using System.Web.Mvc;

namespace ESearch.Feature.SearchBox.Controllers
{
    public class SearchBoxController: Controller
    {
        private readonly ISearchBoxRepository _repository;

        public SearchBoxController()
        {
            _repository = ServiceLocator.ServiceProvider.GetService(typeof(ISearchBoxRepository)) as ISearchBoxRepository;
        }

        public ActionResult Index()
        {
            var model = _repository.GetModel();
            return View("/Views/ESearch/SearchBox.cshtml", model);
        }

        [HttpPost]
        public ActionResult SearchBoxResult(SearchBoxModel data)
        {
            var model = _repository.GetResultModel(data);
            return View("/Views/ESearch/SearchBoxResult.cshtml", model);
        }
    }
}
