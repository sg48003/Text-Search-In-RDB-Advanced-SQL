using NMBP_TextSearch.Models;
using NMBP_TextSearch.ViewModels;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using NMBP_TextSearch.Enums;
using NMBP_TextSearch.Helper_classes;
using RedWillow.MvcToastrFlash;

namespace NMBP_TextSearch.Controllers
{
    public class SearchController : Controller
    {
        public ActionResult SearchForm()
        {
            var viewModel = new SearchFormViewModel();
            return View("SearchForm", viewModel);
        }

        #region Autocomplete - Fuzzy Search

        [HttpPost]
        public JsonResult Autocomplete(string prefix)
        {
            List<string> movies = TextSearch.Fuzzy(prefix);
            return Json(movies);
        }

        #endregion

        #region Morphology & Semantic Search

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(SearchFormViewModel searchParameters)
        {
            if (ModelState.IsValid == false)
            {
                this.Flash(Toastr.ERROR, "Error", "Enter search pattern!");
                return RedirectToAction("SearchForm", "Search");                
            }

            string[] phrases = searchParameters.SearchPattern.Split('"');

            //svaki neparni index u phrases je razmak, pa ga mičemo
            IEnumerable<string[]> removeBlank = phrases.Select((item, index) => index % 2 == 0 ?
                                                            item.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                                            : new[] { item });
            List<string> pattern = removeBlank.SelectMany(element => element).ToList();

            string tsPattern = ConvertTo.LogicalPattern(pattern, searchParameters.LogicalOperator == (int)Operator.Or ? Operator.Or : Operator.And);

            string queryString;
            List<MovieViewModel> movies = TextSearch.MorphologySemantic(tsPattern, out queryString);

            var viewModel = new SearchFormViewModel()
            {
                SearchPattern = searchParameters.SearchPattern,
                LogicalOperator = searchParameters.LogicalOperator,
                QueryString = queryString,
                Movies = movies

            };

            UpdateSearchHistory(tsPattern);

            ModelState.Clear(); //bez ovoga ne bi se mijenjao QueryString
            return View("SearchForm", viewModel);
        }
       
        #endregion

        #region Search History => Analysis

        private void UpdateSearchHistory(string tsPattern)
        {
            var searchHistory = new SearchHistory()
            {
                SearchInput = tsPattern,
                InputDateTime = DateTime.Now
            };

            InsertCommand.InsertSearchHistory(searchHistory);

        }

        #endregion      

    }

}