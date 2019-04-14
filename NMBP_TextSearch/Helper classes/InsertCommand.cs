using NMBP_TextSearch.Models;
using Npgsql;
using System.Configuration;

namespace NMBP_TextSearch.Helper_classes
{
    public class InsertCommand
    {
        public static int InsertMovie(Movie movie)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings[GlobalVariables.ConnectionStringName].ConnectionString))
            {
                connection.Open();

                string queryString = "INSERT INTO movie (title, categories, summary, description, vector)" +
                                        "VALUES(@title, @categories, @summary, @description, " +
                                        "setweight(to_tsvector(coalesce(@title, '')), 'A')" +
                                        "|| setweight(to_tsvector(coalesce(@categories, '')), 'B')" +
                                        "|| setweight(to_tsvector(coalesce(@summary,'')), 'C')" +
                                        "|| setweight(to_tsvector(coalesce(@description,'')), 'D'))";

                var query = new NpgsqlCommand(queryString, connection);


                query.Parameters.AddWithValue("@title", movie.Title);
                query.Parameters.AddWithValue("@categories", movie.Category);
                query.Parameters.AddWithValue("@summary", movie.Summary);
                query.Parameters.AddWithValue("@description", movie.Description);

                return query.ExecuteNonQuery();
            }
            
        }

        public static int InsertSearchHistory(SearchHistory searchHistory)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings[GlobalVariables.ConnectionStringName].ConnectionString))
            {
                connection.Open();

                string queryString = "INSERT INTO search_history (searchInput, inputDateTime)" +
                                     "VALUES(@searchInput, @inputDateTime)";

                var query = new NpgsqlCommand(queryString, connection);

                query.Parameters.AddWithValue("@searchInput", searchHistory.SearchInput);
                query.Parameters.AddWithValue("@inputDateTime", searchHistory.InputDateTime);

                return query.ExecuteNonQuery();
            }         
        }
    }
}