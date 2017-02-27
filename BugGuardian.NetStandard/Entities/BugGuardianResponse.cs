using System;

namespace DBTek.BugGuardian.Entities
{
    public class BugGuardianResponse
    {
        public bool Success { get; set; }

        public string Response { get; set; }

        public Exception Exception { get; set; }
    }
}
