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
        public ApplicantFilesController(ApiHttpClient apiClient, CustomSettings settings)
            : base(apiClient, settings)
        {
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
                        var extension = (Path.GetExtension(file.FileName) ?? "").ToLower().Trim('.');
                        if (type.Is("Cv") && (file.ContentLength > MaxFileSize || !_documentExtensions.Contains(extension)))
                            ModelState.AddModelError("upload", @"Error-Cv");
                        else if (type.Is("Photo") && (file.ContentLength > MaxFileSize || !_imageExtensions.Contains(extension)))
                            ModelState.AddModelError("upload", @"Error-Cv");
                        else if ((file.ContentLength > MaxFileSize || !_documentExtensions.Union(_imageExtensions).Contains(extension)))
                            ModelState.AddModelError("upload", @"Error-Cv");

                        if (!ModelState.IsValid)
                            return View(new IndexViewModel(await GetMandator(), applicant));

                        using (var reader = new BinaryReader(file.InputStream))
                        {
                            var document = new DocumentContent
                                {
                                    Content = reader.ReadBytes(file.ContentLength),
                                    Extension = (Path.GetExtension(file.FileName) ?? "").Trim('.'),
                                    Name = Path.GetFileNameWithoutExtension(file.FileName),
                                    Type = type
                                };
                            await ApiClient.SendAndReadAsync<string>(new ApplicantDocumentPostRequest(applicant.Id, document));
                        }
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
