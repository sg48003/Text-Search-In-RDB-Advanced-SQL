using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NMBP_TextSearch.ViewModels
{
    public class SearchFormViewModel
    {
        [Display(Name = "Search patterns")]
        [Required(ErrorMessage = "Search pattern is required")]
        public string SearchPattern { get; set; }

        [Display(Name = "Logical operator")]
        [Required(ErrorMessage = "Logical operator is required")]
        public int LogicalOperator { get; set; }

        [Display(Name = "Query string")]
        public string QueryString { get; set; }

        [Display(Name = "Search results")]
        public List<MovieViewModel> Movies { get; set; }

    }
}