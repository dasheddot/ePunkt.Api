using System.Collections.Generic;
using ePunkt.Api.Models;

namespace ePunkt.Api.Responses
{
    public class JobsResponse
    {
        public IEnumerable<Job> Jobs { get; set; }
        public string Channel { get; set; }
        public string Culture { get; set; }
    }
}
