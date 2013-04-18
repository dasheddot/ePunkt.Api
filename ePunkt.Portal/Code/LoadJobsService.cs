using ePunkt.Api;
using ePunkt.Api.Client;
using ePunkt.Api.Client.Requests;
using ePunkt.Utilities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ePunkt.Portal
{
    public class LoadJobsService
    {
        private readonly ApiHttpClient _client;

        public LoadJobsService(ApiHttpClient client)
        {
            _client = client;
        }

        public async Task<JobsResponse> LoadJobsForChannel(string channel)
        {
            return await _client.SendAndReadAsyncCached<JobsResponse>(new JobsRequest(channel));
        }

        public async Task<JobsResponse> LoadJobsForCurrentPortal(Uri requestUri, Mandator mandator)
        {
            return await _client.SendAndReadAsyncCached<JobsResponse>(new JobsRequest(string.Empty));
        }

        private bool UrlsMatch(Uri requestUri, string baseUrl)
        {
            var baseUri = new Uri(baseUrl);
            return requestUri.Authority.Is(baseUri.Authority);
        }
    }
}