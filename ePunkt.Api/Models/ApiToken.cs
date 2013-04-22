using System;

namespace ePunkt.Api.Models
{
    public class ApiToken
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }
}
