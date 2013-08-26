using ePunkt.Api.Responses;
using ePunkt.Utilities;
using System.Linq;

namespace ePunkt.Portal.Models
{
    public class ApplicantCompleteness
    {
        public ApplicantCompleteness(ApplicantResponse applicantResponse)
        {
            CalculatePersonalInformation(applicantResponse);
            CalculateFiles(applicantResponse);
        }

        private void CalculatePersonalInformation(ApplicantResponse applicantResponse)
        {
            PersonalInformation = 100;
            PersonalInformation -= applicantResponse.FirstName.IsNoE() ? 15 : 0;
            PersonalInformation -= applicantResponse.LastName.IsNoE() ? 15 : 0;
            PersonalInformation -= applicantResponse.Email.IsNoE() ? 15 : 0;
            PersonalInformation -= applicantResponse.Phone.IsNoE() && applicantResponse.MobilePhone.IsNoE() ? 15 : 0;
            PersonalInformation -= !applicantResponse.Gender.HasValue ? 15 : 0;
            PersonalInformation -= !applicantResponse.BirthDate.HasValue ? 10 : 0;
            PersonalInformation -= applicantResponse.Street.IsNoE() ? 5 : 0;
            PersonalInformation -= applicantResponse.ZipCode.IsNoE() ? 5 : 0;
            PersonalInformation -= applicantResponse.City.IsNoE() ? 5 : 0;
        }

        private void CalculateFiles(ApplicantResponse applicantResponse)
        {
            Files = 100;
            Files -= !applicantResponse.Documents.Any(x => x.Name.Is("Cv")) ? 75 : 0;
            Files -= !applicantResponse.Documents.Any(x => x.Name.Is("Photo")) ? 25 : 0;
        }

        public int PersonalInformation { get; set; }
        public int Files { get; set; }
    }
}