using ePunkt.Api.Responses;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;

namespace ePunkt.Api.Client.Requests
{
    public class ApplicationsRequest : HttpRequestMessage<IEnumerable<ApplicationResponse>>
    {
        public ApplicationsRequest(int applicantId)
            : base(HttpMethod.Get, "Applications/" + applicantId + "?culture=" + CultureInfo.CurrentUICulture)
        {
        }
    }
}
