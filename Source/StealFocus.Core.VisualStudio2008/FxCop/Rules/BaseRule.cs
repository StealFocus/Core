// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="BaseRule.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the BaseRule type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.VisualStudio2008.FxCop.Rules
{
    using System;
    using Microsoft.FxCop.Sdk;

    /// <summary>
    /// Provides the base <see langword="abstract"/> class for all sample rules.
    /// </summary>
    [CLSCompliant(false)]
    public abstract class BaseRule : BaseIntrospectionRule
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRule"/> class with the specified name.
        /// </summary>
        /// <param name="name">The name of the rule.</param>
        protected BaseRule(string name) : base(name, "StealFocus.Core.VisualStudio2008.FxCop.Rules.Rules.xml", typeof(BaseRule).Assembly)
        {
        }

        #endregion Constructors
    }
}
