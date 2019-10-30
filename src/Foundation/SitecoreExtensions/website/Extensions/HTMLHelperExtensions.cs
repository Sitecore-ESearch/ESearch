using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Helpers;
using System.Web;

namespace ESearch.Foundation.SitecoreExtensions.Extensions
{
    public static class HTMLHelperExtensions
    {
        public static HtmlString ImageField(this SitecoreHelper helper, ID fieldID, int mh = 0, int mw = 0, string cssClass = null, bool disableWebEditing = false)
        {
            return helper.Field(fieldID.ToString(), new
            {
                mh,
                mw,
                DisableWebEdit = disableWebEditing,
                @class = cssClass ?? string.Empty
            });
        }

        public static HtmlString ImageField(this SitecoreHelper helper, ID fieldID, Item item, int mh = 0, int mw = 0, string cssClass = null, bool disableWebEditing = false)
        {
            return helper.Field(fieldID.ToString(), item, new
            {
                mh,
                mw,
                DisableWebEdit = disableWebEditing,
                @class = cssClass ?? string.Empty
            });
        }

        public static HtmlString ImageField(this SitecoreHelper helper, string fieldName, Item item, int mh = 0, int mw = 0, string cssClass = null, bool disableWebEditing = false)
        {
            return helper.Field(fieldName, item, new
            {
                mh,
                mw,
                DisableWebEdit = disableWebEditing,
                @class = cssClass ?? string.Empty
            });
        }

        public static HtmlString Field(this SitecoreHelper helper, ID fieldID)
        {
            Assert.ArgumentNotNullOrEmpty(fieldID, nameof(fieldID));
            return helper.Field(fieldID.ToString());
        }

        public static HtmlString Field(this SitecoreHelper helper, ID fieldID, object parameters)
        {
            Assert.ArgumentNotNullOrEmpty(fieldID, nameof(fieldID));
            Assert.IsNotNull(parameters, nameof(parameters));
            return helper.Field(fieldID.ToString(), parameters);
        }

        public static HtmlString Field(this SitecoreHelper helper, ID fieldID, Item item, object parameters)
        {
            Assert.ArgumentNotNullOrEmpty(fieldID, nameof(fieldID));
            Assert.IsNotNull(item, nameof(item));
            Assert.IsNotNull(parameters, nameof(parameters));
            return helper.Field(fieldID.ToString(), item, parameters);
        }

        public static HtmlString Field(this SitecoreHelper helper, ID fieldID, Item item)
        {
            Assert.ArgumentNotNullOrEmpty(fieldID, nameof(fieldID));
            Assert.IsNotNull(item, nameof(item));
            return helper.Field(fieldID.ToString(), item);
        }
    }
}
