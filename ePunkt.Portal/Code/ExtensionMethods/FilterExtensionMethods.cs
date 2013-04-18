using ePunkt.Api;
using ePunkt.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace ePunkt.Portal
{
    public static class FilterExtensionMethods
    {
        public static IEnumerable<Job> FilterByText(this IEnumerable<Job> jobs, string searchPhrase)
        {
            return jobs.Where(x => HtmlUtility.ConvertHtmlToText(x.Html).ToLower().Contains(searchPhrase.ToLower()));
        }

        public static IEnumerable<Job> FilterByJobProfiles(this IEnumerable<Job> jobs, params string[] jobProfiles)
        {
            return jobs.Where(x => x.JobProfiles.Any(y => jobProfiles.Any(z => z.Is(y))));
        }

        public static IEnumerable<Job> FilterByRegions(this IEnumerable<Job> jobs, params string[] regions)
        {
            return jobs.Where(x => x.Regions.Any(y => regions.Any(z => z.Is(y))));
        }

        public static IEnumerable<Job> Filter(this IEnumerable<Job> jobs, string filter, string jobProfiles, string regions)
        {
            if (filter.HasValue())
                jobs = jobs.FilterByText(filter);
            var jobProfilesFilter = (jobProfiles ?? "").Split('|').Select(x => x.Trim()).Where(x => x.HasValue()).ToArray();
            if (jobProfilesFilter.Any())
                jobs = jobs.FilterByJobProfiles(jobProfilesFilter);
            var regionsFilter = (regions ?? "").Split('|').Select(x => x.Trim()).Where(x => x.HasValue()).ToArray();
            if (regionsFilter.Any())
                jobs = jobs.FilterByRegions(regionsFilter);
            return jobs;
        }
    }
}