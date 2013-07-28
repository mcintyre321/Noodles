using System;

namespace Noodles
{
    public class LinkAttribute : Attribute
    {
        public LinkAttribute()
        {
            UiHint = "";
        }
        public string DisplayName { get; set; }
        public string UiHint { get; set; }
        public string Slug { get; set; }
    }

    public class LinksAttribute : Attribute
    {
        public LinksAttribute()
        {
            UiHint = "";
        }
        public string UiHint { get; set; }
    }
}