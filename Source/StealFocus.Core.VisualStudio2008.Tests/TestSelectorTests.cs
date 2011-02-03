// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="TestSelectorTests.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the TestSelectorTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.VisualStudio2008.Tests
{
    using System.Collections.Specialized;
    using System.IO;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for <see cref="TestSelector"/>.
    /// </summary>
    [TestClass]
    public class TestSelectorTests
    {
        #region Unit Tests

        /// <summary>
        /// Tests <see cref="TestSelector.GetAllTestNames(string, string)"/>.
        /// </summary>
        [TestMethod]
        public void IntegrationTestGetAllTestNames()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(".");
            StringCollection testNames = TestSelector.GetAllTestNames(directoryInfo.FullName, "StealFocus*Tests*dll");
            Assert.IsTrue(testNames.Count > 0, "No Test names were returned where some were expected to be.");
        }

        #endregion // Unit Tests
    }
}
