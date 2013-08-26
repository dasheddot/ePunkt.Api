using ePunkt.Api.Client.Requests;
using ePunkt.Api.Parameters;
using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.Caching;
using System.Threading.Tasks;

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

        public async Task<T> SendAndReadAsync<T>(HttpRequestMessage requestMessage, bool runTimer = true) where T : class
        {
            if (runTimer)
            {
                _watch.Reset();
                _watch.Start();
            }
            var request = await SendAsync(requestMessage);
            LastReceivedContent = await request.Content.ReadAsStringAsync();

            T result;
            try
            {
                result = await request.Content.ReadAsAsync<T>();
            }
            catch (JsonSerializationException)
            {
                throw new ApplicationException("Unable to deserialize to type '" + typeof(T).Name + "'. The result was: " + Environment.NewLine + request.Content.ReadAsStringAsync().Result);
            }
            catch (JsonReaderException readerException)
            {
                throw new ApplicationException("Unable to read result: " + readerException.Message + "." + Environment.NewLine + "The result was: " + request.Content.ReadAsStringAsync().Result);
            }

            if (runTimer)
            {
                _watch.Stop();
                ElapsedMillisecondsInLastCall = _watch.ElapsedMilliseconds;
            }
            return result;
        }

        public async Task<T> SendAndReadAsyncCached<T>(CachedHttpRequestMessage requestMessage, TimeSpan cacheDuration) where T : class
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

        public async Task<T> SendAndReadAsyncCached<T>(CachedHttpRequestMessage requestMessage) where T : class
        {
            return await SendAndReadAsyncCached<T>(requestMessage, TimeSpan.FromSeconds(CacheDurationInSeconds));
        }

        public long ElapsedMillisecondsInLastCall { get; private set; }
        public string LastReceivedContent { get; private set; }
    }
}
