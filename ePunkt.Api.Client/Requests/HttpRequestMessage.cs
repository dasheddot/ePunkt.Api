using System.Net.Http;
using System.Threading.Tasks;

namespace ePunkt.Api.Client.Requests
{
    public abstract class HttpRequestMessage<T> : HttpRequestMessage where T : class
    {
        protected HttpRequestMessage(HttpMethod method, string requestUri) : base(method, requestUri) { }

        public virtual async Task<T> LoadResult(ApiHttpClient client)
        {
            return await client.SendAndReadAsync<T>(this);
        }
    }
}
