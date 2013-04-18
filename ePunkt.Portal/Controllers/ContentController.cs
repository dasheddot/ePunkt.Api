using System.Text;
using System.Web;
using System.Web.UI;
using ePunkt.Api.Client;
using System.IO;
using System.Web.Mvc;
using ePunkt.Utilities;

namespace ePunkt.Portal.Controllers
{
    public class ContentController : ControllerBase
    {
        public ContentController(ApiHttpClient apiClient, CustomSettings settings)
            : base(apiClient, settings)
        {
        }

        [OutputCache(Duration = 86400, VaryByParam = "id", Location = OutputCacheLocation.Client)]
        public ActionResult File(string id)
        {
            var path = Path.Combine(Server.MapPath(Settings.CustomFolder), "Content", id);
            var mimeType = MimeMapping.GetMimeMapping(path);

            if (System.IO.File.Exists(path))
                return new FilePathResult(path, mimeType);

            if (mimeType.Is("text/css"))
                return new ContentResult
                    {
                        Content = "",
                        ContentEncoding = Encoding.Unicode,
                        ContentType = mimeType
                    };

            return HttpNotFound();
        }
    }
}
