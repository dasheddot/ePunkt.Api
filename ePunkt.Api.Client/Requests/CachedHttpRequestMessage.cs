using System.Net.Http;

namespace ePunkt.Api.Client
{
    public abstract class CachedHttpRequestMessage : HttpRequestMessage
    {
        protected CachedHttpRequestMessage(HttpMethod method, string requestUri) : base(method, requestUri) { }

        public abstract string GetCacheKey();
    }
}
