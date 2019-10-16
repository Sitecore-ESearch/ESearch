using Sitecore.Data;

namespace ESearch.Project.SampleWebsite
{
    public struct Templates
    {
        public struct ArticlePage
        {
            public static readonly ID ID = new ID("{8DF4EAC2-BB4A-48FE-BA7F-7103D84DD80E}");

            public struct Fields
            {
                public static readonly ID ArticleContent = new ID("{E7D512B3-D5D1-4016-AF79-2EDF4C7C9ACF}");
                public static readonly ID ArticleDate = new ID("{8E5FADB9-1F1B-431A-84BE-A24AF13ACCFC}");
                public static readonly ID ArticleCategory = new ID("{BED89F56-0A0E-46A0-9C01-7C2F1FEA3424}");
                public static readonly ID ArticleTags = new ID("{9258ECCE-B90B-4B1C-9E28-5079D427023D}");
            }
        }
    }
}
