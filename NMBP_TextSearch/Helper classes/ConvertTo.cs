using NMBP_TextSearch.Enums;
using System.Collections.Generic;

namespace NMBP_TextSearch.Helper_classes
{
    public class ConvertTo
    {
        public static string LogicalPattern(List<string> pattern, Operator logicalOperator)
        {
            var sqlPattern = new List<string>();
            foreach (var item in pattern)
            {
                sqlPattern.Add("(" + item.Replace(" ", " & ") + ")");
            }
            var separator = logicalOperator == Operator.And
                                                ? " & "
                                                : " | ";
            return string.Join(separator, sqlPattern);
        }
    }
}