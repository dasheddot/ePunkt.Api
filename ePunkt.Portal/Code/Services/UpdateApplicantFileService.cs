using ePunkt.Api.Client;
using ePunkt.Api.Client.Requests;
using ePunkt.Api.Responses;
using ePunkt.Utilities;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ePunkt.Portal
{
    public class UpdateApplicantFileService
    {
        private const int MaxFileSize = 1024 * 1024 * 10;
        private readonly string[] _documentExtensions = { "pdf", "doc", "docx" };
        private readonly string[] _imageExtensions = { "jpg", "jpeg", "png" };

        public enum CheckFileResult
        {
            InvalidExtension,
            InvalidSize,
            Ok
        }

        public CheckFileResult CheckFile(HttpPostedFileBase file, string type)
        {
            var extension = (Path.GetExtension(file.FileName) ?? "").ToLower().Trim('.');

            if (file.ContentLength > MaxFileSize)
                return CheckFileResult.InvalidSize;
            if (type.Is("CV") && !_documentExtensions.Contains(extension))
                return CheckFileResult.InvalidExtension;
            if (type.Is("Photo") && !_imageExtensions.Contains(extension))
                return CheckFileResult.InvalidExtension;
            if (!_documentExtensions.Union(_imageExtensions).Contains(extension))
                return CheckFileResult.InvalidExtension;
            return CheckFileResult.Ok;
        }

        public async Task AddFile(ApiHttpClient apiClient, ApplicantResponse applicantResponse, HttpPostedFileBase file, string type)
        {
            using (var reader = new BinaryReader(file.InputStream))
            {
                var document = new ApplicantDocumentResponse
                    {
                        Content = reader.ReadBytes(file.ContentLength),
                        FileExtension = (Path.GetExtension(file.FileName) ?? "").Trim('.'),
                        Name = Path.GetFileNameWithoutExtension(file.FileName),
                        Type = type
                    };
                await new ApplicantDocumentPostRequest(applicantResponse.Id, document).LoadResult(apiClient);
            }
        }

    }
}