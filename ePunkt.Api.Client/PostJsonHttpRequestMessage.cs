﻿using System;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace ePunkt.Api.Client
{
    public class PostJsonHttpRequestMessage : HttpRequestMessage
    {
        public PostJsonHttpRequestMessage(string url, object param)
            : base(HttpMethod.Post, url)
        {
            if (param == null)
                throw new ArgumentNullException("param");
            Content = new ObjectContent(param.GetType(), param, new JsonMediaTypeFormatter());
        }
    }
}
