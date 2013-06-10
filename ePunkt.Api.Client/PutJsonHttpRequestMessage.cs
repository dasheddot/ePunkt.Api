using System;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace ePunkt.Api.Client
{
    public class PutJsonHttpRequestMessage : HttpRequestMessage
    {
        public PutJsonHttpRequestMessage(string url, object param)
            : base(HttpMethod.Put, url)
        {
            if (param == null)
                throw new ArgumentNullException("param");
            Content = new ObjectContent(param.GetType(), param, new JsonMediaTypeFormatter());
        }
    }
}
