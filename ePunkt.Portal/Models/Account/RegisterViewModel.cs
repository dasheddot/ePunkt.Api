using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using ePunkt.Api.Models;

namespace ePunkt.Portal.Models.Account
{
    public class RegisterViewModel
    {



        public RegisterViewModel Prepare(Mandator mandator, Job job)
        {
            if (job != null)
                JobId = job.Id;

            AllowEmptyGender = mandator.Settings.AllowEmptyGender;
            AvailableTitlesBeforeName = from x in mandator.TitlesBeforeName
                                        select new SelectListItem
                                            {
                                                Value = x.Name,
                                                Text = x.Name
                                            };
            AvailableTitlesAfterName = from x in mandator.TitlesAfterName
                                       select new SelectListItem
                                       {
                                           Value = x.Name,
                                           Text = x.Name
                                       };

            return this;
        }


        public int? JobId { get; private set; }

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

        public bool AllowEmptyGender { get; set; }
        public IEnumerable<SelectListItem> AvailableTitlesBeforeName { get; set; }
        public IEnumerable<SelectListItem> AvailableTitlesAfterName { get; set; }
    }
}