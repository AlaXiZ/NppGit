using Microsoft.VisualStudio.TestTools.UnitTesting;
using NppKate.Modules.SnippetFeature;

namespace NppKateTest.SnippetTest
{
    [TestClass]
    public class SnippetTextBuilderTest
    {
        private readonly ISnippetTextBuilder _snippetTextBuilder;
        private readonly Snippet _zeroParamSnippet;
        private readonly Snippet _oneParamSnippet;
        private readonly Snippet _twoParamSnippet;

        public SnippetTextBuilderTest()
        {
            _snippetTextBuilder = new SnippetTextBuilder(null, true);
            _zeroParamSnippet = new Snippet("_", "_", "TEXT");
            _oneParamSnippet = new Snippet("_", "_", "{0}");
            _twoParamSnippet = new Snippet("_", "_", "{0} {1}");
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
            Assert.AreEqual(outStr, "1\r\n;\r\n2");
        }

        [TestMethod]
        public void TextBuilderTwoParamEmpty()
        {
            var outStr = _snippetTextBuilder.BuildText(_twoParamSnippet, "");
            Assert.AreEqual(outStr, " ");
        }
    }
}
