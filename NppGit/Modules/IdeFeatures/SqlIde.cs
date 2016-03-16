using NppGit.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NppGit.Modules.IdeFeatures
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
            _manager.RegisteCommandItem(new CommandItem
            {
                Name = "Swap",
                Hint = "Swap",
                Action = DoSwap
            });

            _manager.RegisteCommandItem(new CommandItem
            {
                Name = "Align columns",
                Hint = "Align columns",
                Action = DoAlign
            });

            // -----------------------------------------------------------------
            _manager.RegisteCommandItem(new CommandItem
            {
                Name = "-",
                Hint = "-",
                Action = null
            });
        }
        // ---------------------------------------------------------------------

        private void DoSwap()
        {
            var eol = PluginUtils.GetEOL();;
            var src = Regex.Replace(PluginUtils.GetSelectedText().Trim().Replace('\t', ' '), @"[ ]{2,}", " ").Replace(eol, "\n");

            var list = (from item in src.Split('\n')
                       select SwapInLine(item.Trim())).ToArray();
            
            AlignColumn(ref list);

            PluginUtils.ReplaceSelectedText(list);
        }

        private void DoAlign()
        {
            var eol = PluginUtils.GetEOL(); ;
            var src = Regex.Replace(PluginUtils.GetSelectedText().Trim().Replace('\t', ' ').Replace(",", " , "), @"[ ]{2,}", " ").Replace(eol, "\n");
            var list = src.Split('\n');

            AlignColumn(ref list);

            PluginUtils.ReplaceSelectedText(list);
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
