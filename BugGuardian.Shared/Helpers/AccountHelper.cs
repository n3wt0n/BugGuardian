using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBTek.BugGuardian.Factories;
using DBTek.BugGuardian.Entities;

namespace DBTek.BugGuardian.Helpers
{
    public class AccountHelper
    {
        public static Account GenerateAccount()
        {
            return new Account()
            {
                AccountName = ConfigurationFactory.AccountName,
                AltUsername = ConfigurationFactory.AlternateUsername,
                AltPassword = ConfigurationFactory.AlternatePassword,
                ProjectName = ConfigurationFactory.ProjectName
            };
        }
    }
}
