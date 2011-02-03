// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ICoreTestAspect.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the ICoreTestAspect type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.VisualStudio2008.MSTest
{
    using System.Runtime.Remoting.Messaging;

    /// <summary>
    /// ICoreTestAspect Interface.
    /// </summary>
    public interface ICoreTestAspect
    {
        /// <summary>
        /// Add a message sink.
        /// </summary>
        /// <param name="messageSink">An <see cref="IMessageSink"/>.</param>
        void AddMessageSink(IMessageSink messageSink);
    }
}