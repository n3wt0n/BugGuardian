using DBTek.BugGuardian.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DBTek.BugGuardian.Helpers
{
    internal class IterationsHelper
    {
        //Api version query parameter
        private static string _apiVersion = "api-version=2.0";

        public async Task<string> GetCurrentIteration(Account account)
        {
            //Pattern:            
            //GET https://{account}.visualstudio.com/{Collection}/{Project}/_apis/work/teamsettings/iterations?$timeframe=current&api-version={version}
            var currentIterationRequestUrl = $"{account.Url}/{account.CollectionName}/{account.ProjectName}/_apis/work/teamsettings/iterations?$timeframe=current&{_apiVersion}";

            using (var handler = new HttpClientHandler())
            {
                if (!account.isAzDO) //is TFS, requires NTLM
                    handler.Credentials = new System.Net.NetworkCredential(account.Username, account.Password);

                using (var client = new HttpClient(handler))
                {
                    if (account.isAzDO)
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //Set alternate credentials
                    string credentials = $"{account.Username}:{account.Password}";

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                        Convert.ToBase64String(Converters.StringToAsciiConverter.StringToAscii(credentials)));

                    try
                    {
                        var responseBody = await HttpOperationsHelper.GetAsync(client, currentIterationRequestUrl);
                        
                        var valueArray = JToken.Parse(responseBody).Value<JArray>("value");
                        var iterationPath = valueArray.First.Value<string>("path");

                        return iterationPath;
                    }
                    catch (Exception) { }

                    return null;
                }
            }
        }
    }
}
