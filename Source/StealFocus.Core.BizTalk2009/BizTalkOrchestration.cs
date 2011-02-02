// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="BizTalkOrchestration.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the BizTalkOrchestration type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.BizTalk2009
{
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Management;

    /// <summary>
    /// BizTalkOrchestration Class.
    /// </summary>
    public class BizTalkOrchestration
    {
        #region Fields

        /// <summary>
        /// Holds the name.
        /// </summary>
        private readonly string name;

        #endregion // Fields

        #region Constructors
        
        /// <summary>
        /// Initializes a new instance of the <see cref="BizTalkOrchestration"/> class. 
        /// </summary>
        /// <param name="name">The Orchestration name.</param>
        public BizTalkOrchestration(string name)
        {
            this.name = name;
        }

        #endregion // Constructors

        #region Methods

        /// <summary>
        /// Terminate instances.
        /// </summary>
        public void TerminateInstances()
        {
            string serviceName = this.name.Substring(this.name.LastIndexOf(".", StringComparison.OrdinalIgnoreCase) + 1);
            string query = string.Format(CultureInfo.CurrentCulture, "SELECT * FROM MSBTS_ServiceInstance WHERE ServiceClass = {0} and ServiceStatus > 2 AND ServiceName = '{1}'", 1, serviceName);
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(new ManagementScope(@"root\MicrosoftBizTalkServer"), new WqlObjectQuery(query), null))
            {
                if (searcher.Get().Count > 0)
                {
                    StringCollection instanceIds = new StringCollection();
                    StringCollection serviceClassIds = new StringCollection();
                    StringCollection serviceTypeIds = new StringCollection();
                    string hostName = string.Empty;
                    int instanceCount = 0;
                    foreach (ManagementObject obj2 in searcher.Get())
                    {
                        if (string.IsNullOrEmpty(hostName))
                        {
                            hostName = obj2["HostName"].ToString();
                        }

                        // This seems to be a magic number, carried over from original source (it is un-documented)
                        if (instanceCount > 1999)
                        {
                            TerminateServiceInstancesByID(hostName, instanceIds, serviceClassIds, serviceTypeIds);
                            instanceCount = 0;
                            serviceClassIds.Clear();
                            serviceTypeIds.Clear();
                            instanceIds.Clear();
                        }

                        serviceClassIds.Add(obj2["ServiceClassId"].ToString());
                        serviceTypeIds.Add(obj2["ServiceTypeId"].ToString());
                        instanceIds.Add(obj2["InstanceID"].ToString());
                        instanceCount++;
                    }

                    if (serviceClassIds.Count > 0)
                    {
                        TerminateServiceInstancesByID(hostName, instanceIds, serviceClassIds, serviceTypeIds);
                    }
                }
            }
        }

        /// <summary>
        /// Terminate Service instances by ID.
        /// </summary>
        /// <param name="hostName">The host name.</param>
        /// <param name="instanceIdList">The instance IDs.</param>
        /// <param name="serviceClassIdList">The service class IDs.</param>
        /// <param name="serviceTypeIdList">The service type IDs.</param>
        private static void TerminateServiceInstancesByID(string hostName, StringCollection instanceIdList, StringCollection serviceClassIdList, StringCollection serviceTypeIdList)
        {
            string managementObjectPath = string.Format(CultureInfo.CurrentCulture, "root\\MicrosoftBizTalkServer:MSBTS_HostQueue.HostName=\"{0}\"", hostName);
            using (ManagementObject managementObject = new ManagementObject(managementObjectPath))
            {
                string[] serviceClassIds = new string[serviceClassIdList.Count];
                serviceClassIdList.CopyTo(serviceClassIds, 0);
                string[] serviceTypeIds = new string[serviceTypeIdList.Count];
                serviceTypeIdList.CopyTo(serviceTypeIds, 0);
                string[] instanceIds = new string[instanceIdList.Count];
                instanceIdList.CopyTo(instanceIds, 0);
                managementObject.InvokeMethod("TerminateServiceInstancesByID", new object[] { serviceClassIds, serviceTypeIds, instanceIds });
            }
        }

        #endregion // Methods
    }
}