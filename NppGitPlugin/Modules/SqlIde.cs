using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NppGit.Modules
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
            _manager.RegisterMenuItem(new MenuItem
            {
                Name = "Swap expression",
                Hint = "Swap expression",
                Action = DoSwap
            });

            // -----------------------------------------------------------------
            _manager.RegisterMenuItem(new MenuItem
            {
                Name = "-",
                Hint = "-",
                Action = null
            });
        }
        // ---------------------------------------------------------------------

        private void DoSwap()
        {
            var src = PluginUtils.GetSelectedText().Trim();
            var eol = PluginUtils.GetEOL();
            var origin_eol = eol;
            // replace end of line
            if (eol == "\r\n")
            {
                src = src.Replace(eol, "\n");
                eol = "\n";
            }

            var list = (from item in src.Split(eol[0])
                       select SwapInLine(item)).ToArray<string>();
            
            AlignColumn(ref list);

            var result = string.Join(origin_eol, list);

            PluginUtils.ReplaceSelectedText(result);
        }

        private static string SwapInLine(string src)
        {
            var src_copy = src.Trim().Replace('\t', ' ') ;
            var tmp = src_copy.Replace("  ", " ");
            while (src_copy != tmp)
            {
                src_copy = tmp;
                tmp = src_copy.Replace("  ", " ");
            }

            var srcArr = src_copy.Split(' ');

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
                var items = line.Split(' ');
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
                lines[i] = buf.ToString().TrimStart(' ');
            }
        }
    }
}
