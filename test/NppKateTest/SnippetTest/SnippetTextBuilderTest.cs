using Microsoft.VisualStudio.TestTools.UnitTesting;
using NppKate.Modules.SnippetFeature;
using System;

namespace NppKateTest.SnippetTest
{
    [TestClass]
    public class SnippetTextBuilderTest
    {
        private readonly ISnippetTextBuilder _snippetTextBuilder;
        private readonly Snippet _zeroParamSnippet;
        private readonly Snippet _oneParamSnippet;
        private readonly Snippet _twoParamSnippet;
        private readonly Snippet _autoDateSnippet;
        private readonly Snippet _snipInSnipFullSnippet;
        private readonly Snippet _snipInSnipShortSnippet;
        private readonly ISnippetManager _manager;

        public SnippetTextBuilderTest()
        {
            _manager = new SnippetManager("");
            _snippetTextBuilder = new SnippetTextBuilder(_manager, true);
            _zeroParamSnippet = new Snippet("ZeroParam", "zp", "TEXT");
            _oneParamSnippet = new Snippet("OneParam", "op", "{0}");
            _twoParamSnippet = new Snippet("TwoParam", "tp", "{0} {1}");
            _autoDateSnippet = new Snippet("AutoDateParam", "adp", "$(DATE)");
            _snipInSnipFullSnippet = new Snippet("SnippetInFullSnippet", "sisf", "$(SNIPPET:ZeroParam)");
            _snipInSnipShortSnippet = new Snippet("SnippetInShortSnippet", "siss", "$(SNIPPET:zp)");

            _manager.AddOrUpdate(_zeroParamSnippet);
            _manager.AddOrUpdate(_oneParamSnippet);
            _manager.AddOrUpdate(_twoParamSnippet);
            _manager.AddOrUpdate(_autoDateSnippet);
            _manager.AddOrUpdate(_snipInSnipFullSnippet);
            _manager.AddOrUpdate(_snipInSnipShortSnippet);
        }

        [TestMethod]
        public void TextBuilderZeroParam()
        {
            var outStr = _snippetTextBuilder.BuildText(_zeroParamSnippet, "");
            Assert.AreEqual(outStr, _zeroParamSnippet.Text);
        }

        [TestMethod]
        public void TextBuilderOneParamEmpty()
        {
            var outStr = _snippetTextBuilder.BuildText(_oneParamSnippet, "");
            Assert.AreEqual(outStr, "");
        }

        [TestMethod]
        public void TextBuilderOneParamOneIn()
        {
            var outStr = _snippetTextBuilder.BuildText(_oneParamSnippet, "1");
            Assert.AreEqual(outStr, "1");
        }

        [TestMethod]
        public void TextBuilderOneParamTwoInWhitespace()
        {
            var outStr = _snippetTextBuilder.BuildText(_oneParamSnippet, "1 2");
            Assert.AreEqual(outStr, "1\r\n2");
        }

        [TestMethod]
        public void TextBuilderOneParamTwoInComma1()
        {
            var outStr = _snippetTextBuilder.BuildText(_oneParamSnippet, "1,2");
            Assert.AreEqual(outStr, "1\r\n2");
        }

        [TestMethod]
        public void TextBuilderOneParamTwoInComma2()
        {
            var outStr = _snippetTextBuilder.BuildText(_oneParamSnippet, "1, 2");
            Assert.AreEqual(outStr, "1\r\n2");
        }

        [TestMethod]
        public void TextBuilderOneParamTwoInComma3()
        {
            var outStr = _snippetTextBuilder.BuildText(_oneParamSnippet, "1 ,2");
            Assert.AreEqual(outStr, "1\r\n2");
        }

        [TestMethod]
        public void TextBuilderOneParamTwoInComma4()
        {
            var outStr = _snippetTextBuilder.BuildText(_oneParamSnippet, "1 , 2");
            Assert.AreEqual(outStr, "1\r\n2");
        }

        [TestMethod]
        public void TextBuilderOneParamOneInNotComma()
        {
            var outStr = _snippetTextBuilder.BuildText(_oneParamSnippet, "1;2");
            Assert.AreEqual(outStr, "1;2");
        }

        [TestMethod]
        public void TextBuilderOneParamOneInNotComma1()
        {
            var outStr = _snippetTextBuilder.BuildText(_oneParamSnippet, "1 ;2");
            Assert.AreEqual(outStr, "1\r\n;2");
        }

        [TestMethod]
        public void TextBuilderOneParamOneInNotComma2()
        {
            var outStr = _snippetTextBuilder.BuildText(_oneParamSnippet, "1; 2");
            Assert.AreEqual(outStr, "1;\r\n2");
        }

        [TestMethod]
        public void TextBuilderOneParamOneInNotComma3()
        {
            var outStr = _snippetTextBuilder.BuildText(_oneParamSnippet, "1 ; 2");
            Assert.AreEqual("1\r\n;\r\n2", outStr);
        }

        [TestMethod]
        public void TextBuilderTwoParamEmpty()
        {
            var outStr = _snippetTextBuilder.BuildText(_twoParamSnippet, "");
            Assert.AreEqual(" ", outStr);
        }

        [TestMethod]
        public void TextBuilderAutoParamDate()
        {
            var outStr = _snippetTextBuilder.BuildText(_autoDateSnippet, "");
            Assert.AreEqual(DateTime.Now.ToString("dd.MM.yyyy"), outStr);
        }

        [TestMethod]
        public void TextBuilderSnippetInSnippetFullName()
        {
            var outStr = _snippetTextBuilder.BuildText(_snipInSnipFullSnippet, "");
            Assert.AreEqual(_zeroParamSnippet.Text, outStr);
        }

        [TestMethod]
        public void TextBuilderSnippetInSnippetShortName()
        {
            var outStr = _snippetTextBuilder.BuildText(_snipInSnipShortSnippet, "");
            Assert.AreEqual(_zeroParamSnippet.Text, outStr);
        }

        [TestMethod]
        public void TextBuilderSnippetSingleLineOneMultiParam()
        {
            var outStr = _snippetTextBuilder.BuildText(_oneParamSnippet, "{{1 2 3}}");
            Assert.AreEqual("1 2 3", outStr);
        }

        [TestMethod]
        public void TextBuilderSnippetMultyLineOneMultiParam()
        {
            var outStr = _snippetTextBuilder.BuildText(_oneParamSnippet, "{{1\r\n2}}");
            Assert.AreEqual("1\r\n2", outStr);
        }

        [TestMethod]
        public void TextBuilderSnippetMultyLineTwoMultiParam()
        {
            var outStr = _snippetTextBuilder.BuildText(_twoParamSnippet, "{{1\r\n2}} {{2\r\n3}}");
            Assert.AreEqual("1\r\n2 2\r\n3", outStr);
        }
    }
}
