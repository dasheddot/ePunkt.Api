using System;

namespace ePunkt.Api
{
    public class ApplicantUpdateParameter : ApplicantCreateParameter
    {
        public string TitleBeforeName { get; set; }
        public string TitleAfterName { get; set; }

        public DateTime? BirthDate { get; set; }

        public string Street { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string Nationality { get; set; }

        public string Phone { get; set; }
        public string MobilePhone { get; set; }
    }
}
