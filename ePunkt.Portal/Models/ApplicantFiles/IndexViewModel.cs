using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ePunkt.Api.Responses;

namespace ePunkt.Portal.Models.ApplicantFiles
{
    public class IndexViewModel
    {
        public IndexViewModel(MandatorResponse mandatorResponse, ApplicantResponse applicantResponse)
        {
            ApplicantResponse = applicantResponse;
            AvailableDocumentTypes = mandatorResponse.ApplicantDocumentTypes.Select(x => new SelectListItem
                {
                    Value = x,
                    Text = Translations.TlT(mandatorResponse, x)
                });
        }

        public ApplicantResponse ApplicantResponse { get; set; }
        public IEnumerable<SelectListItem> AvailableDocumentTypes { get; set; }
    }
}