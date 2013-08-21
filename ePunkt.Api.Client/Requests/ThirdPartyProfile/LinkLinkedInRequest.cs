using System;
using System.Net.Http;

namespace ePunkt.Api.Client.Requests
{
    public class LinkLinkedInRequest : HttpRequestMessage
    {
        public LinkLinkedInRequest(int id, string thirdPartyIdentifier, Uri profileUrl)
            : base(HttpMethod.Post, "Applicant/LinkedIn/" + id + "?identifier=" + thirdPartyIdentifier + "&profileUrl=" + profileUrl)
        {
        }
    }
}
