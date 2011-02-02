// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="AdapterTests.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the AdapterTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.BizTalk2006.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for <see cref="Adapter"/>.
    /// </summary>
    [TestClass]
    public class AdapterTests
    {
        /// <summary>
        /// Tests <see cref="Adapter.GetAdapters"/>.
        /// </summary>
        [TestMethod]
        public void IntegrationTestGetAdapters()
        {
            string[] adapters = Adapter.GetAdapters();
            Assert.IsTrue(adapters.Length > 0);
        }
    }
}
