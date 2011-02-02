// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="BizTalkApplicationTests.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the BizTalkApplicationTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.BizTalk2009.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for <see cref="BizTalkApplication"/>.
    /// </summary>
    [TestClass]
    public class BizTalkApplicationTests
    {
        /// <summary>
        /// Tests <see cref="BizTalkApplication.StopAll"/>.
        /// </summary>
        [TestMethod]
        public void IntegrationTestStopAll()
        {
            BizTalkApplication bizTalkApplication = new BizTalkApplication("server=.\\MSSqlSvr2008;database=BizTalkMgmtDb;integrated security=sspi;", "BizTalk Application 1");
            bizTalkApplication.StopAll();
        }
    }
}
