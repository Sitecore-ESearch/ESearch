using ESearch.Foundation.SitecoreExtensions.App_Start;
using Sitecore.Pipelines;
using System.Web.Routing;

namespace ESearch.Foundation.SitecoreExtensions.Pipelines
{

    public class RegisterCustomRoutesPipeline
    {
        public void Process(PipelineArgs args)
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
