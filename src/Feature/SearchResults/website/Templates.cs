






// ReSharper disable InconsistentNaming
namespace ESearch.Feature.SearchResults
{
    public struct Templates
    {
        #region /sitecore/templates/Feature/ESearch/Search Results/Search Result Data
        public struct SearchResultData
        {
            public static readonly Sitecore.Data.ID ID = new Sitecore.Data.ID("{7A79306A-F5B3-4CC6-A3DB-F82099CECDC1}");

            public struct Fields
            {
                public static readonly Sitecore.Data.ID Description = new Sitecore.Data.ID("{4EE1C5F3-8E7D-4A4C-9600-6FCBC4EEADC7}");
                public static readonly Sitecore.Data.ID Thumbnail = new Sitecore.Data.ID("{C8C9C3D5-E990-44CC-B92A-0E88D66FE69F}");
                public static readonly Sitecore.Data.ID Title = new Sitecore.Data.ID("{509114B1-FA52-4BEF-87D5-B0F1515A3368}");
            }
        }
        #endregion
        #region /sitecore/templates/Feature/ESearch/Search Results/Search Results
        public struct SearchResults
        {
            public static readonly Sitecore.Data.ID ID = new Sitecore.Data.ID("{6B260939-A836-4497-B509-7F530FF5593E}");

            public struct Fields
            {
                public static readonly Sitecore.Data.ID ReadMoreLabel = new Sitecore.Data.ID("{5794BCAA-9780-4252-83D2-DCACD7632B84}");
            }
        }
        #endregion
        #region /sitecore/templates/Feature/ESearch/Search Results/Rendering Parameter/Search Results Parameters
        public struct SearchResultsParameters
        {
            public static readonly Sitecore.Data.ID ID = new Sitecore.Data.ID("{9100A3D4-D0D8-4180-9383-CD01298CA30B}");

            public struct Fields
            {
                public static readonly Sitecore.Data.ID ItemCssClass = new Sitecore.Data.ID("{6EED8552-3764-4A3E-B34F-9CD5E9C2D29A}");
            }
        }
        #endregion
    }
}


