using System.Net.Http;

namespace ePunkt.Api.Client.Requests
{
    public class JobSalaryImageRequest : HttpRequestMessage
    {
        public JobSalaryImageRequest(int jobId)
            : base(HttpMethod.Get, "JobSalaryImage/" + jobId)
        {
        }
    }
}
