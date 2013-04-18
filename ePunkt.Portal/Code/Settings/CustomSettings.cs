using System.Collections.Generic;

namespace ePunkt.Portal
{
    public class CustomSettings
    {
        public string CustomFolder { get; set; }

        public int MandatorId { get; set; }
        public string ApiEndpoint { get; set; }
        public string ApiKey { get; set; }

        public Dictionary<string, string> AdditionalSettings { get; set; }
    }
}