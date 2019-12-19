using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using RFReborn.Extensions;
using RFReborn.Helpers;

namespace RFReborn
{
    /// <summary>
    /// Dynamically replaces parameters / keys in a string with a value based on a <see cref="Func{TResult}"/>
    /// </summary>
    public class StringParameterizer
    {
        private readonly Dictionary<string, Func<string>> _parameterMap;

        /// <summary>
        /// Initializes a new instance of <see cref="StringParameterizer"/> with an empty Map
        /// </summary>
        public StringParameterizer() => _parameterMap = new Dictionary<string, Func<string>>();

        /// <summary>
        /// Initializes a new instance of <see cref="StringParameterizer"/> with an <see cref="IEnumerable{T}"/> of <see cref="KeyValuePair{TKey, TValue}"/> of <see cref="string"/> and <see cref="Func{TResult}"/>
        /// </summary>
        /// <param name="parameters">Parameters to initialize map with</param>
        public StringParameterizer(IEnumerable<KeyValuePair<string, Func<string>>> parameters) => _parameterMap = new Dictionary<string, Func<string>>(parameters);

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
        public void Add(string parameterKey, Func<string> parameterValue) => _parameterMap.Add(parameterKey, parameterValue);

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
            regex = regex.Replace(WildcardString, ".*");
            return new Regex(regex);
        }

        /// <summary>
        /// Makes the formatted string by replacing all parameters with their values
        /// </summary>
        /// <param name="inputName">String to make</param>
        public string Make(string inputName)
        {
            if (_parameterMap.Count == 0)
            {
                return inputName;
            }

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
            if (_parameterMap.Count == 0)
            {
                return inputName;
            }

            Regex formatterRegex = GetRegexFormatter();

            //return formatterRegex.Replace(inputName, m => _parameterMap.ContainsKey(m.Value) ? _parameterMap[m.Value].Invoke() : m.Value);
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

        /// <summary>
        /// Makes the formatted string from a stream by calling the evaluator on every parameter, if this returns null it will replace all parameters with their set values
        /// </summary>
        /// <param name="input">Stream to make formatted string from</param>
        /// <param name="evaluator">Custom evaluator to use before formatting parameters</param>

        public string Make(Stream input, Func<string, string?> evaluator)
        {
            StreamReader reader = new StreamReader(input);
            if (_parameterMap.Count == 0)
            {
                return reader.ReadToEnd();
            }

            StringBuilder sb = new StringBuilder();
            StringBuilder appender = new StringBuilder();
            int size = OpenTag.Length;
            Span<char> buffer = new char[size];
            Span<char> appendBuffer = new char[CloseTag.Length];
            while (true)
            {
                int read = reader.Read(buffer);
                if (read == 0)
                {
                    break;
                }

                if (read == size)
                {
                    if (buffer.FastEquals(OpenTag))
                    {
                        appender.Clear();
                        appender.Append(buffer);
                        while (true)
                        {
                            int appendRead = reader.Read(appendBuffer);
                            if (appendRead == 0)
                            {
                                sb.Append(appender);
                                break;
                            }

                            appender.Append(appendBuffer);

                            if (appendBuffer.FastEquals(CloseTag))
                            {
                                string value = appender.ToString();
                                string? customEvaluator = evaluator(value);
                                if (customEvaluator is { })
                                {
                                    sb.Append(value);
                                }

                                sb.Append(ParameterEvaluator(value));
                                break;
                            }
                        }
                    }
                    else
                    {
                        sb.Append(buffer);
                    }
                }
                else
                {
                    sb.Append(buffer.Slice(0, read));
                }
            }

            return sb.ToString();
        }

        private IEnumerable<string> GetTaggedKeys()
        {
            foreach (string item in _parameterMap.Keys)
            {
                yield return MakeParamFromKey(item);
            }
        }

        private bool ParamIsEnclosed(string param) => param.StartsWith(OpenTag) && param.EndsWith(CloseTag);

        private string MakeParamFromKey(string key) => OpenTag + key + CloseTag;

        private string GetKeyFromParam(string param) => param.Substring(OpenTag.Length, param.Length - CloseTag.Length - OpenTag.Length);

        private string ParameterEvaluator(string parameterName) => _parameterMap.ContainsKey(parameterName) ? _parameterMap[parameterName].Invoke() : parameterName;
    }
}
