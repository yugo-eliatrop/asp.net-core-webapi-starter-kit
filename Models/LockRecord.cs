using System.ComponentModel.DataAnnotations;

namespace FindbookApi.Models
{
    public class LockRecord
    {
        public int Id { get; set; }

        [Required]
        [MinLength(20)]
        [MaxLength(200)]
        public string Reason { get; set; }

        [Required]
        public int AdminId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public LockRecord()
        { }

        public LockRecord(string reason, int adminId)
        {
            Reason = reason;
            AdminId = adminId;
        }
    }
}