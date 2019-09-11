using Sitecore.Data;

namespace ESearch.Foundation.Indexing
{
    public struct Templates
    {
        public struct SearchSettings
        {
            public static readonly ID ID = new ID("{D3FA770C-1132-43A1-8DDC-95659EC52108}");

            public struct Fields
            {
                public static readonly ID Scope = new ID("{1BC703DD-565A-4B9D-A5B9-6E837B899EAC}");
                public static readonly ID PageSize = new ID("{2AD3D76D-4348-4D23-9988-A1E0CF9C2D0F}");
                public static readonly ID TargetTemplates = new ID("{2451C71D-4BCA-480E-875D-456C31DF189C}");
                public static readonly ID KeywordSearchTargets = new ID("{3DB968F2-72C0-4F98-B33E-B83C2F951F1E}");
            }
        }
    }
}
