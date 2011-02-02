// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ServiceStatus.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the ServiceStatus type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.BizTalk2006
{
    /// <summary>
    /// ServiceStatus Class.
    /// </summary>
    internal static class ServiceStatus
    {
        /// <summary>
        /// Holds the ReadyToRun status.
        /// </summary>
        internal const string ReadyToRun = "1";

        /// <summary>
        /// Holds the Active status.
        /// </summary>
        internal const string Active = "2";

        /// <summary>
        /// Holds the SuspendedResumable status.
        /// </summary>
        internal const string SuspendedResumable = "4";

        /// <summary>
        /// Holds the Dehydrated status.
        /// </summary>
        internal const string Dehydrated = "8";

        /// <summary>
        /// Holds the CompletedWithDiscardedMessages status.
        /// </summary>
        internal const string CompletedWithDiscardedMessages = "16";

        /// <summary>
        /// Holds the SuspendedNotResumable status.
        /// </summary>
        internal const string SuspendedNotResumable = "32";

        /// <summary>
        /// Holds the InBreakpoint status.
        /// </summary>
        internal const string InBreakpoint = "64";
    }
}