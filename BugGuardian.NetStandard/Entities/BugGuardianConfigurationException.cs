using System;

namespace DBTek.BugGuardian.Entities
{

    public class BugGuardianConfigurationException : Exception
    {
        public BugGuardianConfigurationException() : base("The BugGuardian configuration is missing or not valid") { }
    }
}
