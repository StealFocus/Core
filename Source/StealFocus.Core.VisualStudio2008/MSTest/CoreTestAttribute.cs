// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="CoreTestAttribute.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the CoreTestAttribute type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.VisualStudio2008.MSTest
{
    using System;
    using System.Runtime.Remoting.Activation;
    using System.Runtime.Remoting.Contexts;
    using System.Security.Permissions;

    /// <summary>
    /// CoreTestAttribute Class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class CoreTestAttribute : ContextAttribute
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreTestAttribute"/> class. 
        /// </summary>
        public CoreTestAttribute() : base("CoreTest")
        {
        }

        #endregion // Constructors

        #region Methods

        /// <summary>
        /// Gets the Properties for the new Context.
        /// </summary>
        /// <param name="msg">The message.</param>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
        public override void GetPropertiesForNewContext(IConstructionCallMessage msg)
        {
            if (msg == null)
            {
                throw new ArgumentNullException("msg");
            }

            msg.ContextProperties.Add(new CoreTestProperty<TestTimerAspect>());
            msg.ContextProperties.Add(new CoreTestProperty<TestTransactionAspect>());
            msg.ContextProperties.Add(new CoreTestProperty<ExpectedExceptionMessageAspect>());
        }

        #endregion // Methods
    }
}