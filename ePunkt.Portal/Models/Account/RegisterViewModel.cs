using System.Linq;
using System.Web.Mvc;
using ePunkt.Api.Models;
using ePunkt.Portal.Models.Shared;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace ePunkt.Portal.Models.Account
{
    public class RegisterViewModel : PersonalInformationViewModel
    {

        public RegisterViewModel Prepare(Mandator mandator, Job job)
        {
            if (job != null)
                JobId = job.Id;

            AvailableDocumentTypes = mandator.ApplicantDocumentTypes.Select(x => new SelectListItem { Text = x, Value = x }).ToList();

            base.Prepare(mandator);

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