// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="TestTransactionAspect.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the TestTransactionAspect type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.VisualStudio2008.MSTest
{
    using System;
    using System.Runtime.Remoting.Messaging;
    using System.Security.Permissions;
    using System.Transactions;

    /// <summary>
    /// TestTransactionAspect Class.
    /// </summary>
    public class TestTransactionAspect : CoreTestAspect<TestTransactionAttribute>, IMessageSink, ICoreTestAspect
    {
        #region Fields

        /// <summary>
        /// Holds the next sink.
        /// </summary>
        private IMessageSink nextSink;

        #endregion // Fields

        #region IMessageSink Members

        /// <summary>
        /// Gets the next sink.
        /// </summary>
        public IMessageSink NextSink
        {
            [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
            get { return this.nextSink; }
        }

        /// <summary>
        /// SyncProcessMessage Method.
        /// </summary>
        /// <param name="msg">An <see cref="IMessage"/>. The message.</param>
        /// <returns>An <see cref="IMessage"/>. The reply.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
        public IMessage SyncProcessMessage(IMessage msg)
        {
            if (msg == null)
            {
                throw new ArgumentNullException("msg");
            }

            TestTransactionAttribute testTransactionAttribute = GetAttribute(msg);
            IMessage returnMethod;
            if (testTransactionAttribute != null)
            {
                using (TransactionScope scope = new TransactionScope(testTransactionAttribute.TransactionScopeOption, testTransactionAttribute.TransactionOptions))
                {
                    returnMethod = this.nextSink.SyncProcessMessage(msg);
                    if (testTransactionAttribute.Commit)
                    {
                        Console.WriteLine("Committing Transaction in Test Method.");
                        scope.Complete();
                    }
                }
            }
            else
            {
                returnMethod = this.nextSink.SyncProcessMessage(msg);
            }

            return returnMethod;
        }

        /// <summary>
        /// AsyncProcessMessage Method.
        /// </summary>
        /// <param name="msg">An <see cref="IMessage"/>. The message.</param>
        /// <param name="replySink">An <see cref="IMessageSink"/>. The reply.</param>
        /// <returns>An <see cref="IMessageSink"/>. The return.</returns>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
        public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
        {
            throw new InvalidOperationException();
        }

        #endregion // IMessageSink Members

        #region ICoreTestAspect

        /// <summary>
        /// Adds a message sink.
        /// </summary>
        /// <param name="messageSink">An <see cref="IMessageSink"/>. The message sink.</param>
        public void AddMessageSink(IMessageSink messageSink)
        {
            this.nextSink = messageSink;
        }

        #endregion // ICoreTestAspect
    }
}
