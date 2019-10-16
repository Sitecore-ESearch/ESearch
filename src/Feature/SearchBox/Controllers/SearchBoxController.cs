using ESearch.Feature.SearchBox.Models;
using ESearch.Feature.SearchBox.Repositories;
using Sitecore.DependencyInjection;
using System.Web.Mvc;

namespace ESearch.Feature.SearchBox.Controllers
{
    /// <summary>
    /// controller of search box component
    /// </summary>
    public class SearchBoxController: Controller
    {
        /// <summary>
        /// instance of the repository
        /// </summary>
        private readonly ISearchBoxRepository _repository;

        /// <summary>
        /// constructor
        /// </summary>
        public SearchBoxController()
        {
            _repository = ServiceLocator.ServiceProvider.GetService(typeof(ISearchBoxRepository)) as ISearchBoxRepository;
        }

        /// <summary>
        /// action method called Index
        /// </summary>
        /// <returns>View of the search box component</returns>
        public ActionResult Index()
        {
            var model = _repository.GetModel();
            return View("/Views/ESearch/SearchBox.cshtml", model);
        }

        /// <summary>
        /// action method called SearchBoxResult
        /// </summary>
        /// <remarks>assume calls from ajax.</remarks>
        /// <param name="data">Data posted when performing a search in the search box component</param>
        /// <returns>View of the search result  for the search box component</returns>
        [HttpPost]
        public ActionResult SearchBoxResult(SearchBoxModel data)
        {
            var model = _repository.GetResultModel(data);
            return View("/Views/ESearch/SearchBoxResult.cshtml", model);
        }
    }
}
