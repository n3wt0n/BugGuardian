using DBTek.BugGuardian.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DBTek.BugGuardian
{
    public class Creator
    {
        static String _baseUrl = "https://{0}.visualstudio.com/DefaultCollection";

        // Get the alternate credentials that you'll use to access the Visual Studio Online account.
        static Account _account;

        //Api version query parameter
        static String _apiVersion = "api-version=1.0";

        //VSOFields
        const string TitleField = "/fields/System.Title";
        const string ReproStepsField = "/fields/Microsoft.VSTS.TCM.ReproSteps";
        const string SystemInfoField = "/fields/Microsoft.VSTS.TCM.SystemInfo";
        const string TagsField = "/fields/System.Tags";


        public Creator()
        {
            _account = Helpers.AccountHelper.GenerateAccount();
        }

        public async Task AddBug(Exception ex)
        {
            //Pattern:
            //PATCH https://{account}.visualstudio.com/defaultcollection/{project}/_apis/wit/workitems/${workitemtypename}?api-version={version}

            //Content-Type: application/json-patch+json

            //https://www.visualstudio.com/integrate/api/wit/fields

            var responseBody = String.Empty;
            var requestUrl = "{0}/{1}/_apis/wit/workitems/${2}?{3}";
            requestUrl = String.Format(requestUrl, String.Format(_baseUrl, _account.AccountName), _account.ProjectName, "Bug", _apiVersion);

            var workItemCreatePATCHData = new List<VSORequest>();

            //Title: Exception Name
            workItemCreatePATCHData.Add(
                    new WorkItemCreateRequest()
                    {
                        Operation = WITOperationType.add,
                        Path = TitleField,
                        Value = ex.GetType().ToString() + " - " + ex.Message
                    });

            //Tags
            workItemCreatePATCHData.Add(
                    new WorkItemCreateRequest()
                    {
                        Operation = WITOperationType.add,
                        Path = TagsField,
                        Value = "BugGuardian"
                    });

            //Repro Steps: Stack Trace
            workItemCreatePATCHData.Add(
                    new WorkItemCreateRequest()
                    {
                        Operation = WITOperationType.add,
                        Path = ReproStepsField,
                        Value = Helpers.ExceptionsHelper.BuildExceptionString(ex)
                    });

            //System Info
            workItemCreatePATCHData.Add(
                    new WorkItemCreateRequest()
                    {
                        Operation = WITOperationType.add,
                        Path = SystemInfoField,
                        Value = Helpers.SystemInfoHelper.BuildSystemInfoString()
                    });


            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //Set alternate credentials
                string credentials = string.Format("{0}:{1}", _account.AltUsername, _account.AltPassword);
                
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", 
                    Convert.ToBase64String(DBTek.BugGuardian.Converters.StringToAsciiConverter.StringToAscii(credentials)));

                responseBody = await Helpers.HttpOperationsHelper.PatchAsync(client, requestUrl, workItemCreatePATCHData);

                //var queueResult = JsonConvert.DeserializeObject<BuildRequest>(responseBody);
            }
        }
    }
}
