






// ReSharper disable InconsistentNaming
namespace ESearch.Feature.SortIndicator
{
    public struct Templates
    {
        #region /sitecore/templates/Feature/ESearch/Sort Indicator/Sort Field
        public struct SortField
        {
            public static readonly Sitecore.Data.ID ID = new Sitecore.Data.ID("{632DA6D2-5B7E-4BAF-981C-54EB3D4FD946}");

            public struct Fields
            {
                public static readonly Sitecore.Data.ID DisplayName = new Sitecore.Data.ID("{ECCCF19B-2456-4108-99B8-F90D3849EA18}");
                public static readonly Sitecore.Data.ID FieldName = new Sitecore.Data.ID("{D75B25B9-2BA8-49BD-8315-84172049B644}");
            }
        }
        #endregion
        #region /sitecore/templates/Feature/ESearch/Sort Indicator/Sort Indicator
        public struct SortIndicator
        {
            public static readonly Sitecore.Data.ID ID = new Sitecore.Data.ID("{5119F67D-EA7D-489E-8A69-CB160D54189C}");

            public struct Fields
            {
                public static readonly Sitecore.Data.ID DefaultText = new Sitecore.Data.ID("{33BE8D8A-772F-4666-8EC4-66A63E2023C9}");
                public static readonly Sitecore.Data.ID SortFields = new Sitecore.Data.ID("{632F4F3B-3F60-4790-9F91-265A4CFDF503}");
            }
        }
        #endregion
    }
}


