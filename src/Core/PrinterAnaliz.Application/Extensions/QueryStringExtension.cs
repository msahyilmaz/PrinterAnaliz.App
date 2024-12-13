using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;

namespace PrinterAnaliz.Application.Extensions
{
    public static class QueryStringExtension
    {
        public static string ConvertSPIdList(this string str) 
        {
            if (str.IsNullOrEmpty())  return "";
            var idClearList = string.Join(",", Regex.Matches(str, @"^\d+|\d+").Cast<Match>().Select(m => m.Value).Distinct());

            return idClearList;
        }
    }
}
