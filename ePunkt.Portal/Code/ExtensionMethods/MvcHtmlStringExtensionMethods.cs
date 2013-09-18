using ePunkt.Utilities;
using System.Web.Mvc;

namespace ePunkt.Portal
{
    public static class MvcHtmlStringExtensionMethods
    {
        public static MvcHtmlString MakeAlertDanger(this MvcHtmlString s)
        {
            if (s.ToString().HasValue() && !IsEmptyValidationBlock(s))
                return new MvcHtmlString("<div class=\"alert alert-danger\">" + s + "</div>");
            return s;
        }

        private static bool IsEmptyValidationBlock(MvcHtmlString s)
        {
            return s.ToString().Is("<div class=\"validation-summary-valid\" data-valmsg-summary=\"true\"><ul><li style=\"display:none\"></li>\r\n</ul></div>");
        }
    }
}