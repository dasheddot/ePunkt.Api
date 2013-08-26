using System.Globalization;
using System.Net.Http;

namespace ePunkt.Api.Client.Requests
{
    public class JobsRequest : CachedHttpRequestMessage
    {
        private readonly string _channel;
        private readonly CultureInfo _culture;

        public JobsRequest(string channel, CultureInfo culture)
            : base(HttpMethod.Get, "Jobs?channel=" + channel + "&culture=" + culture.Name)
        {
            _culture = culture;
            _channel = channel;
        }

        public JobsRequest(string channel)
            : this(channel, CultureInfo.CurrentCulture)
        {
        }

        public override string GetCacheKey()
        {
            return "Jobs_" + _channel + "_" + _culture.Name;
        }
    }
}
