using Sitecore.Data;

namespace ESearch.Feature.SortIndicator
{
    public struct Templates
    {
        public struct SortIndicator
        {
            public static readonly ID ID = new ID("{5119F67D-EA7D-489E-8A69-CB160D54189C}");
            public struct Fields
            {
                public static readonly ID SortFields = new ID("{632F4F3B-3F60-4790-9F91-265A4CFDF503}");
                public static readonly ID DefaultText = new ID("{33BE8D8A-772F-4666-8EC4-66A63E2023C9}");
            }
        }

        public struct SortField
        {
            public static readonly ID ID = new ID("{632DA6D2-5B7E-4BAF-981C-54EB3D4FD946}");
            public struct Fields
            {
                public static readonly ID FieldName = new ID("{D75B25B9-2BA8-49BD-8315-84172049B644}");
                public static readonly ID DisplayName = new ID("{ECCCF19B-2456-4108-99B8-F90D3849EA18}");
            }
        }
    }
}
