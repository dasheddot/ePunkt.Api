using ePunkt.Api.Client;
using ePunkt.Api.Client.Requests;
using ePunkt.Api.Models;
using ePunkt.Api.Parameters;
using ePunkt.Portal.Models.Shared;
using System.Threading.Tasks;

namespace ePunkt.Portal
{
    public class UpdateApplicantService
    {
        public async Task<Applicant> UpdatePersonalInformation(ApiHttpClient apiClient, Applicant applicant, PersonalInformationViewModel model)
        {
            //set the remaining applicant data by updating it
            var updateParameter = new ApplicantUpdateParameter(applicant)
                {
                    BirthDate = model.BirthDate,
                    City = model.City,
                    Country = model.Country,
                    Nationality = model.Nationality,
                    Phone = model.Phone,
                    Street = model.Street,
                    TitleBeforeName = model.TitleBeforeName,
                    TitleAfterName = model.TitleAfterName,
                    ZipCode = model.ZipCode
                };
            return await apiClient.SendAndReadAsync<Applicant>(new ApplicantRequest(applicant.Id, updateParameter));
        }
    }
}