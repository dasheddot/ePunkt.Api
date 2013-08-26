using ePunkt.Api.Client.Requests;
using ePunkt.Api.Parameters;
using ePunkt.Api.Responses;
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
        private readonly Func<ApiKeyParameter> _apiKeyFunction;
        private readonly Uri _baseAddress;

        public AuthorizationHandler([NotNull] Uri baseAddress, [NotNull] Func<ApiKeyParameter> apiKeyFunction, [NotNull] Func<ApiTokenCache> apiCacheFunction)
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

        private ApiTokenResponse RefreshApiToken(ApiKeyParameter apiKeyParameter, ApiTokenCache tokenCache)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = _baseAddress;
                var request = client.SendAsync(new ApiKeyRequest(apiKeyParameter)).Result;
                if (request.StatusCode == HttpStatusCode.InternalServerError)
                    throw new ApplicationException("Request for API token failed: " + request.Content.ReadAsStringAsync().Result);

                var token = request.Content.ReadAsAsync<ApiTokenResponse>().Result;
                tokenCache.AddToken(apiKeyParameter.MandatorId, token);
                return token;
            }
        }
    }
}
