using DBTek.BugGuardian.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
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

        //VSTSFields
        private const string TitleField = "/fields/System.Title";
        private const string ReproStepsField = "/fields/Microsoft.VSTS.TCM.ReproSteps";
        private const string SystemInfoField = "/fields/Microsoft.VSTS.TCM.SystemInfo";
        private const string TagsField = "/fields/System.Tags";
        private const string FoundInField = "/fields/Microsoft.VSTS.Build.FoundIn";
        private const string HistoryField = "/fields/System.History";

        private const string DefaultTags = "BugGuardian;";

        /// <summary>
        /// Check if a Bug with the same hash already exists on VSTS/TFS
        /// </summary>
        /// <param name="exceptionHash"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public static async Task<BugData> GetExistentBugId(string exceptionHash, Account account)
            => await GetExistentWorkItemId(exceptionHash, "Bug", account);

        /// <summary>
        /// Check if a Task with the same hash already exists on VSTS/TFS
        /// </summary>
        /// <param name="exceptionHash"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public static async Task<BugData> GetExistentTaskId(string exceptionHash, Account account)
            => await GetExistentWorkItemId(exceptionHash, "Task", account);

        private static async Task<BugData> GetExistentWorkItemId(string exceptionHash, string workItemType, Account account)
        {
            //Pattern:
            //POST https://{account}.visualstudio.com/defaultcollection/_apis/wit/wiql?api-version={version}
            var wiqlRequestUrl = $"{account.Url}/{account.CollectionName}/_apis/wit/wiql?{_apiVersion}";

            var workItemQueryPOSTData = new WorkItemWIQLRequest()
            {
                Query = $"Select [System.Id] From WorkItems Where [System.WorkItemType] = '{workItemType}' AND [State] <> 'Done' AND [State] <> 'Removed' " +
                            $"AND [System.TeamProject] = '{account.ProjectName}' AND [Microsoft.VSTS.Build.FoundIn] = '{exceptionHash}'"
            };

            using (var handler = new HttpClientHandler())
            {
                if (!account.IsVSTS) //is TFS, requires NTLM
                    handler.Credentials = new System.Net.NetworkCredential(account.Username, account.Password);

                using (var client = new HttpClient(handler))
                {
                    if (account.IsVSTS)
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //Set alternate credentials
                    string credentials = $"{account.Username}:{account.Password}";

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                        Convert.ToBase64String(Converters.StringToAsciiConverter.StringToAscii(credentials)));

                    try
                    {
                        var responseBody = await HttpOperationsHelper.PostAsync(client, wiqlRequestUrl, workItemQueryPOSTData);
                        var responseBodyObj = JsonConvert.DeserializeObject<dynamic>(responseBody);
                        var workItems = (JArray)responseBodyObj.workItems;
                        if (workItems.HasValues)
                        {
                            //Retrieve bug data
                            var id = workItems.First.Value<int>("id");

                            //Pattern:                        
                            //GET https://{account}.visualstudio.com/defaultcollection/_apis/wit/WorkItems?id={id}&api-version=1.0
                            var dataRequestUrl = $"{account.Url}/{account.CollectionName}/_apis/wit/WorkItems?id={id}&{_apiVersion}";
                            responseBody = await HttpOperationsHelper.GetAsync(client, dataRequestUrl);
                            var bugData = JsonConvert.DeserializeObject<BugData>(responseBody);

                            //Retrieve bug history
                            //Pattern:                        
                            //GET https://{account}.visualstudio.com/defaultcollection/_apis/wit/WorkItems/{id}/history
                            var historyRequestUrl = $"{account.Url}/{account.CollectionName}/_apis/wit/WorkItems/{id}/history";
                            responseBody = await HttpOperationsHelper.GetAsync(client, historyRequestUrl);
                            responseBodyObj = JsonConvert.DeserializeObject<dynamic>(responseBody);
                            var historyItems = (JArray)responseBodyObj.value;
                            if (historyItems.HasValues)
                                bugData.History = historyItems.ToObject<List<History>>();

                            return bugData;
                        }
                    }
                    catch (Exception)
                    {
                    }

                    return null;
                }
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
            var createRequestUrl = $"{account.Url}/{account.CollectionName}/{account.ProjectName}/_apis/wit/workitems/$Bug?{_apiVersion}";

            var workItemCreatePATCHData = new List<VSTSRequest>();

            //Title: Exception Name
            workItemCreatePATCHData.Add(
                    new WorkItemCreateRequest()
                    {
                        Operation = WITOperationType.add,
                        Path = TitleField,
                        Value = ExceptionsHelper.BuildExceptionTitle(ex)
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
                        Value = ExceptionsHelper.BuildExceptionString(ex, message)  // Include custom message, if any
                    });

            //System Info
            workItemCreatePATCHData.Add(
                    new WorkItemCreateRequest()
                    {
                        Operation = WITOperationType.add,
                        Path = SystemInfoField,
                        Value = SystemInfoHelper.BuildSystemInfoString()
                    });

            //FoundIn: Hash of stack trace
            workItemCreatePATCHData.Add(
                    new WorkItemCreateRequest()
                    {
                        Operation = WITOperationType.add,
                        Path = FoundInField,
                        Value = ExceptionsHelper.BuildExceptionHash(ex)
                    });

            using (var handler = new HttpClientHandler())
            {
                if (!account.IsVSTS) //is TFS, requires NTLM
                    handler.Credentials = new System.Net.NetworkCredential(account.Username, account.Password);

                using (var client = new HttpClient(handler))
                {
                    if (account.IsVSTS)
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //Set alternate credentials
                    string credentials = $"{account.Username}:{account.Password}";

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                        Convert.ToBase64String(Converters.StringToAsciiConverter.StringToAscii(credentials)));
                    try
                    {
                        var responseBody = await HttpOperationsHelper.PatchAsync(client, createRequestUrl, workItemCreatePATCHData);
                        return new BugGuardianResponse() { Success = true, Response = responseBody };
                    }
                    catch (Exception internalException)
                    {
                        return new BugGuardianResponse() { Success = false, Response = "An error occured. See the Exception.", Exception = internalException };
                    }
                }
            }
        }

        /// <summary>
        /// Updates the Title and the History of an Existing Bug
        /// </summary>
        /// <param name="bugData"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public static async Task<BugGuardianResponse> UpdateBug(BugData bugData, Account account)
        {
            // Pattern:
            //PATCH https://{account}.visualstudio.com/defaultcollection/_apis/wit/workitems/{wworkitemid}?api-version={version}                         
            var updateRequestUrl = $"{account.Url}/{account.CollectionName}/_apis/wit/workitems/{bugData.ID}?{_apiVersion}";

            var historyMessage = "Exception thrown again";

            var reportedTimes = (bugData.History?.Where(h => h.Value.Contains(historyMessage)).Count() ?? 0) + 2;
            var newTitle = $"{bugData.Title.Replace($" ({reportedTimes - 1})", string.Empty)} ({reportedTimes})";

            var workItemCreatePATCHData = new List<VSTSRequest>();

            //Update Title
            workItemCreatePATCHData.Add(
                    new WorkItemCreateRequest()
                    {
                        Operation = WITOperationType.add,
                        Path = TitleField,
                        Value = newTitle
                    });

            //Update History
            workItemCreatePATCHData.Add(
                    new WorkItemCreateRequest()
                    {
                        Operation = WITOperationType.add,
                        Path = HistoryField,
                        Value = $"{historyMessage} (Total: {reportedTimes})"
                    });

            using (var handler = new HttpClientHandler())
            {
                if (!account.IsVSTS) //is TFS, requires NTLM
                    handler.Credentials = new System.Net.NetworkCredential(account.Username, account.Password);

                using (var client = new HttpClient(handler))
                {
                    if (account.IsVSTS)
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //Set alternate credentials
                    string credentials = $"{account.Username}:{account.Password}";

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                        Convert.ToBase64String(Converters.StringToAsciiConverter.StringToAscii(credentials)));

                    try
                    {
                        var responseBody = await HttpOperationsHelper.PatchAsync(client, updateRequestUrl, workItemCreatePATCHData);
                        return new BugGuardianResponse() { Success = true, Response = responseBody };
                    }
                    catch (Exception internalException)
                    {
                        return new BugGuardianResponse() { Success = false, Response = "An error occured. See the Exception.", Exception = internalException };
                    }
                }
            }
        }
    }
}
