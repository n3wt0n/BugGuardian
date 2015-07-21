using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBTek.BugGuardian.Helpers
{
    public class SystemInfoHelper
    {
        public static string BuildSystemInfoString()
        {
            var systemInfoString = new StringBuilder();

#if WINDOWS_APP || WINDOWS_PHONE_APP
            Windows.Security.ExchangeActiveSyncProvisioning.EasClientDeviceInformation deviceInfo = new Windows.Security.ExchangeActiveSyncProvisioning.EasClientDeviceInformation();

            systemInfoString.AppendFormat("<strong>Operating System:</strong> {0} <br />", deviceInfo.OperatingSystem);
            systemInfoString.AppendFormat("<strong>Friendly Name:</strong> {0} <br />", deviceInfo.FriendlyName);
            systemInfoString.AppendFormat("<strong>System Manufacturer:</strong> {0} <br />", deviceInfo.SystemManufacturer);
            systemInfoString.AppendFormat("<strong>System Product Name:</strong> {0} <br />", deviceInfo.SystemProductName);
            systemInfoString.AppendFormat("<strong>System SKU:</strong> {0} <br />", deviceInfo.SystemSku);
#if WINDOWS_PHONE_APP
            systemInfoString.AppendFormat("<strong>System Firmware Version:</strong> {0} <br />", deviceInfo.SystemFirmwareVersion);
            systemInfoString.AppendFormat("<strong>System Hardware Version:</strong> {0} <br />", deviceInfo.SystemHardwareVersion);
#endif
#else
            systemInfoString.AppendFormat("<strong>OS:</strong> {0} <br />", Environment.OSVersion.VersionString);
            systemInfoString.AppendFormat("<strong>64 bit OS:</strong> {0} <br />", Environment.Is64BitOperatingSystem);
            systemInfoString.AppendFormat("<strong>64 bit Process:</strong> {0} <br />", Environment.Is64BitProcess);
            systemInfoString.AppendFormat("<strong>CLR version:</strong> {0} <br />", Environment.Version);
#endif


            return systemInfoString.ToString();
        }


    }
}
