using System.Web.Mvc;

namespace ePunkt.Portal
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CultureCookieAttribute());
            filters.Add(new ReferrerCookieAttribute());
        }
    }
}