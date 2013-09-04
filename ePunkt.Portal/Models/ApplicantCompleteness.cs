using ePunkt.Api.Responses;
using ePunkt.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace ePunkt.Portal.Models
{
    public class ApplicantCompleteness
    {
        public ApplicantCompleteness(ApplicantResponse applicantResponse, IEnumerable<ApplicantDocumentResponse> documents, IEnumerable<string> jobProfiles)
        {
            CalculatePersonalInformation(applicantResponse);
            CalculateFiles(documents);
            CalculateJobProfiles(jobProfiles);
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

        private void CalculateFiles(IEnumerable<ApplicantDocumentResponse> documents)
        {
            Files = 100;
            var documentsAsList = documents.ToList();
            Files -= documentsAsList.All(x => x.Id != -1) ? 75 : 0; // the CV
            Files -= documentsAsList.Any(x => x.Id == -2) ? 0 : 25; // the photo
        }

        private void CalculateJobProfiles(IEnumerable<string> jobProfiles)
        {
            JobProfiles = 100;
            if (!jobProfiles.Any())
                JobProfiles = 0;
        }

        public int PersonalInformation { get; set; }
        public int Files { get; set; }
        public int JobProfiles { get; set; }
    }
}