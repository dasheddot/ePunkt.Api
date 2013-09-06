using ePunkt.Api.Responses;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;

namespace ePunkt.Portal.Models.ApplicantActivities
{
    public class EditViewModel
    {
        public EditViewModel Prepare()
        {
            var years = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = @"-",
                    Value = "0"
                }
            };
            for (var i = 1960; i <= DateTime.Now.Year; i++)
                years.Add(new SelectListItem
                {
                    Text = i.ToString(CultureInfo.CurrentCulture),
                    Value = i.ToString(CultureInfo.CurrentCulture)
                });
            AvailableYears = years;

            var months = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = @"-",
                    Value = "0"
                }
            };
            for (var i = 1; i <= 12; i++)
                months.Add(new SelectListItem
                {
                    Text = new DateTime(2000, i, 1).ToString("MMMM"),
                    Value = i.ToString(CultureInfo.CurrentCulture)
                });
            AvailableMonths = months;

            var days = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = @"-",
                    Value = "0"
                }
            };
            for (var i = 1; i <= 31; i++)
                days.Add(new SelectListItem
                {
                    Text = i.ToString(CultureInfo.CurrentCulture).PadLeft(2, '0'),
                    Value = i.ToString(CultureInfo.CurrentCulture)
                });
            AvailableDays = days;

            return this;
        }

        public EditViewModel Fill([NotNull] ApplicantActivityResponse activity)
        {
            Id = activity.Id;
            Name = activity.Name;
            StartYear = activity.Start.Year ?? 0;
            StartMonth = activity.Start.Month ?? 0;
            StartDay = activity.Start.Day ?? 0;
            EndYear = activity.End.Year ?? 0;
            EndMonth = activity.End.Month ?? 0;
            EndDay = activity.End.Day ?? 0;

            return this;
        }

        public IEnumerable<SelectListItem> AvailableYears { get; set; }
        public IEnumerable<SelectListItem> AvailableMonths { get; set; }
        public IEnumerable<SelectListItem> AvailableDays { get; set; }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public int StartYear { get; set; }
        public int StartMonth { get; set; }
        public int StartDay { get; set; }
        public int EndYear { get; set; }
        public int EndMonth { get; set; }
        public int EndDay { get; set; }
    }
}