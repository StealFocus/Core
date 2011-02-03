// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="GlobalAssemblyCacheTests.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the GacTest type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for <see cref="GlobalAssemblyCache"/>.
    /// </summary>
    [TestClass]
    public class GlobalAssemblyCacheTests
    {
        /// <summary>
        /// Tests <see cref="GlobalAssemblyCache.GetAssemblyList(GlobalAssemblyCacheCategoryType)"/>.
        /// </summary>
        [TestMethod]
        public void UnitTestGetAssemblyList()
        {
            GlobalAssemblyCacheItem[] gacAssemblies = GlobalAssemblyCache.GetAssemblyList(GlobalAssemblyCacheCategoryTypes.Gac);
            Assert.IsTrue(gacAssemblies.Length > 0, "No assemblies were returned in the list.");
        }
    }
}