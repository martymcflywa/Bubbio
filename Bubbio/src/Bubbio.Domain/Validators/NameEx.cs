using System.Text;
using System.Text.RegularExpressions;
using Bubbio.Core.Helpers;

namespace Bubbio.Domain.Validators
{
    public static class NameEx
    {
        public static string Validate(this string name)
        {
            return name
                .FixQuotes()
                .FixCommaAndDot()
                .TrimKnownChars()
                .CapitalizeName()
                .CapitalizeSplitNames()
                .HandleApostropheNearEndOfName()
                .NullIfContainsInvalidCharacters()
                .NullIfEmpty();
        }

        private static string FixQuotes(this string name)
        {
            return name?
                .Replace('\u2019', '\u0027');
        }

        private static string FixCommaAndDot(this string name)
        {
            return name?
                .Replace(',', '\'')
                .Replace('.', '\'');
        }

        private static string TrimKnownChars(this string name)
        {
            return name?
                .Trim(' ', ',', '.', '`', '*', '?', '-', '\'', '\u0009');
        }

        private static string CapitalizeName(this string name)
        {
            if (name.IsEmpty())
                return null;

            var pattern = new Regex(@"^[\p{L}- ']+$");
            return pattern.Match(name).Success ? name.Capitalize() : name;
        }

        private static string CapitalizeSplitNames(this string name)
        {
            if (name.IsEmpty())
                return null;

            name = name.CapitalizeSplitNames('-');
            name = name.CapitalizeSplitNames(' ');

            if (!name.Contains("'") || name.Contains(" ") || name.Contains("-"))
                return name;

            var split = name.Split('\'');
            name = Capitalize(split[0]) + "'" + Capitalize(split[1]);

            return name;
        }

        private static string CapitalizeSplitNames(this string name, char splitChar)
        {
            if (name.IsEmpty())
                return null;

            var splitNames = name.Split(splitChar);

            if (splitNames.Length < 2)
                return name;

            var sb = new StringBuilder();
            foreach (var n in splitNames)
            {
                if (n.Contains("'"))
                {
                    var split = n.Split('\'');
                    sb.Append(Capitalize(split[0]) + "'" + Capitalize(split[1]));
                }
                else
                {
                    sb.Append(n.Capitalize());
                    sb.Append(splitChar);
                }
            }
            return sb.ToString().Trim(splitChar);
        }

        private static string Capitalize(this string name)
        {
            if (name.IsEmpty())
                return null;

            if (name.Length < 2)
                return name.ToUpper();

            if (name.ToLower().StartsWith("mc") && name.Length > 2)
                name = "Mc" + name[2].ToString().ToUpper() + name.Substring(3).ToLower();
            else
                name = name[0].ToString().ToUpper() + name.Substring(1).ToLower();
            return name;
        }

        private static string HandleApostropheNearEndOfName(this string name)
        {
            if (name.IsEmpty())
                return null;

            if (name.Length > 2 && name.Substring(name.Length - 2, 1) == "'")
                name = name.Substring(0, name.Length - 1) + name.Substring(name.Length - 1).ToLower();

            return name;
        }

        private static string NullIfContainsInvalidCharacters(this string name)
        {
            if (name.IsEmpty())
                return null;

            var pattern = new Regex(@"^[\p{L}- ']+$");
            return pattern.Match(name).Success ? name : null;
        }
    }
}