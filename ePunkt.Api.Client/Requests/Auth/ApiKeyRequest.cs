﻿using ePunkt.Api.Parameters;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace ePunkt.Api.Client.Requests
{
    public class ApiKeyRequest : HttpRequestMessage
    {
        public ApiKeyRequest(ApiKeyParameter apiKeyParameter)
            : base(HttpMethod.Post, "Auth")
        {
            Content = new ObjectContent(typeof (ApiKeyParameter), apiKeyParameter, new JsonMediaTypeFormatter());
        }
    }
}
