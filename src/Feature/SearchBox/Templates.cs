using Sitecore.Data;

namespace ESearch.Feature.SearchBox
{
    public struct Templates
    {
        public struct SearchBox
        {
            public static readonly ID ID = new ID("{A1DD0C72-0926-46AD-9BB6-9F0A37175EC2}");
            public struct Fields
            {
                public static readonly ID PlaceholderLabel = new ID("{0E5C8A9C-77C3-401D-815F-1594C8791B01}");
                public static readonly ID ExecuteButtonLabel = new ID("{C9799703-CCAA-46BE-B91D-E7265526ACF1}");
            }
        }

        public struct SearchResultData
        {
            public static readonly ID ID = new ID("{7A79306A-F5B3-4CC6-A3DB-F82099CECDC1}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{509114B1-FA52-4BEF-87D5-B0F1515A3368}");
            }
        }
    }
}
