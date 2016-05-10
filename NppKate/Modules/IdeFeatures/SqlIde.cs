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

using NppKate.Common;
using NppKate.Npp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NppKate.Modules.IdeFeatures
{
    public class SqlIde : IModule
    {
        private IModuleManager _manager;

        public bool IsNeedRun
        {
            get { return Settings.Modules.SQLIDE; }
        }

        public void Final()
        {

        }

        public void Init(IModuleManager manager)
        {
            _manager = manager;
            _manager.RegisterCommandItem(new CommandItem
            {
                Name = "Swap",
                Hint = "Swap",
                Action = DoSwap
            });

            _manager.RegisterCommandItem(new CommandItem
            {
                Name = "Align columns",
                Hint = "Align columns",
                Action = DoAlign
            });

            // -----------------------------------------------------------------
            _manager.RegisterCommandItem(new CommandItem
            {
                Name = "-",
                Hint = "-",
                Action = null
            });
        }
        // ---------------------------------------------------------------------

        private void DoSwap()
        {
            var eol = NppUtils.GetEOL();;
            var src = Regex.Replace(NppUtils.GetSelectedText().Trim().Replace('\t', ' '), @"[ ]{2,}", " ").Replace(eol, "\n");

            var list = (from item in src.Split('\n')
                       select SwapInLine(item.Trim())).ToArray();
            
            AlignColumn(ref list);

            NppUtils.ReplaceSelectedText(list);
        }

        private void DoAlign()
        {
            var eol = NppUtils.GetEOL(); ;
            var src = Regex.Replace(NppUtils.GetSelectedText().Trim().Replace('\t', ' ').Replace(",", " , "), @"[ ]{2,}", " ").Replace(eol, "\n");
            var list = src.Split('\n');

            AlignColumn(ref list);

            NppUtils.ReplaceSelectedText(list);
        }

        private static string SwapInLine(string src)
        {
            var srcArr = src.Split(' ');

            if (srcArr.Length == 3)
            {
                return string.Format("{2} {1} {0}", srcArr);
            }
            else {
                return src;
            }
        }

        private static void AlignColumn(ref string[] lines)
        {
            var list = new List<string[]>();
            var lens = new List<int>();
            foreach(var line in lines)
            {
                var items = line.Trim().Split(' ');
                var lenCount = lens.Count;
                for (int i = 0; i < items.Length - lenCount; i++)
                {
                    lens.Add(0);
                }
                for (int i = 0; i < items.Length; i++)
                {
                    lens[i] = Math.Max(lens[i], items[i].Length);
                }
                list.Add(items);
            }
            var buf = new StringBuilder();

            for(int i = 0; i < lines.Length; i++)
            {
                buf.Clear();
                var items = list[i];
                for(int j = 0; j < items.Length; j++)
                {
                    buf.Append(" ").
                        Append(items[j]).
                        Append(new string(' ', lens[j] - items[j].Length));
                }
                lines[i] = buf.ToString().Trim();
            }
        }
    }
}
