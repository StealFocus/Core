// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="GlobalAssemblyCacheCategoryTypes.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the GlobalAssemblyCacheCategoryTypes type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core
{
    using System;

    /// <summary>
    /// GlobalAssemblyCacheCategoryTypes Enum.
    /// </summary>
    [Flags]
    public enum GlobalAssemblyCacheCategoryTypes
    {
        /// <summary>
        /// The None Type.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// The Zap Type.
        /// </summary>
        Zap = 0x1,

        /// <summary>
        /// The Gac Type.
        /// </summary>
        Gac = 0x2,

        /// <summary>
        /// The Download Type.
        /// </summary>
        Download = 0x4
    }
}
