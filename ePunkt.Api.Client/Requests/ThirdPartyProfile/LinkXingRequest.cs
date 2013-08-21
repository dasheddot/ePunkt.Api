using System;
using System.Net.Http;

namespace ePunkt.Api.Client.Requests
{
    public class LinkXingRequest : HttpRequestMessage
    {
        public LinkXingRequest(int id, string thirdPartyIdentifier, Uri profileUrl)
            : base(HttpMethod.Post, "Applicant/Xing/" + id + "?identifier=" + thirdPartyIdentifier + "&profileUrl=" + profileUrl)
        {
        }
    }
}
