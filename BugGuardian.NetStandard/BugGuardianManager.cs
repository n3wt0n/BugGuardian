using DBTek.BugGuardian.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DBTek.BugGuardian
{
    public class BugGuardianManager : IBugGuardianManager
    {
        // Get the alternate credentials that you'll use to access the Visual Studio Online account
        private Account _account;

        public BugGuardianManager()
        {
            _account = Helpers.AccountHelper.GenerateAccount();

            if (_account.IsValid)
                throw new BugGuardianConfigurationException();
        }

        /// <summary>
        /// Add a Bug, with the info about the given Exception. You can optionally indicate a custom error message and a list of tags
        /// </summary>
        /// <param name="ex">The exception to report</param>
        /// <param name="message">Optional message to be added to the WorkItem</param>
        /// <param name="tags">Optional tags (list separated by comma) to be added to the WorkItem</param>
        /// <returns></returns>
        public BugGuardianResponse AddBug(Exception ex, string message = null, IEnumerable<string> tags = null)
            => Task.Factory.StartNew(async delegate
            {
                return await AddWorkItemAsync(WorkItemType.Bug, ex, message, tags);
            }).Unwrap().Result;

        /// <summary>
        /// Add a Bug in async, with the info about the given Exception. You can optionally indicate a custom error message and a list of tags
        /// </summary>
        /// <param name="ex">The exception to report</param>
        /// <param name="message">Optional message to be added to the WorkItem</param>
        /// <param name="tags">Optional tags (list separated by comma) to be added to the WorkItem</param>
        /// <returns></returns>
        public Task<BugGuardianResponse> AddBugAsync(Exception ex, string message = null, IEnumerable<string> tags = null)
                => AddWorkItemAsync(WorkItemType.Bug, ex, message, tags);

        /// <summary>
        /// Add a Task, with the info about the given Exception. You can optionally indicate a custom error message and a list of tags
        /// </summary>
        /// <param name="ex">The exception to report</param>
        /// <param name="message">Optional message to be added to the WorkItem</param>
        /// <param name="tags">Optional tags (list separated by comma) to be added to the WorkItem</param>
        /// <returns></returns>
        public BugGuardianResponse AddTask(Exception ex, string message = null, IEnumerable<string> tags = null)
            => Task.Factory.StartNew(async delegate
            {
                return await AddWorkItemAsync(WorkItemType.Task, ex, message, tags);
            }).Unwrap().Result;

        /// <summary>
        /// Add a Task in async, with the info about the given Exception. You can optionally indicate a custom error message and a list of tags
        /// </summary>
        /// <param name="ex">The exception to report</param>
        /// <param name="message">Optional message to be added to the WorkItem</param>
        /// <param name="tags">Optional tags (list separated by comma) to be added to the WorkItem</param>
        /// <returns></returns>
        public Task<BugGuardianResponse> AddTaskAsync(Exception ex, string message = null, IEnumerable<string> tags = null)
            => AddWorkItemAsync(WorkItemType.Task, ex, message, tags);

        private async Task<BugGuardianResponse> AddWorkItemAsync(WorkItemType workItemType, Exception ex, string message, IEnumerable<string> tags)
        {
            var exceptionHash = Helpers.ExceptionsHelper.BuildExceptionHash(ex);
            WorkItemData bugData = null;

            //Check if aready reported            
            if (Factories.ConfigurationFactory.AvoidMultipleReport)
                bugData = await Helpers.WorkItemsHelper.GetExistentWorkItemId(exceptionHash, workItemType, _account);

            //Create or Update Work Item
            if (bugData?.ID > 0)
                return await Helpers.WorkItemsHelper.UpdateWorkItem(bugData, _account);

            return await Helpers.WorkItemsHelper.CreateNewWorkItem(workItemType, ex, _account, message, tags);
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
