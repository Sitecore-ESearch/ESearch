using Sitecore.Data;

namespace ESearch.Feature.PageSelector
{
    public struct Templates
    {
        public struct PageSelector
        {
            public static readonly ID ID = new ID("{1CBE2C9C-ADF3-46A4-8866-4018B026C88B}");

            public struct Fields
            {
                public static readonly ID SelectorSize = new ID("{B0AC5ADC-B5DD-4D12-99FE-B0B4DD69476C}");
                public static readonly ID PreviousLinkLabel = new ID("{844DC433-8BD7-4C6C-B88D-5A921E53EE47}");
                public static readonly ID NextLinkLabel = new ID("{0E7D76A2-2C71-4470-8C24-1AA992F71726}");
                public static readonly ID FirstLinkLabel = new ID("{313E7431-D0AB-4227-BA4D-98A8E1F5D763}");
                public static readonly ID LastLinkLabel = new ID("{5345578D-59C9-40C8-9977-041ED15D4E32}");
            }
        }
    }
}
