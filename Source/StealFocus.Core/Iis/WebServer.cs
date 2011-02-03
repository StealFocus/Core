// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="WebServer.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the WebServer type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.Iis
{
    using System;
    using System.Globalization;
    using System.Management;

    using Core;

    using OperatingSystem = StealFocus.Core.OperatingSystem;

    /// <summary>
    /// WebServer Class.
    /// </summary>
    public static class WebServer
    {
        /// <summary>
        /// Gets the version of IIS.
        /// </summary>
        /// <returns>The OS version.</returns>
        public static Version GetVersion()
        {
            string currentMachineName = Environment.ExpandEnvironmentVariables("ComputerName");
            return GetVersion(currentMachineName);
        }

        /// <summary>
        /// Gets the version of IIS.
        /// </summary>
        /// <param name="machineName">The machine name.</param>
        /// <returns>The OS version.</returns>
        public static Version GetVersion(string machineName)
        {
            return GetVersion(machineName, new ConnectionOptions());
        }

        /// <summary>
        /// Gets the version of IIS.
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
        /// Get the default ASP.NET username.
        /// </summary>
        /// <returns>The username.</returns>
        public static string GetDefaultAspNetUserName()
        {
            return GetDefaultAspNetUserName(Environment.ExpandEnvironmentVariables("ComputerName"));
        }

        /// <summary>
        /// Get the default ASP.NET username for the given machine.
        /// </summary>
        /// <param name="computerName">The machine name.</param>
        /// <returns>The username.</returns>
        public static string GetDefaultAspNetUserName(string computerName)
        {
            if (string.Compare(computerName, "localhost", true, CultureInfo.CurrentCulture) == 0)
            {
                string exceptionMessage = string.Format(CultureInfo.CurrentCulture, "Please supply the Computer name, do not use '{0}'.", computerName);
                throw new ArgumentException(exceptionMessage);
            }

            Version iisVersion = GetVersion(computerName);
            if (iisVersion == new Version(5, 5, 0, 0))
            {
                string defaultAspNetUsername = computerName + @"\ASPNET";
                return defaultAspNetUsername;
            }

            return @"NT AUTHORITY\NETWORK SERVICE";
        }

        /// <summary>
        /// Gets the ASP.NET username without the domain/machine qualification.
        /// </summary>
        /// <param name="computerName">The computer name.</param>
        /// <returns>The username.</returns>
        /// <remarks>
        /// For example returns "NETWORK SERVICE" instead of "NT AUTHORITY\NETWORK SERVICE".
        /// </remarks>
        public static string GetDefaultAspNetUserNameUnqualified(string computerName)
        {
            return GetDefaultAspNetUserName(computerName).Split('\\')[1];
        }

        /// <summary>
        /// Gets the version of IIS for the given machine.
        /// </summary>
        /// <param name="machineName">The machine name.</param>
        /// <param name="options">The connection options.</param>
        /// <returns>The OS version.</returns>
        private static Version GetVersion(string machineName, ConnectionOptions options)
        {
            Version operatingSystemVersion = OperatingSystem.GetVersion(machineName, options);
            if (operatingSystemVersion.Major == 6 && operatingSystemVersion.Minor == 0)
            {
                return new Version(7, 0, 0, 0);
            }

            if (operatingSystemVersion.Major == 6 && operatingSystemVersion.Minor == 1)
            {
                return new Version(7, 0, 0, 0);
            }

            if (operatingSystemVersion.Major == 5 && operatingSystemVersion.Minor == 2)
            {
                return new Version(6, 0, 0, 0);
            }

            if (operatingSystemVersion.Major == 5 && operatingSystemVersion.Minor == 1)
            {
                return new Version(5, 5, 0, 0);
            }

            throw new CoreException("Unidentified Operating System Version.");
        }
    }
}
