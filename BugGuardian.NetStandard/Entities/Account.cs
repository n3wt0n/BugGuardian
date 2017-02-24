namespace DBTek.BugGuardian.Entities
{
    internal class Account
    {
        public string Url { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string CollectionName { get; set; }

        public string ProjectName { get; set; }

        public bool IsVSTS => Url?.Contains("visualstudio.com") ?? false;
    }
}
