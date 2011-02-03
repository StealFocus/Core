// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="WebServerTests.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the WebServerTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.Tests.Iis
{
    using System;

    using Core.Iis;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using VisualStudio2008.MSTest;

    /// <summary>
    /// Tests for <see cref="WebServer"/>.
    /// </summary>
    [TestClass]
    public class WebServerTests : CoreTestClass
    {
        #region Unit Tests

        /// <summary>
        /// Tests <see cref="WebServer.GetDefaultAspNetUserName(string)"/>.
        /// </summary>
        [TestMethod]
        public void IntegrationTestGetDefaultAspNetUserName()
        {
            string defaultAspNetUserName = WebServer.GetDefaultAspNetUserName(Environment.MachineName);
            string message = "Default ASP.NET Username is '{0}'.".FormatWith(defaultAspNetUserName);
            Console.WriteLine(message);
        }

        /// <summary>
        /// Tests <see cref="WebServer.GetDefaultAspNetUserName(string)"/>.
        /// </summary>
        [TestMethod]
        public void IntegrationTestGetDefaultAspNetUserNameUnqualified()
        {
            string defaultAspNetUserName = WebServer.GetDefaultAspNetUserNameUnqualified(Environment.MachineName);
            string message = "Default ASP.NET Username is '{0}'.".FormatWith(defaultAspNetUserName);
            Console.WriteLine(message);
        }

        /// <summary>
        /// Tests <see cref="WebServer.GetDefaultAspNetUserName(string)"/>.
        /// </summary>
        [TestMethod]
        [ExpectedExceptionMessage(typeof(ArgumentException), "Please supply the Computer name, do not use 'localhost'.")]
        public void IntegrationTestGetDefaultAspNetUserNameUsingLocalhost()
        {
            WebServer.GetDefaultAspNetUserName("localhost");
        }

        #endregion // Unit Tests
    }
}