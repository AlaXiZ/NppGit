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
using System;
using System.Text.RegularExpressions;

namespace NppKate.Modules.SnippetFeature
{
    [Serializable]
    public class ParamException : Exception
    {
        public ParamException() { }
        public ParamException(string message) : base(message) { }
        public ParamException(string message, Exception inner) : base(message, inner) { }
        protected ParamException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    public class SnippetValidator : ISnippetValidator
    {
        public const string ParameterNumberAbove = "Parameter number above max number";
        public const string ParameterNumberBelow = "Parameter number below zero";
        public const string ParameterMissing = "Parameter is missing";

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private static readonly Regex _paramsSearch = new Regex(@"({)(-?\d+?)(})");
        private static readonly Regex _wrongParam = new Regex(@"([{]+)|([}]+)");
        const int MaxParamCount = 1024;

        public bool SnippetIsValid(Snippet snippet)
        {
            var paramCount = CheckCountParam(snippet.Text) + 1;
            if (paramCount > 0)
            {
                CheckSyntaxCorrect(snippet.Text, paramCount);
            }
            return true;
        }

        private static int CheckCountParam(string text)
        {
            int param = -1;
            int maxParam = -1;
            byte[] mask = new byte[MaxParamCount + 4];
            mask.Initialize();
            var match = _paramsSearch.Match(text);
            while (match.Success)
            {
                if (int.TryParse(match.Groups[2].Value, out param))
                {
                    if (param >= MaxParamCount)
                    {
                        var error = $"{ParameterNumberAbove} ({param} > {MaxParamCount})";
                        logger.Info(error);
                        throw new ParamException(error);
                    }
                    if (param < 0)
                    {
                        var error = $"{ParameterNumberBelow} ({param} < 0)";
                        logger.Info(error);
                        throw new ParamException(error);
                    }
                    maxParam = maxParam < param ? param : maxParam;
                    mask[param] = 1;
                }
                match = match.NextMatch();
            }
            var zeroIndex = Array.IndexOf<byte>(mask, 0);
            if (zeroIndex < maxParam)
            {
                var error = $"{ParameterMissing} ({zeroIndex})";
                logger.Info(error);
                throw new ParamException(error);
            }
            return maxParam;
        }

        private static void CheckSyntaxCorrect(string text, int paramCount)
        {
            var parameters = new object[paramCount];
            string.Format(text, parameters);
        }
    }
}
