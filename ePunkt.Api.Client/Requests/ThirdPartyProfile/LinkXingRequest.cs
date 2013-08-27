using System;
using System.Net.Http;
using ePunkt.Api.Responses;

namespace ePunkt.Api.Client.Requests
{
    public class LinkXingRequest : HttpRequestMessage<ApplicantResponse>
    {
        public LinkXingRequest(int id, string thirdPartyIdentifier, Uri profileUrl)
            : base(HttpMethod.Post, "Applicant/Xing/" + id + "?identifier=" + thirdPartyIdentifier + "&profileUrl=" + profileUrl)
        {
        }
    }
}
