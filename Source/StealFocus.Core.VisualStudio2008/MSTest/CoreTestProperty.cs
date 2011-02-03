// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="CoreTestProperty.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the CoreTestProperty type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.VisualStudio2008.MSTest
{
    using System;
    using System.Runtime.Remoting.Contexts;
    using System.Runtime.Remoting.Messaging;
    using System.Security.Permissions;

    /// <summary>
    /// CoreTestProperty Class.
    /// </summary>
    /// <typeparam name="T">The Test Aspect.</typeparam>
    public class CoreTestProperty<T> : IContextProperty, IContributeObjectSink where T : IMessageSink, ICoreTestAspect, new()
    {
        #region Fields

        /// <summary>
        /// Holds the assembly qualified name of the Test Aspect.
        /// </summary>
        private readonly string name = typeof(T).AssemblyQualifiedName;

        #endregion // Fields

        #region IContextProperty Members

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name
        {
            [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
            get { return this.name; }
        }

        /// <summary>
        /// IsNewContextOK Method.
        /// </summary>
        /// <param name="newCtx">The new context.</param>
        /// <returns>Whether the new context is okay.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
        public bool IsNewContextOK(Context newCtx)
        {
            return true;
        }

        /// <summary>
        /// Freeze Method.
        /// </summary>
        /// <param name="newContext">The new context.</param>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
        public void Freeze(Context newContext)
        {
        }

        #endregion // IContextProperty Members

        #region IContributeObjectSink Members

        /// <summary>
        /// GetObjectSink Method.
        /// </summary>
        /// <param name="obj">An <see cref="object"/>.</param>
        /// <param name="nextSink">An <see cref="IMessageSink"/>.</param>
        /// <returns>An <see cref="IMessageSink"/>. The sink.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
        public IMessageSink GetObjectSink(MarshalByRefObject obj, IMessageSink nextSink)
        {
            T coreTestAspect = new T();
            coreTestAspect.AddMessageSink(nextSink);
            return coreTestAspect;
        }

        #endregion // IContributeObjectSink Members
    }
}
