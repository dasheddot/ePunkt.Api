using ePunkt.Api.Client;
using ePunkt.Api.Client.Requests;
using ePunkt.Api.Parameters;
using ePunkt.Api.Responses;
using ePunkt.Portal.Models.Shared;
using System.Threading.Tasks;

namespace ePunkt.Portal
{
    public class UpdateApplicantService
    {
        public async Task<ApplicantResponse> UpdatePersonalInformation(ApiHttpClient apiClient, ApplicantResponse applicantResponse, PersonalInformationViewModel model)
        {
            //set the remaining applicant data by updating it
            var updateParameter = new ApplicantParameter(applicantResponse)
                {
                    BirthDate = model.BirthDate,
                    City = model.City,
                    Country = model.Country,
                    Nationality = model.Nationality,
                    Phone = model.Phone,
                    Street = model.Street,
                    TitleBeforeName = model.TitleBeforeName,
                    TitleAfterName = model.TitleAfterName,
                    ZipCode = model.ZipCode,
                    Email = model.Email
                };
            return await new ApplicantPostRequest(applicantResponse.Id, updateParameter).LoadResult(apiClient);
        }
    }
}