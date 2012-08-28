using System;

namespace Noodles.Web.Helpers.Icons
{
    public class IconAttribute : Attribute
    {
        public string[] Parts { get; set; }

        public IconAttribute(params string[] parts)
        {
            Parts = parts;
        }
    }
}