using System.Reflection;
using System.Web.Mvc;

namespace ePunkt.Portal {
    public class RequireRouteValueAttribute : ActionMethodSelectorAttribute {
        public RequireRouteValueAttribute(string key) {
            Keys = new[] { key };
        }

        public RequireRouteValueAttribute(params string[] keys) {
            Keys = keys;
        }

        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo) {
            foreach (var key in Keys)
                if (!controllerContext.RouteData.Values.ContainsKey(key))
                    return false;
            return true;
        }

        public string[] Keys {
            get;
            private set;
        }
    }

}
