using System.ComponentModel.DataAnnotations;

namespace FindbookApi.RequestModels
{
    public class LockOutRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int Minutes { get; set; }

        [Required]
        [MaxLength(200)]
        [MinLength(20)]
        public string Reason { get; set; }
    }
}
