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
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace NppKate.Modules.SnippetFeature
{

    [Serializable]
    public class NameExistsException : Exception
    {
        public NameExistsException() { }
        public NameExistsException(string message) : base(message) { }
        public NameExistsException(string message, Exception inner) : base(message, inner) { }
        protected NameExistsException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }


    [Serializable]
    public class ShortNameExistsException : Exception
    {
        public ShortNameExistsException() { }
        public ShortNameExistsException(string message) : base(message) { }
        public ShortNameExistsException(string message, Exception inner) : base(message, inner) { }
        protected ShortNameExistsException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    public class SnippetManager : ISnippetManager
    {
        private const string SNIPPETS = "Snippets";
        private const string SNIPPET = "Snippet";
        private const string NAME = "Name";
        private const string ISSHOW = "IsShowInMenu";
        private const string CATEGORY = "Category";
        private const string FILEEXT = "FileExt";
        private const string SHORTNAME = "ShortName";

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
                    _instance = new SnippetManager(Path.Combine(NppUtils.ConfigDir, Properties.Resources.PluginName, Properties.Resources.SnippetsXml));
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
            }
            else
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
                var element = new XElement(SNIPPET, snippet.SnippetText,
                                            new XAttribute(NAME, snippet.Name),
                                            new XAttribute(ISSHOW, snippet.IsShowInMenu),
                                            new XAttribute(CATEGORY, snippet.Category),
                                            new XAttribute(FILEEXT, snippet.FileExt),
                                            new XAttribute(SHORTNAME, snippet.ShortName)
                                            );
                root.Add(element);
                Save();
            }
            else
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
                foreach (var x in _doc.Root.Elements(SNIPPET))
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
            get
            {
                // Full name
                if (_snippets.ContainsKey(index))
                    return _snippets[index];
                else
                {
                    var snip = _snippets.Values.Where(s => s.ShortName == index)?.FirstOrDefault();
                    // Short name
                    if (snip != null)
                        return snip;
                }
                return Snippet.Null;
            }
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
                         select e).ToDictionary(e => e.Attribute(NAME).Value,
                            (e) =>
                            {
                                return new Snippet(e.Attribute(NAME).Value,
                                                   e.Value,
                                                   bool.Parse(e.Attribute(ISSHOW)?.Value ?? "true"),
                                                   e.Attribute(CATEGORY)?.Value,
                                                   e.Attribute(FILEEXT)?.Value,
                                                   e.Attribute(SHORTNAME)?.Value
                                                   );
                            });
        }

        public List<string> GetCategories()
        {
            List<string> result;
            result = (from s in _snippets.Values
                      select s.Category).Distinct().ToList();
            result.Sort();
            return result;
        }

        public List<string> GetExt()
        {
            List<string> result;
            result = (from s in _snippets.Values
                      select s.FileExt).Distinct().ToList();
            if (!result.Contains("*"))
            {
                result.Add("*");
            }
            result.Sort();
            return result;
        }

        public void AddOrUpdate(Snippet snippet)
        {
            throw new NotImplementedException();
        }

        public void Remove(Snippet snippet)
        {
            throw new NotImplementedException();
        }

        public void Remove(string snippetName)
        {
            throw new NotImplementedException();
        }

        public Snippet FindByName(string snippetName)
        {
            throw new NotImplementedException();
        }
    }
}
