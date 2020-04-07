using System.ComponentModel.DataAnnotations;

namespace FindbookApi.ViewModels
{
    public class UserSignInView
    {
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Length must be between 1 and 100 characters")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string UserName { get; set; }
    }
}
