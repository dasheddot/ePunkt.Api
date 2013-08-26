using ePunkt.Api.Responses;
using ePunkt.Portal.Models.Shared;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ePunkt.Portal.Models.Account
{
    public class RegisterViewModel : PersonalInformationViewModel
    {

        public RegisterViewModel Prepare(MandatorResponse mandatorResponse, JobResponse jobResponse)
        {
            if (jobResponse != null)
                JobId = jobResponse.Id;

            AvailableDocumentTypes = mandatorResponse.ApplicantDocumentTypes.Select(x => new SelectListItem { Text = x, Value = x }).ToList();

            base.Prepare(mandatorResponse);

            return this;
        }

        public int? JobId { get; private set; }

        [Required(ErrorMessage = @"Error-Cv")]
        public HttpPostedFileBase Cv { get; set; }
        public HttpPostedFileBase Photo { get; set; }
        public IEnumerable<HttpPostedFileBase> Documents { get; set; }
        public IList<string> DocumentTypes { get; set; }

        public IEnumerable<SelectListItem> AvailableDocumentTypes { get; set; }
    }
}