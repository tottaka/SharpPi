using System;
using System.Collections.Generic;
using System.Text;

namespace SharpPi.Text
{
    public static class DictionaryExt
    {
        /// <summary>
        /// For each line in the specified <paramref name="lines"/> parameter, the line will get split by the specified <paramref name="splitChar"/> character.
        /// The left side will become the key, and the right side will be the value returned by the <paramref name="parseValue"/> function, giving you the ability to do custom value parsing.
        /// </summary>
        public static Dictionary<string, T> GetKeyValuePairs<T>(string[] lines, char splitChar, Func<string, T> parseValue) where T : class
        {
            Dictionary<string, T> dict = new Dictionary<string, T>();
            foreach (string line in lines)
            {
                string[] kvPair = line.Split(new char[] { splitChar }, 2, StringSplitOptions.RemoveEmptyEntries);
                dict.Add(kvPair[0].Trim(), kvPair.Length > 1 ? parseValue(string.Join(splitChar.ToString(), kvPair, 1, kvPair.Length - 1)) : null);
            }
            return dict;
        }

        /// <summary>
        /// For each element in the supplied <see cref="Dictionary{TKey, TValue}"/>, <paramref name="writeAction"/> will be called, supplying the method with formatted text of the Key and Value.
        /// </summary>
        /// <param name="showType">Whether or not to also include the key and value object types in the formatted text for each element.</param>
        public static void Dump<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Action<string> writeAction, bool showType = false)
        {
            foreach (KeyValuePair<TKey, TValue> item in dictionary)
            {
                if (showType)
                    writeAction(string.Format("{0}<{1}>: {2}<{3}>", item.Key.ToString(), item.Key.GetType().Name, item.Value.ToString(), item.Value.GetType().Name));
                else
                    writeAction(string.Format("{0}: {1}", item.Key.ToString(), item.Value.ToString()));
            }
        }
    }
}
