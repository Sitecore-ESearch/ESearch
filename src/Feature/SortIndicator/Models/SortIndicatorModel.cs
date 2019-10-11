using System.Collections.Generic;
using System.Web;
using System.Linq;
using Sitecore;
using Sitecore.Data.Items;
using ESearch.Foundation.Indexing.Models;
using ESearch.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Install.Framework;

namespace ESearch.Feature.SortIndicator.Models
{
    public class SortIndicatorModel
    {
        #region Property
        public IEnumerable<Item> SortItems { get; set; }
        public string DefaultText { get; set; }
        public string TargetField { get; set; }
        public SortDirection Direction { get; set; }
        #endregion

        #region Constructor
        public SortIndicatorModel(Item sortSetting, SearchQuery searchQuery)
        {
            SortItems = sortSetting.GetMultiListValueItems(Templates.SortIndicator.Fields.SortFields);
            DefaultText = sortSetting[Templates.SortIndicator.Fields.DefaultText];

            if (SortItems.Any())
            {
                TargetField = GetTargetField(searchQuery);
                Direction = GetDirection(searchQuery);
            }
        }
        #endregion

        #region Method
        public string GetTargetField(SearchQuery searchQuery)
        {
            return searchQuery.SortConditions.FirstOrDefault()?.TargetField;
        }

        public SortDirection GetDirection(SearchQuery searchQuery)
        {
            return searchQuery.SortConditions.FirstOrDefault()?.Direction ?? default;
        }

        public string GetSortLink(string fieldName, SortDirection direction)
        {
            var absolutePath = Context.HttpContext.Request.Url.AbsolutePath;
            var query = HttpUtility.ParseQueryString(Context.HttpContext.Request.Url.Query);

            if (!string.IsNullOrEmpty(fieldName))
            {
                if (query["sort"] == null)
                {
                    query.Add("sort", $"{fieldName}:{direction}");
                }
                else
                {
                    query["sort"] = $"{fieldName}:{direction}";
                }
            }

            return $"{absolutePath}?{query}";

        }
        #endregion
    }
}
