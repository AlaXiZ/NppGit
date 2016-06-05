using Microsoft.VisualStudio.TestTools.UnitTesting;
using NppKate.Modules.SnippetFeature;

namespace NppKateTest.SnippetTest
{
    /// <summary>
    /// Сводное описание для SnippetTest
    /// </summary>
    [TestClass]
    public class SnippetTest
    {
        const string SnipName = "snip_name";
        const string ShipShortName = "sh_nm";

        [TestMethod]
        public void SnippetCheckName()
        {
            var snippet = CreateSnippetFirstCtor();
            Assert.AreEqual<string>(snippet.Name, SnipName);
        }

        [TestMethod]
        public void SnippetCheckShortName()
        {
            const string name = "snip_name";
            const string shortName = "sh_nm";
            var snippet = new Snippet(name, "", false, shortName: shortName);
            Assert.AreEqual<string>(snippet.ShortName, shortName);
        }

        [TestMethod]
        public void SnippetCheckDefaultShortName()
        {
            var snippet = CreateSnippetFirstCtor();
            Assert.AreEqual(snippet.ShortName, SnipName);
        }

        [TestMethod]
        public void SnippetCheckDefaultCategory()
        {
            var snippet = CreateSnippetFirstCtor();
            Assert.AreEqual(snippet.Category, "default");
        }

        [TestMethod]
        public void SnippetCheckDefaultExtention()
        {
            var snippet = CreateSnippetFirstCtor();
            Assert.AreEqual(snippet.FileExt, "*");
        }

        private Snippet CreateSnippetFirstCtor()
        {
            return new Snippet(SnipName);
        }
    }
}
