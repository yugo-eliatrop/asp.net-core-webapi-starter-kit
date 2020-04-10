using System.ComponentModel.DataAnnotations;

namespace FindbookApi.RequestModels
{
    public class BookEditModel
    {
        [Required]
        [MaxLength(120)]
        public string Title { get; set; }

        [Required]
        [MaxLength(120)]
        public string Author { get; set; }

        [Range(1, 2020)]
        public int Year { get; set; }

        public int Count { get; set; }

        public string Genre { get; set; }
    }
}