// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="OperatingSystem.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the OperatingSystem type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core
{
    using System;
    using System.Management;

    /// <summary>
    /// OperatingSystem Class.
    /// </summary>
    public static class OperatingSystem
    {
        /// <summary>
        /// Gets the version of the current OS.
        /// </summary>
        /// <returns>The OS version.</returns>
        public static Version GetVersion()
        {
            string currentMachineName = Environment.ExpandEnvironmentVariables("ComputerName");
            return GetVersion(currentMachineName);
        }

        /// <summary>
        /// Gets the version of the OS for the given machine.
        /// </summary>
        /// <param name="machineName">The machine name.</param>
        /// <returns>The OS version.</returns>
        public static Version GetVersion(string machineName)
        {
            return GetVersion(machineName, new ConnectionOptions());
        }

        /// <summary>
        /// Gets the version of the OS for the given machine.
        /// </summary>
        /// <param name="machineName">The machine name.</param>
        /// <param name="userName">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>The OS version.</returns>
        public static Version GetVersion(string machineName, string userName, string password)
        {
            ConnectionOptions connectionOptions = new ConnectionOptions();
            connectionOptions.Username = userName;
            connectionOptions.Password = password;
            return GetVersion(machineName, connectionOptions);
        }

        /// <summary>
        /// Gets the version of the OS for the given machine.
        /// </summary>
        /// <param name="machineName">The machine name.</param>
        /// <param name="options">The connection options.</param>
        /// <returns>The OS version.</returns>
        public static Version GetVersion(string machineName, ConnectionOptions options)
        {
            Version version = new Version();
            ManagementScope scope = new ManagementScope(@"\\" + machineName + @"\root\cimv2", options);
            ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query))
            {
                foreach (ManagementObject obj2 in searcher.Get())
                {
                    version = new Version(obj2["Version"].ToString());
                }
            }

            return version;
        }
    }
}
