// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ImpersonationTests.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the ImpersonationTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.Tests.Security
{
    using System.Globalization;
    using System.Security.Principal;

    using Core.Security;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// ImpersonationTests Class.
    /// </summary>
    [TestClass]
    public class ImpersonationTests
    {
        /// <summary>
        /// Holds the domain qualified test username.
        /// </summary>
        private const string TestUsername = @"domain\username";

        /// <summary>
        /// Test Method.
        /// </summary>
        [TestMethod]
        [Ignore]
        public void IntegrationTestWithCredentials()
        {
            Assert.AreNotEqual(TestUsername, WindowsIdentity.GetCurrent().Name.ToLower(CultureInfo.CurrentCulture), "Current Windows Identity Name not as expected.");
            using (new Impersonation("username", "domain", "password"))
            {
                Assert.AreEqual(TestUsername, WindowsIdentity.GetCurrent().Name.ToLower(CultureInfo.CurrentCulture), "Current Windows Identity Name not as expected.");
            }

            Assert.AreNotEqual(TestUsername, WindowsIdentity.GetCurrent().Name.ToLower(CultureInfo.CurrentCulture), "Current Windows Identity Name not as expected.");
        }
    }
}
