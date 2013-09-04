using System.Linq;
using System.Web.Mvc;
using ePunkt.Api.Responses;
using System.Collections.Generic;
using ePunkt.Utilities;

namespace ePunkt.Portal.Models.ApplicantJobProfiles
{
    public class IndexViewModel
    {
        public IndexViewModel Fill(MandatorResponse mandatorResponse, IEnumerable<string> jobProfiles)
        {
            JobProfiles = jobProfiles.OrderBy(x => x).ToList();

            AvailableJobProfiles = (from x in mandatorResponse.JobProfiles
                                    where !JobProfiles.Any(y => y.Is(x))
                                    orderby x
                                    select new SelectListItem
                                    {
                                        Text = x,
                                        Value = x
                                    }).ToList();
            return this;
        }

        public IEnumerable<string> JobProfiles { get; set; }
        public IEnumerable<SelectListItem> AvailableJobProfiles { get; set; }

        public string SelectedJobProfile { get; set; }
    }
}