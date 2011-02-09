// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="HostTests.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the HostTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.BizTalk2009.Tests
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for <see cref="Host"/>.
    /// </summary>
    [TestClass]
    public class HostTests
    {
        /// <summary>
        /// Holds the BizTalk Service username used in integration tests.
        /// </summary>
        private const string BizTalkWindowsGroupName = "BizTalk Application Users";

        /// <summary>
        /// Holds the BizTalk Service password used in integration tests.
        /// </summary>
        private const string BizTalkServicePassword = "P4ssword";

        /// <summary>
        /// Holds the default BizTalk host name used in integration tests.
        /// </summary>
        private const string DefaultBizTalkHostName = "BizTalkServerApplication";

        /// <summary>
        /// Holds the BizTalk Service username used in integration tests.
        /// </summary>
        private static readonly string BizTalkServiceUsername = Environment.MachineName + "\\BizTalk.Service";

        /// <summary>
        /// Tests <see cref="Host.Create"/> and <see cref="Host.Delete(string)"/>.
        /// </summary>
        [TestMethod]
        public void IntegrationTestCreateAndDelete()
        {
            const string HostName = "BTS_CoreTestHost1";
            try
            {
                // Make sure the Host does not exist
                Host.Delete(HostName);
            }
            catch (CoreException)
            {
                // Do nothing
            }

            Host.Create(HostName, BizTalkWindowsGroupName, false, HostType.InProcess, true, false);
            Host.CreateInstance(Environment.MachineName, HostName, BizTalkServiceUsername, BizTalkServicePassword);
            Assert.IsTrue(Host.Exists(HostName), "Host was not reported to exist when it was expected to.");
            Host.Delete(HostName);
        }

        /// <summary>
        /// Tests <see cref="Host.Create"/> and <see cref="Host.Delete(string)"/>.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void IntegrationTestCreateAndDeleteWithPeriodForHostName()
        {
            const string HostName = "BTS_CoreTestHost2";
            try
            {
                // Make sure the Host does not exist
                Host.Delete(HostName);
            }
            catch (CoreException)
            {
                // Do nothing
            }

            Host.Create(HostName, BizTalkWindowsGroupName, false, HostType.InProcess, true, false);
            try
            {
                Host.CreateInstance(Environment.MachineName, ".", BizTalkServiceUsername, BizTalkServicePassword);
            }
            catch
            {
                Host.Delete(HostName);
                throw;
            }
        }

        /// <summary>
        /// Tests various.
        /// </summary>
        [TestMethod]
        public void IntegrationTestManipulation()
        {
            const string HostName = "BTS_CoreTestHost3";

            // Make sure the Handlers do not exist
            ReceiveHandler.Delete("FILE", HostName);
            SendHandler.Delete("FILE", HostName);
            try
            {
                // Make sure the Host does not exist
                Host.Delete(HostName);
            }
            catch (CoreException)
            {
                // Do nothing
            }

            Host.Create(HostName, BizTalkWindowsGroupName, false, HostType.InProcess, true, false);
            SendHandler.Create("FILE", HostName);
            ReceiveHandler.Create("FILE", HostName);
            Host.Start(HostName);
            Host.Stop(HostName);
            Host.CleanQueue(HostName);
            ReceiveHandler.Delete("FILE", HostName);
            SendHandler.Delete("FILE", HostName);
            Host.Delete(HostName);
        }

        /// <summary>
        /// Tests <see cref="Host.GetReceiveHandlers(string)"/>.
        /// </summary>
        [TestMethod]
        public void IntegrationTestGetReceiveHandlers()
        {
            string[] receiveHandlers = Host.GetReceiveHandlers(DefaultBizTalkHostName);
            Assert.IsTrue(receiveHandlers.Length > 0);
        }

        /// <summary>
        /// Tests <see cref="Host.GetSendHandlers(string)"/>.
        /// </summary>
        [TestMethod]
        public void IntegrationTestGetSendHandlers()
        {
            string[] sendHandlers = Host.GetSendHandlers(DefaultBizTalkHostName);
            Assert.IsTrue(sendHandlers.Length > 0);
        }
    }
}
