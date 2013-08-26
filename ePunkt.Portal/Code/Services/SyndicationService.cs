using ePunkt.Api.Responses;
using ePunkt.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml.Linq;

namespace ePunkt.Portal
{
    public class SyndicationService
    {
        private readonly MandatorResponse _mandatorResponse;
        private readonly UrlBuilderService _urlBuilder;


        public SyndicationService(MandatorResponse mandatorResponse, UrlBuilderService urlBuilder)
        {
            _urlBuilder = urlBuilder;
            _mandatorResponse = mandatorResponse;
        }

        public XElement BuildXml(IEnumerable<JobResponse> jobs, Uri requestUri, string referrer, bool formatIsHtml)
        {
            var xml = new XElement("jobs");
            var jobsCount = 0;
            foreach (var job in jobs)
            {
                jobsCount++;
                var jobXml = new XElement("job");
                jobXml.Add(new XElement("id", job.Id));
                jobXml.Add(new XElement("name", job.Title ?? ""));
                jobXml.Add(new XElement("subTitle", job.SubTitle ?? ""));
                jobXml.Add(new XElement("companyDescription", new XCData(job.CompanyDescription)));
                jobXml.Add(new XElement("location", job.Location ?? ""));
                jobXml.Add(new XElement("description", new XCData(job.Block1 ?? "")));
                jobXml.Add(new XElement("requirements", new XCData(job.Block2 ?? "")));
                jobXml.Add(new XElement("additionalText", new XCData(job.Block3 ?? "")));

                if (_mandatorResponse.PortalSettings.EnableFourthJobBlockInXmlFeed)
                    jobXml.Add(new XElement("footer", new XCData(job.Block4 ?? "")));

                jobXml.Add(new XElement("responsibleUser", job.UserFullName));
                jobXml.Add(new XElement("responsibleEmail", job.UserEmail));
                jobXml.Add(new XElement("responsibleGender", job.UserGender.HasValue && job.UserGender.Value ? "1" : "0"));

                foreach (var jobProfile in job.JobProfiles)
                    jobXml.Add(new XElement("jobProfile", jobProfile));

                jobXml.Add(new XElement("url", _urlBuilder.GetAbsolutJobUrl(job.Id, requestUri, referrer)));
                jobXml.Add(new XElement("applyUrl", _urlBuilder.GetAbsoluteSignUpUrl(job.Id, requestUri, referrer)));
                jobXml.Add(new XElement("content", new XCData(BuildContent(job, requestUri, referrer, formatIsHtml))));

                if (_mandatorResponse.PortalSettings.EnableExtendedXml)
                {
                    jobXml.Add(new XElement("dateOfCreation", job.CreationDate));
                    jobXml.Add(new XElement("dateOfUpdate", job.UpdateDate));
                    jobXml.Add(new XElement("dateSinceOnline", job.OnlineDate));
                    jobXml.Add(new XElement("dateSinceOnlineCorrected", job.OnlineDateCorrected));
                    jobXml.Add(new XElement("occupationType", job.OccupationTypes));
                    jobXml.Add(new XElement("careerLevel", job.CareerLevels));
                    jobXml.Add(from x in job.Regions select new XElement("region", x));
                    jobXml.Add(from x in job.PublishedOn select new XElement("publishedOn", x));
                }
                else
                {
                    jobXml.Add(new XElement("dateSinceOnline", job.OnlineDateCorrected));
                }

                foreach (var customField in job.CustomFields.Where(x => x.IsPublic))
                {
                    var value = customField.BoolValue.HasValue ? customField.BoolValue.Value.ToString() : "";
                    if (value.IsNoE())
                        value = customField.DateValue.HasValue ? customField.DateValue.Value.ToString(CultureInfo.CurrentCulture) : "";
                    if (value.IsNoE())
                        value = customField.NumberValue.HasValue ? customField.NumberValue.Value.ToString(CultureInfo.CurrentCulture) : "";
                    if (value.IsNoE())
                        value = customField.StringValue;

                    jobXml.Add(new XElement(XmlUtility.EnsureValidTagName(customField.InternalName ?? Guid.NewGuid().ToString()), new XAttribute("name", customField.Name), value));
                }

                if (_mandatorResponse.PortalSettings.EnableJobTagsInXmlFeed)
                {
                    foreach (var tag in job.Tags)
                        jobXml.Add(new XElement("tag", tag));
                }

                xml.Add(jobXml);
            }


            xml.AddFirst(new XComment(jobsCount + " jobs."));
            return xml;
        }

        public SyndicationFeed BuildFeed(IEnumerable<JobResponse> jobs, Uri requestUri, string referrer, bool formatIsHtml)
        {
            var feed = new SyndicationFeed
                {
                    Title = new TextSyndicationContent(_mandatorResponse.Name)
                };
            var items = new List<SyndicationItem>();

            foreach (var job in jobs)
            {
                var url = _urlBuilder.GetAbsolutJobUrl(job.Id, requestUri, referrer);

                var item = new SyndicationItem
                    {
                        Title = SyndicationContent.CreatePlaintextContent(job.Title),
                        Content = SyndicationContent.CreateHtmlContent(BuildContent(job, requestUri, referrer, formatIsHtml))
                    };
                item.Authors.Add(new SyndicationPerson(job.UserEmail, job.UserFullName, null));
                item.Id = url;
                item.AddPermalink(new Uri(url));
                item.LastUpdatedTime = job.OnlineDateCorrected;
                items.Add(item);
            }

            feed.Items = items;
            return feed;
        }

        public string BuildContent(JobResponse jobResponse, Uri requestUri, string referrer, bool formatIsHtml)
        {
            var result = "";
            if (formatIsHtml)
            {
                var fixUrlsService = new FixJobAdUrlsService(_mandatorResponse);
                result = fixUrlsService.ReplaceUrlsWithCurrent(jobResponse.Html, requestUri);
                result = fixUrlsService.InjectReferrer(result, jobResponse.Id, referrer);
            }
            else
                result += "<p>" + jobResponse.CompanyDescription + "</p>" +
                          "<p><b>" + jobResponse.Title + "</b><br />" +
                          jobResponse.SubTitle + "<br />" +
                          jobResponse.Location + "</p>" +
                          "<p>" + jobResponse.Block1 + "</p>" +
                          "<p>" + jobResponse.Block2 + "</p>" +
                          "<p>" + jobResponse.Block3 + "</p>" +
                          "<p><a href=\"" + _urlBuilder.GetAbsolutJobUrl(jobResponse.Id, requestUri, referrer) + "\">" + _urlBuilder.GetAbsolutJobUrl(jobResponse.Id, requestUri, referrer) + "</a></p>";

            return result;
        }
    }
}