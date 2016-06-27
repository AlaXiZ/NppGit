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


    [Serializable]
    public class ValidationException : Exception
    {
        public ValidationException() { }
        public ValidationException(string message) : base(message) { }
        public ValidationException(string message, Exception inner) : base(message, inner) { }
        protected ValidationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    public class SnippetManager : ISnippetManager
    {
        private const string SnippetsTag = "Snippets";
        private const string SnippetTag = "Snippet";
        private const string NameTag = "Name";
        private const string ShowTag = "IsVisible";
        private const string CategoryName = "Category";
        private const string FileExtTag = "FileExt";
        private const string ShortNameTag = "ShortName";

        private static Logger logger = LogManager.GetCurrentClassLogger();

        private Dictionary<string, Snippet> _snippets;
        private XDocument _doc;
        private string _fileName;

        public SnippetManager(string fileName)
        {
            _snippets = new Dictionary<string, Snippet>();
            _fileName = fileName;
            if (string.IsNullOrEmpty(_fileName)) return;

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
                _doc.Add(new XElement(SnippetsTag));
            }
        }

        public void AddOrUpdate(Snippet snippet)
        {
            if (_snippets.ContainsKey(snippet.Name))
            {
                Remove(snippet.Name);
            }

            _snippets.Add(snippet.Name, snippet);

            if (string.IsNullOrEmpty(_fileName)) return;
            var root = _doc.Root;
            var element = new XElement(SnippetTag, snippet.Text,
                                        new XAttribute(NameTag, snippet.Name),
                                        new XAttribute(ShowTag, snippet.IsVisible),
                                        new XAttribute(CategoryName, snippet.Category),
                                        new XAttribute(FileExtTag, snippet.FileExt),
                                        new XAttribute(ShortNameTag, snippet.ShortName)
                                        );
            root.Add(element);
            Save();
        }

        public void Remove(string snippetName)
        {
            if (_snippets.ContainsKey(snippetName))
            {
                _snippets.Remove(snippetName);

                if (string.IsNullOrEmpty(_fileName)) return;
                foreach (var x in _doc.Root.Elements(SnippetTag))
                {
                    if (x.Attribute(NameTag).Value == snippetName)
                    {
                        x.Remove();
                        Save();
                        break;
                    }
                }
            }
        }

        private void Save()
        {
            _doc.Save(_fileName);
        }

        public bool Contains(string name)
        {
            return _snippets.Values.FirstOrDefault(s => s.Name == name || s.ShortName == name) != null;
        }

        private void LoadSnippets()
        {
            _snippets = (from e in _doc.Descendants(SnippetTag)
                         select e).ToDictionary(e => e.Attribute(NameTag).Value,
                            (e) =>
                            {
                                return new Snippet(e.Attribute(NameTag).Value,
                                                   e.Attribute(ShortNameTag)?.Value,
                                                   e.Value,
                                                   bool.Parse(e.Attribute(ShowTag)?.Value ?? "true"),
                                                   e.Attribute(CategoryName)?.Value,
                                                   e.Attribute(FileExtTag)?.Value
                                                   );
                            });
        }

        public void Remove(Snippet snippet)
        {
            Remove(snippet.Name);
        }

        public Snippet FindByName(string snippetName)
        {
            return _snippets.Values.FirstOrDefault(s => s.Name == snippetName) ?? Snippet.Null;
        }

        public Snippet FindByShortName(string snippetShortName)
        {
            return _snippets.Values.FirstOrDefault(s => s.ShortName == snippetShortName) ?? Snippet.Null;
        }

        public List<Snippet> GetAllSnippets()
        {
            return _snippets.Values.ToList();
        }

        public Snippet FindByBothName(string name)
        {
            return _snippets.Values.FirstOrDefault(s => s.Name == name || s.ShortName == name) ?? Snippet.Null;
        }
    }
}
