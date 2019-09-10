using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Presentation;

namespace ESearch.Foundation.SitecoreExtensions.Extensions
{
    public static class RenderingExtensions
    {
        public static Item GetItemParameter(this Rendering rendering, string parameterName)
        {
            Assert.ArgumentNotNull(rendering, nameof(rendering));

            var idOrPath = rendering.Parameters[parameterName];
            return rendering.Item.Database.GetItem(idOrPath);
        }
    }
}
