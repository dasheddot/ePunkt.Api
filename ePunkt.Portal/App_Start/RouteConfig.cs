using System.Web.Mvc;
using System.Web.Routing;

namespace ePunkt.Portal
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            #region Legacy Routes
            routes.MapRoute(
               "Legacy_Images",
               "Resources/Image/{*id}",
               new { controller = "Content", action = "File" }
            );
            routes.MapRoute(
               "Legacy_JobSalaryImage",
               "Resources/JobSalaryImage/{id}",
               new { controller = "Jobs", action = "SalaryImage" }
            );
            #endregion

            routes.MapRoute(
                "Favicon",
                "favicon.ico",
                new { controller = "Content", action = "File", id = "favicon.ico" }
                );

            routes.MapRoute(
               "Content",
               "Content/{*id}",
               new { controller = "Content", action = "File" }
               );

            routes.MapRoute(
                "Job Detail Page",
                "Job/{id}",
                new { controller = "Jobs", action = "Job" }
                );

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                );
        }
    }
}