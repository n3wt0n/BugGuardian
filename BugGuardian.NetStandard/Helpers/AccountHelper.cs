using DBTek.BugGuardian.Entities;
using DBTek.BugGuardian.Factories;

namespace DBTek.BugGuardian.Helpers
{
    internal class AccountHelper
    {
        public static Account GenerateAccount()
            => new Account()
            {
                Url = ConfigurationFactory.Url,
                Username = ConfigurationFactory.Username,
                Password = ConfigurationFactory.Password,
                CollectionName = ConfigurationFactory.CollectionName,
                ProjectName = ConfigurationFactory.ProjectName
            };
    }
}
