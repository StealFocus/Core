// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="SendHandler.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the SendHandler type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.BizTalk2006
{
    using System.Management;

    /// <summary>
    /// SendHandler Class.
    /// </summary>
    public static class SendHandler
    {
        /// <summary>
        /// Create a Send Handler.
        /// </summary>
        /// <param name="adapterName">The Adapter name.</param>
        /// <param name="hostName">The Host name.</param>
        public static void Create(string adapterName, string hostName)
        {
            Create(adapterName, hostName, false);
        }

        /// <summary>
        /// Create a Send Handler.
        /// </summary>
        /// <param name="adapterName">The Adapter name.</param>
        /// <param name="hostName">The Host name.</param>
        /// <param name="isDefault">Indicating if the Handler is the default.</param>
        public static void Create(string adapterName, string hostName, bool isDefault)
        {
            PutOptions options = new PutOptions();
            options.Type = PutType.CreateOnly;
            ManagementClass handlerManagementClass = new ManagementClass("root\\MicrosoftBizTalkServer", "MSBTS_SendHandler2", null);
            foreach (ManagementObject handler in handlerManagementClass.GetInstances())
            {
                if ((string)handler["AdapterName"] == adapterName && (string)handler["HostName"] == hostName)
                {
                    handler.Delete();
                }
            }

            ManagementObject handlerInstance = handlerManagementClass.CreateInstance();
            if (handlerInstance == null)
            {
                throw new CoreException("Could not create Management Object.");
            }

            handlerInstance["AdapterName"] = adapterName;
            handlerInstance["HostName"] = hostName;
            handlerInstance["IsDefault"] = isDefault;
            handlerInstance.Put(options);
        }

        /// <summary>
        /// Check if a Send Handler exists.
        /// </summary>
        /// <param name="adapterName">The Adapter name.</param>
        /// <param name="hostName">The Host name.</param>
        /// <returns>Indicating is the Handler exists.</returns>
        public static bool Exists(string adapterName, string hostName)
        {
            bool exists = false;
            ManagementClass sendHandlerManagementClass = new ManagementClass("root\\MicrosoftBizTalkServer", "MSBTS_SendHandler2", null);
            foreach (ManagementObject sendHandler in sendHandlerManagementClass.GetInstances())
            {
                if ((string)sendHandler["AdapterName"] == adapterName && (string)sendHandler["HostName"] == hostName)
                {
                    exists = true;
                    break;
                }
            }

            return exists;
        }

        /// <summary>
        /// Delete a Send Handler.
        /// </summary>
        /// <param name="adapterName">The Adapter name.</param>
        /// <param name="hostName">The Host name.</param>
        public static void Delete(string adapterName, string hostName)
        {
            ManagementClass sendHandlerManagementClass = new ManagementClass("root\\MicrosoftBizTalkServer", "MSBTS_SendHandler2", null);
            foreach (ManagementObject sendHandler in sendHandlerManagementClass.GetInstances())
            {
                if ((string)sendHandler["AdapterName"] == adapterName && (string)sendHandler["HostName"] == hostName)
                {
                    sendHandler.Delete();
                    break;
                }
            }
        }

        /// <summary>
        /// Delete all Send Handlers.
        /// </summary>
        /// <param name="adapterName">The Adapter name.</param>
        public static void DeleteAll(string adapterName)
        {
            ManagementClass sendHandlerManagementClass = new ManagementClass("root\\MicrosoftBizTalkServer", "MSBTS_SendHandler2", null);
            foreach (ManagementObject sendHandler in sendHandlerManagementClass.GetInstances())
            {
                if ((string)sendHandler["AdapterName"] == adapterName)
                {
                    sendHandler.Delete();
                    break;
                }
            }
        }
    }
}