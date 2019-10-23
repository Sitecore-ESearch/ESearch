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

        public static int? GetIntegerParameter(this Rendering rendering, string parameterName)
        {
            Assert.ArgumentNotNull(rendering, nameof(rendering));

            var value = rendering.Parameters[parameterName];
            if (string.IsNullOrEmpty(value))
            {
                return default;
            }

            return int.TryParse(value, out var result) ? result : default(int?);
        }
    }
}
