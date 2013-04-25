using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using ePunkt.Api.Models;
using ePunkt.Utilities;

namespace ePunkt.Portal.Models.ApplicantPersonalInformation
{
    public class IndexViewModel
    {
        public IndexViewModel(Api.Models.Mandator mandator, Api.Models.Applicant applicant)
        {
            Applicant = applicant;
            TitlesBeforeName = FilterTitles(mandator.TitlesBeforeName).Select(x => new SelectListItem
                {
                    Value = x.Name,
                    Text = Translations.TlT(mandator, x.Name),
                    Selected = applicant.TitleBeforeName.Is(x.Name)
                });
            TitlesAfterName = FilterTitles(mandator.TitlesAfterName).Select(x => new SelectListItem
            {
                Value = x.Name,
                Text = Translations.TlT(mandator, x.Name),
                Selected = applicant.TitleAfterName.Is(x.Name)
            });
        }

        private IEnumerable<Title> FilterTitles(IEnumerable<Title> titles)
        {
            var currentCulture = CultureInfo.CurrentCulture;
            return titles.Where(x => !x.CultureFilter.Any() || x.CultureFilter.Any(y => currentCulture.ToString().Is(y) || currentCulture.ToString().Substring(2).Is(y)));
        }

        public Api.Models.Applicant Applicant { get; set; }
        public IEnumerable<SelectListItem> TitlesBeforeName { get; set; }
        public IEnumerable<SelectListItem> TitlesAfterName { get; set; }
    }
}