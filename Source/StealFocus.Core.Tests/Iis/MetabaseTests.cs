// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="MetabaseTests.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the MetabaseTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.Tests.Iis
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using StealFocus.Core.Iis;
    using StealFocus.Core.VisualStudio2008.MSTest;

    /// <summary>
    /// MetabaseTests Class.
    /// </summary>
    [TestClass]
    public class MetabaseTests : CoreTestClass
    {
        #region Test Methods

        /// <summary>
        /// Tests Is AppPool Running.
        /// </summary>
        [TestMethod]
        public void IntegrationTestsIsAppPoolRunning()
        {
            bool running = Metabase.IsAppPoolRunning("DefaultAppPool");
            Assert.IsTrue(running);
        }

        /// <summary>
        /// Tests Is AppPool Running.
        /// </summary>
        [TestMethod]
        [ExpectedExceptionMessage(typeof(CoreException), "Could not find an AppPool with a name of 'InvalidAppPoolName'.")]
        public void IntegrationTestsIsAppPoolRunningWithInvalidAppPoolName()
        {
            Metabase.IsAppPoolRunning("InvalidAppPoolName");
        }

        /// <summary>
        /// Tests Is Website Running.
        /// </summary>
        [TestMethod]
        public void IntegrationTestsIsWebsiteRunning()
        {
            bool running = Metabase.IsWebsiteRunning("Default Web Site");
            Assert.IsTrue(running);
        }

        /// <summary>
        /// Tests Is Website Running.
        /// </summary>
        [TestMethod]
        [ExpectedExceptionMessage(typeof(CoreException), "Could not find a Web Site with a description of 'InvalidWebsiteDescription'.")]
        public void IntegrationTestsIsWebsiteRunningWithInvalidWebsiteDescription()
        {
            Metabase.IsWebsiteRunning("InvalidWebsiteDescription");
        }

        #endregion // Test Methods
    }
}
