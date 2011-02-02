// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="Adapter.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the Adapter type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.BizTalk2009
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Management;

    /// <summary>
    /// Adapter Class.
    /// </summary>
    public static class Adapter
    {
        #region Methods

        /// <summary>
        /// Adds the Adapter with the specified name.
        /// </summary>
        /// <param name="name">The name of the Adapter.</param>
        /// <param name="managementClassId">The management class ID.</param>
        public static void Add(string name, Guid managementClassId)
        {
            Add(name, managementClassId, string.Empty);
        }

        /// <summary>
        /// Adds the specified name.
        /// </summary>
        /// <param name="name">The name of the Adapter.</param>
        /// <param name="managementClassId">The management class ID.</param>
        /// <param name="comment">The comment.</param>
        public static void Add(string name, Guid managementClassId, string comment)
        {
            using (ManagementClass addAdapter_objClass = new ManagementClass(@"root\MicrosoftBizTalkServer", "MSBTS_AdapterSetting", new ObjectGetOptions()))
            {
                ManagementObject addAdapter_objInstance = addAdapter_objClass.CreateInstance();
                if (addAdapter_objInstance == null)
                {
                    throw new CoreException("Could not create Management Object.");
                }

                addAdapter_objInstance.SetPropertyValue("Name", name);

                // "B" GUID format example - "{9A7B0162-2CD5-4F61-B7EB-C40A3442A5F8}"
                addAdapter_objInstance.SetPropertyValue("MgmtCLSID", managementClassId.ToString("B"));
                if (!string.IsNullOrEmpty(comment))
                {
                    addAdapter_objInstance.SetPropertyValue("Comment", comment);
                }

                addAdapter_objInstance.Put();
            }
        }

        /// <summary>
        /// Deletes the Adapter with the specified name.
        /// </summary>
        /// <param name="adapterName">Name of the adapter.</param>
        public static void Delete(string adapterName)
        {
            SendHandler.DeleteAll(adapterName);
            ReceiveHandler.DeleteAll(adapterName);
            using (ManagementClass managementClass = new ManagementClass(@"root\MicrosoftBizTalkServer", "MSBTS_AdapterSetting", new ObjectGetOptions()))
            {
                foreach (ManagementObject adapter in managementClass.GetInstances())
                {
                    if ((string)adapter["Name"] == adapterName)
                    {
                        adapter.Delete();
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the adapters.
        /// </summary>
        /// <returns>A list of Adapter names.</returns>
        public static string[] GetAdapters()
        {
            ArrayList adapterList = new ArrayList();
            using (ManagementClass managementClass = new ManagementClass(@"root\MicrosoftBizTalkServer", "MSBTS_AdapterSetting", new ObjectGetOptions()))
            {
                foreach (ManagementObject adapter in managementClass.GetInstances())
                {
                    adapterList.Add(adapter["Name"]);
                }
            }

            return (string[])adapterList.ToArray(typeof(string));
        }

        /// <summary>
        /// Gets the management class ID of an Adapter.
        /// </summary>
        /// <param name="adapterName">Name of the adapter.</param>
        /// <returns>The management class ID.</returns>
        public static Guid GetManagementClassId(string adapterName)
        {
            using (ManagementClass managementClass = new ManagementClass(@"root\MicrosoftBizTalkServer", "MSBTS_AdapterSetting", new ObjectGetOptions()))
            {
                foreach (ManagementObject adapter in managementClass.GetInstances())
                {
                    if ((string)adapter["Name"] == adapterName)
                    {
                        string managementClassId = (string)adapter["MgmtCLSID"];
                        return new Guid(managementClassId);
                    }
                }
            }

            string exceptionMessage = string.Format(CultureInfo.CurrentCulture, "No BizTalk Adapter was found with name '{0}'.", adapterName);
            throw new CoreException(exceptionMessage);
        }
        
        #endregion // Methods
    }
}