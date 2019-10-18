using Sitecore.Data;

namespace ESearch.Feature.SearchResultSummary
{
    public struct Templates
    {
        public struct SearchResultSummary
        {
            public static readonly ID ID = new ID("{853CA968-EF71-4E8C-9725-564F2E1BA0FE}");
            public struct Fields
            {
                public static readonly ID TotalCountLabel = new ID("{C67C252F-802E-4B77-BD10-3D44C2093232}");
                public static readonly ID SearchConditionsLabel = new ID("{8D3E9D4F-9F6F-4A27-9CF1-17600BC41324}");
            }
        }
    }
}
