using ePunkt.Api;
using ePunkt.Api.Client;
using ePunkt.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ePunkt.Portal.Controllers
{
    public class SyndicationController : ControllerBase
    {
        private readonly LoadJobsService _jobsService;
        private readonly UrlBuilderService _urlBuilder;
        private readonly ReferrerService _referrerService;

        public SyndicationController(ApiHttpClient apiClient, CustomSettings settings, LoadJobsService jobsService, UrlBuilderService urlBuilder, ReferrerService referrerService)
            : base(apiClient, settings)
        {
            _referrerService = referrerService;
            _urlBuilder = urlBuilder;
            _jobsService = jobsService;
        }

        public async Task<ActionResult> Rss(string channel, string referrer, string filter, string jobProfiles, string regions, string format)
        {
            referrer = referrer ?? _referrerService.GetReferrer(Request) ?? "RSS";
            return new RssActionResult {Feed = await GetFeed(channel, referrer, filter, jobProfiles, regions, format)};
        }

        public async Task<ActionResult> Atom(string channel, string referrer, string filter, string jobProfiles, string regions, string format)
        {
            referrer = referrer ?? _referrerService.GetReferrer(Request) ?? "Atom";
            return new AtomActionResult {Feed = await GetFeed(channel, referrer, filter, jobProfiles, regions, format)};
        }

        public async Task<ActionResult> Xml(string channel, string referrer, string filter, string jobProfiles, string regions, string format)
        {
            referrer = referrer ?? _referrerService.GetReferrer(Request) ?? "XML";

            var mandator = await GetMandator();
            var jobs = await GetJobs(channel, filter, jobProfiles, regions);

            var syndicationService = new SyndicationService(mandator, _urlBuilder);

            return new XmlActionResult
                {
                    Xml = syndicationService.BuildXml(jobs, Request.Url, referrer, format.Is("html"))
                };
        }

        private async Task<SyndicationFeed> GetFeed(string channel, string referrer, string filter, string jobProfiles, string regions, string format)
        {
            var mandator = await GetMandator();
            var jobs = await GetJobs(channel, filter, jobProfiles, regions);

            var syndicationService = new SyndicationService(mandator, _urlBuilder);
            return syndicationService.BuildFeed(jobs, Request.Url, referrer, format.Is("html"));
        }

        private async Task<IEnumerable<Job>> GetJobs(string channel, string filter, string jobProfiles, string regions)
        {
            var response = await _jobsService.LoadJobsForChannel(channel);
            return response.Jobs.Filter(filter, jobProfiles, regions).OrderByDescending(x => x.OnlineDateCorrected);
        }
    }
}
