using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace UnityModule.CommandLine {

    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "LoopCanBeConvertedToQuery")]
    [SuppressMessage("ReSharper", "InvertIf")]
    public static class Arguments {

        private static Func<string[]> getCommandLineArguments;

        public static Func<string[]> GetCommandLineArguments {
            get {
                return getCommandLineArguments ?? Environment.GetCommandLineArgs;
            }
            set {
                getCommandLineArguments = value;
                HasParsed = false;
                mainArgumentList = new List<string>();
                switchList = new List<string>();
                optionMap = new Dictionary<string, string>();
            }
        }

        private static bool HasParsed { get; set; }

        private static List<string> mainArgumentList;

        private static List<string> MainArgumentList {
            get {
                if (mainArgumentList == default(List<string>)) {
                    mainArgumentList = new List<string>();
                }
                return mainArgumentList;
            }
        }

        private static List<string> switchList;

        private static List<string> SwitchList {
            get {
                if (switchList == default(List<string>)) {
                    switchList = new List<string>();
                }
                return switchList;
            }
        }

        private static Dictionary<string, string> optionMap;

        private static Dictionary<string, string> OptionMap {
            get {
                if (optionMap == default(Dictionary<string, string>)) {
                    optionMap = new Dictionary<string, string>();
                }
                return optionMap;
            }
        }

        public static List<string> GetMainArgumentList() {
            Parse();
            return MainArgumentList;
        }

        public static bool HasSwitch(string key) {
            return HasSwitch(new [] { key });
        }

        public static bool HasSwitch(IEnumerable<string> keys) {
            Parse();
            foreach (string key in keys) {
                if (SwitchList.Contains(key.Replace("-", string.Empty))) {
                    return true;
                }
            }
            return false;
        }

        public static string GetOption(string key) {
            return GetOption(new [] { key });
        }

        public static string GetOption(IEnumerable<string> keys) {
            Parse();
            foreach (string key in keys) {
                if (OptionMap.ContainsKey(key.Replace("-", string.Empty))) {
                    return OptionMap[key.Replace("-", string.Empty)];
                }
            }
            return null;
        }

        public static string GetOptionString(string key, string defaultValue = default(string)) {
            return GetOptionString(new [] { key }, defaultValue);
        }

        public static string GetOptionString(IEnumerable<string> keys, string defaultValue = default(string)) {
            return GetOption(keys) ?? defaultValue;
        }

        public static int GetOptionInt(string key, int defaultValue = default(int)) {
            return GetOptionInt(new [] { key }, defaultValue);
        }

        public static int GetOptionInt(IEnumerable<string> keys, int defaultValue = default(int)) {
            string value = GetOption(keys);
            if (value == null) {
                return defaultValue;
            }
            if (value == string.Empty) {
                return 0;
            }
            int result;
            int.TryParse(value, out result);
            return result;
        }

        public static bool GetOptionBool(string key, bool defaultValue = default(bool)) {
            return GetOptionBool(new [] { key }, defaultValue);
        }

        public static bool GetOptionBool(IEnumerable<string> keys, bool defaultValue = default(bool)) {
            string value = GetOption(keys);
            if (value == string.Empty) {
                return false;
            }
            return !string.IsNullOrEmpty(value)
                ? Regex.IsMatch(
                    value,
                    "^ *(1|true|y(es)?|ok) *$",
                    RegexOptions.IgnoreCase
                )
                : defaultValue;
        }

        private static void Parse() {
            if (HasParsed) {
                return;
            }
            HasParsed = true;
            string[] arguments = GetCommandLineArguments();
            bool isMainArgument = true;
            string currentKey = string.Empty;
            for (int i = 0; i < arguments.Length; i++) {
                // 先頭から順に "-" で始まらない引数は全てメイン引数と見なす
                if (isMainArgument) {
                    if (!Regex.IsMatch(arguments[i], "^--?[a-zA-Z_]+$")) {
                        MainArgumentList.Add(arguments[i]);
                    } else {
                        // ハイフンを含んだ時点でメイン引数ではなくなったと見なす
                        isMainArgument = false;
                    }
                }
                if (!isMainArgument) {
                    bool isKey = false;
                    // ハイフンで始まる場合はキーであると見なす
                    if (Regex.IsMatch(arguments[i], "^--?[a-zA-Z_]+$")) {
                        currentKey = Regex.Replace(arguments[i], "^--?", string.Empty);
                        isKey = true;
                    }
                    if (isKey && (i + 1 == arguments.Length || Regex.IsMatch(arguments[i + 1], "^-"))) {
                        // キーであり、次の要素が無いかキーである場合はスイッチ要素と見なす
                        SwitchList.Add(currentKey);
                    } else if (!isKey) {
                        string value = arguments[i];
                        if (value == "\"\"" || value == "''") {
                            value = string.Empty;
                        }
                        // キーで無い場合は値を格納する
                        if (OptionMap.ContainsKey(currentKey)) {
                            OptionMap[currentKey] = string.Format("{0} {1}", OptionMap[currentKey], value);
                        } else {
                            OptionMap[currentKey] = value;
                        }
                    }
                }
            }
        }

    }

}