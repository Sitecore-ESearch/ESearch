using ESearch.Foundation.Indexing.Models;
using ESearch.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Diagnostics;
using Sitecore.Mvc.Presentation;
using System;

namespace ESearch.Foundation.Indexing.Extensions
{
    public static class RenderingContextExtensions
    {
        public static ISearchSettings GetSearchSettings(this RenderingContext context)
        {
            Assert.IsNotNull(context, string.Empty);

            var searchSettings = context.Rendering.GetItemParameter("Search Settings");
            if (searchSettings == null)
            {
                throw new InvalidOperationException("'Search Settings' parameter is empty or link broken.");
            }

            return new SearchSettings(searchSettings);
        }
    }
}
