// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="TestTimerAspect.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the TestTimerAspect type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.VisualStudio2008.MSTest
{
    using System;
    using System.Globalization;
    using System.Runtime.Remoting.Messaging;
    using System.Security.Permissions;

    /// <summary>
    /// TestTimerAspect Class.
    /// </summary>
    public class TestTimerAspect : CoreTestAspect<TestTimerAttribute>, IMessageSink, ICoreTestAspect
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

            DateTime timeBeforeTest = DateTime.Now;
            IMessage returnMessage = this.nextSink.SyncProcessMessage(msg);
            DateTime timeAfterTest = DateTime.Now;
            TestTimerAttribute testTimerAttribute = GetAttribute(msg);
            if (testTimerAttribute != null)
            {
                TimeSpan targetTestLength = testTimerAttribute.TestLength;
                TimeSpan actualTestLength = timeAfterTest - timeBeforeTest;
                if (actualTestLength > targetTestLength)
                {
                    string exceptionMessage = string.Format(CultureInfo.CurrentCulture, "The test exceeded the TestTimer constraint. Target test length '{0}', actual test length '{1}'.", targetTestLength, actualTestLength);
                    throw new CoreException(exceptionMessage);
                }
            }

            return returnMessage;
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
