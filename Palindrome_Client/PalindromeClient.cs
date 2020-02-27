using System;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using static System.Boolean;

namespace Palindrome_Client
{
    public class PalindromeClient
    {
        private HttpClient client;

        public PalindromeClient()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ServerUri"]);
        }

        public async Task<bool?> IsPalindromeAsync(string value)
        {
            bool? result = null;
            var response = await client.GetAsync($"api/palindrome?value={value}");
            if (response.IsSuccessStatusCode)
            {
                var str = await response.Content.ReadAsStringAsync();
                if(TryParse(str, out var palindrome)) result = palindrome;
            }

            return result;
        }
    }
}