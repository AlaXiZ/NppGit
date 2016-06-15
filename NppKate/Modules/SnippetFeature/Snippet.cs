/*
Copyright (c) 2015-2016, Schadin Alexey (schadin@gmail.com)
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted 
provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this list of conditions 
and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions 
and the following disclaimer in the documentation and/or other materials provided with 
the distribution.

3. Neither the name of the copyright holder nor the names of its contributors may be used to endorse 
or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR 
IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND 
FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR 
CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL 
DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, 
DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER 
IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF 
THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using NLog;
using NppKate.Npp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace NppKate.Modules.SnippetFeature
{
    public class Snippet
    {
        const string AutoDate = "$(DATE)";
        const string AutoFileName = "$(FILENAME)";
        const string AutoUsername = "$(USERNAME)";

        const string DefaultCategory = "default";
        const string DefaultFileExtention = "*";

        string _snippetText = "";
        private static Regex _paramsSearch = new Regex(@"[{]\d+[}]");
        private static Regex _wrongParam = new Regex(@"[{]\D*[}]");
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static readonly Snippet Null = new Snippet("", "", "");

        public string Name { get; set; }
        [Obsolete("This property is deprecated.", false)]
        public bool IsSimpleSnippet { get { return ParamCount == 0; } }
        [Obsolete("This property is deprecated.", false)]
        public uint ParamCount { get; protected set; }
        [Obsolete("This property is deprecated. Use property Text", false)]
        public string SnippetText
        {
            get { return Text; }
            set { Text = value; }
        }
        public bool IsShowInMenu { get; set; }
        public string Category { get; set; }
        public string FileExt { get; set; }
        public string ShortName { get; set; }
        public string Text
        {
            get { return _snippetText; }
            set
            {
                _snippetText = value;
            }
        }

        [Obsolete("This constructor is deprecated", false)]
        public Snippet(string name) : this(name, name, string.Empty) { }

        [Obsolete("This constructor is deprecated", false)]
        public Snippet(string name, string snippet, bool isShowInMenu, string category = null, string fileExt = null, string shortName = null)
            : this(name, shortName ?? name, snippet, isShowInMenu, category ?? DefaultCategory, fileExt ?? DefaultFileExtention) { }

        public Snippet(string name, string shortName, string text, bool isVisible = false, string category = DefaultCategory, string fileExt = DefaultFileExtention)
        {
            Name = name;
            ShortName = !string.IsNullOrEmpty(shortName) ? shortName : name;
            Text = text;
            IsShowInMenu = isVisible;
            Category = !string.IsNullOrEmpty(category) ? category : DefaultCategory;
            FileExt = !string.IsNullOrEmpty(fileExt) ? fileExt : DefaultFileExtention;
        }

        public string[] Assemble(string param)
        {
            var snippetText = ReplaceAutoParam(_snippetText);

            if (ParamCount > 0)
            {
                param = Regex.Replace(param.Trim(), @"(\s+)|(\s*[,;]\s*)", " ");
                var formatArgs = new string[ParamCount];
                var outStrings = new List<string>();
                if (string.IsNullOrEmpty(param))
                {
                    for (int i = 0; i < formatArgs.Length; i++)
                    {
                        formatArgs[i] = Settings.Snippets.InsertEmpty ? "EMPTY_PARAM" : "";
                    }
                    outStrings.Add(string.Format(snippetText, formatArgs));
                }
                else
                {
                    uint count = 0;
                    var args = param.Split(' ');
                    for (int i = 0; i < args.Length; i++)
                    {
                        formatArgs[count] = args[i];
                        count++;
                        if (i + 1 == args.Length)
                        {
                            for (uint j = count; j < ParamCount; j++)
                            {
                                formatArgs[j] = "EMPTY_PARAM";
                            }
                            count = ParamCount;
                        }
                        if (count == ParamCount)
                        {
                            outStrings.Add(string.Format(snippetText, formatArgs));
                            count = 0;
                        }
                    }
                }
                return outStrings.ToArray();
            }
            else
            {
                return snippetText.Replace("\r", "").Split('\n'); // split by line
            }
        }

        private static string ReplaceAutoParam(string source)
        {
            var result = new StringBuilder(source);

            if (source.Contains(AutoDate))
            {
                result.Replace(AutoDate, DateTime.Now.ToString("dd.MM.yyyy"));
            }
            if (source.Contains(AutoFileName))
            {
                result.Replace(AutoFileName, NppUtils.CurrentFileName);
            }
            if (source.Contains(AutoUsername))
            {
                result.Replace(AutoUsername, Interop.Win32.GetUserNameEx(Interop.ExtendedNameFormat.NameDisplay));
            }

            return result.ToString();
        }

        private static byte CalcParamCount(string snippet)
        {
            byte param = 0;
            long mask = 0;
            var m = _paramsSearch.Match(snippet);
            while (m.Success)
            {
                if (byte.TryParse(m.Value.Replace("{", "").Replace("}", ""), out param))
                {
                    if (param > 63)
                    {
                        throw new Exception("Amount of parameters above 64");
                    }
                    mask |= (1L << param);
                    logger.Debug("Match: {0}", m.Value);
                }
                m = m.NextMatch();

            }
            byte matchCount = 0;
            for (byte i = 0; i < 64; i++)
            {
                matchCount += (byte)((mask >> i) & 1);
            }
            logger.Debug("In snippet {0} params", matchCount);
            return matchCount;
        }

        public static bool CheckCorrectSnippet(string snippet)
        {
            var result = true;
            var match = _wrongParam.Match(snippet);
            result = !match.Success;
            if (!result)
            {
                logger.Debug("Wrong param in snippet: {0}", snippet);
                return result;
            }
            // Amount of params
            byte param = 0;
            long mask = 0;
            short maxParam = -1;
            match = _paramsSearch.Match(snippet);
            while (match.Success)
            {
                if (byte.TryParse(match.Value.Replace("{", "").Replace("}", ""), out param))
                {
                    if (param > 63)
                    {
                        logger.Debug("Param number above 63 ({0})", param);
                        result = false;
                        break;
                    }
                    mask |= (1L << param);
                    maxParam = Math.Max(param, maxParam);
                }
                match = match.NextMatch();

            }
            if (!result)
                return result;
            byte matchCount = 0;
            for (byte i = 0; i < 64; i++)
            {
                matchCount += (byte)((mask >> i) & 1);
                //logger.Debug($"mask >> {i} & 1 = {(mask >> i) & 1}");
            }
            logger.Debug($"In snippet {matchCount} param(s), max param number {maxParam}");
            result = matchCount == maxParam + 1;
            return result;
        }
    }

    public class SnippetNew
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Text { get; set; }
    }
}
