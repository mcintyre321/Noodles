using System.Web.Mvc;
using CsQuery;
using Noodles.Models;

namespace Noodles.AspMvc.RequestHandling.Transforms
{
    public class ModalAttribute: NodeLinkDocumentTransformAttribute
    {
        public override void Transform(CQ element, NodeLink nodeLink, ControllerContext cc, INode resource)
        {
            var link = element.Find("> a");
            var uri = link.Attr("href");
            if (!uri.Contains("?")) uri += "?";
            uri += "fragment-selector=.node-container";
            link.Attr("href", uri);
            link.Data("toggle", "modal").Data("target", "#noodles-modal");
            
            if (element.Document.QuerySelector("#noodles-modal") == null)
            {
                CQ.Create("<div class=\"modal hide\" id=\"noodles-modal\"><div class=\"modal-header\"><a class=\"close\" data-dismiss=\"modal\">×</a><h3 class=\"title\">&nbsp;</h3></div><div class=\"modal-body\"></div><div class=\"modal-footer\"></div></div>")
                  .AppendTo(element.Document.Body);

            }
        }
    }
}