using System;
using System.Net.Http;

namespace ePunkt.Api.Client.Requests
{
    public class MandatorRequest : CachedHttpRequestMessage
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
