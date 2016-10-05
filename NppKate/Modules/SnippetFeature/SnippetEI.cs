using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NppKate.Modules.SnippetFeature
{
    public class SnippetEI
    {
        private ISnippetManager _manager;

        public SnippetEI(ISnippetManager manager)
        {
            _manager = manager;
        }

        /// <summary>
        /// Экспорт сниппетов в файл
        /// </summary>
        /// <param name="fileName"></param>
        public void Export(string fileName)
        {

        }

        /// <summary>
        /// Импорт сниппетов из файла
        /// </summary>
        /// <param name="fileName"></param>
        public void Import(string fileName)
        {
            
        }
    }
}
