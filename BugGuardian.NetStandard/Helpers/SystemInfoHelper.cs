using System.Runtime.InteropServices;
using System.Text;

namespace DBTek.BugGuardian.Helpers
{
    internal class SystemInfoHelper
    {
        public static string BuildSystemInfoString()
        {
            var systemInfoString = new StringBuilder();

            systemInfoString.Append($"<strong>OS:</strong> {RuntimeInformation.OSDescription}<br />");
            systemInfoString.Append($"<strong>OS Architecture:</strong> {RuntimeInformation.OSArchitecture}<br />");
            systemInfoString.Append($"<strong>Framework:</strong> {RuntimeInformation.FrameworkDescription}<br />");
            systemInfoString.Append($"<strong>Process Architecture:</strong> {RuntimeInformation.ProcessArchitecture}<br />");

            return systemInfoString.ToString();
        }
    }
}
