using System.Collections.Generic;

namespace Noodles.AspMvc.Models.Layout
{
    public class FooterVm
    {
        public FooterVm()
        {
            HtmlColumns = new List<string>();
        }

        public IList<string> HtmlColumns { get; set; }
    }
}