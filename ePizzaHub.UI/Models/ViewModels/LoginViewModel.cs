using System.ComponentModel.DataAnnotations;

namespace ePizzaHub.UI.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string UserName { get; set; }
        [Required]
        [MinLength(5, ErrorMessage = "Minimum length of Password should be 5 characters")]
        [MaxLength(10, ErrorMessage = "Maximum length of Password should be 10 characters")]
        public string Password { get; set; }
    }
}
