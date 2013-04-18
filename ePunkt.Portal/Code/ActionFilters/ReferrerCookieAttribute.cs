using ePunkt.Utilities;
using System.Web.Mvc;

namespace ePunkt.Portal
{
    public class ReferrerCookieAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var referrer = filterContext.HttpContext.Request.QueryString["referrer"] ?? filterContext.HttpContext.Request.QueryString["Referrer"];
            if (referrer.HasValue())
            {
                var referrerService = new ReferrerService();
                referrerService.SetReferrer(filterContext.HttpContext.Response, referrer);
            }
        }
    }
}
