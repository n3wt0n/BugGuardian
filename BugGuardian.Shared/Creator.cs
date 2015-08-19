using BugGuardian.Shared.Entities;
using DBTek.BugGuardian.Entities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DBTek.BugGuardian
{
    public class Creator : ICreator
    {
        // Get the alternate credentials that you'll use to access the Visual Studio Online account
        private Account _account;

        //Api version query parameter
        private string _apiVersion = "api-version=1.0";

        //VSOFields
        private const string TitleField = "/fields/System.Title";
        private const string ReproStepsField = "/fields/Microsoft.VSTS.TCM.ReproSteps";
        private const string SystemInfoField = "/fields/Microsoft.VSTS.TCM.SystemInfo";
        private const string TagsField = "/fields/System.Tags";

        private const string DefaultTags = "BugGuardian;";

        public Creator()
        {
            _account = Helpers.AccountHelper.GenerateAccount();
        }

        /// <summary>
        /// Add a Bug, with the info about the given Exception. You can optionally indicate a custom error message and a list of tags
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="message"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public BugGuardianResponse AddBug(Exception ex, string message = null, IEnumerable<string> tags = null)
            => AddBugAsync(ex, message, tags).Result;        

        /// <summary>
        /// Add a Bug in async, with the info about the given Exception. You can optionally indicate a custom error message and a list of tags
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="message"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public async Task<BugGuardianResponse> AddBugAsync(Exception ex, string message = null, IEnumerable<string> tags = null)
        {
            //Pattern:
            //PATCH https://{account}.visualstudio.com/defaultcollection/{project}/_apis/wit/workitems/${workitemtypename}?api-version={version}
            //See https://www.visualstudio.com/integrate/api/wit/fields for the fields explanation

            var requestUrl = $"{_account.Url}/{_account.CollectionName}/{_account.ProjectName}/_apis/wit/workitems/$Bug?{_apiVersion}";

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

            var handler = new HttpClientHandler();

            if (!_account.IsVSO) //is TFS, requires NTLM
                handler.Credentials = new System.Net.NetworkCredential(_account.Username, _account.Password);

            using (HttpClient client = new HttpClient(handler))
            {
                if (_account.IsVSO)
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Set alternate credentials
                string credentials = $"{_account.Username}:{_account.Password}";

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

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _account = null;
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
