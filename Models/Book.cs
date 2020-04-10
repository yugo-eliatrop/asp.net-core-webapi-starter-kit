using System.ComponentModel.DataAnnotations;
using FindbookApi.RequestModels;

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

        public Book() {}

        public Book(BookEditModel model)
        {
            CopyParams(model);
            Id = 0;
        }

        public void Update(BookEditModel model)
        {
            CopyParams(model);
        }

        private void CopyParams(BookEditModel model)
        {
            Title = model.Title;
            Author = model.Author;
            Year = model.Year;
            Count = model.Count;
            Genre = model.Genre;
        }
    }
}