using System.Collections.Generic;
using System.Text;
using System.Web.Routing;
using ePunkt.Utilities;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace ePunkt.Portal
{
    public static class HtmlExtensionMethods
    {
        public static MvcHtmlString TranslatedValidationSummary(this HtmlHelper helper)
        {
            foreach (var modelState in helper.ViewData.ModelState)
            {
                var newErrors = modelState.Value.Errors.Select(x => new ModelError(Loc(helper, x.ErrorMessage).ToString())).ToList();
                modelState.Value.Errors.Clear();
                foreach (var newError in newErrors)
                    modelState.Value.Errors.Add(newError);
            }
            return helper.ValidationSummary();
        }

        public static MvcHtmlString Loc(this HtmlHelper helper, string key, params object[] values)
        {
            var controller = (Controllers.ControllerBase)helper.ViewContext.Controller;
            var view = ((RazorView)helper.ViewContext.View);

            var pathToView = controller.ViewBag.__LocSource ?? view.ViewPath;
            var customPathToView = controller.Settings.CustomFolder + "/" + pathToView.TrimStart('~', '/');

            var resource = GetResourceOrNull(key, pathToView, customPathToView);

            //no resource file found for the View, lets try the Layout page as well
            if (resource == null)
            {
                var pathToLayout = view.LayoutPath;
                pathToLayout = pathToLayout.HasValue() ? pathToLayout : "~/Views/Shared/_Layout.cshtml";
                var customPathToLayout = controller.Settings.CustomFolder + "/" + pathToLayout.TrimStart('~', '/');
                resource = GetResourceOrNull(key, pathToLayout, customPathToLayout);
            }

            if (resource == null)
                throw new Exception("Resource '" + key + "' for view '" + pathToView + "' not found.");

            return new MvcHtmlString(string.Format(resource, values));
        }

        public static MvcHtmlString DropDownList(this HtmlHelper helper, string name, IEnumerable<GroupedSelectListItem> items, object htmlAttributes = null)
        {

            var optionsHtml = new StringBuilder();
            string lastOptGroup = null;
            foreach (var item in items.OrderBy(x => x.Group))
            {
                var currentGroup = item.Group.HasValue() ? item.Group : null;

                if (currentGroup != lastOptGroup)
                {
                    if (lastOptGroup.HasValue())
                        optionsHtml.AppendLine("</optgroup");
                    optionsHtml.AppendLine("<optgroup label=\"" + currentGroup + "\"" + (item.Selected ? " selected=\"selected\"" : "") + ">");
                    lastOptGroup = currentGroup;
                }
                optionsHtml.Append("<option value=\"" + item.Value + "\">" + item.Text + "</option>");
            }

            var tagBuilder = new TagBuilder("select")
            {
                InnerHtml = optionsHtml.ToString()
            };
            tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            tagBuilder.MergeAttribute("name", name, true);
            tagBuilder.GenerateId(name);

            return new MvcHtmlString(tagBuilder.ToString());
        }

        public class GroupedSelectListItem : SelectListItem
        {
            public string Group { get; set; }
        }

        public static MvcHtmlString TlT(this HtmlHelper helper, string originalText)
        {
            var controller = (Controllers.ControllerBase)helper.ViewContext.Controller;
            return new MvcHtmlString(controller.TlT(originalText));
        }

        private static string GetResourceOrNull(string key, string defaultPath, string customPath)
        {
            try
            {
                return HttpContext.GetLocalResourceObject(customPath, key) as string;
            }
            catch
            {
                try
                {
                    return HttpContext.GetLocalResourceObject(defaultPath, key) as string;
                }
                catch
                {
                    return null;
                }
            }
        }

        public static LocSourceSwitcher SwitchLocSource(this HtmlHelper helper, string viewPath)
        {
            var controller = (Controllers.ControllerBase)helper.ViewContext.Controller;
            return new LocSourceSwitcher(controller.ViewBag, viewPath);
        }

        public class LocSourceSwitcher : IDisposable
        {
            private readonly dynamic _viewBag;

            public LocSourceSwitcher(dynamic viewBag, string viewPath)
            {
                _viewBag = viewBag;
                _viewBag.__LocSource = viewPath;
            }

            public void Dispose()
            {
                _viewBag.__LocSource = null;
            }
        }


    }
}