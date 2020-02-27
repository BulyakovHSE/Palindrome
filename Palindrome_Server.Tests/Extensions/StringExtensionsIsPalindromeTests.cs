using Microsoft.VisualStudio.TestTools.UnitTesting;
using Palindrome_Server.Extensions;

namespace Palindrome_Server.Tests.Extensions
{
    [TestClass]
    public class StringExtensionsIsPalindromeTests
    {
        [TestMethod]
        public void OneCharString()
        {
            var value = "a";

            var result = value.IsPalindrome();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TwoCharString_True()
        {
            var value = "aa";

            var result = value.IsPalindrome();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TwoCharString_False()
        {
            var value = "ab";

            var result = value.IsPalindrome();

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ThreeCharString_True()
        {
            var value = "aba";

            var result = value.IsPalindrome();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ThreeCharString_False()
        {
            var value = "aab";

            var result = value.IsPalindrome();

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void AFetString_True()
        {
            var value = "А роза упала на лапу Азора";

            var result = value.IsPalindrome();

            Assert.IsTrue(result);
        }

            [TestMethod]
        public void MadamString_True()
        {
            var value = "Madam, I’m Adam";

            var result = value.IsPalindrome();

            Assert.IsTrue(result);
        }
    }
}