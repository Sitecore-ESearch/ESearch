namespace ESearch.Foundation.SitecoreExtensions.Pipelines
{
    using Sitecore.Pipelines;
    using System.Web.Routing;
    using ESearch.Foundation.SitecoreExtensions.App_Start;

    public class RegisterCustomRoutesPipeline
    {
        public void Process(PipelineArgs args)
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
