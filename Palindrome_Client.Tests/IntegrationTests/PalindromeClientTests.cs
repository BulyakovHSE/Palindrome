using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Palindrome_Client.Tests.IntegrationTests
{
    [TestClass]
    public class PalindromeClientTests
    {
        [TestMethod]
        public async Task IsPalindrome_ReturnTrue()
        {
            var client = new PalindromeClient();
            var value = "А роза упала на лапу Азора";

            var result = await client.IsPalindromeAsync(value);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Value);
        }

        [TestMethod]
        public async Task IsPalindrome_ReturnFalse()
        {
            var client = new PalindromeClient();
            var value = "А наша роза упала на лапу Азора";

            var result = await client.IsPalindromeAsync(value);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Value);
        }

        [TestMethod]
        public async Task IsPalindrome_ThreadLimit_BadRequest()
        {
            var client = new PalindromeClient();
            var value = "А роза упала на лапу Азора";
            var threadCount = int.Parse(ConfigurationManager.AppSettings["ThreadCount"]);

            for (int i = 0; i < threadCount; i++)
            {
                ThreadPool.QueueUserWorkItem((o) =>
                {
                    client.IsPalindromeAsync(value);
                });
            }
            Thread.Sleep(100);
            var result = await client.IsPalindromeAsync(value);

            Assert.IsNull(result);

            Thread.Sleep(1000);
        }

        [TestMethod]
        public async Task IsPalindrome_ThreadLimit_Ok()
        {
            var client = new PalindromeClient();
            var value = "А роза упала на лапу Азора";
            var threadCount = int.Parse(ConfigurationManager.AppSettings["ThreadCount"]);

            for (int i = 0; i < threadCount - 1; i++)
            {
                ThreadPool.QueueUserWorkItem((o) =>
                {
                    client.IsPalindromeAsync(value);
                });
            }
            var result = await client.IsPalindromeAsync(value);
            Console.WriteLine();
            Assert.IsNotNull(result);

            Thread.Sleep(1000);
        }
    }
}