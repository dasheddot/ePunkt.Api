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
               name: "Legacy_Images",
               url: "Resources/Image/{*id}",
               defaults: new { controller = "Content", action = "File" }
            );
            routes.MapRoute(
               name: "Legacy_JobSalaryImage",
               url: "Resources/JobSalaryImage/{id}",
               defaults: new { controller = "Jobs", action = "SalaryImage" }
            );
            #endregion

            routes.MapRoute(
                name: "Favicon",
                url: "favicon.ico",
                defaults: new { controller = "Content", action = "File", id = "favicon.ico" }
                );

            routes.MapRoute(
               name: "Content",
               url: "Content/{*id}",
               defaults: new { controller = "Content", action = "File" }
               );

            routes.MapRoute(
                name: "Job Detail Page",
                url: "Job/{id}",
                defaults: new { controller = "Jobs", action = "Job" }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                );
        }
    }
}