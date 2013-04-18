
namespace ePunkt.Api
{
    public class ApplicantUpdateParameter : ApplicantCreateParameter
    {

        public string Street { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }

        public string Phone { get; set; }
        public string MobilePhone { get; set; }

    }
}
