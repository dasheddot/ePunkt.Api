using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace ePunkt.Portal
{
    public class RequireRouteValueAttribute : ActionMethodSelectorAttribute
    {
        public RequireRouteValueAttribute(string key)
        {
            Keys = new[] { key };
        }

        public RequireRouteValueAttribute(params string[] keys)
        {
            Keys = keys;
        }

        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            return Keys.All(key => controllerContext.RouteData.Values.ContainsKey(key));
        }

        public string[] Keys
        {
            get;
            private set;
        }
    }

}
