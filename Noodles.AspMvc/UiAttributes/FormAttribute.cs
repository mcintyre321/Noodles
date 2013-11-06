using System;
using System.Web;
using System.Web.Mvc;
using Noodles.AspMvc.Helpers;
using Noodles.Models;

namespace Noodles.AspMvc.UiAttributes
{
    [NotInFormHelper]
    public class FormAttribute : Attribute, ITransformHtml
    {
        public bool Ajax { get; set; } 
        public IHtmlString Transform(HtmlHelper htmlHelper, INode node, string html)
        {
            var helperResult = NoodlesHelper.Form(htmlHelper, node);
            if (Ajax)
            {
                return helperResult.Transform(cq => cq.Select("form").AddClass("ajax-form"));
            }
            return helperResult;
        }
    }
}