// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="Metabase.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the Metabase type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.Iis
{
    using System.Globalization;
    using System.IO;
    using System.Management;

    /// <summary>
    /// Metabase Class.
    /// </summary>
    public static class Metabase
    {
        /// <summary>
        /// Indicates if the Web Site is running.
        /// </summary>
        /// <param name="websiteDescription">The website description e.g. "Default Web Site".</param>
        /// <returns>An <see cref="bool"/>. Indicating if the Web Site is running.</returns>
        public static bool IsWebsiteRunning(string websiteDescription)
        {
            string query = string.Format(CultureInfo.CurrentCulture, "SELECT * FROM IIsWebServerSetting");
            string websiteName = null;
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(new ManagementScope(@"root\MicrosoftIISv2"), new WqlObjectQuery(query), null))
            {
                foreach (ManagementObject website in searcher.Get())
                {
                    if (website[IIsWebServerSettingPropertyNames.ServerComment].ToString() == websiteDescription)
                    {
                        websiteName = website[IIsWebServerSettingPropertyNames.Name].ToString();
                        break;
                    }
                }
            }

            if (string.IsNullOrEmpty(websiteName))
            {
                string exceptionMessage = string.Format(CultureInfo.CurrentCulture, "Could not find a Web Site with a description of '{0}'.", websiteDescription);
                throw new CoreException(exceptionMessage);
            }

            string managementObjectPath = string.Format(CultureInfo.CurrentCulture, "root\\MicrosoftIISv2:IIsWebServer.Name=\"{0}\"", websiteName);
            int state;
            using (ManagementObject managementObject = new ManagementObject(managementObjectPath))
            {
                state = (int)managementObject[IIsWebServerPropertyNames.ServerState];
            }

            switch (state)
            {
                case WebsiteStatus.Started:
                    return true;
                case WebsiteStatus.Stopped:
                    return false;
                default:
                    string exceptionMessage = string.Format(CultureInfo.CurrentCulture, "The Web Site with a description of '{0}' had an unknown status.", websiteDescription);
                    throw new CoreException(exceptionMessage);
            }
        }

        /// <summary>
        /// Indicates if the AppPool is running.
        /// </summary>
        /// <param name="appPoolName">The AppPool name.</param>
        /// <returns>An <see cref="bool"/>. Indicating if the AppPool is running.</returns>
        public static bool IsAppPoolRunning(string appPoolName)
        {
            string managementObjectPath = string.Format(CultureInfo.CurrentCulture, "root\\MicrosoftIISv2:IIsApplicationPoolSetting.Name=\"W3SVC/APPPOOLS/{0}\"", appPoolName);
            int state;
            using (ManagementObject managementObject = new ManagementObject(managementObjectPath))
            {
                try
                {
                    state = (int)managementObject[IisApplicationPoolSettingPropertyNames.AppPoolState];
                }
                catch (DirectoryNotFoundException e)
                {
                    if (e.Message == "Win32: The system cannot find the path specified.\r\n")
                    {
                        string exceptionMessage = string.Format(CultureInfo.CurrentCulture, "Could not find an AppPool with a name of '{0}'.", appPoolName);
                        throw new CoreException(exceptionMessage);
                    }

                    throw;
                }
            }
            
            switch (state)
            {
                case AppPoolStatus.Started:
                    return true;
                case AppPoolStatus.Stopped:
                    return false;
                default:
                    string exceptionMessage = string.Format(CultureInfo.CurrentCulture, "The AppPool with a name of '{0}' had an unknown status.", appPoolName);
                    throw new CoreException(exceptionMessage);
            }
        }

        /// <summary>
        /// Holds the Property names found in the IIsWebServerSetting Management Object.
        /// </summary>
        private struct IIsWebServerSettingPropertyNames
        {
            /// <summary>
            /// The ServerComment Property name.
            /// </summary>
            public const string ServerComment = "ServerComment";

            /// <summary>
            /// The Name Property name.
            /// </summary>
            public const string Name = "Name";
        }

        /// <summary>
        /// Holds the Property names found in the IIsWebServerPropertyNames Management Object.
        /// </summary>
        private struct IIsWebServerPropertyNames
        {
            /// <summary>
            /// The Name Property name.
            /// </summary>
            public const string ServerState = "ServerState";
        }

        /// <summary>
        /// Holds the available Website states.
        /// </summary>
        private struct WebsiteStatus
        {
            /// <summary>
            /// The Started value.
            /// </summary>
            public const int Started = 2;

            /// <summary>
            /// The Stopped value.
            /// </summary>
            public const int Stopped = 4;
        }

        /// <summary>
        /// Holds the Property names found in the IisApplicationPoolSetting Management Object.
        /// </summary>
        private struct IisApplicationPoolSettingPropertyNames
        {
            /// <summary>
            /// The AppPoolState Property name.
            /// </summary>
            public const string AppPoolState = "AppPoolState";
        }

        /// <summary>
        /// Holds the available AppPool states.
        /// </summary>
        private struct AppPoolStatus
        {
            /// <summary>
            /// The Started value.
            /// </summary>
            public const int Started = 2;

            /// <summary>
            /// The Stopped value.
            /// </summary>
            public const int Stopped = 4;
        }
    }
}
