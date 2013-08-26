using System.Globalization;
using System.Net.Http;

namespace ePunkt.Api.Client.Requests
{
    public class JobRequest : CachedHttpRequestMessage
    {
        private readonly string _channel;
        private readonly CultureInfo _culture;
        private readonly int _jobId;

        public JobRequest(int jobId, string channel, CultureInfo culture)
            : base(HttpMethod.Get, "Job/" + jobId + "?channel=" + channel + "&culture=" + culture.Name)
        {
            _culture = culture;
            _channel = channel;
            _jobId = jobId;
        }

        public JobRequest(int jobId, string channel)
            : this(jobId, channel, CultureInfo.CurrentCulture)
        {
        }

        public JobRequest(int jobId)
            : this(jobId, "")
        {
        }

        public override string GetCacheKey()
        {
            return "Job_" + _jobId + "_" + _channel + "_" + _culture.Name;
        }
    }
}
