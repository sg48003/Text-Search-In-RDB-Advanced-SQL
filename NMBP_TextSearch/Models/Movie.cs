using System.ComponentModel.DataAnnotations;

namespace NMBP_TextSearch.Models
{
    public class Movie
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Summary is required")]
        public string Summary { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
    }
}