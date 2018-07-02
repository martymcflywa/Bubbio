namespace Bubbio.Core.Helpers
{
    public static class StringEx
    {
        public static bool IsEmpty(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        public static string NullIfEmpty(this string str)
        {
            return str.IsEmpty() ? null : str;
        }
    }
}