using System;
using System.Net.Http;
using ePunkt.Api.Responses;

namespace ePunkt.Api.Client.Requests
{
    public class MandatorRequest : CachedHttpRequestMessage<MandatorResponse>
    {
        private readonly Uri _currentUri;

        public MandatorRequest(Uri currentUri)
            : base(HttpMethod.Get, "Mandator?uri=" + currentUri)
        {
            _currentUri = currentUri;
        }

        public override string GetCacheKey()
        {
            return "MandatorRequest_" + _currentUri;
        }
    }
}
