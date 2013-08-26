using System;

namespace ePunkt.Api.Responses
{
    public class ApiTokenResponse
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }
}
