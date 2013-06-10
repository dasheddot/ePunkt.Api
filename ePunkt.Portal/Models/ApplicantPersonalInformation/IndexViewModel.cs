using ePunkt.Api.Models;
using ePunkt.Utilities;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ePunkt.Portal.Models.ApplicantPersonalInformation
{
    public class IndexViewModel
    {
        public IndexViewModel(Mandator mandator, Api.Models.Applicant applicant)
        {
            Applicant = applicant;
            TitlesBeforeName = FilterTitles(mandator.TitlesBeforeName).Select(x => new HtmlExtensionMethods.GroupedSelectListItem
                {
                    Value = x.Name,
                    Text = Translations.TlT(mandator, x.Name),
                    Selected = applicant.TitleBeforeName.Is(x.Name),
                    Group = x.Group
                });
            TitlesAfterName = FilterTitles(mandator.TitlesAfterName).Select(x => new HtmlExtensionMethods.GroupedSelectListItem
            {
                Value = x.Name,
                Text = Translations.TlT(mandator, x.Name),
                Selected = applicant.TitleAfterName.Is(x.Name),
                Group = x.Group
            });
        }

        private IEnumerable<Title> FilterTitles(IEnumerable<Title> titles)
        {
            var currentCulture = CultureInfo.CurrentCulture;
            return titles.Where(x => !x.CultureFilter.Any() || x.CultureFilter.Any(y => currentCulture.ToString().Is(y) || currentCulture.ToString().Substring(2).Is(y)));
        }

        public Api.Models.Applicant Applicant { get; set; }
        public IEnumerable<HtmlExtensionMethods.GroupedSelectListItem> TitlesBeforeName { get; set; }
        public IEnumerable<HtmlExtensionMethods.GroupedSelectListItem> TitlesAfterName { get; set; }
    }
}