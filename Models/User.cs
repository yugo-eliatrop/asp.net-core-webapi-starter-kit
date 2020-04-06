using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace FindbookApi.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Length must be between 1 and 50 characters")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [StringLength(5)]
        public string Salt { get; set; }

        [Required]
        [RegularExpression(@"Admin|Regular")]
        public string Role { get; set; }

        public bool Confirmed { get; set; }
    }

    enum UserTypes
    {
        Admin,
        Regular
    }
}
