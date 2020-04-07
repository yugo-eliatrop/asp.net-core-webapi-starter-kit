using System.ComponentModel.DataAnnotations;

namespace FindbookApi.ViewModels
{
    public class UserSignUpView
    {
        [Required]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Length must be between 1 and 100 characters")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [RegularExpression(@"Admin|Customer")]
        public string Role { get; set; }
    }
}
