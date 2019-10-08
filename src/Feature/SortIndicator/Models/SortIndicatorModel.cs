using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using Sitecore;
using System.Web;
using System.Linq;
using ESearch.Foundation.Indexing.Models;

namespace ESearch.Feature.SortIndicator.Models
{
    public class SortIndicatorModel
    {
        #region Property
        public Item[] SortItems { get; set; }
        public string TargetField { get; set; }
        public SortDirection Direction { get; set; }
        #endregion

        #region Constructor
        public SortIndicatorModel(Item sortSetting, SearchQuery searchQuery)
        {
            MultilistField sortItems = sortSetting.Fields[Templates.SortIndicator.Fields.SortFields];
            SortItems = sortItems?.GetItems();

            if (SortItems.Length > 0 )
            {
                var query = HttpUtility.ParseQueryString(Context.HttpContext.Request.Url.Query);
                TargetField = GetTargetField(SortItems, searchQuery);
                Direction = GetDirection(searchQuery);
            }

        }
        #endregion

        #region Method
        public string GetTargetField(Item[] sortItems, SearchQuery searchQuery)
        {
            if (searchQuery.SortConditions.Count == 0)
            {
                return sortItems[0][Templates.SortField.Fields.FieldName];               
            }
            else
            {
                return searchQuery.SortConditions.First().TargetField;
            }
        }

        public SortDirection GetDirection(SearchQuery searchQuery)
        {
            if (searchQuery.SortConditions.Count == 0)
            {
                return SortDirection.Asc;
            }
            else
            {
                return searchQuery.SortConditions.First().Direction;
            }
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
