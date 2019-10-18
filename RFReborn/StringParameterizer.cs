﻿using System;
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
        /// Attempts to add a new parameter key and a <see cref="Func{TResult}"/> to retrieve the value
        /// </summary>
        /// <param name="parameterKey">Parameter key</param>
        /// <param name="parameterValue"><see cref="Func{TResult}"/> to retrieve the value</param>
        public bool TryAdd(string parameterKey, Func<string> parameterValue) => _parameterMap.TryAdd(MakeParamFromKey(parameterKey), parameterValue);

        /// <summary>
        /// Adds a new parameter key and a <see cref="Func{TResult}"/> to retrieve the value
        /// </summary>
        /// <param name="parameterKey">Parameter key</param>
        /// <param name="parameterValue"><see cref="Func{TResult}"/> to retrieve the value</param>
        public void Add(string parameterKey, Func<string> parameterValue) => _parameterMap.Add(MakeParamFromKey(parameterKey), parameterValue);

        /// <summary>
        /// Sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to set.</param>
        public Func<string> this[string key]
        {
            set => _parameterMap[MakeParamFromKey(key)] = value;
        }

        /// <summary>
        /// Makes the formatted string by replacing all parameters with their values
        /// </summary>
        /// <param name="inputName">String to make</param>
        public string Make(string inputName)
        {
            Regex formatterRegex = new Regex(string.Join("|", RegexHelper.Escape(_parameterMap.Keys)));

            return formatterRegex.Replace(inputName, m => _parameterMap[m.Value].Invoke());
        }

        private bool ParamIsEnclosed(string param) => param.StartsWith(OpenTag) && param.EndsWith(CloseTag);

        private string MakeParamFromKey(string key)
        {
            if (ParamIsEnclosed(key))
            {
                return key;
            }

            return OpenTag + key + CloseTag;
        }
    }
}