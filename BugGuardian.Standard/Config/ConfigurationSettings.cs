using System;
using System.Collections.Specialized;

namespace DBTek.BugGuardian.Config
{
    public class ConfigurationSettings
    {
        private static object sLockingObject = new object();
        private static BugGuardianSettingsSection BugGuardianSettings = null;

        /// <summary>
        /// Gets the BugGuardian.Configuration.AppSettingsSection data based on the 
        /// machine's codebase for the current application's 
        /// codebase default settings.
        /// In addition gets the System.Configuration.AppSettingSection data for
        /// the current application's default settings if the setting does not 
        /// exist in the BugGuardian.Configuration.AppSettingsSection.
        /// </summary>
        /// <param name="name">The name of the setting to be retreived.</param>
        /// <returns>Returns the setting specified.</returns>
        [System.Diagnostics.DebuggerNonUserCode()]
        public static NameValueCollection AppSettings
        {
            get
            {
                lock (sLockingObject)
                {
                    //If the settings weren't loaded then load them.
                    if (BugGuardianSettings == null)
                    {
                        BugGuardianSettings = new BugGuardianSettingsSection();
                        BugGuardianSettings.GetSettings();
                    }
                }
                return (NameValueCollection)BugGuardianSettings;
            }
        }

        #region Private BugGuardianSettingsSection Class
        private class BugGuardianSettingsSection : NameValueCollection
        {
            /// <summary>
            /// Populates the collection with the BugGuardian and Application Settings 
            /// based on the current codebase.
            /// </summary>
            [System.Diagnostics.DebuggerNonUserCode()]
            public void GetSettings()
            {
                //If the settings collection is not populated, populate it.
                if (base.Count == 0)
                {
                    //Load the BugGuardianConfiguration section from the .Config file.
                    BugGuardianConfiguration ConfigSettings = BugGuardianConfiguration.GetConfig();

                    //Only populate if the section exists.
                    if (ConfigSettings != null)
                    {
                        foreach (BugGuardianSettingElements setting in ConfigSettings.BugGuardianSettings)
                        {
                            base.Add(setting.Key, setting.Value);
                        }
                    }

                    // Load System.ConfigurationManager.AppSettings for 
                    //all settings
                    // not loaded in the BugGuardian Configuration Setting Section.
                    NameValueCollection appSettings = System.Configuration.ConfigurationManager.AppSettings;
                    for (int i = 0; i < appSettings.Count; i++)
                    {
                        string key = appSettings.Keys[i];

                        //If the Key does not exist in the BugGuardian settings add it.
                        if (base[key] == null)
                            base.Add(key, appSettings[i]);
                    }
                }
            }

            #region Overrides
            /// <summary>
            /// This configuration is read only and calling 
            /// this method will throw an exception.
            /// </summary>
            [System.Diagnostics.DebuggerNonUserCode()]
            public override void Clear()
            {
                throw new Exception("The configuration is read only.");
            }

            /// <summary>
            /// This configuration is read only and calling this 
            /// method will throw an exception.
            /// </summary>
            [System.Diagnostics.DebuggerNonUserCode()]
            public override void Add(string name, string value)
            {
                throw new Exception("The configuration is read only.");
            }

            /// <summary>
            /// This configuration is read only and calling this method 
            /// will throw an exception.
            /// </summary>
            [System.Diagnostics.DebuggerNonUserCode()]
            public override void Remove(string name)
            {
                throw new Exception("The configuration is read only.");
            }

            /// <summary>
            /// This configuration is read only and calling this 
            /// method will throw an exception.
            /// </summary>
            [System.Diagnostics.DebuggerNonUserCode()]
            public override void Set(string name, string value)
            {
                throw new Exception("The configuration is read only.");
            }
            #endregion
        }
        #endregion
    }
}
