using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NppGit.Utils
{
    public class Snippet
    {
        string _snippetText = "";
        private static Regex _paramsSearch = new Regex(@"[{]\d+[}]");
        private static Regex _wrongParam = new Regex(@"[{]\D*[}]");
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static readonly Snippet Null = new Snippet("");

        public string Name { get; set; }
        public bool IsSimpleSnippet { get { return ParamCount == 0; } }
        public uint ParamCount { get; protected set; }
        public string SnippetText
        {
            get { return _snippetText; }
            set
            {
                _snippetText = value;
                ParamCount = CalcParamCount(_snippetText);
            }
        }

        public Snippet(string name, string snippet)
        {
            Name = name;
            SnippetText = snippet;
        }

        public Snippet(string name) : this(name, "") { }

        public string[] Assemble(string param)
        {
            if (ParamCount > 0)
            {
                param = Regex.Replace(param.Trim(), @"(\s+)|(\s*[,\.:;]\s*)", " ");
                uint count = 0;
                var args = param.Split(' ');
                var formatArgs = new string[ParamCount];
                var outStrings = new List<string>();
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
                        outStrings.Add(string.Format(_snippetText, formatArgs));
                        count = 0;
                    }
                }
                return outStrings.ToArray();
            }
            else
            {
                return _snippetText.Replace("\r", "").Split('\n'); // split by line
            }
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
                logger.Debug("mask >> {0} & 1 = {1}", i, (mask >> i) & 1);
            }
            logger.Debug("In snippet {0} param(s), max param number {1}", matchCount, maxParam);
            result = matchCount == maxParam + 1;
            return result;
        }
    }
}
