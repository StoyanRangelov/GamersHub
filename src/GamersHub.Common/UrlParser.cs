namespace GamersHub.Common
{
    using System.Text;
    using System.Text.RegularExpressions;

    public static class UrlParser
    {
        public static string ParseToUrl(string input)
        {
            var matches = Regex.Matches(input, "[^!*'();:@&=+$,/?#[\\]]+");

            var result = new StringBuilder();

            foreach (Match match in matches)
            {
                var trimValue = match.Value.TrimEnd();
                result.Append(trimValue);
            }

            result.Replace(' ', '-');

            return result.ToString();
        }
    }
}
