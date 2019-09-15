using Sitecore.Data;

namespace ESearch.Feature.SearchResults
{
    public struct Templates
    {
        public struct SearchResults
        {
            public static readonly ID ID = new ID("{6B260939-A836-4497-B509-7F530FF5593E}");
        }

        public struct SearchResulData
        {
            public static readonly ID ID = new ID("{7A79306A-F5B3-4CC6-A3DB-F82099CECDC1}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{509114B1-FA52-4BEF-87D5-B0F1515A3368}");
                public static readonly ID Thumbnail = new ID("{C8C9C3D5-E990-44CC-B92A-0E88D66FE69F}");
                public static readonly ID Description = new ID("{4EE1C5F3-8E7D-4A4C-9600-6FCBC4EEADC7}");
            }
        }
    }
}
