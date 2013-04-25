using ePunkt.Utilities;
using System.Linq;

namespace ePunkt.Portal.Models
{
    public class ApplicantCompleteness
    {
        public ApplicantCompleteness(Api.Models.Applicant applicant)
        {
            CalculatePersonalInformation(applicant);
            CalculateFiles(applicant);
        }

        private void CalculatePersonalInformation(Api.Models.Applicant applicant)
        {
            PersonalInformation = 100;
            PersonalInformation -= applicant.FirstName.IsNoE() ? 15 : 0;
            PersonalInformation -= applicant.LastName.IsNoE() ? 15 : 0;
            PersonalInformation -= applicant.Email.IsNoE() ? 15 : 0;
            PersonalInformation -= applicant.Phone.IsNoE() && applicant.MobilePhone.IsNoE() ? 15 : 0;
            PersonalInformation -= !applicant.Gender.HasValue ? 15 : 0;
            PersonalInformation -= !applicant.BirthDate.HasValue ? 10 : 0;
            PersonalInformation -= applicant.Street.IsNoE() ? 5 : 0;
            PersonalInformation -= applicant.ZipCode.IsNoE() ? 5 : 0;
            PersonalInformation -= applicant.City.IsNoE() ? 5 : 0;
        }

        private void CalculateFiles(Api.Models.Applicant applicant)
        {
            Files = 100;
            Files -= !applicant.Documents.Any(x => x.Name.Is("Cv")) ? 75 : 0;
            Files -= !applicant.Documents.Any(x => x.Name.Is("Photo")) ? 25 : 0;
        }

        public int PersonalInformation { get; set; }
        public int Files { get; set; }
    }
}