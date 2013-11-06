using System;

namespace Noodles.AspMvc.UiAttributes
{
    /// <summary>
    /// INTERNAL: this attribute prevents the attribute it is on being run in the NoodlesHelper.Form method
    /// </summary>
    public class NotInFormHelperAttribute : Attribute
    {
    }
}