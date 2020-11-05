using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using RFReborn.Helpers;

namespace RFReborn
{
    /// <summary>
    /// Dynamically replaces parameters / keys in a string with a value based on a <see cref="Func{TResult}"/>
    /// </summary>
    public class StringParameterizer
    {
        private readonly ConcurrentDictionary<string, Func<string>> _parameterMap;

        /// <summary>
        /// Initializes a new instance of <see cref="StringParameterizer"/> with an empty Map
        /// </summary>
        public StringParameterizer() => _parameterMap = new ConcurrentDictionary<string, Func<string>>();

        /// <summary>
        /// Initializes a new instance of <see cref="StringParameterizer"/> with an <see cref="IEnumerable{T}"/> of <see cref="KeyValuePair{TKey, TValue}"/> of <see cref="string"/> and <see cref="Func{TResult}"/>
        /// </summary>
        /// <param name="parameters">Parameters to initialize map with</param>
        public StringParameterizer(IEnumerable<KeyValuePair<string, Func<string>>> parameters) => _parameterMap = new ConcurrentDictionary<string, Func<string>>(parameters);

        /// <summary>
        /// Open Tag / start of a parameter / key
        /// </summary>
        public string OpenTag { get; set; } = "{";

        /// <summary>
        /// Close Tag / end of a parameter / key
        /// </summary>
        public string CloseTag { get; set; } = "}";

        /// <summary>
        /// Wildcard String which will match anything
        /// </summary>
        public string WildcardString { get; set; } = "<!%%%!>";

        /// <summary>
        /// String that represents a Timestamp begin, default "Timestamp"
        /// </summary>
        public string Timestamp { get; set; } = "Timestamp";

        /// <summary>
        /// <see cref="Func{TResult}"/> that returns a <see cref="DateTime"/> which is used when formatting a timestamp, default returns <see cref="DateTime.Now"/>
        /// </summary>
        public Func<DateTime> DateTimeFactory { get; set; } = GetDateTimeNow;

        /// <summary>
        /// Attempts to add a new parameter key and a <see cref="Func{TResult}"/> to retrieve the value
        /// </summary>
        /// <param name="parameterKey">Parameter key</param>
        /// <param name="parameterValue"><see cref="Func{TResult}"/> to retrieve the value</param>
        public bool TryAdd(string parameterKey, Func<string> parameterValue) => _parameterMap.TryAdd(parameterKey, parameterValue);

        /// <summary>
        /// Adds a new parameter key and a <see cref="Func{TResult}"/> to retrieve the value
        /// </summary>
        /// <param name="parameterKey">Parameter key</param>
        /// <param name="parameterValue"><see cref="Func{TResult}"/> to retrieve the value</param>
        public void Add(string parameterKey, Func<string> parameterValue) => TryAdd(parameterKey, parameterValue);

        /// <summary>
        /// Adds a new parameter key and a value
        /// </summary>
        /// <param name="parameterKey">Parameter key</param>
        /// <param name="parameterValue">Value to set</param>
        public void Add(string parameterKey, string parameterValue) => Add(parameterKey, () => parameterValue);

        /// <summary>
        /// Sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to set.</param>
        public Func<string> this[string key]
        {
            set => _parameterMap[key] = value;
        }

        private Regex GetRegexFormatter()
        {
            string regex = string.Join("|", RegexHelper.Escape(GetTaggedKeys()));
            regex = regex.Replace(WildcardString, ".*?");
            return new Regex(regex);
        }

        /// <summary>
        /// Makes the formatted string by replacing all parameters with their values
        /// </summary>
        /// <param name="inputName">String to make</param>
        public string Make(string inputName)
        {
            Regex formatterRegex = GetRegexFormatter();

            return formatterRegex.Replace(inputName, match => ParameterEvaluator(GetKeyFromParam(match.Value)));
        }

        /// <summary>
        /// Makes the formatted string by calling the evaluator on every parameter, if this returns null it will replace all parameters with their set values
        /// </summary>
        /// <param name="inputName">String to make</param>
        /// <param name="evaluator">Custom evaluator to use before formatting parameters</param>
        public string Make(string inputName, Func<string, string?> evaluator)
        {
            Regex formatterRegex = GetRegexFormatter();

            return formatterRegex.Replace(inputName, match =>
            {
                string key = GetKeyFromParam(match.Value);

                string? customEvaluator = evaluator(key);
                if (customEvaluator is { })
                {
                    return customEvaluator;
                }

                return ParameterEvaluator(key);
            });
        }

        private IEnumerable<string> GetKeys()
        {
            // Cheat the timestamp matcher in
            yield return $"{Timestamp} {WildcardString}";

            foreach (string item in _parameterMap.Keys)
            {
                yield return item;
            }
        }

        private IEnumerable<string> GetTaggedKeys()
        {
            foreach (string item in GetKeys())
            {
                yield return MakeParamFromKey(item);
            }
        }

        private bool ParamIsEnclosed(string param) => param.StartsWith(OpenTag) && param.EndsWith(CloseTag);

        private string MakeParamFromKey(string key) => OpenTag + key + CloseTag;

        private string GetKeyFromParam(string param) => param[OpenTag.Length..^CloseTag.Length];

        private string ParameterEvaluator(string parameterName)
        {
            // Check for timestamp begin and format accordingly
            if (parameterName.StartsWith($"{Timestamp} "))
            {
                int len = Timestamp.Length + 1;
                string dateTimeFormat = parameterName.Substring(len, parameterName.Length - len);
                return DateTimeFactory().ToString(dateTimeFormat);
            }

            // If the param map contains the param invoke its func, default just return param
            return _parameterMap.ContainsKey(parameterName) ? _parameterMap[parameterName].Invoke() : parameterName;
        }

        private static DateTime GetDateTimeNow() => DateTime.Now;
    }
}
