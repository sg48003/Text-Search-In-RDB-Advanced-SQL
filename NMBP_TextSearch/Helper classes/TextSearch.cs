using NMBP_TextSearch.ViewModels;
using Npgsql;
using System.Collections.Generic;
using System.Configuration;

namespace NMBP_TextSearch.Helper_classes
{
    public class TextSearch
    {
        public static List<string> Fuzzy(string pattern)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings[GlobalVariables.ConnectionStringName].ConnectionString))
            {
                connection.Open();

                var query = new NpgsqlCommand(Query.FuzzySearch(pattern), connection);

                var reader = query.ExecuteReader();
                var movies = new List<string>();
                while (reader.Read())
                {
                    movies.Add(reader[0].ToString());
                }

                return movies;
            }
        }

        public static List<MovieViewModel> MorphologySemantic(string tsPattern, out string queryString)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings[GlobalVariables.ConnectionStringName].ConnectionString))
            {
                connection.Open();

                queryString = Query.MorphologySemanticSearch(tsPattern);
                NpgsqlCommand query = new NpgsqlCommand(queryString, connection);
                NpgsqlDataReader dataReader = query.ExecuteReader();

                List<MovieViewModel> movies = new List<MovieViewModel>();
                while (dataReader.Read())
                {
                    movies.Add(new MovieViewModel()
                    {
                        Id = int.Parse(dataReader[0].ToString()),
                        Title = dataReader[1].ToString(),
                        Rank = double.Parse(dataReader[4].ToString())

                    });
                }

                return movies;
            }
        }
    }
}