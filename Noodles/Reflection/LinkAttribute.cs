using System;

namespace Noodles
{
    public class LinkAttribute : Attribute
    {
        public LinkAttribute()
        {
            UiHint = "";
        }
        public string UiHint { get; set; }
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