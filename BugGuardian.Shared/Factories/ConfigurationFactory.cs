#if !WINDOWS_APP && !WINDOWS_PHONE_APP && !WINDOWS_UWP
using DBTek.BugGuardian.Config;
#endif
using System;

namespace DBTek.BugGuardian.Factories
{
    public class ConfigurationFactory
    {
        private const string DefaultCollectionName = "DefaultCollection";

        /// <summary>
        /// Allows to set the condifuration from code. If used, overrides the configuration present in the config file
        /// </summary>
        /// <param name="url">The url of VSO or the TFS Server to target</param>
        /// <param name="username">The username of the account used to connect to the service</param>
        /// <param name="password">The password of the account used to connect to the service</param>        
        /// <param name="projectName">The name of the Team Project where the bugs will be open</param>
        /// <param name="avoidMultipleReport">If true, if the application throws the same exception more the once it will be reported only once. If false, every time will be created a new Bug to VSO/TFS.</param>
        public static void SetConfiguration(string url, string username, string password, string projectName, bool avoidMultipleReport = true)
            => SetConfiguration(url, username, password, null, projectName);

        /// <summary>
        /// Allows to set the condifuration from code. If used, overrides the configuration present in the config file
        /// </summary>
        /// <param name="url">The url of VSO or the TFS Server to target</param>
        /// <param name="username">The username of the account used to connect to the service</param>
        /// <param name="password">The password of the account used to connect to the service</param>
        /// <param name="collectionName">The name of the Team Collection that contains the Team Project</param>
        /// <param name="projectName">The name of the Team Project where the bugs will be open</param>
        /// <param name="avoidMultipleReport">If true, if the application throws the same exception more the once it will be reported only once. If false, every time will be created a new Bug to VSO/TFS.</param>
        public static void SetConfiguration(string url, string username, string password, string collectionName, string projectName, bool avoidMultipleReport = true)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException(nameof(url));

            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException(nameof(username));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException(nameof(password));

            if (string.IsNullOrWhiteSpace(projectName))
                throw new ArgumentNullException(nameof(projectName));

            _url = url;
            _username = username;
            _password = password;
            _collectionName = collectionName ?? DefaultCollectionName;
            _projectName = projectName;
            _avoidMultipleReport = avoidMultipleReport;
        }

        private static string _url;
        internal static string Url
        {
            get
            {
#if WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP
                return CleanUrl(_url);
#else
                return CleanUrl(_url ?? ConfigurationSettings.AppSettings["Url"]);
#endif
            }
        }

        private static string _username;
        internal static string Username
        {
            get
            {
#if WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP
                return _username;
#else
                return _username ?? ConfigurationSettings.AppSettings["Username"];
#endif
            }
        }

        private static string _password;
        internal static string Password
        {
            get
            {
#if WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP
                return _password;
#else
                return _password ?? ConfigurationSettings.AppSettings["Password"];
#endif
            }
        }

        private static string _collectionName;
        internal static string CollectiontName
        {
            get
            {
#if WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP
                return _collectionName;
#else
                return (_collectionName ?? ConfigurationSettings.AppSettings["CollectionName"]) ?? DefaultCollectionName;
#endif
            }
        }

        private static string _projectName;
        internal static string ProjectName
        {
            get
            {
#if WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP
                return _projectName;
#else
                return _projectName ?? ConfigurationSettings.AppSettings["ProjectName"];
#endif
            }
        }

        private static bool? _avoidMultipleReport;
        internal static bool AvoidMultipleReport
        {
            get
            {                
#if WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP
                return _avoidMultipleReport ?? true;
#else
                return _avoidMultipleReport ?? bool.Parse(ConfigurationSettings.AppSettings["AvoidMultipleReport"] ?? "true");
#endif
            }
        }

        private static string CleanUrl(string url)
        {
            url = url.Replace(@"\", "/").ToLower();

            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
                url = $"http://{url}";

            if (url.EndsWith("/"))
                url = url.TrimEnd('/');
                    
            return url;
        }
    }
}
