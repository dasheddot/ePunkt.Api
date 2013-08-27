using System;
using System.Net.Http;
using ePunkt.Api.Responses;

namespace ePunkt.Api.Client.Requests
{
    public class LinkLinkedInRequest : HttpRequestMessage<ApplicantResponse>
    {
        public LinkLinkedInRequest(int id, string thirdPartyIdentifier, Uri profileUrl)
            : base(HttpMethod.Post, "Applicant/LinkedIn/" + id + "?identifier=" + thirdPartyIdentifier + "&profileUrl=" + profileUrl)
        {
        }
    }
}
