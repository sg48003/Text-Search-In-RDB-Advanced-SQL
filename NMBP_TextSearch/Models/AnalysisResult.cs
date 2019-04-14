using System.Collections.Generic;

namespace NMBP_TextSearch.Models
{
    public class AnalysisResult
    {
        public string SearchPattern { get; set; }

        public List<string> DatesOrHours { get; set; }
    }
}