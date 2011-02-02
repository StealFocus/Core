// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="BizTalkApplication.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the BizTalkApplication type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.BizTalk2006
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;

    using Microsoft.BizTalk.ExplorerOM;

    /// <summary>
    /// BizTalkApplication Class.
    /// </summary>
    public class BizTalkApplication : IBizTalkApplication
    {
        #region Fields

        /// <summary>
        /// Holds the <c>BizTalk</c> Application.
        /// </summary>
        private readonly Microsoft.BizTalk.ExplorerOM.IBizTalkApplication bizTalkApplication;

        #endregion // Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BizTalkApplication"/> class.
        /// </summary>
        /// <param name="managementDatabaseConnectionString">The management database connection string.</param>
        /// <param name="applicationName">The application name.</param>
        public BizTalkApplication(string managementDatabaseConnectionString, string applicationName) : this(new BizTalkCatalogExplorer(managementDatabaseConnectionString), applicationName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BizTalkApplication"/> class.
        /// </summary>
        /// <param name="bizTalkCatalogExplorer">The <c>BizTalk</c> catalog.</param>
        /// <param name="applicationName">The application name.</param>
        public BizTalkApplication(IBizTalkCatalogExplorer bizTalkCatalogExplorer, string applicationName)
        {
            if (bizTalkCatalogExplorer == null)
            {
                throw new ArgumentNullException("bizTalkCatalogExplorer");
            }

            if (string.IsNullOrEmpty(applicationName))
            {
                throw new ArgumentException("The applicationName must not be null or empty.", "applicationName");
            }

            if (bizTalkCatalogExplorer.ApplicationExists(applicationName))
            {
                this.bizTalkApplication = bizTalkCatalogExplorer.GetApplication(applicationName);
            }
            else
            {
                throw new CoreException("No application with that name exists in supplied BizTalk Group.");
            }
        }

        #endregion // Constructors

        #region IBizTalkApplication Members

        /// <summary>
        /// Stop the <c>BizTalk</c> Application.
        /// </summary>
        public void StopAll()
        {
            this.bizTalkApplication.Stop(ApplicationStopOption.StopAll);
            this.bizTalkApplication.BtsCatalogExplorer.SaveChanges();
        }

        /// <summary>
        /// Start the <c>BizTalk</c> Application.
        /// </summary>
        public void StartAll()
        {
            this.bizTalkApplication.Start(ApplicationStartOption.StartAll);
            this.bizTalkApplication.BtsCatalogExplorer.SaveChanges();
        }

        #region Receive Location Operations

        /// <summary>
        /// Enable the Receive Location.
        /// </summary>
        /// <param name="receiveLocationName">The Receive Location name.</param>
        public void EnableReceiveLocation(string receiveLocationName)
        {
            this.EnableReceiveLocations(new[] { receiveLocationName });
        }

        /// <summary>
        /// Enable the Receive Locations.
        /// </summary>
        /// <param name="receiveLocationNames">The Receive Location names.</param>
        public void EnableReceiveLocations(string[] receiveLocationNames)
        {
            StringCollection receiveLocationNameList = new StringCollection();
            receiveLocationNameList.AddRange(receiveLocationNames);
            foreach (ReceivePort receivePort in this.bizTalkApplication.ReceivePorts)
            {
                foreach (IReceiveLocation2 receiveLocation in receivePort.ReceiveLocations)
                {
                    if (receiveLocationNameList.Contains(receiveLocation.Name))
                    {
                        receiveLocation.Enable = true;
                    }
                }
            }

            this.bizTalkApplication.BtsCatalogExplorer.SaveChanges();
        }

        /// <summary>
        /// Enable all Receive Locations.
        /// </summary>
        public void EnableAllReceiveLocations()
        {
            foreach (ReceivePort receivePort in this.bizTalkApplication.ReceivePorts)
            {
                foreach (IReceiveLocation2 receiveLocation in receivePort.ReceiveLocations)
                {
                    receiveLocation.Enable = true;
                }
            }

            this.bizTalkApplication.BtsCatalogExplorer.SaveChanges();
        }

        /// <summary>
        /// Disable the Receive Location.
        /// </summary>
        /// <param name="receiveLocationName">The Receive Location name.</param>
        public void DisableReceiveLocation(string receiveLocationName)
        {
            this.DisableReceiveLocations(new[] { receiveLocationName });
        }

        /// <summary>
        /// Disable the Receive Locations.
        /// </summary>
        /// <param name="receiveLocationNames">The Receive Location names.</param>
        public void DisableReceiveLocations(string[] receiveLocationNames)
        {
            StringCollection receiveLocationNameList = new StringCollection();
            receiveLocationNameList.AddRange(receiveLocationNames);
            foreach (IReceiveLocation2 receiveLocation in this.bizTalkApplication.ReceivePorts)
            {
                if (receiveLocationNameList.Contains(receiveLocation.Name))
                {
                    receiveLocation.Enable = false;
                }
            }

            this.bizTalkApplication.BtsCatalogExplorer.SaveChanges();
        }

        /// <summary>
        /// Disable all Receive Locations.
        /// </summary>
        public void DisableAllReceiveLocations()
        {
            this.bizTalkApplication.Stop(ApplicationStopOption.DisableAllReceiveLocations);
            this.bizTalkApplication.BtsCatalogExplorer.SaveChanges();
        }

        #endregion // Receive Location Operations

        #region Receive Port Operations

        /// <summary>
        /// Remove Receive Ports.
        /// </summary>
        public void RemoveReceivePorts()
        {
            IReceivePort2[] receivePorts = new IReceivePort2[this.bizTalkApplication.ReceivePorts.Count];
            this.bizTalkApplication.ReceivePorts.CopyTo(receivePorts, 0);
            foreach (IReceivePort2 receivePort in receivePorts)
            {
                this.bizTalkApplication.BtsCatalogExplorer.RemoveReceivePort(receivePort);
            }

            this.bizTalkApplication.BtsCatalogExplorer.SaveChanges();
        }

        #endregion // Receive Port Operations

        #region Orchestration Operations

        /// <summary>
        /// Get all Orchestration names.
        /// </summary>
        /// <returns>The names.</returns>
        public string[] GetAllOrchestrationNames()
        {
            string[] orchestrationNames = new string[this.bizTalkApplication.Orchestrations.Count];
            int count = 0; // Can't access ICollection via index, so must "foreach" and keep a count
            foreach (IBtsOrchestration2 orchestration in this.bizTalkApplication.Orchestrations)
            {
                orchestrationNames[count] = orchestration.FullName;
                count++;
            }

            return orchestrationNames;
        }

        /// <summary>
        /// Terminate the Orchestration.
        /// </summary>
        /// <param name="orchestrationName">The Orchestration name.</param>
        public void TerminateOrchestration(string orchestrationName)
        {
            this.TerminateOrchestrations(new[] { orchestrationName });
        }

        /// <summary>
        /// Terminate the Orchestrations.
        /// </summary>
        /// <param name="orchestrationNames">The Orchestration names.</param>
        public void TerminateOrchestrations(string[] orchestrationNames)
        {
            if (orchestrationNames == null)
            {
                throw new ArgumentNullException("orchestrationNames");
            }

            if (orchestrationNames.Length > 0)
            {
                ArrayList orchestrationNamesList = new ArrayList(orchestrationNames);
                foreach (BtsOrchestration orchestration in this.bizTalkApplication.Orchestrations)
                {
                    if (orchestrationNamesList.Contains(orchestration.FullName))
                    {
                        BizTalkOrchestration bizTalkOrchestration = new BizTalkOrchestration(orchestration.FullName);
                        bizTalkOrchestration.TerminateInstances();
                    }
                }

                this.bizTalkApplication.BtsCatalogExplorer.SaveChanges();
            }
        }

        /// <summary>
        /// Terminates all Orchestrations.
        /// </summary>
        public void TerminateAllOrchestrations()
        {
            this.TerminateOrchestrations(this.GetAllOrchestrationNames());
        }

        /// <summary>
        /// Start the Orchestration.
        /// </summary>
        /// <param name="orchestrationName">The Orchestration name.</param>
        public void StartOrchestration(string orchestrationName)
        {
            this.StartOrchestrations(new[] { orchestrationName });
        }

        /// <summary>
        /// Start the Orchestrations.
        /// </summary>
        /// <param name="orchestrationNames">The Orchestration names.</param>
        public void StartOrchestrations(string[] orchestrationNames)
        {
            foreach (string orchestrationName in orchestrationNames)
            {
                foreach (BtsOrchestration orchestration in this.bizTalkApplication.Orchestrations)
                {
                    if (orchestration.FullName == orchestrationName)
                    {
                        if (orchestration.Status != OrchestrationStatus.Started)
                        {
                            orchestration.Status = OrchestrationStatus.Started;
                        }

                        break;
                    }
                }
            }

            this.bizTalkApplication.BtsCatalogExplorer.SaveChanges();
        }

        /// <summary>
        /// Start all Orchestrations.
        /// </summary>
        public void StartAllOrchestrations()
        {
            foreach (BtsOrchestration orchestration in this.bizTalkApplication.Orchestrations)
            {
                if (orchestration.Status != OrchestrationStatus.Started)
                {
                    orchestration.Status = OrchestrationStatus.Started;
                }
            }

            this.bizTalkApplication.BtsCatalogExplorer.SaveChanges();
        }

        #endregion // Orchestration Operations

        #region Send Port Operations

        /// <summary>
        /// Start the Send Port.
        /// </summary>
        /// <param name="sendPortName">The Send Port name.</param>
        public void StartSendPort(string sendPortName)
        {
            this.StartSendPorts(new[] { sendPortName });
        }

        /// <summary>
        /// Start the Send Ports.
        /// </summary>
        /// <param name="sendPortNames">The Send Port names.</param>
        public void StartSendPorts(string[] sendPortNames)
        {
            this.ChangeSendPortStatus(sendPortNames, PortStatus.Started);
            this.bizTalkApplication.BtsCatalogExplorer.SaveChanges();
        }

        /// <summary>
        /// Start all Send Ports.
        /// </summary>
        public void StartAllSendPorts()
        {
            this.StartSendPorts(this.GetAllSendPortNames());
        }

        /// <summary>
        /// Stop the Send Port.
        /// </summary>
        /// <param name="sendPortName">The Send Port name.</param>
        public void StopSendPort(string sendPortName)
        {
            this.StopSendPorts(new[] { sendPortName });
        }

        /// <summary>
        /// Stop the Send Ports.
        /// </summary>
        /// <param name="sendPortNames">The Send Port names.</param>
        public void StopSendPorts(string[] sendPortNames)
        {
            this.ChangeSendPortStatus(sendPortNames, PortStatus.Stopped);
            this.bizTalkApplication.BtsCatalogExplorer.SaveChanges();
        }

        /// <summary>
        /// Stop all Send Ports.
        /// </summary>
        public void StopAllSendPorts()
        {
            this.StopSendPorts(this.GetAllSendPortNames());
        }

        /// <summary>
        /// Remove all Send Ports.
        /// </summary>
        public void RemoveSendPorts()
        {
            ISendPort2[] sendPorts = new ISendPort2[this.bizTalkApplication.SendPorts.Count];
            this.bizTalkApplication.SendPorts.CopyTo(sendPorts, 0);
            foreach (ISendPort2 sendPort in sendPorts)
            {
                this.bizTalkApplication.BtsCatalogExplorer.RemoveSendPort(sendPort);
            }

            this.bizTalkApplication.BtsCatalogExplorer.SaveChanges();
        }

        #endregion // Send Port Operations

        #region Send Port Group Operations

        /// <summary>
        /// Start Send Port Group.
        /// </summary>
        /// <param name="sendPortGroupName">The Send Port Group name.</param>
        public void StartSendPortGroup(string sendPortGroupName)
        {
            this.StartSendPortGroups(new[] { sendPortGroupName });
        }

        /// <summary>
        /// Start Send Port Groups.
        /// </summary>
        /// <param name="sendPortGroupNames">The Send Port Group names.</param>
        public void StartSendPortGroups(string[] sendPortGroupNames)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Start all Send Port Groups.
        /// </summary>
        public void StartAllSendPortGroups()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Stop Send Port Group.
        /// </summary>
        /// <param name="sendPortGroupName">The Send Port Group name.</param>
        public void StopSendPortGroup(string sendPortGroupName)
        {
            this.StopSendPortGroups(new[] { sendPortGroupName });
        }

        /// <summary>
        /// Start Send Port Groups.
        /// </summary>
        /// <param name="sendPortGroupNames">The Send Port Group names.</param>
        public void StopSendPortGroups(string[] sendPortGroupNames)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Stop all Send Port Groups.
        /// </summary>
        public void StopAllSendPortGroups()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Remove Send Port Groups.
        /// </summary>
        public void RemoveSendPortGroups()
        {
            ISendPortGroup2[] sendPortGroups = new ISendPortGroup2[this.bizTalkApplication.SendPortGroups.Count];
            this.bizTalkApplication.SendPortGroups.CopyTo(sendPortGroups, 0);
            foreach (ISendPortGroup2 sendPortGroup in sendPortGroups)
            {
                this.bizTalkApplication.BtsCatalogExplorer.RemoveSendPortGroup(sendPortGroup);
            }

            this.bizTalkApplication.BtsCatalogExplorer.SaveChanges();
        }

        #endregion // Send Port Group Operations

        #endregion // IBizTalkApplication Members

        #region Methods

        /// <summary>
        /// Change Send Ports status.
        /// </summary>
        /// <param name="sendPortNames">The Send Port names.</param>
        /// <param name="newPortStatus">The Send Port status.</param>
        private void ChangeSendPortStatus(IEnumerable<string> sendPortNames, PortStatus newPortStatus)
        {
            foreach (string sendPortName in sendPortNames)
            {
                foreach (ISendPort2 sendPort in this.bizTalkApplication.SendPorts)
                {
                    // Can't access ICollection via index, so must "foreach" and check each
                    if (sendPort.Name == sendPortName)
                    {
                        sendPort.Status = newPortStatus;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Get all Send Port names.
        /// </summary>
        /// <returns>The Send Port names.</returns>
        private string[] GetAllSendPortNames()
        {
            string[] sendPortNames = new string[this.bizTalkApplication.SendPorts.Count];
            int count = 0; // Can't access ICollection via index, so must "foreach" and keep a count
            foreach (ISendPort2 sendPort in this.bizTalkApplication.SendPorts)
            {
                sendPortNames[count] = sendPort.Name;
                count++;
            }

            return sendPortNames;
        }

        #endregion // Methods
    }
}