using System;
using System.ComponentModel.DataAnnotations;

namespace FindbookApi.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }

        public int Year { get; set; }

        public int Count { get; set; }

        public string Genre { get; set; }
    }
}