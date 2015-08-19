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
        public static void SetConfiguration(string url, string username, string password, string projectName)
            => SetConfiguration(url, username, password, null, projectName);
        
        /// <summary>
        /// Allows to set the condifuration from code. If used, overrides the configuration present in the config file
        /// </summary>
        /// <param name="url">The url of VSO or the TFS Server to target</param>
        /// <param name="username">The username of the account used to connect to the service</param>
        /// <param name="password">The password of the account used to connect to the service</param>
        /// <param name="collectionName">The name of the Team Collection that contains the Team Project</param>
        /// <param name="projectName">The name of the Team Project where the bugs will be open</param>
        public static void SetConfiguration(string url, string username, string password, string collectionName, string projectName)
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
        }

        private static string _url;
        public static string Url
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
        public static string Username
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
        public static string Password
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
        public static string CollectiontName
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
        public static string ProjectName
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
