using ePunkt.Api.Responses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace ePunkt.Portal.Models.ApplicantActivities
{
    public class IndexViewModel
    {
        public IndexViewModel Fill(IEnumerable<ApplicantActivityResponse> activities)
        {
            Activities = activities.ToList();

            var years = new List<SelectListItem>();
            for (var i = 1960; i <= DateTime.Now.Year; i++)
                years.Add(new SelectListItem
                {
                    Text = i.ToString(CultureInfo.CurrentCulture),
                    Value = i.ToString(CultureInfo.CurrentCulture)
                });

            var months = new List<SelectListItem>();
            for (var i = 1; i <= 12; i++)
                months.Add(new SelectListItem
                {
                    Text = i.ToString(CultureInfo.CurrentCulture),
                    Value = i.ToString(CultureInfo.CurrentCulture)
                });

            var days = new List<SelectListItem>();
            for (var i = 1; i <= 31; i++)
                days.Add(new SelectListItem
                {
                    Text = i.ToString(CultureInfo.CurrentCulture),
                    Value = i.ToString(CultureInfo.CurrentCulture)
                });

            return this;
        }

        public IEnumerable<ApplicantActivityResponse> Activities { get; set; }

        public IEnumerable<SelectListItem> AvailableYears { get; set; }
        public IEnumerable<SelectListItem> AvailableMonths { get; set; }
        public IEnumerable<SelectListItem> AvailableDays { get; set; }

        public string SelectedName { get; set; }
        public int? SelectedStartYear { get; set; }
        public int? SelectedStartMonth { get; set; }
        public int? SelectedStartDay { get; set; }
        public int? SelectedEndYear { get; set; }
        public int? SelectedEndMonth { get; set; }
        public int? SelectedEndDay { get; set; }
    }
}