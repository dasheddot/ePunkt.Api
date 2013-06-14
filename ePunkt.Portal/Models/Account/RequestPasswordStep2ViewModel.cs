using System.ComponentModel.DataAnnotations;

namespace ePunkt.Portal.Models.Account
{
    public class RequestPasswordStep2ViewModel
    {
        [Required(ErrorMessage = @"Error-NewPassword")]
        public string NewPassword { get; set; }
        public string NewPassword2 { get; set; }

        public string Email { get; set; }
        public string Code { get; set; }
    }
}