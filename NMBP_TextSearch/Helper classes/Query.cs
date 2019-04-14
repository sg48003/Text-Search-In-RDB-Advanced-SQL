using System;
using System.Collections.Generic;

namespace NMBP_TextSearch.Helper_classes
{
    public class Query
    {

        public static string FuzzySearch(string pattern)
        {
            string query = $"SELECT \tsummary,\n " +
                                        $"\tsimilarity(summary, '{pattern}') sim\n " +
                            $"FROM movie\n " +
                            $"WHERE similarity(summary, '{pattern}') >= 0.1\n " +
                            $"ORDER BY sim DESC " +
                            $"LIMIT 5";
            return query;
        }

        public static string MorphologySemanticSearch(string tsPattern)
        {
            string query = $"SELECT \tmovieId," + Environment.NewLine +
                                $"\tts_headline(title, to_tsquery('english', '{tsPattern}'))," + Environment.NewLine +
                                $"\tts_headline(description, to_tsquery('english', '{tsPattern}'))," + Environment.NewLine +
                                $"\tdescription," + Environment.NewLine +
                                $"\tts_rank(array[0.1,0.2,0.4,1.0], vector , to_tsquery('english', '{tsPattern}')) rank" + Environment.NewLine +
                            $"FROM movie" + Environment.NewLine +
                            $"WHERE vector @@ to_tsquery('english', '{tsPattern}')" + Environment.NewLine +
                            $"ORDER BY rank DESC";
            return query;
        }

        public static Tuple<List<string>, string, string, string> AnalysisByHours(DateTime startDate, DateTime endDate)
        {
            int count = 0;
            var headers = new List<string>();
            var tempTable = "CREATE TABLE temp (header text); " +
                            "INSERT INTO temp(header) " +
                            "VALUES ";

            var query = "SELECT * " +
                        "FROM crosstab( $$SELECT searchinput::text si, " +
                                                "to_char(inputdatetime,'HH24')::text idt, " +
                                                "count(to_char(inputdatetime,'HH24'))::text count " +
                                        "FROM search_history " +
                                        $"WHERE date(inputdatetime) >= '{startDate.ToString("yyyy-MM-dd")}' and date(inputdatetime) <= '{endDate.ToString("yyyy-MM-dd")}' " +
                                        "GROUP BY si, idt " +
                                        "ORDER BY si, idt$$, " +
                                        "$$SELECT header " +
                                        "FROM temp " +
                                        "ORDER BY header$$) as pivotTable (si text, ";

            for (var i = 1; i <= 24; i++)
            {
                var hourToString = i.ToString("D2");
                tempTable = tempTable + $"('{hourToString}'), ";
                headers.Add(hourToString);
                query = query + $"header{count++} text, "; 
            }

            tempTable = tempTable.Substring(0, tempTable.Length - 2);
            query = query.Substring(0, query.Length - 2) + ")";

            return new Tuple<List<string>, string, string, string>(headers, tempTable, query, "DROP TABLE temp"); 
        }

        public static Tuple<List<string>, string, string, string> AnalysisByDays(DateTime startDate, DateTime endDate)
        {
            int count = 0;
            var headers = new List<string>();
            var tempTable = "CREATE TABLE temp (header text); " +
                            "INSERT INTO temp(header) " +
                            "VALUES ";

            var query = "SELECT * " +
                        "FROM crosstab( $$SELECT searchinput::text si, " +
                                                "date(inputdatetime)::text idt, " +
                                                "count(date(inputdatetime))::text count " +
                                        "FROM search_history " +
                                        $"WHERE date(inputdatetime) >= '{startDate.ToString("yyyy-MM-dd")}' and date(inputdatetime) <= '{endDate.ToString("yyyy-MM-dd")}' " +
                                        "GROUP BY si, idt " +
                                        "ORDER BY si, idt$$, " +
                                        "$$SELECT header " +
                                        "FROM temp " +
                                        "ORDER BY header$$) as pivotTable (si text, ";

            for (var i = startDate; i <= endDate; i = i.AddDays(1))
            {
                var dateToString = i.ToString("yyyy-MM-dd");
                tempTable = tempTable + $"('{dateToString}'), ";
                headers.Add(dateToString);
                query = query + $"header{count++} text, "; 
            }

            tempTable = tempTable.Substring(0, tempTable.Length - 2);
            query = query.Substring(0, query.Length - 2) + ")";

            return new Tuple<List<string>, string, string, string>(headers, tempTable, query, "DROP TABLE temp"); 
        }

    }

}

