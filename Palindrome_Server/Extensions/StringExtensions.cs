using System.Linq;
using System.Text.RegularExpressions;

namespace Palindrome_Server.Extensions
{
    public static class StringExtensions
    {
        public static bool IsPalindrome(this string value)
        {
            var replaced = Regex.Replace(value.ToLower(), "[^a-zA-Z0-9а-яА-Я]", "");
            var reversed = new string(replaced.Reverse().ToArray());
            return Equals(replaced, reversed);
        }
    }
}