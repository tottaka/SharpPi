using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPi.Text
{
    public static class String
    {
        /// <summary>
        /// Returns the number of times any of the strings supplied to <paramref name="any"/> occure in the input string.
        /// </summary>
        public static int CountOf(this string input, params string[] any)
        {
            int count = 0;

            foreach (string str in any)
                count += str.AllIndexesOf(str).Length;

            return count;
        }

        public static int[] AllIndexesOf(this string str, string value)
        {
            List<int> indexes = new List<int>();

            for (int index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index);
                if (index == -1)
                    break;
                indexes.Add(index);
            }

            return indexes.ToArray();
        }

        public static string[] RemoveEmptyEntries(this string[] input)
        {
            List<string> output = new List<string>();
            foreach(string str in input)
                if (!string.IsNullOrWhiteSpace(str))
                    output.Add(str);
            return output.ToArray();
        }
    }
}
