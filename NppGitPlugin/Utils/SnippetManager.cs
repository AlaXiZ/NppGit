using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace NppGit.Utils
{
    public class SnippetManager
    {
        private const string SNIPPETS = "Snippets";
        private const string SNIPPET = "Snippet";
        private const string NAME = "Name";

        private static SnippetManager _instance;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private Dictionary<string, Snippet> _snippets;
        private XDocument _doc;
        private string _fileName;

        public static SnippetManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SnippetManager(Path.Combine(PluginUtils.ConfigDir, Properties.Resources.PluginName, Properties.Resources.SnippetsXml));
                }
                return _instance;
            }
        }

        private SnippetManager(string fileName)
        {
            _snippets = new Dictionary<string, Snippet>();
            _fileName = fileName;
            if (File.Exists(fileName))
            {
                _doc = XDocument.Load(fileName);
                LoadSnippets();
            } else
            {
                if (!Directory.Exists(Path.GetDirectoryName(fileName)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(fileName));
                }
                _doc = new XDocument();
                _doc.Add(new XElement(SNIPPETS));
            }
        }

        public void AddSnippet(Snippet snippet)
        {
            if (!_snippets.ContainsKey(snippet.Name))
            {
                _snippets.Add(snippet.Name, snippet);
                var root = _doc.Root;
                var element = new XElement(SNIPPET, snippet.SnippetText, new XAttribute(NAME, snippet.Name));
                root.Add(element);
                Save();
            } else
            {
                throw new Exception(string.Format("Snippent with name '{0}' exists", snippet.Name));
            }
        }

        public void UpdateSnippet(string snippetName, Snippet snippet)
        {
            if (_snippets.ContainsKey(snippetName))
            {
                RemoveSnippet(snippetName);
            }
            AddSnippet(snippet);
        }

        public void RemoveSnippet(string snippetName)
        {
            if (_snippets.ContainsKey(snippetName))
            {
                _snippets.Remove(snippetName);
                foreach(var x in _doc.Root.Elements(SNIPPET))
                {
                    if (x.Attribute(NAME).Value == snippetName)
                    {
                        x.Remove();
                        Save();
                        break;
                    }
                }
            }
        }

        public Snippet this[string index]
        {
            get { return _snippets.ContainsKey(index) ? _snippets[index] : Snippet.Null; }
        }

        public Dictionary<string, Snippet> Snippets
        {
            get { return _snippets; }
        }

        public bool Contains(string snippet)
        {
            return _snippets.ContainsKey(snippet);
        }

        private void Save()
        {
            _doc.Save(_fileName);
        }

        private void LoadSnippets()
        {
            _snippets = (from e in _doc.Descendants(SNIPPET)
                         select e).ToDictionary(e => e.Attribute(NAME).Value, (e) => { return new Snippet(e.Attribute(NAME).Value, e.Value); });
        }
    }
}
