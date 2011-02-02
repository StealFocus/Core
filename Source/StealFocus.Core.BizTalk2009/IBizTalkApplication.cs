// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="IBizTalkApplication.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the IBizTalkApplication type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.BizTalk2006
{
    /// <summary>
    /// IBizTalkApplication Interface.
    /// </summary>
    public interface IBizTalkApplication
    {
        /// <summary>
        /// Stop the <c>BizTalk</c> Application.
        /// </summary>
        void StopAll();

        /// <summary>
        /// Start the <c>BizTalk</c> Application.
        /// </summary>
        void StartAll();

        #region Receive Location Operations

        /// <summary>
        /// Enable the Receive Location.
        /// </summary>
        /// <param name="receiveLocationName">The Receive Location name.</param>
        void EnableReceiveLocation(string receiveLocationName);

        /// <summary>
        /// Enable the Receive Locations.
        /// </summary>
        /// <param name="receiveLocationNames">The Receive Location names.</param>
        void EnableReceiveLocations(string[] receiveLocationNames);

        /// <summary>
        /// Enable all Receive Locations.
        /// </summary>
        void EnableAllReceiveLocations();

        /// <summary>
        /// Disable the Receive Location.
        /// </summary>
        /// <param name="receiveLocationName">The Receive Location name.</param>
        void DisableReceiveLocation(string receiveLocationName);

        /// <summary>
        /// Disable the Receive Locations.
        /// </summary>
        /// <param name="receiveLocationNames">The Receive Location names.</param>
        void DisableReceiveLocations(string[] receiveLocationNames);

        /// <summary>
        /// Disable all Receive Locations.
        /// </summary>
        void DisableAllReceiveLocations();

        #endregion // Receive Location Operations

        #region Receive Port Operations

        /// <summary>
        /// Remove Receive Ports.
        /// </summary>
        void RemoveReceivePorts();
        
        #endregion // Receive Port Operations

        #region Orchestration Operations

        /// <summary>
        /// Get all Orchestration names.
        /// </summary>
        /// <returns>The names.</returns>
        string[] GetAllOrchestrationNames();

        /// <summary>
        /// Terminate the Orchestration.
        /// </summary>
        /// <param name="orchestrationName">The Orchestration name.</param>
        void TerminateOrchestration(string orchestrationName);

        /// <summary>
        /// Terminate the Orchestrations.
        /// </summary>
        /// <param name="orchestrationNames">The Orchestration names.</param>
        void TerminateOrchestrations(string[] orchestrationNames);

        /// <summary>
        /// Terminates all Orchestrations.
        /// </summary>
        void TerminateAllOrchestrations();

        /// <summary>
        /// Start the Orchestration.
        /// </summary>
        /// <param name="orchestrationName">The Orchestration name.</param>
        void StartOrchestration(string orchestrationName);

        /// <summary>
        /// Start the Orchestrations.
        /// </summary>
        /// <param name="orchestrationNames">The Orchestration names.</param>
        void StartOrchestrations(string[] orchestrationNames);

        /// <summary>
        /// Start all Orchestrations.
        /// </summary>
        void StartAllOrchestrations();

        #endregion // Orchestration Operations

        #region Send Port Operations

        /// <summary>
        /// Start the Send Port.
        /// </summary>
        /// <param name="sendPortName">The Send Port name.</param>
        void StartSendPort(string sendPortName);

        /// <summary>
        /// Start the Send Ports.
        /// </summary>
        /// <param name="sendPortNames">The Send Port names.</param>
        void StartSendPorts(string[] sendPortNames);

        /// <summary>
        /// Start all Send Ports.
        /// </summary>
        void StartAllSendPorts();

        /// <summary>
        /// Stop the Send Port.
        /// </summary>
        /// <param name="sendPortName">The Send Port name.</param>
        void StopSendPort(string sendPortName);

        /// <summary>
        /// Stop the Send Ports.
        /// </summary>
        /// <param name="sendPortNames">The Send Port names.</param>
        void StopSendPorts(string[] sendPortNames);

        /// <summary>
        /// Stop all Send Ports.
        /// </summary>
        void StopAllSendPorts();

        /// <summary>
        /// Remove all Send Ports.
        /// </summary>
        void RemoveSendPorts();

        #endregion // Send Port Operations

        #region Send Port Group Operations

        /// <summary>
        /// Start Send Port Group.
        /// </summary>
        /// <param name="sendPortGroupName">The Send Port Group name.</param>
        void StartSendPortGroup(string sendPortGroupName);

        /// <summary>
        /// Start Send Port Groups.
        /// </summary>
        /// <param name="sendPortGroupNames">The Send Port Group names.</param>
        void StartSendPortGroups(string[] sendPortGroupNames);

        /// <summary>
        /// Start all Send Port Groups.
        /// </summary>
        void StartAllSendPortGroups();

        /// <summary>
        /// Stop Send Port Group.
        /// </summary>
        /// <param name="sendPortGroupName">The Send Port Group name.</param>
        void StopSendPortGroup(string sendPortGroupName);

        /// <summary>
        /// Start Send Port Groups.
        /// </summary>
        /// <param name="sendPortGroupNames">The Send Port Group names.</param>
        void StopSendPortGroups(string[] sendPortGroupNames);

        /// <summary>
        /// Stop all Send Port Groups.
        /// </summary>
        void StopAllSendPortGroups();

        /// <summary>
        /// Remove Send Port Groups.
        /// </summary>
        void RemoveSendPortGroups();

        #endregion // Send Port Group Operations
    }
}