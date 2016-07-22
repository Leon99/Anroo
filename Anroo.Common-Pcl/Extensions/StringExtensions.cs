using System;

namespace Anroo.Common
{
    public static class StringExtensions
    {
        public static string RemoveEnd(this string s, int count)
        {
            var substringLength = s.Length - count;
            if (substringLength < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            return s.Substring(0, substringLength);
        }
    }
}