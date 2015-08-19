using System.Text;

namespace DBTek.BugGuardian.Helpers
{
    internal class SystemInfoHelper
    {
        public static string BuildSystemInfoString()
        {
            var systemInfoString = new StringBuilder();

#if WINDOWS_APP || WINDOWS_PHONE_APP || WINDOWS_UWP
            Windows.Security.ExchangeActiveSyncProvisioning.EasClientDeviceInformation deviceInfo = new Windows.Security.ExchangeActiveSyncProvisioning.EasClientDeviceInformation();

            systemInfoString.Append($"<strong>Operating System:</strong> {deviceInfo.OperatingSystem}<br />");
            systemInfoString.Append($"<strong>Friendly Name:</strong> {deviceInfo.FriendlyName}<br />");
            systemInfoString.Append($"<strong>System Manufacturer:</strong> {deviceInfo.SystemManufacturer}<br />");
            systemInfoString.Append($"<strong>System Product Name:</strong> {deviceInfo.SystemProductName}<br />");
            systemInfoString.Append($"<strong>System SKU:</strong> {deviceInfo.SystemSku}<br />");
#if WINDOWS_PHONE_APP
            systemInfoString.Append($"<strong>System Firmware Version:</strong> {deviceInfo.SystemFirmwareVersion}<br />");
            systemInfoString.Append($"<strong>System Hardware Version:</strong> {deviceInfo.SystemHardwareVersion}<br />");
#endif
#else
            systemInfoString.Append($"<strong>OS:</strong> {System.Environment.OSVersion.VersionString}<br />");
            systemInfoString.Append($"<strong>64 bit OS:</strong> {System.Environment.Is64BitOperatingSystem}<br />");
            systemInfoString.Append($"<strong>64 bit Process:</strong> {System.Environment.Is64BitProcess}<br />");
            systemInfoString.Append($"<strong>CLR version:</strong> {System.Environment.Version}<br />");
#endif

            return systemInfoString.ToString();
        }
    }
}
