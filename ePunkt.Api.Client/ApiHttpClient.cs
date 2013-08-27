using ePunkt.Api.Client.Requests;
using ePunkt.Api.Parameters;
using JetBrains.Annotations;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Threading.Tasks;
using HttpRequestMessage = System.Net.Http.HttpRequestMessage;

namespace ePunkt.Api.Client
{
    public class ApiHttpClient : HttpClient
    {
        private readonly int _mandatorId;
        private readonly Stopwatch _watch;
        private const int CacheDurationInSeconds = 300;

        public ApiHttpClient([NotNull] Uri baseAddress, [NotNull] Func<ApiKeyParameter> apiKeyFunction, [NotNull] Func<ApiTokenCache> apiCacheFunction)
            : base(new AuthorizationHandler(baseAddress, apiKeyFunction, apiCacheFunction))
        {
            _mandatorId = apiKeyFunction.Invoke().MandatorId;
            _watch = new Stopwatch();
            BaseAddress = baseAddress;
        }

        internal async Task<T> SendAndReadAsync<T>(HttpRequestMessage requestMessage, bool runTimer = true) where T : class
        {
            if (runTimer)
            {
                _watch.Reset();
                _watch.Start();
            }

            T result;
            var response = await SendAsync(requestMessage).ConfigureAwait(false);
            try
            {
                response.EnsureSuccessStatusCode();
                LastReceivedContent = await response.Content.ReadAsStringAsync();
                result = await response.Content.ReadAsAsync<T>();
            }
            catch (Exception)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                    throw new NotFoundException();
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    throw new UnauthorizedException();
                throw;
            }

            if (runTimer)
            {
                _watch.Stop();
                ElapsedMillisecondsInLastCall = _watch.ElapsedMilliseconds;
            }
            return result;
        }

        internal async Task<T> SendAndReadAsyncCached<T>(CachedHttpRequestMessage<T> requestMessage, TimeSpan cacheDuration) where T : class
        {
            _watch.Reset();
            _watch.Start();

            var cache = MemoryCache.Default;
            var cacheKey = _mandatorId + "_" + requestMessage.GetCacheKey();

            if (!cache.Contains(cacheKey))
                cache.Add(cacheKey, await SendAndReadAsync<T>(requestMessage, false), DateTimeOffset.Now.Add(cacheDuration));
            else
                ElapsedMillisecondsInLastCall = 0;

            var result = cache.Get(cacheKey) as T;
            _watch.Stop();
            ElapsedMillisecondsInLastCall = _watch.ElapsedMilliseconds;

            return result;
        }

        internal async Task<T> SendAndReadAsyncCached<T>(CachedHttpRequestMessage<T> requestMessage) where T : class
        {
            return await SendAndReadAsyncCached(requestMessage, TimeSpan.FromSeconds(CacheDurationInSeconds));
        }

        public long ElapsedMillisecondsInLastCall { get; private set; }
        public string LastReceivedContent { get; private set; }
    }

    public class NotFoundException : Exception
    {
        public NotFoundException() : base("Not found (404).") { }
    }

    public class UnauthorizedException : Exception
    {
        public UnauthorizedException() : base("Unauthorized (401).") { }
    }
}
