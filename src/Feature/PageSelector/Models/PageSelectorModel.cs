using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESearch.Feature.PageSelector.Models
{
    public class PageSelectorModel
    {
        private readonly string _searchPageUrl;

        public ICollection<int> PageIndexes { get; set; }
        public int CurrentPageIndex { get; set; }
        public int FirstPageIndex { get; set; }
        public int LastPageIndex { get; set; }
        public bool HasPreviousPage => FirstPageIndex < CurrentPageIndex;
        public bool HasNextPage => CurrentPageIndex < LastPageIndex;

        public PageSelectorModel(string searchPageUrl, int pageSize, int resultsCount, int selectorSize)
        {
            _searchPageUrl = searchPageUrl;
            CurrentPageIndex = GetPageQueryValue(searchPageUrl);
            FirstPageIndex = 1;
            LastPageIndex = CalculateLastPage(resultsCount, pageSize);
            PageIndexes = CreatePageIndexes(CurrentPageIndex, selectorSize, LastPageIndex);
        }

        public string GetPageUrl(int index)
        {
            var uri = new Uri(_searchPageUrl);
            var query = HttpUtility.ParseQueryString(uri.Query);
            query["page"] = index.ToString();
            return $"{uri.AbsolutePath}?{query}";
        }

        private static int GetPageQueryValue(string url)
        {
            var uri = new Uri(url);
            var query = HttpUtility.ParseQueryString(uri.Query);
            if (string.IsNullOrEmpty(query["page"]))
            {
                return 1;
            }

            return int.TryParse(query["page"], out var page) ? page : 1;
        }

        private static int CalculateLastPage(int resultsCount, int pageSize)
        {
            var lastPage = (int)Math.Ceiling((double)resultsCount / pageSize);
            return lastPage > 0 ? lastPage : 1;
        }

        private static List<int> CreatePageIndexes(int currentPageIndex, int selectorSize, int lastPageIndex)
        {
            return Enumerable.Range(1, lastPageIndex).Where(IsVisibleIndex).ToList();

            bool IsVisibleIndex(int index)
            {
                var pagerLength = selectorSize * 2 + 1;
                var half = Math.Floor((double)pagerLength / 2);
                if (currentPageIndex <= half)
                {
                    return 1 <= index && index <= pagerLength;
                }

                if (lastPageIndex - half < currentPageIndex)
                {
                    return lastPageIndex - pagerLength < index && index <= lastPageIndex;
                }

                return currentPageIndex - half <= index && index <= currentPageIndex + half;
            }
        }
    }
}
