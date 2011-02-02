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
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for <see cref="Host"/>.
    /// </summary>
    [TestClass]
    public class HostTests
    {
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

            Host.Create(HostName, "BizTalk Application Users", false, HostType.InProcess, true, false);
            Assert.IsTrue(Host.Exists(HostName), "Host was not reported to exist when it was expected to.");
            Host.Delete(HostName);
        }

        /// <summary>
        /// Tests various.
        /// </summary>
        [TestMethod]
        public void IntegrationTestManipulation()
        {
            const string HostName = "BTS_CoreTestHost2";

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

            Host.Create(HostName, "BizTalk Application Users", false, HostType.InProcess, true, false);
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
            string[] receiveHandlers = Host.GetReceiveHandlers("BizTalkServerApplication");
            Assert.IsTrue(receiveHandlers.Length > 0);
        }

        /// <summary>
        /// Tests <see cref="Host.GetSendHandlers(string)"/>.
        /// </summary>
        [TestMethod]
        public void IntegrationTestGetSendHandlers()
        {
            string[] sendHandlers = Host.GetSendHandlers("BizTalkServerApplication");
            Assert.IsTrue(sendHandlers.Length > 0);
        }
    }
}
