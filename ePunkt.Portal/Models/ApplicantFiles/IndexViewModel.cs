using ePunkt.Api.Responses;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ePunkt.Portal.Models.ApplicantFiles
{
    public class IndexViewModel
    {
        public IndexViewModel(MandatorResponse mandatorResponse, IEnumerable<ApplicantDocumentResponse> documents)
        {
            AvailableDocumentTypes = mandatorResponse.ApplicantDocumentTypes.Select(x => new SelectListItem
            {
                Value = x,
                Text = Translations.TlT(mandatorResponse, x)
            });

            var documentsAsList = documents.ToList();
            HasCv = documentsAsList.Any(x => x.Id == -1);
            HasPhoto = documentsAsList.Any(x => x.Id == -2);
            Documents = documentsAsList.Where(x => x.Id > 0).ToList();
        }

        public bool HasCv { get; set; }
        public bool HasPhoto { get; set; }
        public IEnumerable<ApplicantDocumentResponse> Documents { get; set; } 
        public IEnumerable<SelectListItem> AvailableDocumentTypes { get; set; }
    }
}