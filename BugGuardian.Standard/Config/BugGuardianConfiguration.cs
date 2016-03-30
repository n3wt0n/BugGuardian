namespace DBTek.BugGuardian.Config
{
    public class BugGuardianConfiguration : System.Configuration.ConfigurationSection
    {
        private static string BugGuardianConfigurationSectionConst = "BugGuardianConfiguration";

        /// <summary>
        /// Returns an BugGuardianConfiguration instance
        /// </summary>
        public static BugGuardianConfiguration GetConfig()
            => (BugGuardianConfiguration)System.Configuration.ConfigurationManager.GetSection(BugGuardianConfiguration.BugGuardianConfigurationSectionConst) ?? new BugGuardianConfiguration();        

        [System.Configuration.ConfigurationProperty("BugGuardianSettings")]
        public BugGuardianSettingCollection BugGuardianSettings
            => (BugGuardianSettingCollection)this["BugGuardianSettings"] ?? new BugGuardianSettingCollection();                    
    }
}
