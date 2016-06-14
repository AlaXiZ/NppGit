using Microsoft.VisualStudio.TestTools.UnitTesting;
using NppKate.Modules.SnippetFeature;

namespace NppKateTest.SnippetTest
{
    [TestClass]
    public class SnippetTest
    {
        const string SnipName = "snip_name";
        const string SnipShortName = "sn_nm";
        const string SnipCategory = "sn_category";
        const string SnipFileExt = "test";

        [TestMethod]
        public void SnippetCheckName()
        {
            var snippet = CreateSnippetDefaultCtor();
            Assert.AreEqual<string>(snippet.Name, SnipName);
        }

        [TestMethod]
        public void SnippetCheckShortName()
        {
            var snippet = new Snippet(SnipName, SnipShortName, "");
            Assert.AreEqual<string>(snippet.ShortName, SnipShortName);
        }

        [TestMethod]
        public void SnippetCheckDefaultShortName()
        {
            var snippet = new Snippet(SnipName, "", "");
            Assert.AreEqual(snippet.ShortName, SnipName);
        }

        [TestMethod]
        public void SnippetCheckDefaultCategory()
        {
            var snippet = CreateSnippetDefaultCtor();
            Assert.AreEqual(snippet.Category, "default");
        }

        [TestMethod]
        public void SnippetCheckDefaultExtention()
        {
            var snippet = CreateSnippetDefaultCtor();
            Assert.AreEqual(snippet.FileExt, "*");
        }

        [TestMethod]
        public void SnippetCheckDefaultMenuVisible()
        {
            var snippet = CreateSnippetDefaultCtor();
            Assert.IsTrue(!snippet.IsShowInMenu);
        }

        [TestMethod]
        public void SnippetChekcCategory()
        {
            var snippet = CreateSnippetFull();
            Assert.AreEqual(snippet.Category, SnipCategory);
        }

        [TestMethod]
        public void SnippetCheckExtention()
        {
            var snippet = CreateSnippetFull();
            Assert.AreEqual(snippet.FileExt, SnipFileExt);
        }

        [TestMethod]
        public void SnippetCheckMenuVisible()
        {
            var snippet = CreateSnippetFull();
            Assert.IsTrue(snippet.IsShowInMenu);
        }

        private Snippet CreateSnippetDefaultCtor()
        {
            return new Snippet(SnipName, SnipShortName, "");
        }

        private Snippet CreateSnippetFull()
        {
            return new Snippet(SnipName, SnipShortName, "", true, SnipCategory, SnipFileExt);
        }
    }
}
