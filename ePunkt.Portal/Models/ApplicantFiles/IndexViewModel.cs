using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ePunkt.Portal.Models.ApplicantFiles
{
    public class IndexViewModel
    {
        public IndexViewModel(Api.Models.Mandator mandator, Api.Models.Applicant applicant)
        {
            Applicant = applicant;
            AvailableDocumentTypes = mandator.ApplicantDocumentTypes.Select(x => new SelectListItem
                {
                    Value = x,
                    Text = Translations.TlT(mandator, x)
                });
        }

        public Api.Models.Applicant Applicant { get; set; }
        public IEnumerable<SelectListItem> AvailableDocumentTypes { get; set; }
    }
}