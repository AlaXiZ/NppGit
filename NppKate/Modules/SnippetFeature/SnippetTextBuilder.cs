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

using NppKate.Npp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace NppKate.Modules.SnippetFeature
{
    public class SnippetTextBuilder : ISnippetTextBuilder
    {
        // Constants
        const string EmptyStringConst = "EMPTY_STRING";
        const string AutoDate = "$(DATE)";
        const string AutoFileName = "$(FILENAME)";
        const string AutoUsername = "$(USERNAME)";
        // Class static private
        private static readonly Regex _simpleParam = new Regex(@"([{])(\d+?)([}])");
        private static readonly Regex _autoParam = new Regex(@"($\()(\D+)(\))");
        private static readonly Regex _multyLine = new Regex(@"({{)(.*?)(}})", RegexOptions.Multiline);
        private static readonly Regex _snippetParam = new Regex(@"(\$\(SNIPPET:)(.*?)(\))", RegexOptions.IgnoreCase);
        // Class private
        private readonly ISnippetManager _snippetManager;
        private readonly bool _replaceAsEmpty;

        public SnippetTextBuilder(ISnippetManager snippetManager, bool replaceAsEmpty)
        {
            _snippetManager = snippetManager;
            _replaceAsEmpty = replaceAsEmpty;
        }

        public string BuildText(Snippet snippet, string paramString)
        {
            var buffer = new StringBuilder(snippet.Text);
            ExpandSnippet(ref buffer);
            var parametes = SplitParam(paramString);
            ReplaceSimpleParam(ref buffer, parametes);
            ReplaceAutoParam(ref buffer);
            return buffer.ToString();
        }

        private string[] SplitParam(string paramString)
        {
            var buffer = new StringBuilder(paramString);
            var match = _multyLine.Match(paramString);
            var paramNumber = -1;
            var mlParams = new List<string>();
            while (match.Success)
            {
                paramNumber++;
                buffer.Replace(match.Value, $"$_{paramNumber}");
                mlParams.Add(match.Groups[2].Value);
                match = match.NextMatch();
            }
            // Заменить все пробелы шириной 2 и более или "," окруженные на единичные пробелы,
            // а затем поделить строку на параметры         
            var outParams = Regex.Replace(buffer.ToString().Trim(), @"(\s{2,})|(\s*[,]\s*)", " ").Split(' ');
            // Заменим $_{N} на многострочный текст
            if (paramNumber >= 0)
            {
                var paramStr = $"$_{paramNumber}";
                for (int i = outParams.Length - 1; i >= 0 && paramNumber >= 0; i--)
                {
                    if (outParams[i] == paramStr)
                    {
                        outParams[i] = mlParams[paramNumber];
                        paramNumber--;
                        paramStr = $"$_{paramNumber}";
                    }
                }
            }
            return outParams;
        }

        private void ReplaceSimpleParam(ref StringBuilder buffer, string[] parameters)
        {
            var text = buffer.ToString();
            buffer.Clear();
            // Определяем количество параметров
            // Считаем, что сниппет был провалидирован
            int countParams = -1;
            var match = _simpleParam.Match(text);
            while (match.Success)
            {
                int number;
                if (int.TryParse(match.Groups[2].Value, out number) && number > countParams)
                {
                    countParams = number;
                }
                match = match.NextMatch();
            }
            countParams++;
            // Заменять нечего
            if (countParams == 0)
            {
                buffer.Append(text);
                return;
            }
            // Заменяем параметры
            var replaceParam = new string[countParams];
            var emptyString = _replaceAsEmpty ? string.Empty : EmptyStringConst;
            var totalCount = 0;
            var fillCount = 0;
            for (; totalCount < parameters.Length; fillCount = 0)
            {
                if (buffer.Length != 0)
                    buffer.AppendLine(string.Empty);
                for (; fillCount < countParams && totalCount < parameters.Length; fillCount++, totalCount++)
                {
                    replaceParam[fillCount] = parameters[totalCount];
                }
                if (fillCount < countParams)
                {
                    for (; fillCount < countParams; fillCount++)
                    {
                        replaceParam[fillCount] = emptyString;
                    }
                }
                buffer.Append(string.Format(text, replaceParam));
            }
        }

        private void ExpandSnippet(ref StringBuilder buffer)
        {
            if (_snippetManager == null) return;
            var match = _snippetParam.Match(buffer.ToString());
            while (match.Success)
            {
                var snippet = _snippetManager.FindByBothName(match.Groups[2].Value);
                if (snippet != Snippet.Null)
                {
                    buffer.Replace(match.Value, snippet.Text);
                }
                match = match.NextMatch();
            }
        }

        private void ReplaceAutoParam(ref StringBuilder buffer)
        {
            buffer.Replace(AutoDate, DateTime.Now.ToString("dd.MM.yyyy"));
            buffer.Replace(AutoFileName, NppUtils.CurrentFileName);
            buffer.Replace(AutoUsername, Interop.Win32.GetUserNameEx(Interop.ExtendedNameFormat.NameDisplay));
        }
    }
}
