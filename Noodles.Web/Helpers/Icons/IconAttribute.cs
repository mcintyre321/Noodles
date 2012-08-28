using System;

namespace Noodles.Web.Helpers.Icons
{
    public class IconAttribute : Attribute
    {
        public string IconName { get; private set; }

        public IconAttribute(string iconName)
        {
            IconName = iconName;
        }
        public IconAttribute(IconNames iconName)
        {
            IconName = iconName.ToString().ToLowerInvariant().Replace("@", "");
        }

    }
}