using System;
using System.Threading.Tasks;

namespace DBTek.BugGuardian
{
    public interface ICreator : IDisposable
    {
        void AddBug(Exception ex);
        Task AddBugAsync(Exception ex);
    }
}