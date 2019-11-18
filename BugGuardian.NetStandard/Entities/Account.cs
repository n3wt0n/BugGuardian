namespace DBTek.BugGuardian.Entities
{
    internal class Account
    {
        public string Url { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        /// <summary>
        /// This represent either the Organization in Azure DevOps (the XXX in https://dev.azure.com/XXX)
        /// or
        /// the Team Project Collection in Azure DevOps Server and TFS
        /// </summary>
        public string CollectionName { get; set; }

        public string ProjectName { get; set; }

        public bool IsVSTS
            => Url?.Contains("visualstudio.com") ?? false;

        public bool IsValid
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Url))
                    return false;

                if (string.IsNullOrWhiteSpace(Username))
                    return false;

                if (string.IsNullOrWhiteSpace(Password))
                    return false;

                if (string.IsNullOrWhiteSpace(ProjectName))
                    return false;

                return true;
            }
        }
    }
}
