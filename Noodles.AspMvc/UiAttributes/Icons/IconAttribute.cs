using System;

namespace Noodles.AspMvc.UiAttributes.Icons
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