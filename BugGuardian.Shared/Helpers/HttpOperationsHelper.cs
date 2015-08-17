using DBTek.BugGuardian.Entities;
using DBTek.BugGuardian.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DBTek.BugGuardian.Helpers
{
    internal class HttpOperationsHelper
    {
        public static async Task<String> GetAsync(HttpClient client, String apiUrl)
        {
            var responseBody = String.Empty;

            try
            {
                using (HttpResponseMessage response = await client.GetAsync(apiUrl))
                {
                    response.EnsureSuccessStatusCode();
                    responseBody = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                //TODO: properly catch the exception
            }

            return responseBody;
        }

        public static async Task<String> PatchAsync(HttpClient client, String apiUrl, List<VSORequest> requestBody)
        {
            var responseBody = String.Empty;

            var jsonRequest = JsonConvert.SerializeObject(requestBody);

            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");
            using (HttpResponseMessage response = await client.PatchAsync(apiUrl, content))
            {
                response.EnsureSuccessStatusCode();
                responseBody = await response.Content.ReadAsStringAsync();
            }

            return responseBody;
        }

        public static async Task<String> PostAsync(HttpClient client, String apiUrl, VSORequest requestBody)
        {
            var responseBody = String.Empty;

            var jsonRequest = "[" + JsonConvert.SerializeObject(requestBody) + "]";

            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            try
            {
                using (HttpResponseMessage response = await client.PostAsync(apiUrl, content))
                {
                    response.EnsureSuccessStatusCode();
                    responseBody = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                //TODO: properly catch the exception
            }
            return responseBody;
        }
    }
}
