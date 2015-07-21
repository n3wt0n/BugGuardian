#if !WINDOWS_APP && !WINDOWS_PHONE_APP
using DBTek.BugGuardian.Config;
#endif
namespace DBTek.BugGuardian.Factories
{
    public class ConfigurationFactory
    {

        public static string AccountName
        {
            get
            {
#if WINDOWS_APP || WINDOWS_PHONE_APP
                return "dbtek";
#else
                return ConfigurationSettings.AppSettings["AccountName"];
#endif
            }
        }

        public static string AlternateUsername
        {
            get
            {
#if WINDOWS_APP || WINDOWS_PHONE_APP
                return "n3wt0n";
#else
                return ConfigurationSettings.AppSettings["AlternateUsername"];
#endif
            }
        }

        public static string AlternatePassword
        {
            get
            {
#if WINDOWS_APP || WINDOWS_PHONE_APP
                return "Gavioli!2015";
#else
                return ConfigurationSettings.AppSettings["AlternatePassword"];
#endif
            }
        }

        public static string ProjectName
        {
            get
            {
#if WINDOWS_APP || WINDOWS_PHONE_APP
                return "Labs";
#else
                return ConfigurationSettings.AppSettings["ProjectName"];
#endif
            }
        }

    }
}
