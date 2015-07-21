using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBTek.BugGuardian.Config
{
    public class BugGuardianConfiguration : System.Configuration.ConfigurationSection
    {
        private static string BugGuardianConfigurationSectionConst = "BugGuardianConfiguration";

        /// <summary>
        /// Returns an BugGuardianConfiguration instance
        /// </summary>
        public static BugGuardianConfiguration GetConfig()
        {
            return (BugGuardianConfiguration)System.Configuration.ConfigurationManager.GetSection(BugGuardianConfiguration.BugGuardianConfigurationSectionConst) ?? new BugGuardianConfiguration();
        }

        [System.Configuration.ConfigurationProperty("BugGuardianSettings")]
        public BugGuardianSettingCollection BugGuardianSettings
        {
            get
            {
                return (BugGuardianSettingCollection)this["BugGuardianSettings"] ?? new BugGuardianSettingCollection();
            }
        }
    }
}
