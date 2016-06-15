using Microsoft.VisualStudio.TestTools.UnitTesting;
using NppKate.Modules.SnippetFeature;

namespace NppKateTest.SnippetTest
{
    [TestClass]
    public class SnippetValidatorTest
    {
        private static readonly ISnippetValidator _validator = new SnippetValidator();

        [TestMethod]
        public void SnippetValidatorValidText()
        {
            var validSnippet = new Snippet("_", "_", "{0}");
            try
            {
                Assert.AreEqual(true, _validator.SnippetIsValid(validSnippet));
            }
            catch
            {
                Assert.Fail("Exception was thrown.");
            }
        }

        [TestMethod]
        public void SnippetValidatorNotValidNegativeNumber()
        {
            var validSnippet = new Snippet("_", "_", "{-1}");
            try
            {
                _validator.SnippetIsValid(validSnippet);
            }
            catch (ParamException e)
            {
                StringAssert.Contains(e.Message, SnippetValidator.ParameterNumberBelow);
                return;
            }
            Assert.Fail("No exception was thrown.");
        }

        [TestMethod]
        public void SnippetValidatorNotValidMissingNumber()
        {
            var validSnippet = new Snippet("_", "_", "{1}");
            try
            {
                _validator.SnippetIsValid(validSnippet);
            }
            catch (ParamException e)
            {
                StringAssert.Contains(e.Message, SnippetValidator.ParameterMissing);
                return;
            }
            Assert.Fail("No exception was thrown.");
        }

        [TestMethod]
        public void SnippetValidatorNotValidNumberAbove()
        {
            var validSnippet = new Snippet("_", "_", "{1024}");
            try
            {
                _validator.SnippetIsValid(validSnippet);
            }
            catch (ParamException e)
            {
                StringAssert.Contains(e.Message, SnippetValidator.ParameterNumberAbove);
                return;
            }
            Assert.Fail("No exception was thrown.");
        }
    }
}
