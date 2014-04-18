using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SqlConsole.BizLogic.Exceptions;

namespace SQLConsole.BizLogic.Text
{
    public class TextUtils : ITextUtils
    {
        private const string DelimiterPattern = @"^--\s+([A-Za-z0-9\s]+)\s+$";
        private readonly char[] _trimChars = new[] {'\n', '\r', ' '};
        private readonly Regex _delimiterRegExp = new Regex(DelimiterPattern, RegexOptions.Multiline | RegexOptions.Compiled);

        public IList<SqlScript> GetScripts(string input)
        {
            MatchCollection matches = _delimiterRegExp.Matches(input);

            var code = new string[matches.Count];

            var results = new List<SqlScript>();

            try
            {
                for (int i = 0; i < matches.Count; i++)
                {
                    int startIndex = matches[i].Index + matches[i].Length;
                    
                    int length = (i + 1 == matches.Count
                        ? input.Length
                        : matches[i + 1].Index) - startIndex;

                    code[i] = input.Substring(startIndex, length).TrimEnd(_trimChars).TrimStart(_trimChars);

                    results.Add(new SqlScript
                        {
                            Code = code[i],
                            Name = matches[i].Groups[1].Value
                        });
                }

                return results;
            }
            catch (Exception ex)
            {
                throw new BizLogicException(ExceptionMessages.SqlScriptParsingError, ex);
            }
        }
    }
}