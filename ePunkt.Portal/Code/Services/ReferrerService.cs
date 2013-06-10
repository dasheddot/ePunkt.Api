using ePunkt.Utilities;
using System;
using System.Web;

namespace ePunkt.Portal
{
    public class ReferrerService
    {
        private const string CookieKey = "r";

        public void SetReferrer(HttpResponseBase response, string referrer)
        {
            if (referrer.HasValue())
            {
                var cookie = new HttpCookie(CookieKey, referrer) {Expires = DateTime.Now.AddDays(14)};
                response.Cookies.Add(cookie);
            }
        }

        public string GetReferrer(HttpRequestBase request)
        {
            var cookie = request.Cookies[CookieKey];
            if (cookie != null && cookie.Value.HasValue())
                return cookie.Value;
            return null;
        }
    }
}