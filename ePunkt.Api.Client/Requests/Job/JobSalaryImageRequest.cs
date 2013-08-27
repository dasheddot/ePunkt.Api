using ePunkt.Api.Responses;
using System.Net.Http;

namespace ePunkt.Api.Client.Requests
{
    public class JobSalaryImageRequest : HttpRequestMessage<JobSalaryImageResponse>
    {
        public JobSalaryImageRequest(int jobId)
            : base(HttpMethod.Get, "JobSalaryImage/" + jobId)
        {
        }
    }
}
