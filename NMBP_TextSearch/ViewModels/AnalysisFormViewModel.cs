using NMBP_TextSearch.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NMBP_TextSearch.ViewModels
{
    public class AnalysisFormViewModel
    {
        [Display(Name = "Start date")]
        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End date")]
        [Required(ErrorMessage = "End date is required")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Granulation is required")]
        public int Granulation { get; set; }

        [Display(Name = "Analysis results")]
        public Tuple<List<string>,List<AnalysisResult>> AnalysisResults { get; set; }
    }
}