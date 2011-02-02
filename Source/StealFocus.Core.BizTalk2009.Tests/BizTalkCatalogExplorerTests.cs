// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="BizTalkCatalogExplorerTests.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the BizTalkCatalogExplorerTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.BizTalk2009.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for <see cref="BizTalkCatalogExplorer"/>.
    /// </summary>
    [TestClass]
    public class BizTalkCatalogExplorerTests
    {
        /// <summary>
        /// Tests <see cref="BizTalkCatalogExplorer.CreateApplication(string)"/> and <see cref="BizTalkCatalogExplorer.RemoveApplication(string)"/>.
        /// </summary>
        [TestMethod]
        public void IntegrationTestCreateAndRemove()
        {
            const string AppName = "MyCoreTestApplication";
            BizTalkCatalogExplorer bizTalkCatalogExplorer = new BizTalkCatalogExplorer("server=.\\MSSqlSvr2008;database=BizTalkMgmtDb;integrated security=sspi;");
            bizTalkCatalogExplorer.CreateApplication(AppName);
            bizTalkCatalogExplorer.RemoveApplication(AppName);
        }
    }
}
