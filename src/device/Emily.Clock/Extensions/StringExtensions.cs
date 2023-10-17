using System.Text;

// ReSharper disable once CheckNamespace
namespace System
{
    public static class StringExtensions
    {
        public static string Replace(this string s, char oldChar, char newChar)
        {
            var stringBuilder = new StringBuilder(s);
            stringBuilder.Replace(oldChar, newChar);
            return stringBuilder.ToString();
        }

        public static string Replace(this string s, string oldValue, string newValue)
        {
            var stringBuilder = new StringBuilder(s);
            stringBuilder.Replace(oldValue, newValue);
            return stringBuilder.ToString();
        }
    }
}
