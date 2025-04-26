using System.Text.RegularExpressions;

namespace BlogWeb.Helpers
{
    public static class HtmlHelpers
    {
        public static string StripHtml(string input) { 
            if(string.IsNullOrEmpty(input)) 
                return string.Empty;

            return Regex.Replace(input, "<.*?>", string.Empty);
            
        }

    }
}
