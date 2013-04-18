using System.Web.Mvc;

namespace ePunkt.Portal
{
    public class CustomViewEngine : RazorViewEngine
    {
        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            var controller = (Controllers.ControllerBase)controllerContext.Controller;

            PartialViewLocationFormats = new[]
                {
                    controller.Settings.CustomFolder + "/Views/{1}/{0}.cshtml",
                    controller.Settings.CustomFolder + "/Views/Shared/{0}.cshtml",
                    "~/Views/{1}/{0}.cshtml",
                    "~/Views/Shared/{0}.cshtml"
                };

            return base.FindPartialView(controllerContext, partialViewName, false);
        }


        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            var controller = (Controllers.ControllerBase)controllerContext.Controller;

            ViewLocationFormats = new[]
                {
                    controller.Settings.CustomFolder + "/Views/{1}/{0}.cshtml",
                    "~/Views/{1}/{0}.cshtml"
                };
            MasterLocationFormats = new[]
                {
                    controller.Settings.CustomFolder + "/Views/Shared/_{0}.cshtml",
                    "~/Views/Shared/_{0}.cshtml"
                };

            return base.FindView(controllerContext, viewName, null, false);
        }

    }
}
