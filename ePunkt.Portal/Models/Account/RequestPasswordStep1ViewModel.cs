using System.ComponentModel.DataAnnotations;

namespace ePunkt.Portal.Models.Account
{
    public class RequestPasswordStep1ViewModel
    {
        [Required(ErrorMessage = @"Error-Email"), DataType(DataType.EmailAddress, ErrorMessage = @"Error-Email")]
        public string Email { get; set; }
    }
}