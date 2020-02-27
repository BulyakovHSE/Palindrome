using System.Configuration;
using System.Threading;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Palindrome_Server.Controllers;

namespace Palindrome_Server.Tests.Controllers
{
    [TestClass]
    public class PalindromeControllerTests
    {
        [TestMethod]
        public void IsPalindrome_ReturnTrue()
        {
            var value = "А роза упала на лапу Азора";
            var controller = new PalindromeController();

            var result = controller.IsPalindrome(value) as OkNegotiatedContentResult<bool>;

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Content);
        }

        [TestMethod]
        public void IsPalindrome_ReturnFalse()
        {
            var value = "А наша роза упала на лапу Азора";
            var controller = new PalindromeController();

            var result = controller.IsPalindrome(value) as OkNegotiatedContentResult<bool>;

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Content);
        }

        [TestMethod]
        public void IsPalindrome_ThreadLimit_Ok()
        {
            var value = "";
            PalindromeController controller;
            var threadCount = int.Parse(ConfigurationManager.AppSettings["ThreadCount"]);

            for (int i = 0; i < threadCount - 1; i++)
            {
                ThreadPool.QueueUserWorkItem((o) =>
                {
                    controller = new PalindromeController();
                    controller.IsPalindrome(value);
                });
            }
            Thread.Sleep(100);
            controller = new PalindromeController();
            var result = controller.IsPalindrome(value);

            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<bool>));
        }

        [TestMethod]
        public void IsPalindrome_ThreadLimit_BadRequest()
        {
            var value = "";
            PalindromeController controller;
            var threadCount = int.Parse(ConfigurationManager.AppSettings["ThreadCount"]);

            for (int i = 0; i < threadCount; i++)
            {
                ThreadPool.QueueUserWorkItem((o) =>
                {
                    controller = new PalindromeController();
                    controller.IsPalindrome(value);
                });
            }
            Thread.Sleep(100);
            controller = new PalindromeController();
            var result = controller.IsPalindrome(value);

            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
        }
    }
}