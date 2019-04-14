using NMBP_TextSearch.Models;
using NMBP_TextSearch.ViewModels;
using System.Web.Mvc;
using RedWillow.MvcToastrFlash;
using NMBP_TextSearch.Helper_classes;

namespace NMBP_TextSearch.Controllers
{
    public class MovieController : Controller
    {       
        public ActionResult MovieForm()
        {
            var viewModel = new MovieFormViewModel();
            return View("MovieForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("MovieForm", "Movie");
            }
                
            if (InsertCommand.InsertMovie(movie) > 0)
            {
                this.Flash(Toastr.SUCCESS, "Success", "Movie saved successfully");
            }
            else
            {
                this.Flash(Toastr.ERROR, "Error", "Movie was not saved");
            }

            return RedirectToAction("MovieForm", "Movie");
        }

    }
}