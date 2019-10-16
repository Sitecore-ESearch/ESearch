using ESearch.Feature.SearchBox.Models;
using ESearch.Feature.SearchBox.Repositories;
using Sitecore.DependencyInjection;
using System.Web.Mvc;

namespace ESearch.Feature.SearchBox.Controllers
{
    public class SearchBoxController: Controller
    {
        private readonly ISearchBoxRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchBoxController" /> class.
        /// </summary>
        public SearchBoxController()
        {
            _repository = ServiceLocator.ServiceProvider.GetService(typeof(ISearchBoxRepository)) as ISearchBoxRepository;
        }

        /// <summary>
        /// Returns the search box component view
        /// </summary>
        /// <returns>the search box component view</returns>
        public ActionResult Index()
        {
            var model = _repository.GetModel();
            return View("/Views/ESearch/SearchBox.cshtml", model);
        }

        /// <summary>
        /// Returns the search result view for the search box component
        /// </summary>
        /// <remarks>Called from ajax.</remarks>
        /// <param name="data">Data posted when performing a search in the search box component</param>
        /// <returns>The search result view for the search box component</returns>
        [HttpPost]
        public ActionResult SearchBoxResult(SearchBoxModel data)
        {
            var model = _repository.GetResultModel(data);
            return View("/Views/ESearch/SearchBoxResult.cshtml", model);
        }
    }
}
