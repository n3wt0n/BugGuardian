using DBTek.BugGuardian.Entities;
using DBTek.BugGuardian.Extensions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DBTek.BugGuardian.Helpers
{
    internal class HttpOperationsHelper
    {
        public static async Task<string> GetAsync(HttpClient client, string apiUrl)
        {
            var responseBody = string.Empty;

            using (HttpResponseMessage response = await client.GetAsync(apiUrl))
            {
                response.EnsureSuccessStatusCode();
                responseBody = await response.Content.ReadAsStringAsync();
            }

            return responseBody;
        }

        public static async Task<string> PatchAsync(HttpClient client, string apiUrl, List<APIRequest> requestBody)
        {
            var responseBody = string.Empty;

            var jsonRequest = JsonConvert.SerializeObject(requestBody);

            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");
            using (HttpResponseMessage response = await client.PatchAsync(apiUrl, content))
            {
                response.EnsureSuccessStatusCode();
                responseBody = await response.Content.ReadAsStringAsync();
            }

            return responseBody;
        }

        public static async Task<string> PostAsync(HttpClient client, string apiUrl, APIRequest requestBody)
        {
            var responseBody = string.Empty;

            var jsonRequest = JsonConvert.SerializeObject(requestBody);

            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            using (HttpResponseMessage response = await client.PostAsync(apiUrl, content))
            {
                response.EnsureSuccessStatusCode();
                responseBody = await response.Content.ReadAsStringAsync();
            }

            return responseBody;
        }
    }
}
