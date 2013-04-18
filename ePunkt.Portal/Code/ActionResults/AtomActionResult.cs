using System.ServiceModel.Syndication;
using System.Web.Mvc;
using System.Xml;

namespace ePunkt.Portal
{
    public class AtomActionResult : ActionResult
    {
        public SyndicationFeed Feed { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "application/atom+xml";

            var formatter = new Atom10FeedFormatter(Feed);
            using (var writer = XmlWriter.Create(context.HttpContext.Response.Output))
                formatter.WriteTo(writer);
        }
    }
}