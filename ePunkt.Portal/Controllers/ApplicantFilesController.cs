using System.IO;
using System.Linq;
using ePunkt.Api.Client;
using ePunkt.Api.Client.Requests;
using ePunkt.Api.Models;
using ePunkt.Portal.Models.ApplicantFiles;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ePunkt.Utilities;

namespace ePunkt.Portal.Controllers
{
    [Authorize]
    public class ApplicantFilesController : ControllerBase
    {
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
            return View(new IndexViewModel(await GetMandator(), applicant));
        }

        private const int MaxFileSize = 1024 * 1024 * 10;
        private readonly string[] _documentExtensions = new[] { "pdf", "doc", "docx" };
        private readonly string[] _imageExtensions = new[] { "jpg", "jpeg", "png" };
        private readonly UpdateApplicantFileService _updateApplicantFileService;

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
                            return View(new IndexViewModel(await GetMandator(), applicant));

                        await _updateApplicantFileService.AddFile(ApiClient, applicant, file, type);
                    }
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Download(string name, string type)
        {
            var applicant = await GetApplicant();
            if (applicant != null)
            {
                var response = await ApiClient.SendAndReadAsync<DocumentContent>(new ApplicantDocumentGetRequest(applicant.Id, name, type));
                return new FileContentResult(response.Content, MimeMapping.GetMimeMapping(response.Name + "." + response.Extension))
                    {
                        FileDownloadName = response.Name + "." + response.Extension
                    };
            }
            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string name, string type)
        {
            var applicant = await GetApplicant();
            if (applicant != null)
                await ApiClient.SendAndReadAsync<string>(new ApplicantDocumentDeleteRequest(applicant.Id, name, type));
            return RedirectToAction("Index");
        }
    }
}
