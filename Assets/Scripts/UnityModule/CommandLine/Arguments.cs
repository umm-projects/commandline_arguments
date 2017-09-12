using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UnityModule.CommandLine {

    public static class Arguments {

        private static bool hasParsed;

        private static Dictionary<string, string> stringArgumentMap;

        private static Dictionary<string, string> StringArgumentMap {
            get {
                if (stringArgumentMap == default(Dictionary<string, string>)) {
                    stringArgumentMap = new Dictionary<string, string>();
                    Parse();
                }
                return stringArgumentMap;
            }
        }

        private static Dictionary<string, bool> boolArgumentMap;

        private static Dictionary<string, bool> BoolArgumentMap {
            get {
                if (boolArgumentMap == default(Dictionary<string, bool>)) {
                    boolArgumentMap = new Dictionary<string, bool>();
                    Parse();
                }
                return boolArgumentMap;
            }
        }

        public static string GetString(string key) {
            if (StringArgumentMap.ContainsKey(key)) {
                return StringArgumentMap[key];
            }
            return string.Empty;
        }

        public static bool GetBool(string key) {
            if (BoolArgumentMap.ContainsKey(key)) {
                return BoolArgumentMap[key];
            }
            return false;
        }

        private static void Parse() {
            if (hasParsed) {
                return;
            }
            hasParsed = true;
            string[] arguments = System.Environment.GetCommandLineArgs();
            for (int i = 0; i < arguments.Length; i++) {
                if (Regex.IsMatch(arguments[i], "^--?[a-zA-Z_]+$")) {
                    string key = Regex.Replace(arguments[i], "^--?", string.Empty);
                    if (i + 1 == arguments.Length || Regex.IsMatch(arguments[i + 1], "^-")) {
                        // 最後の引数であるか、次の要素も引数名である場合は真偽値と見なす
                        BoolArgumentMap[key] = true;
                    } else {
                        // それ以外は文字列と見なす
                        StringArgumentMap[key] = arguments[++i];
                    }
                }
            }
        }

    }

}