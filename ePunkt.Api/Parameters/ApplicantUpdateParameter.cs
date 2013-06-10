using System;
using ePunkt.Api.Models;

namespace ePunkt.Api.Parameters
{
    public class ApplicantUpdateParameter : ApplicantCreateParameter
    {
        public ApplicantUpdateParameter()
        {
        }

        public ApplicantUpdateParameter(Applicant applicant)
        {
            FirstName = applicant.FirstName;
            LastName = applicant.LastName;
            Gender = applicant.Gender;
            Email = applicant.Email;

            TitleBeforeName = applicant.TitleBeforeName;
            TitleAfterName = applicant.TitleAfterName;
            BirthDate = applicant.BirthDate;
            Street = applicant.Street;
            City = applicant.City;
            ZipCode = applicant.ZipCode;
            Country = applicant.Country;
            Nationality = applicant.Nationality;
            Phone = applicant.Phone;
            MobilePhone = applicant.MobilePhone;
            IsActive = applicant.IsActive;
            EnableNewsletter = applicant.EnableNewsletter;
            EnableMatchingJobsAutoMail = applicant.EnableMatchingJobsAutoMail;
        }


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
