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
            var exceptionHash = Helpers.ExceptionsHelper.BuildExceptionHash(ex);
            int bugID = -1;

            //Check if aready reported
            var avoidMultipleReport = false; //TODO: pick from config
            if (avoidMultipleReport)            
                bugID = await Helpers.WorkItemsHelper.GetExistentBugId(exceptionHash, _account);

            //Create or Update Work Item
            if (bugID > 0)
                return await Helpers.WorkItemsHelper.UpdateBug(bugID);

            return await Helpers.WorkItemsHelper.CreateNewBug(ex, _account, message, tags);
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)                
                    _account = null;                

                disposedValue = true;
            }
        }

        public void Dispose()
            => Dispose(true);        
        #endregion
    }
}
