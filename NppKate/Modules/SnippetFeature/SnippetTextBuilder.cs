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

using System.Text;
using System.Text.RegularExpressions;

namespace NppKate.Modules.SnippetFeature
{
    public class SnippetTextBuilder : ISnippetTextBuilder
    {
        // Constants
        const string EmptyStringConst = "EMPTY_STRING";
        // Class static private
        private static readonly Regex _simpleParam = new Regex(@"([{])(\d+)([}])");
        private static readonly Regex _autoParam = new Regex(@"($\()(\D+)(\))");
        // Class private
        private readonly IParamFactory _paramFactory;
        private readonly bool _replaceAsEmpty;

        public SnippetTextBuilder(IParamFactory paramFactory, bool replaceAsEmpty)
        {
            _paramFactory = paramFactory;
            _replaceAsEmpty = replaceAsEmpty;
        }

        public string BuildText(Snippet snippet, string paramString)
        {
            var buffer = new StringBuilder(snippet.Text.Length * 2);
            var parametes = SplitParam(paramString);
            ReplaceSimpleParam(ref buffer, snippet.Text, parametes);

            return buffer.ToString();
        }

        private string[] SplitParam(string paramString)
        {
            // Заменить все пробелы шириной 2 и более или "," окруженные на единичные пробелы,
            // а затем поделить строку на параметры
            return Regex.Replace(paramString.Trim(), @"(\s{2,})|(\s*[,]\s*)", " ").Split(' ');
        }

        private void ReplaceSimpleParam(ref StringBuilder buffer, string text, string[] parameters)
        {
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

        private void ReplaceAutoParam(ref StringBuilder buffer)
        {
            var match = _autoParam.Match(buffer.ToString());
            while (match.Success)
            {

                match = match.NextMatch();
            }
        }
    }
}
