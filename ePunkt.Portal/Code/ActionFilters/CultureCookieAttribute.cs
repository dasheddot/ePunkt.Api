using ePunkt.Utilities;
using System;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace ePunkt.Portal
{
    public class CultureCookieAttribute : ActionFilterAttribute
    {
        private const string CookieKey = "c";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var culture = "";

            var cultureInQueryString = filterContext.HttpContext.Request.QueryString["culture"] ?? filterContext.HttpContext.Request.QueryString["Culture"];
            if (cultureInQueryString.HasValue())
            {
                culture = cultureInQueryString;
            }

            if (culture.IsNoE() && filterContext.HttpContext.Request.Cookies[CookieKey] != null)
                culture = filterContext.HttpContext.Request.Cookies[CookieKey].Value;

            if (culture.HasValue())
            {
                try
                {
                    Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);

                    var cookie = new HttpCookie(CookieKey, culture) { Expires = DateTime.Now.AddYears(1) };
                    filterContext.HttpContext.Response.Cookies.Add(cookie);
                }
                catch (CultureNotFoundException)
                {
                    var cookie = filterContext.HttpContext.Response.Cookies[CookieKey];
                    if (cookie != null)
                        cookie.Expires = DateTime.Now.AddYears(-1);
                }
            }
        }
    }
}
