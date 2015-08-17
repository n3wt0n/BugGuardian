using System;

namespace BugGuardian.Shared.Entities
{
    public class BugGuardianResponse
    {
        public bool Success { get; set; }

        public string Response { get; set; }

        public Exception Exception { get; set; }
    }
}
