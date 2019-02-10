using System;
using System.Linq;

namespace IdentityServerCli.Console.Extensions
{
    public static class StringExtensions
    {
        private static readonly Func<char, int, string> DashrializeMap = (x, i) =>
                i > 0 && char.IsUpper(x) ?
                "-" + x.ToString() :
                x.ToString();

        public static string Dashrialize(this string str)
        {
            return string.Concat(
                    str.Select(DashrializeMap)
                )
                .ToLowerInvariant();
        }
    }
}