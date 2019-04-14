using NMBP_TextSearch.Enums;
using NMBP_TextSearch.Helper_classes;
using NMBP_TextSearch.Models;
using NMBP_TextSearch.ViewModels;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;

namespace NMBP_TextSearch.Controllers
{
    public class AnalysisController : Controller
    {
        public ActionResult AnalysisForm()
        {
            var viewModel = new AnalysisFormViewModel()
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(3),
                Granulation = (int)Granulation.Day,
                AnalysisResults = new Tuple<List<string>, List<AnalysisResult>>(new List<string>(), new List<AnalysisResult>())
            };
            return View("AnalysisForm", viewModel);
            
        }

        #region Analysis

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Analysis(AnalysisFormViewModel analysisParameters)
        {
            AnalysisFormViewModel viewModel = AnalyzingSearchInputs(analysisParameters.StartDate,
                                                                    analysisParameters.EndDate,
                                                                    analysisParameters.Granulation == (int)Granulation.Day ? Granulation.Day : Granulation.Hour);
            return View("AnalysisForm", viewModel);
        }

        private AnalysisFormViewModel AnalyzingSearchInputs(DateTime startDate, DateTime endDate, Granulation granulation)
        {
            Tuple<List<string>, string, string, string> query = granulation == Granulation.Day ?
                                                                            Query.AnalysisByDays(startDate, endDate) :
                                                                            Query.AnalysisByHours(startDate, endDate);

            using (NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings[GlobalVariables.ConnectionStringName].ConnectionString))
            {
                connection.Open();

                using (var createTempTable = new NpgsqlCommand(query.Item2, connection))
                {
                    createTempTable.ExecuteNonQuery();
                }

                List<AnalysisResult> analysisList = new List<AnalysisResult>();
                NpgsqlCommand pivotTable = new NpgsqlCommand(query.Item3, connection);

                using (NpgsqlDataReader dataReader = pivotTable.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        var temp = new AnalysisResult()
                        {
                            SearchPattern = dataReader[0].ToString(),
                            DatesOrHours = new List<string>()
                        };
                        for (int i = 1; i <= query.Item1.Count; i++)
                        {
                            temp.DatesOrHours.Add(dataReader[i].ToString());
                        }
                        analysisList.Add(temp);
                    }

                    pivotTable.Dispose();
                }

                using (var deleteTempTable = new NpgsqlCommand(query.Item4, connection))
                {
                    deleteTempTable.ExecuteNonQuery();
                }

                return new AnalysisFormViewModel()
                {
                    StartDate = startDate,
                    EndDate = endDate,
                    Granulation = (int)granulation,
                    AnalysisResults = new Tuple<List<string>, List<AnalysisResult>>(query.Item1, analysisList)
                };

            }
        }

        #endregion

    }
}