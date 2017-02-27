using DBTek.BugGuardian.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DBTek.BugGuardian
{
    public interface IBugGuardianManager : IDisposable
    {
        BugGuardianResponse AddBug(Exception ex, string message = null, IEnumerable<string> tags = null);

        Task<BugGuardianResponse> AddBugAsync(Exception ex, string message = null, IEnumerable<string> tags = null);

        BugGuardianResponse AddTask(Exception ex, string message = null, IEnumerable<string> tags = null);

        Task<BugGuardianResponse> AddTaskAsync(Exception ex, string message = null, IEnumerable<string> tags = null);
    }
}
