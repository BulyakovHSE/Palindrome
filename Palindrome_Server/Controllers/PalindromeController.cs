using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Palindrome_Server.Extensions;

namespace Palindrome_Server.Controllers
{
    public class PalindromeController : ApiController
    {
        private static int _actualThreadCount;
        private int _threadCount;

        public PalindromeController()
        {
            var threadCountValue = ConfigurationManager.AppSettings["ThreadCount"];
            _threadCount = int.Parse(threadCountValue);
        }

        [HttpGet]
        public async Task<IHttpActionResult> IsPalindrome(string value)
        {
            if (_actualThreadCount < _threadCount)
            {
                _actualThreadCount++;

                var result = value.IsPalindrome();

                await Task.Delay(1000);

                _actualThreadCount--;

                return Ok(result);
            }

            return BadRequest("Свободных потоков для обработки нет");
        }
    }
}
