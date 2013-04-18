using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ePunkt.Api
{
    public class ApplicantCreateParameter
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool? Gender { get; set; }
    }
}
