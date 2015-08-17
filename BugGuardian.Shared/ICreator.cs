using BugGuardian.Shared.Entities;
using System;
using System.Threading.Tasks;

namespace DBTek.BugGuardian
{
    public interface ICreator : IDisposable
    {
        BugGuardianResponse AddBug(Exception ex);
        Task<BugGuardianResponse> AddBugAsync(Exception ex);
    }
}