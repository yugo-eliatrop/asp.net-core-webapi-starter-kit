using System.ComponentModel.DataAnnotations;

namespace FindbookApi.RequestModels
{
    public class UserChangePassModel
    {
        [Required]
        public string OldPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
