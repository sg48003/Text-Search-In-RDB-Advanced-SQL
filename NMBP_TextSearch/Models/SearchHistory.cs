using System;
using System.ComponentModel.DataAnnotations;

namespace NMBP_TextSearch.Models
{
    public class SearchHistory
    {
        [Display(Name = "Search input")]
        [Required(ErrorMessage = "SearchInput is required")]
        public string SearchInput { get; set; }

        [Display(Name = "Input date/time")]
        [Required(ErrorMessage = "InputDateTime is required")]
        public DateTime InputDateTime { get; set; }
    }
}