using ePunkt.Api.Client.Requests;
using JetBrains.Annotations;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ePunkt.Api.Client
{
    public class AuthorizationHandler : DelegatingHandler
    {
        private readonly Func<ApiTokenCache> _apiCacheFunction;
        private readonly Func<ApiKey> _apiKeyFunction;
        private readonly Uri _baseAddress;

        public AuthorizationHandler([NotNull] Uri baseAddress, [NotNull] Func<ApiKey> apiKeyFunction, [NotNull] Func<ApiTokenCache> apiCacheFunction)
        {
            _baseAddress = baseAddress;
            _apiCacheFunction = apiCacheFunction;
            _apiKeyFunction = apiKeyFunction;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var apiKey = _apiKeyFunction.Invoke();
            var tokenCache = _apiCacheFunction.Invoke();

            var token = tokenCache.GetCachedToken(apiKey.MandatorId);

            if (token == null || DateTime.UtcNow >= token.Expires)
                token = RefreshApiToken(apiKey, tokenCache);

            if (InnerHandler == null)
                InnerHandler = new HttpClientHandler();
            if (token == null)
                throw new ApplicationException("Token is still null. This should not have happened.");
            request.Headers.Add("X-ApiToken", token.Token);
            return base.SendAsync(request, cancellationToken);
        }

        private ApiToken RefreshApiToken(ApiKey apiKey, ApiTokenCache tokenCache)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = _baseAddress;
                var request = client.SendAsync(new ApiKeyRequest(apiKey)).Result;
                if (request.StatusCode == HttpStatusCode.InternalServerError)
                    throw new ApplicationException("Request for API token failed: " + request.Content.ReadAsStringAsync().Result);

                var token = request.Content.ReadAsAsync<ApiToken>().Result;
                tokenCache.AddToken(apiKey.MandatorId, token);
                return token;
            }
        }
    }
}
