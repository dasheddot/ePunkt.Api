using ePunkt.Api.Responses;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace ePunkt.Portal.Models.Shared
{
    public class PersonalInformationViewModel
    {
        public virtual PersonalInformationViewModel Prepare(MandatorResponse mandatorResponse)
        {
            return Prepare(mandatorResponse, null);
        }

        public virtual PersonalInformationViewModel Prepare([NotNull]MandatorResponse mandatorResponse, [CanBeNull] ApplicantResponse applicantResponse)
        {
            AllowEmptyGender = mandatorResponse.PortalSettings.AllowEmptyGender;
            AvailableTitlesBeforeName = from x in mandatorResponse.TitlesBeforeName
                                        select new SelectListItem
                                            {
                                                Value = x.Name,
                                                Text = x.Name
                                            };
            AvailableTitlesAfterName = from x in mandatorResponse.TitlesAfterName
                                       select new SelectListItem
                                           {
                                               Value = x.Name,
                                               Text = x.Name
                                           };
            AvailableCountries = from x in mandatorResponse.Countries
                                 select new SelectListItem
                                     {
                                         Value = x,
                                         Text = x
                                     };
            AvailableNationalities = from x in mandatorResponse.Countries
                                     select new SelectListItem
                                         {
                                             Value = x,
                                             Text = x
                                         };

            if (applicantResponse != null)
                AutoMapper.Mapper.DynamicMap(applicantResponse, this);

            return this;
        }


        public bool? Gender { get; set; }
        public string TitleBeforeName { get; set; }
        public string TitleAfterName { get; set; }
        [Required(ErrorMessage = @"Error-FirstName")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = @"Error-LastName")]
        public string LastName { get; set; }
        [Required(ErrorMessage = @"Error-Email")]
        [DataType(DataType.EmailAddress, ErrorMessage = @"Error-Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = @"Error-Phone")]
        public string Phone { get; set; }
        [Required(ErrorMessage = @"Error-Street")]
        public string Street { get; set; }
        [Required(ErrorMessage = @"Error-ZipCode")]
        public string ZipCode { get; set; }
        [Required(ErrorMessage = @"Error-City")]
        public string City { get; set; }
        [Required(ErrorMessage = @"Error-Country")]
        public string Country { get; set; }
        [Required(ErrorMessage = @"Error-Nationality")]
        public string Nationality { get; set; }
        [Required(ErrorMessage = @"Error-BirthDate")]
        public DateTime BirthDate { get; set; }

        public IEnumerable<SelectListItem> AvailableTitlesBeforeName { get; set; }
        public IEnumerable<SelectListItem> AvailableTitlesAfterName { get; set; }
        public IEnumerable<SelectListItem> AvailableCountries { get; set; }
        public IEnumerable<SelectListItem> AvailableNationalities { get; set; }

        public bool AllowEmptyGender { get; set; }
    }
}