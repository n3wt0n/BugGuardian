using System;

namespace DBTek.BugGuardian.Entities
{
    internal class Account
    {
        public string Url { get; set; }

        public String Username { get; set; }

        public String Password { get; set; }

        public String CollectionName { get; set; }

        public String ProjectName { get; set; }

        public bool IsVSO => Url.Contains("visualstudio.com");
    }
}
