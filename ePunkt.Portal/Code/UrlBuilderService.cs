using ePunkt.Utilities;
using System;

namespace ePunkt.Portal
{
    public class UrlBuilderService
    {
        public string GetAbsoluteSignUpUrl(int jobId, Uri currentRequestUri, string referrer)
        {
            var result = currentRequestUri.Scheme + "://" + currentRequestUri.Authority + "/?Job=" + jobId;
            if (referrer.HasValue())
                result += "&referrer=" + referrer;
            return result;
        }

        public string GetAbsoluteSignUpUrl(Uri currentRequestUri, string referrer)
        {
            var result = currentRequestUri.Scheme + "://" + currentRequestUri.Authority + "/";
            if (referrer.HasValue())
                result += "?referrer=" + referrer;
            return result;
        }

        public string GetAbsolutJobUrl(int jobId, Uri currentRequestUri, string referrer)
        {
            var result = currentRequestUri.Scheme + "://" + currentRequestUri.Authority + "/Job/" + jobId;
            if (referrer.HasValue())
                result += "?referrer=" + referrer;
            return result;
        }
    }
}