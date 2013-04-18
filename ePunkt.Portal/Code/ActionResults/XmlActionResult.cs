using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace ePunkt.Portal
{
    public class XmlActionResult : ActionResult
    {
        public XElement Xml { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "text/xml";

            using (var writer = XmlWriter.Create(context.HttpContext.Response.Output))
                Xml.WriteTo(writer);
        }
    }
}