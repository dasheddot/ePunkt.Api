using ePunkt.Api.Responses;
using System;

namespace ePunkt.Api.Parameters
{
    public class ApplicantParameter
    {
        public ApplicantParameter()
        {
        }

        public ApplicantParameter(ApplicantResponse applicantResponse)
        {
            FirstName = applicantResponse.FirstName;
            LastName = applicantResponse.LastName;
            Gender = applicantResponse.Gender;
            Email = applicantResponse.Email;

            TitleBeforeName = applicantResponse.TitleBeforeName;
            TitleAfterName = applicantResponse.TitleAfterName;
            BirthDate = applicantResponse.BirthDate;
            Street = applicantResponse.Street;
            City = applicantResponse.City;
            ZipCode = applicantResponse.ZipCode;
            Country = applicantResponse.Country;
            Nationality = applicantResponse.Nationality;
            Phone = applicantResponse.Phone;
            MobilePhone = applicantResponse.MobilePhone;
            IsActive = applicantResponse.IsActive;
            EnableNewsletter = applicantResponse.EnableNewsletter;
            EnableMatchingJobsAutoMail = applicantResponse.EnableMatchingJobsAutoMail;
        }


        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool? Gender { get; set; }

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

        public bool IsActive { get; set; }
        public bool EnableNewsletter { get; set; }
        public bool EnableMatchingJobsAutoMail { get; set; }
    }
}
