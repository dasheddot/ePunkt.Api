using System.ComponentModel.DataAnnotations;

namespace ePunkt.Portal.Models.Account
{
    public class ChangePasswordViewModel
    {
        public int? JobId { get; set; }

        public string OldPassword { get; set; }

        [Required(ErrorMessage = @"Error-NewPassword")]
        public string NewPassword { get; set; }

        public string NewPassword2 { get; set; }
    }
}