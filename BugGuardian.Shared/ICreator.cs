using BugGuardian.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DBTek.BugGuardian
{
    public interface ICreator : IDisposable
    {
        BugGuardianResponse AddBug(Exception ex, string message = null, IEnumerable<string> tags = null);

        Task<BugGuardianResponse> AddBugAsync(Exception ex, string message = null, IEnumerable<string> tags = null);
    }
}