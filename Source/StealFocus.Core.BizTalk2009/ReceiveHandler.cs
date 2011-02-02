// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ReceiveHandler.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the ReceiveHandler type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.BizTalk2006
{
    using System.Management;

    /// <summary>
    /// ReceiveHandler Class.
    /// </summary>
    public static class ReceiveHandler
    {
        /// <summary>
        /// Create a Receive Handler.
        /// </summary>
        /// <param name="adapterName">The Adapter name.</param>
        /// <param name="hostName">The Host name.</param>
        public static void Create(string adapterName, string hostName)
        {
            PutOptions options = new PutOptions();
            options.Type = PutType.CreateOnly;
            ManagementClass handlerManagementClass = new ManagementClass("root\\MicrosoftBizTalkServer", "MSBTS_ReceiveHandler", null);
            foreach (ManagementObject handler in handlerManagementClass.GetInstances())
            {
                if ((string)handler["AdapterName"] == adapterName && (string)handler["HostName"] == hostName)
                {
                    handler.Delete();
                }
            }

            ManagementObject recieveHandlerManager = handlerManagementClass.CreateInstance();
            if (recieveHandlerManager == null)
            {
                throw new CoreException("Could not create Management Object.");
            }

            recieveHandlerManager["AdapterName"] = adapterName;
            recieveHandlerManager["HostName"] = hostName;
            recieveHandlerManager.Put(options);
        }

        /// <summary>
        /// Check if a Receive Handler exists.
        /// </summary>
        /// <param name="adapterName">The Adapter name.</param>
        /// <param name="hostName">The Host name.</param>
        /// <returns>Indicating is the Handler exists.</returns>
        public static bool Exists(string adapterName, string hostName)
        {
            bool exists = false;
            ManagementClass recieveHandlerManagementClass = new ManagementClass("root\\MicrosoftBizTalkServer", "MSBTS_ReceiveHandler", null);
            foreach (ManagementObject recieveHandler in recieveHandlerManagementClass.GetInstances())
            {
                if ((string)recieveHandler["AdapterName"] == adapterName && (string)recieveHandler["HostName"] == hostName)
                {
                    exists = true;
                    break;
                }
            }

            return exists;
        }

        /// <summary>
        /// Delete a Receive Handler.
        /// </summary>
        /// <param name="adapterName">The Adapter name.</param>
        /// <param name="hostName">The Host name.</param>
        public static void Delete(string adapterName, string hostName)
        {
            ManagementClass recieveHandlerManagementClass = new ManagementClass("root\\MicrosoftBizTalkServer", "MSBTS_ReceiveHandler", null);
            foreach (ManagementObject recieveHandler in recieveHandlerManagementClass.GetInstances())
            {
                if ((string)recieveHandler["AdapterName"] == adapterName && (string)recieveHandler["HostName"] == hostName)
                {
                    recieveHandler.Delete();
                    break;
                }
            }
        }

        /// <summary>
        /// Delete all Receive Handlers.
        /// </summary>
        /// <param name="adapterName">The Adapter name.</param>
        public static void DeleteAll(string adapterName)
        {
            ManagementClass recieveHandlerManagementClass = new ManagementClass("root\\MicrosoftBizTalkServer", "MSBTS_ReceiveHandler", null);
            foreach (ManagementObject recieveHandler in recieveHandlerManagementClass.GetInstances())
            {
                if ((string)recieveHandler["AdapterName"] == adapterName)
                {
                    recieveHandler.Delete();
                    break;
                }
            }
        }
    }
}