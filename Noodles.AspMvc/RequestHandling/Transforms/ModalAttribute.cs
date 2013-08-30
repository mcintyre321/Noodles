using System.Web.Mvc;
using CsQuery;
using Noodles.Models;

namespace Noodles.AspMvc.RequestHandling.Transforms
{
    public class ModalAttribute : ChildNodeDocumentTransformAttribute
    {
        public override void Transform(CQ elements, INode child, ControllerContext cc, INode parent)
        {
            elements.Each(o =>
            {
                var uri = elements.Attr("href");
                if (!uri.Contains("?")) uri += "?";
                uri += "fragment-selector=.node-container";
                //link.Attr("href", uri);
                elements.Data("toggle", "modal").Data("target", "#noodles-modal").Data("remote", uri);

                if (elements.Document.QuerySelector("#noodles-modal") == null)
                {
                    CQ.Create(
                        "<div class=\"modal hide\" id=\"noodles-modal\"><div class=\"modal-header\"><a class=\"close\" data-dismiss=\"modal\">×</a><h3 class=\"title\">&nbsp;</h3></div><div class=\"modal-body\"></div><div class=\"modal-footer\"></div></div>")
                      .AppendTo(elements.Document.Body);

                }
            });
        }
    }
    public class DropdownAttribute : ChildNodeDocumentTransformAttribute
    {
        public override void Transform(CQ elements, INode child, ControllerContext cc, INode parent)
        {
            var link = elements.Find("> a");
            if (link != null)
            {
                var uri = link.Attr("href");
                if (!uri.Contains("?")) uri += "?";
                uri += "fragment-selector=.node-container";
                //link.Attr("href", uri);
                link.Data("toggle", "modal").Data("target", "#noodles-modal").Data("remote", uri);

                if (elements.Document.QuerySelector("#noodles-modal") == null)
                {
                    CQ.Create(
                        "<div class=\"modal hide\" id=\"noodles-modal\"><div class=\"modal-header\"><a class=\"close\" data-dismiss=\"modal\">×</a><h3 class=\"title\">&nbsp;</h3></div><div class=\"modal-body\"></div><div class=\"modal-footer\"></div></div>")
                      .AppendTo(elements.Document.Body);

                }
            }
        }
    }
}