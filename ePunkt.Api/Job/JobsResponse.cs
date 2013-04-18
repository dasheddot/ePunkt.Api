using System.Collections.Generic;

namespace ePunkt.Api
{
    public class JobsResponse
    {
        public IEnumerable<Job> Jobs { get; set; }
        public string Channel { get; set; }
        public string Culture { get; set; }
    }
}
