using System.ComponentModel.DataAnnotations;

namespace ePunkt.Portal.Models.Account
{
    public class IndexViewModel
    {
        public int? JobId { get; set; }

        [Required(ErrorMessage = @"Error-Username")]
        public string Username { get; set; }
        [Required(ErrorMessage = @"Error-Password")]
        public string Password { get; set; }
    }
}