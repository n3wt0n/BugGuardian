using DBTek.BugGuardian.Entities;
using DBTek.BugGuardian.Factories;

namespace DBTek.BugGuardian.Helpers
{
    public class AccountHelper
    {
        public static Account GenerateAccount()
            => new Account()
            {
                AccountName = ConfigurationFactory.AccountName,
                AltUsername = ConfigurationFactory.AlternateUsername,
                AltPassword = ConfigurationFactory.AlternatePassword,
                ProjectName = ConfigurationFactory.ProjectName
            };        
    }
}
