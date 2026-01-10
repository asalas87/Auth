using System.Text.RegularExpressions;

namespace Application.Common
{
    public class Helpers
    {
        public static string? Match(string text, Regex regex)
        {
            var match = regex.Match(text);
            return match.Success ? match.Groups[1].Value.Trim() : null;
        }

        public static DateTime? MatchDate(string text, Regex regex)
        {
            var value = Match(text, regex);
            return DateTime.TryParse(value, out var d) ? d : null;
        }

    }
}
