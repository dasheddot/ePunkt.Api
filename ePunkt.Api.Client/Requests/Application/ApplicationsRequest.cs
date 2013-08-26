using System.Globalization;
using System.Net.Http;

namespace ePunkt.Api.Client.Requests
{
    public class ApplicationsRequest : HttpRequestMessage
    {
        public ApplicationsRequest(int applicantId)
            : base(HttpMethod.Get, "Applications/" + applicantId + "?culture=" + CultureInfo.CurrentUICulture)
        {
        }
    }
}
