using DBTek.BugGuardian.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DBTek.BugGuardian.Helpers
{
    internal class WorkItemsHelper
    {
        //Api version query parameter
        private static string _apiVersion = "api-version=1.0";

        //VSOFields
        private const string TitleField = "/fields/System.Title";
        private const string ReproStepsField = "/fields/Microsoft.VSTS.TCM.ReproSteps";
        private const string SystemInfoField = "/fields/Microsoft.VSTS.TCM.SystemInfo";
        private const string TagsField = "/fields/System.Tags";
        private const string FoundIN = "/fields/Microsoft.VSTS.Build.FoundIn";

        private const string DefaultTags = "BugGuardian;";

        /// <summary>
        /// Check if a bug with the same hash already exists on VSO/TFS
        /// </summary>
        /// <param name="exceptionHash"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public static async Task<int> GetExistentBugId(string exceptionHash, Account account)
        {
            var requestUrlWIQL = $"{account.Url}/{account.CollectionName}/_apis/wit/wiql?{_apiVersion}";

            var workItemQueryPOSTData = new WorkItemWIQLRequest()
            {
                Query = "Select [System.Id] From WorkItems Where [System.WorkItemType] = 'Bug' AND [State] <> 'Done' AND [State] <> 'Removed' " +
                            $"AND [System.TeamProject] = '{account.ProjectName}' AND [Microsoft.VSTS.Build.FoundIn] = '{exceptionHash}'"
            };

            var handlerPre = new HttpClientHandler();

            if (!account.IsVSO) //is TFS, requires NTLM
                handlerPre.Credentials = new System.Net.NetworkCredential(account.Username, account.Password);

            using (HttpClient client = new HttpClient(handlerPre))
            {
                if (account.IsVSO)
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Set alternate credentials
                string credentials = $"{account.Username}:{account.Password}";

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(Converters.StringToAsciiConverter.StringToAscii(credentials)));

                try
                {
                    var responseBody = await HttpOperationsHelper.PostAsync(client, requestUrlWIQL, workItemQueryPOSTData);
                    var responseBodyObj = (dynamic)JsonConvert.DeserializeObject(responseBody);
                    var workItems = (JArray)responseBodyObj.workItems;
                    if (workItems.HasValues)
                        return workItems.First.Value<int>("id");
                }
                catch (Exception)
                {
                }

                return -1;
            }
        }

        /// <summary>
        /// Create a new Bug Work Item with the given values
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="account"></param>
        /// <param name="message"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public static async Task<BugGuardianResponse> CreateNewBug(Exception ex, Account account, string message = null, IEnumerable<string> tags = null)
        {
            //Pattern:
            //PATCH https://{account}.visualstudio.com/defaultcollection/{project}/_apis/wit/workitems/${workitemtypename}?api-version={version}
            //See https://www.visualstudio.com/integrate/api/wit/fields for the fields explanation            
            var requestUrl = $"{account.Url}/{account.CollectionName}/{account.ProjectName}/_apis/wit/workitems/$Bug?{_apiVersion}";

            var workItemCreatePATCHData = new List<VSORequest>();

            //Title: Exception Name
            workItemCreatePATCHData.Add(
                    new WorkItemCreateRequest()
                    {
                        Operation = WITOperationType.add,
                        Path = TitleField,
                        Value = Helpers.ExceptionsHelper.BuildExceptionTitle(ex)
                    });

            //Tags
            workItemCreatePATCHData.Add(
                    new WorkItemCreateRequest()
                    {
                        Operation = WITOperationType.add,
                        Path = TagsField,
                        Value = DefaultTags + (tags != null ? string.Join(";", tags) : String.Empty)
                    });

            //Repro Steps: Stack Trace
            workItemCreatePATCHData.Add(
                    new WorkItemCreateRequest()
                    {
                        Operation = WITOperationType.add,
                        Path = ReproStepsField,
                        Value = Helpers.ExceptionsHelper.BuildExceptionString(ex, message)  // Include custom message, if any
                    });

            //System Info
            workItemCreatePATCHData.Add(
                    new WorkItemCreateRequest()
                    {
                        Operation = WITOperationType.add,
                        Path = SystemInfoField,
                        Value = Helpers.SystemInfoHelper.BuildSystemInfoString()
                    });

            //BISLink: Hash of stack trace
            workItemCreatePATCHData.Add(
                    new WorkItemCreateRequest()
                    {
                        Operation = WITOperationType.add,
                        Path = FoundIN,
                        Value = Helpers.ExceptionsHelper.BuildExceptionHash(ex)
                    });

            var handler = new HttpClientHandler();

            if (!account.IsVSO) //is TFS, requires NTLM
                handler.Credentials = new System.Net.NetworkCredential(account.Username, account.Password);

            using (HttpClient client = new HttpClient(handler))
            {
                if (account.IsVSO)
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Set alternate credentials
                string credentials = $"{account.Username}:{account.Password}";

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(Converters.StringToAsciiConverter.StringToAscii(credentials)));
                try
                {
                    var responseBody = await Helpers.HttpOperationsHelper.PatchAsync(client, requestUrl, workItemCreatePATCHData);
                    return new BugGuardianResponse() { Success = true, Response = responseBody };
                }
                catch (Exception internalException)
                {
                    return new BugGuardianResponse() { Success = false, Response = "An error occured. See the Exception.", Exception = internalException };
                }

            }
        }


        public static async Task<BugGuardianResponse> UpdateBug (int bugID)
        {
            //TODO: Implement
            try
            {
                return new BugGuardianResponse() { Success = true, Response = "XXX" };
            }
            catch (Exception internalException)
            {
                return new BugGuardianResponse() { Success = false, Response = "An error occured. See the Exception.", Exception = internalException };
            }
        }
    }
}
