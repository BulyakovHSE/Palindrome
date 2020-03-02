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
        private static int _threadCount;
        private static object _lock = new object();

        static PalindromeController()
        {
            var threadCountValue = ConfigurationManager.AppSettings["ThreadCount"];
            _threadCount = int.Parse(threadCountValue);
        }

        [HttpGet]
        public async Task<IHttpActionResult> IsPalindrome(string value)
        {
            if (_actualThreadCount < _threadCount)
            {
                var task = Task.Factory.StartNew(() =>
                {
                    lock (_lock)
                    {
                        _actualThreadCount++;
                    }

                    var result = value.IsPalindrome();

                    Thread.Sleep(1000);

                    lock (_lock)
                    {
                        _actualThreadCount--;
                    }

                    return result;
                });
                var res = await task;

                return Ok(res);
            }

            return BadRequest("Свободных потоков для обработки нет");
        }
    }
}
