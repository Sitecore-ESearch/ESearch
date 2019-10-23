using System.Web.Mvc;
using System.Web.Routing;

namespace ESearch.Foundation.SitecoreExtensions.App_Start
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "SearchBoxResult",
                url: "esearch/searchbox",
                defaults: new { controller = "SearchBox", action = "SearchBoxResult" }
            );
        }
    }
}
