using ePunkt.Api.Client;
using ePunkt.Api.Client.Requests;
using ePunkt.Api.Responses;
using System;
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

        public async Task<JobsResponse> LoadJobsForCurrentPortal(Uri requestUri, MandatorResponse mandatorResponse)
        {
            return await _client.SendAndReadAsyncCached<JobsResponse>(new JobsRequest(string.Empty));
        }

        public async Task<JobResponse> LoadSingleJob(int jobId)
        {
            return await _client.SendAndReadAsyncCached<JobResponse>(new JobRequest(jobId));
        }
    }
}