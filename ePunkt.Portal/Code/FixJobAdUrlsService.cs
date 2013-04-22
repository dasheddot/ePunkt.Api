using ePunkt.Api.Models;
using ePunkt.Utilities;
using System;
using System.Text.RegularExpressions;

namespace ePunkt.Portal
{
    /// <summary>
    /// Replaces certain URLs in Job-Ads to automatically point to the current request URL.
    /// </summary>
    public class FixJobAdUrlsService
    {
        private readonly Mandator _mandator;

        public FixJobAdUrlsService(Mandator mandator)
        {
            _mandator = mandator;
        }

        public string InjectReferrer(string html, int jobId, string referrerToInject)
        {
            return html.Replace("Job=" + jobId, "referrer=" + referrerToInject + "&Job=" + jobId);
        }

        public string ReplaceUrlsWithCurrent(string html, Uri currentRequestUri)
        {
            foreach (var url in _mandator.Urls)
                html = Replace(html, url, currentRequestUri);
            return html;
        }

        private string Replace(string html, string urlToReplace, Uri currentRequestUri)
        {
            if (urlToReplace.IsNoW())
                return html;

            var relativeUrlsToLookFor = new[] { "/\\?Job=", "/Resources/JobSalaryImage"};

            foreach (var relativeUrl in relativeUrlsToLookFor)
            {
                urlToReplace = urlToReplace.Replace("http://", "").Replace("https://", "");
                urlToReplace = urlToReplace.Trim('/');

                var oldValue = urlToReplace + relativeUrl;
                var newValue = (currentRequestUri.Authority + relativeUrl).Replace("\\", "");

                html = Regex.Replace(html, "http://" + oldValue, "http://" + newValue, RegexOptions.IgnoreCase);
                html = Regex.Replace(html, "https://" + oldValue, "https://" + newValue, RegexOptions.IgnoreCase);
            }

            return html;
        }
    }
}