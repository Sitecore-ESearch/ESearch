using ESearch.Feature.FacetFilter.Repositories;
using Sitecore.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ESearch.Feature.FacetFilter.Controllers
{
    public class FacetFilterController : Controller
    {
        private IFacetFilterRepository _facetFilterRepository;

        public FacetFilterController()
        {
            _facetFilterRepository = ServiceLocator.ServiceProvider.GetService(typeof(IFacetFilterRepository)) as IFacetFilterRepository;
        }

        public ActionResult Index()
        {
            var model = _facetFilterRepository.GetModel();
            return View("/Views/ESearch/FacetFilter.cshtml", model);
        }
    }
}
