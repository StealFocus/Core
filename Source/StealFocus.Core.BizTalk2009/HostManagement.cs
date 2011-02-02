// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="HostManagement.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the HostManagement type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.BizTalk2009
{
    using System.Globalization;
    using System.Management;

    /// <summary>
    /// HostManagement Class.
    /// </summary>
    internal static class HostManagement
    {
        #region Methods

        /// <summary>
        /// Get the Host object.
        /// </summary>
        /// <param name="hostName">The host name.</param>
        /// <returns>The Host object.</returns>
        public static ManagementObject GetHostObject(string hostName)
        {
            ManagementScope scope = GetManagementScope();
            ObjectQuery query = new ObjectQuery(string.Format(CultureInfo.InvariantCulture, "select * from {0} where name = '{1}'", new object[] { "MSBTS_Host", hostName }));
            using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(scope, query, null))
            {
                ManagementObjectCollection objects = managementObjectSearcher.Get();
                using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = objects.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        return (ManagementObject)enumerator.Current;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Get the Host settings object.
        /// </summary>
        /// <param name="hostSettingName">The host setting name.</param>
        /// <returns>The Host settings object.</returns>
        public static ManagementObject GetHostSettingsObject(string hostSettingName)
        {
            ManagementScope scope = GetManagementScope();
            ObjectQuery query = new ObjectQuery(string.Format(CultureInfo.InvariantCulture, "select * from {0} where name = '{1}'", new object[] { "MSBTS_HostSetting", hostSettingName }));
            using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(scope, query, null))
            {
                ManagementObjectCollection objects = managementObjectSearcher.Get();
                using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = objects.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        return (ManagementObject)enumerator.Current;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the host setting class.
        /// </summary>
        /// <returns>The host setting.</returns>
        public static ManagementClass GetHostSettingClass()
        {
            const string ClassPath = "MSBTS_HostSetting";
            return GetManagementClass(ClassPath);
        }

        /// <summary>
        /// Gets the management scope.
        /// </summary>
        /// <returns>The management scope.</returns>
        private static ManagementScope GetManagementScope()
        {
            const string ScopePath = @"root\MicrosoftBizTalkServer";
            ManagementScope managementScope = new ManagementScope(ScopePath);
            managementScope.Options.EnablePrivileges = true;
            managementScope.Options.Impersonation = ImpersonationLevel.Impersonate;
            managementScope.Connect();
            return managementScope;
        }

        /// <summary>
        /// Gets the management class.
        /// </summary>
        /// <param name="path">The WMI path.</param>
        /// <returns>The management class.</returns>
        private static ManagementClass GetManagementClass(string path)
        {
            ManagementScope managementScope = GetManagementScope();
            ManagementPath managementPath = new ManagementPath(path);
            using (ManagementClass managementClass = new ManagementClass(managementScope, managementPath, null))
            {
                managementClass.Get();
                return managementClass;
            }
        }

        // /// <remarks />
        // public static ManagementClass GetServerHostClass()
        // {
        //    const string ClassPath = "MSBTS_ServerHost";
        //    return GetManagementClass(ClassPath);
        // }

        // /// <remarks />
        // public static ManagementClass GetHostInstanceClass()
        // {
        //    const string ClassPath = "MSBTS_HostInstance";
        //    return GetManagementClass(ClassPath);
        // }
        #endregion // Methods
    }
}