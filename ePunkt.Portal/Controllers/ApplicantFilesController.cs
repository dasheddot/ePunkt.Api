using ePunkt.Api.Client;
using ePunkt.Api.Client.Requests;
using ePunkt.Api.Responses;
using ePunkt.Portal.Models.ApplicantFiles;
using ePunkt.Utilities;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ePunkt.Portal.Controllers
{
    [Authorize]
    public class ApplicantFilesController : ControllerBase
    {
        private readonly UpdateApplicantFileService _updateApplicantFileService;

        public ApplicantFilesController(ApiHttpClient apiClient, CustomSettings settings, UpdateApplicantFileService updateApplicantFileService)
            : base(apiClient, settings)
        {
            _updateApplicantFileService = updateApplicantFileService;
        }

        public async Task<ActionResult> Index()
        {
            var applicant = await GetApplicant();
            if (applicant == null)
                return RedirectToAction("Logoff", "Account");

            var documents = await new ApplicantDocumentsGetRequest(applicant.Id).LoadResult(ApiClient);
            return View(new IndexViewModel(await GetMandator(), documents));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(string type)
        {
            var applicant = await GetApplicant();
            if (applicant != null)
            {
                foreach (var key in Request.Files.AllKeys)
                {
                    var file = Request.Files[key];
                    if (file != null)
                    {

                        if (_updateApplicantFileService.CheckFile(file, type) != UpdateApplicantFileService.CheckFileResult.Ok)
                            if (type.Is("CV"))
                                ModelState.AddModelError("Documents", @"Error-Cv");
                            else if (type.Is("Photo"))
                                ModelState.AddModelError("Documents", @"Error-Photo");
                            else
                                ModelState.AddModelError("Documents", @"Error-Documents");

                        if (!ModelState.IsValid)
                        {
                            var documents = await new ApplicantDocumentsGetRequest(applicant.Id).LoadResult(ApiClient);
                            return View(new IndexViewModel(await GetMandator(), documents));
                        }


                        if (type.Is("CV"))
                            await _updateApplicantFileService.AddCv(ApiClient, applicant, file);
                        else if (type.Is("Photo"))
                            await _updateApplicantFileService.AddPhoto(ApiClient, applicant, file);
                        else
                            await _updateApplicantFileService.AddFile(ApiClient, applicant, file, type);
                    }
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Download(int id)
        {
            var applicant = await GetApplicant();
            if (applicant != null)
            {
                ApplicantDocumentResponse response;
                if (id == -1)
                    response = await new ApplicantCvGetRequest(applicant.Id).LoadResult(ApiClient);
                else if (id == -2)
                    response = await new ApplicantPhotoGetRequest(applicant.Id).LoadResult(ApiClient);
                else
                    response = await new ApplicantDocumentGetRequest(id).LoadResult(ApiClient);

                return new FileContentResult(response.Content, MimeMapping.GetMimeMapping(response.Name + "." + response.FileExtension))
                    {
                        FileDownloadName = response.Name + "." + response.FileExtension
                    };
            }
            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            var applicant = await GetApplicant();
            if (applicant != null)
            {
                if (id == -1)
                    await new ApplicantCvDeleteRequest(applicant.Id).LoadResult(ApiClient);
                else if (id == -2)
                    await new ApplicantPhotoDeleteRequest(applicant.Id).LoadResult(ApiClient);
                else
                    await new ApplicantDocumentDeleteRequest(id).LoadResult(ApiClient);
            }
            return RedirectToAction("Index");
        }
    }
}
