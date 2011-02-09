// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="Host.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the Host type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.BizTalk2009
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Management;

    /// <summary>
    /// Host Class.
    /// </summary>
    public static class Host
    {
        #region Methods

        /// <summary>
        /// Create a new Host.
        /// </summary>
        /// <param name="hostName">The Host name.</param>
        /// <param name="windowsGroupName">The Windows Group name.</param>
        /// <param name="trusted">Whether a trusted host.</param>
        /// <param name="hostType">The Host type.</param>
        /// <param name="hostTracking">Whether Host tracking is enabled.</param>
        /// <param name="isDefault">Whether the Host is default.</param>
        public static void Create(string hostName, string windowsGroupName, bool trusted, HostType hostType, bool hostTracking, bool isDefault)
        {
            PutOptions options = new PutOptions();
            options.Type = PutType.CreateOnly;
            ManagementObject btsHostSetting = HostManagement.GetHostSettingClass().CreateInstance();
            if (btsHostSetting == null)
            {
                throw new CoreException("Could not create Management Object.");
            }

            btsHostSetting["Name"] = hostName;
            btsHostSetting["HostType"] = (int)hostType;
            btsHostSetting["NTGroupName"] = windowsGroupName;
            btsHostSetting["AuthTrusted"] = trusted;
            btsHostSetting["HostTracking"] = hostTracking;
            btsHostSetting["IsDefault"] = isDefault;
            btsHostSetting.Put(options);
        }

        /// <summary>
        /// Create a new Host instance.
        /// </summary>
        /// <param name="serverName">The server name.</param>
        /// <param name="hostName">The Host name.</param>
        /// <param name="userName">The username.</param>
        /// <param name="password">The password.</param>
        public static void CreateInstance(string serverName, string hostName, string userName, string password)
        {
            if (hostName == ".")
            {
                throw new ArgumentException("The 'hostName' may not be a period ('.'), use the actual host name.", "hostName");
            }

            PutOptions options = new PutOptions();
            options.Type = PutType.CreateOnly;
            ObjectGetOptions bts_objOptions = new ObjectGetOptions();
            using (ManagementClass bts_AdminObjClassServerHost = new ManagementClass(@"root\MicrosoftBizTalkServer", "MSBTS_ServerHost", bts_objOptions))
            {
                using (ManagementObject bts_AdminObjectServerHost = bts_AdminObjClassServerHost.CreateInstance())
                {
                    if (bts_AdminObjectServerHost == null)
                    {
                        throw new CoreException("Could not create Management Object.");
                    }

                    bts_AdminObjectServerHost["ServerName"] = serverName;
                    bts_AdminObjectServerHost["HostName"] = hostName;
                    bts_AdminObjectServerHost.InvokeMethod("Map", null);
                    using (ManagementClass bts_AdminObjClassHostInstance = new ManagementClass(@"root\MicrosoftBizTalkServer", "MSBTS_HostInstance", bts_objOptions))
                    {
                        using (ManagementObject bts_AdminObjectHostInstance = bts_AdminObjClassHostInstance.CreateInstance())
                        {
                            if (bts_AdminObjectHostInstance == null)
                            {
                                throw new CoreException("Could not create Management Object.");
                            }

                            bts_AdminObjectHostInstance["Name"] = string.Format(CultureInfo.CurrentCulture, "Microsoft BizTalk Server {0} {1}", hostName, serverName);
                            string user = userName;
                            string pwd = password;
                            object[] objparams = new object[3];
                            objparams[0] = user;
                            objparams[1] = pwd;
                            objparams[2] = true;
                            bts_AdminObjectHostInstance.InvokeMethod("Install", objparams);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Check if a Host exists.
        /// </summary>
        /// <param name="hostName">The Host name.</param>
        /// <returns>Indicates if the Host exists.</returns>
        public static bool Exists(string hostName)
        {
            return HostManagement.GetHostObject(hostName) != null;
        }

        /// <summary>
        /// Delete a Host.
        /// </summary>
        /// <param name="hostName">The Host name.</param>
        public static void Delete(string hostName)
        {
            ManagementObject hostObject = HostManagement.GetHostSettingsObject(hostName);
            if (hostObject == null)
            {
                string exceptionMessage = string.Format(CultureInfo.CurrentCulture, "The BizTalk Host named '{0}' did not exist.", hostName);
                throw new CoreException(exceptionMessage);
            }

            hostObject.Delete();
        }

        /// <summary>
        /// Stop a Host.
        /// </summary>
        /// <param name="hostName">The Host name.</param>
        public static void Stop(string hostName)
        {
            ManagementObject hostObject = HostManagement.GetHostObject(hostName);
            if (hostObject == null)
            {
                string exceptionMessage = string.Format(CultureInfo.CurrentCulture, "The BizTalk Host named '{0}' did not exist.", hostName);
                throw new CoreException(exceptionMessage);
            }

            hostObject.InvokeMethod("Stop", null, null);
        }

        /// <summary>
        /// Start a Host.
        /// </summary>
        /// <param name="hostName">The Host name.</param>
        public static void Start(string hostName)
        {
            ManagementObject hostObject = HostManagement.GetHostObject(hostName);
            if (hostObject == null)
            {
                string exceptionMessage = string.Format(CultureInfo.CurrentCulture, "The BizTalk Host named '{0}' did not exist.", hostName);
                throw new CoreException(exceptionMessage);
            }

            hostObject.InvokeMethod("Start", null, null);
        }

        /// <summary>
        /// Restart a Host.
        /// </summary>
        /// <param name="hostName">The Host name.</param>
        public static void Restart(string hostName)
        {
            ManagementObject hostObject = HostManagement.GetHostObject(hostName);
            if (hostObject == null)
            {
                string exceptionMessage = string.Format(CultureInfo.CurrentCulture, "The BizTalk Host named '{0}' did not exist.", hostName);
                throw new CoreException(exceptionMessage);
            }

            hostObject.InvokeMethod("Stop", null, null);
            hostObject.InvokeMethod("Start", null, null);
        }

        /// <summary>
        /// Get the Send Handlers for a Host.
        /// </summary>
        /// <param name="hostName">The Host name.</param>
        /// <returns>The Send Handler names.</returns>
        public static string[] GetSendHandlers(string hostName)
        {
            ArrayList handlerList = new ArrayList();
            using (ManagementClass handlerManagementClass = new ManagementClass("root\\MicrosoftBizTalkServer", "MSBTS_SendHandler2", null))
            {
                foreach (ManagementObject handler in handlerManagementClass.GetInstances())
                {
                    if ((string)handler["HostName"] == hostName)
                    {
                        handlerList.Add(handler["AdapterName"]);
                    }
                }
            }

            return (string[])handlerList.ToArray(typeof(string));
        }

        /// <summary>
        /// Get the Receive Handlers for a Host.
        /// </summary>
        /// <param name="hostName">The Host name.</param>
        /// <returns>The Receive Handler names.</returns>
        public static string[] GetReceiveHandlers(string hostName)
        {
            ArrayList handlerList = new ArrayList();
            using (ManagementClass handlerManagementClass = new ManagementClass("root\\MicrosoftBizTalkServer", "MSBTS_ReceiveHandler", null))
            {
                foreach (ManagementObject handler in handlerManagementClass.GetInstances())
                {
                    if ((string)handler["HostName"] == hostName)
                    {
                        handlerList.Add(handler["AdapterName"]);
                    }
                }
            }

            return (string[])handlerList.ToArray(typeof(string));
        }

        /// <summary>
        /// Clean queue.
        /// </summary>
        /// <param name="hostName">The Host name.</param>
        public static void CleanQueue(string hostName)
        {
            int noMessageInstances = CountMessageInstances(hostName);
            if (noMessageInstances > 0)
            {
                SuspendMessageInstances(hostName);
                TerminateMessageInstances(hostName);
                noMessageInstances = CountMessageInstances(hostName);
                if (noMessageInstances > 0)
                {
                    throw new CoreException("There are still message instances in the message box, check the host has been stopped");
                }
            }

            int noServiceInstances = CountServiceInstances(hostName);
            if (noServiceInstances > 0)
            {
                SuspendServiceInstances(hostName);
                TerminateServiceInstances(hostName);
                noServiceInstances = CountServiceInstances(hostName);
                if (noServiceInstances > 0)
                {
                    throw new CoreException("There are still service instances in the message box, check the host has been stopped");
                }
            }
        }

        /// <summary>
        /// Determines if the instance can be terminated.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns>Indicating if the service can be terminated.</returns>
        private static bool CanTerminate(string status)
        {
            if (status == ServiceStatus.CompletedWithDiscardedMessages
                || status == ServiceStatus.SuspendedNotResumable
                || status == ServiceStatus.SuspendedResumable)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines if the instance can be suspended.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns>Indicating if the service can be terminated.</returns>
        private static bool CanSuspend(string status)
        {
            if (status != ServiceStatus.CompletedWithDiscardedMessages
                && status != ServiceStatus.SuspendedNotResumable
                && status != ServiceStatus.SuspendedResumable)
            {
                return true;
            }
            
            return false;
        }

        /// <summary>
        /// Displays the message instances currently on this queue.
        /// </summary>
        /// <param name="hostName">The Host name.</param>
        /// <returns>An <see cref="int"/>. The number of messages.</returns>
        private static int CountMessageInstances(string hostName)
        {
            int count = 0;
            string query = string.Format(CultureInfo.CurrentCulture, "SELECT * FROM MSBTS_MessageInstance");
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(new ManagementScope(@"root\MicrosoftBizTalkServer"), new WqlObjectQuery(query), null))
            {
                foreach (ManagementObject messageInstanceManager in searcher.Get())
                {
                    string host = messageInstanceManager["HostName"].ToString();
                    if (host == hostName)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// This will terminate all message instances belonging to a host.
        /// </summary>
        /// <param name="hostName">The Host name.</param>
        private static void TerminateMessageInstances(string hostName)
        {
            string query = string.Format(CultureInfo.CurrentCulture, "SELECT * FROM MSBTS_MessageInstance");
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(new ManagementScope(@"root\MicrosoftBizTalkServer"), new WqlObjectQuery(query), null))
            {
                foreach (ManagementObject messageInstanceManager in searcher.Get())
                {
                    string host = messageInstanceManager["HostName"].ToString();
                    string status = messageInstanceManager["ServiceInstanceStatus"].ToString();
                    if (host == hostName)
                    {
                        if (CanTerminate(status))
                        {
                            // For some reason the service class id always seems to return nullref exception 
                            // and the service type seems to work if used instead.
                            string serviceInstanceId = messageInstanceManager["ServiceInstanceID"].ToString();
                            string serviceTypeId = messageInstanceManager["ServiceTypeId"].ToString();
                            string serviceClassId = messageInstanceManager["ServiceTypeId"].ToString();
                            TerminateServiceInstancesByID(hostName, serviceInstanceId, serviceClassId, serviceTypeId);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This will suspend all message instances belonging to a host.
        /// </summary>
        /// <param name="hostName">The Host name.</param>
        private static void SuspendMessageInstances(string hostName)
        {
            string query = string.Format(CultureInfo.CurrentCulture, "SELECT * FROM MSBTS_MessageInstance");
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(new ManagementScope(@"root\MicrosoftBizTalkServer"), new WqlObjectQuery(query), null))
            {
                foreach (ManagementObject messageInstanceManager in searcher.Get())
                {
                    string host = messageInstanceManager["HostName"].ToString();
                    string status = messageInstanceManager["ServiceInstanceStatus"].ToString();
                    if (host == hostName)
                    {
                        if (CanSuspend(status))
                        {
                            // For some reason the service class id always seems to return nullref exception 
                            // and the service type seems to work if used instead.
                            string serviceInstanceId = messageInstanceManager["ServiceInstanceID"].ToString();
                            string serviceTypeId = messageInstanceManager["ServiceTypeId"].ToString();
                            string serviceClassId = messageInstanceManager["ServiceTypeId"].ToString();
                            SuspendServiceInstancesByID(hostName, serviceInstanceId, serviceClassId, serviceTypeId);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Suspends all service instances for a host.
        /// </summary>
        /// <param name="hostName">The Host name.</param>
        /// <returns>An <see cref="int"/>. The number of instances.</returns>
        private static int CountServiceInstances(string hostName)
        {
            int count = 0;
            string query = string.Format(CultureInfo.CurrentCulture, "SELECT * FROM MSBTS_ServiceInstance");
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(new ManagementScope(@"root\MicrosoftBizTalkServer"), new WqlObjectQuery(query), null))
            {
                foreach (ManagementObject serviceInstanceManager in searcher.Get())
                {
                    string host = serviceInstanceManager["HostName"].ToString();
                    if (host == hostName)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        /// <summary>
        /// Suspends all service instances for a host.
        /// </summary>
        /// <param name="hostName">The Host name.</param>
        private static void SuspendServiceInstances(string hostName)
        {
            string query = string.Format(CultureInfo.CurrentCulture, "SELECT * FROM MSBTS_ServiceInstance");
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(new ManagementScope(@"root\MicrosoftBizTalkServer"), new WqlObjectQuery(query), null))
            {
                foreach (ManagementObject serviceInstanceManager in searcher.Get())
                {
                    string host = serviceInstanceManager["HostName"].ToString();
                    string status = serviceInstanceManager["ServiceStatus"].ToString();
                    if (host == hostName)
                    {
                        if (CanSuspend(status))
                        {
                            // For some reason the service class id always seems to return nullref exception 
                            // and the service type seems to work if used instead.
                            string serviceInstanceId = serviceInstanceManager["InstanceID"].ToString();
                            string serviceTypeId = serviceInstanceManager["ServiceTypeId"].ToString();
                            string serviceClassId = serviceInstanceManager["ServiceTypeId"].ToString();
                            SuspendServiceInstancesByID(hostName, serviceInstanceId, serviceClassId, serviceTypeId);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Terminates all service instances for a host.
        /// </summary>
        /// <param name="hostName">The Host name.</param>
        private static void TerminateServiceInstances(string hostName)
        {
            string query = string.Format(CultureInfo.CurrentCulture, "SELECT * FROM MSBTS_ServiceInstance");
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(new ManagementScope(@"root\MicrosoftBizTalkServer"), new WqlObjectQuery(query), null))
            {
                foreach (ManagementObject serviceInstanceManager in searcher.Get())
                {
                    string host = serviceInstanceManager["HostName"].ToString();
                    string status = serviceInstanceManager["ServiceStatus"].ToString();
                    if (host == hostName)
                    {
                        if (CanTerminate(status))
                        {
                            // For some reason the service class id always seems to return nullref exception 
                            // and the service type seems to work if used instead.
                            string serviceInstanceId = serviceInstanceManager["InstanceID"].ToString();
                            string serviceTypeId = serviceInstanceManager["ServiceTypeId"].ToString();
                            string serviceClassId = serviceInstanceManager["ServiceTypeId"].ToString();
                            TerminateServiceInstancesByID(hostName, serviceInstanceId, serviceClassId, serviceTypeId);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Suspends one service instance.
        /// </summary>
        /// <param name="hostName">The Host name.</param>
        /// <param name="serviceInstanceId">The service instance ID.</param>
        /// <param name="serviceClassId">The service class ID.</param>
        /// <param name="serviceTypeId">The service type ID.</param>
        private static void SuspendServiceInstancesByID(string hostName, string serviceInstanceId, string serviceClassId, string serviceTypeId)
        {
            List<string> serviceInstanceIdList = new List<string>();
            List<string> serviceClassIdList = new List<string>();
            List<string> serviceTypeIdList = new List<string>();
            serviceInstanceIdList.Add(serviceInstanceId);
            serviceClassIdList.Add(serviceClassId);
            serviceTypeIdList.Add(serviceTypeId);
            string managementObjectPath = string.Format(CultureInfo.CurrentCulture, "root\\MicrosoftBizTalkServer:MSBTS_HostQueue.HostName=\"{0}\"", hostName);
            using (ManagementObject managementObject = new ManagementObject(managementObjectPath))
            {
                managementObject.InvokeMethod("SuspendServiceInstancesByID", new object[] { serviceClassIdList.ToArray(), serviceTypeIdList.ToArray(), serviceInstanceIdList.ToArray() });
            }
        }

        /// <summary>
        /// Terminates one service instance.
        /// </summary>
        /// <param name="hostName">The Host name.</param>
        /// <param name="serviceInstanceId">The service instance ID.</param>
        /// <param name="serviceClassId">The service class ID.</param>
        /// <param name="serviceTypeId">The service type ID.</param>
        private static void TerminateServiceInstancesByID(string hostName, string serviceInstanceId, string serviceClassId, string serviceTypeId)
        {
            List<string> serviceInstanceIdList = new List<string>();
            List<string> serviceClassIdList = new List<string>();
            List<string> serviceTypeIdList = new List<string>();
            serviceInstanceIdList.Add(serviceInstanceId);
            serviceClassIdList.Add(serviceClassId);
            serviceTypeIdList.Add(serviceTypeId);
            string managementObjectPath = string.Format(CultureInfo.CurrentCulture, "root\\MicrosoftBizTalkServer:MSBTS_HostQueue.HostName=\"{0}\"", hostName);
            using (ManagementObject managementObject = new ManagementObject(managementObjectPath))
            {
                managementObject.InvokeMethod("TerminateServiceInstancesByID", new object[] { serviceClassIdList.ToArray(), serviceTypeIdList.ToArray(), serviceInstanceIdList.ToArray() });
            }
        }

        #endregion // Methods
    }
}