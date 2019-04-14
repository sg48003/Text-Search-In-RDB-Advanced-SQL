using System.ComponentModel.DataAnnotations;

namespace NMBP_TextSearch.ViewModels
{
    public class MovieViewModel
    {
        
        [Required(ErrorMessage = "Movie ID is required")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Movie title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Movie rank is required")]
        public double Rank { get; set; }
    }
}