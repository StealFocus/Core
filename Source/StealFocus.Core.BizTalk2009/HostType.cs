// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="HostType.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the HostType type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.BizTalk2009
{
    /// <summary>
    /// Host types.
    /// </summary>
    public enum HostType
    {
        /// <summary>
        /// Invalid host type.
        /// </summary>
        Invalid,

        /// <summary>
        /// Inprocess host.
        /// </summary>
        InProcess,

        /// <summary>
        /// Isolated host, for example IIS.
        /// </summary>
        Isolated
    }
}