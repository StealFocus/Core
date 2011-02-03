// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="CoreTestClassTests.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the CoreTestClassTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.VisualStudio2008.Tests.MSTest
{
    using System;
    using System.Threading;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using VisualStudio2008.MSTest;

    /// <summary>
    /// CoreTestClassTests Class.
    /// </summary>
    [TestClass]
    public class CoreTestClassTests : CoreTestClass
    {
        #region Unit Tests

        /// <summary>
        /// Tests <see cref="CoreTestClass"/>.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CoreException))]
        public void UnitTestInterceptionWithNoAttributes()
        {
            Console.WriteLine("Inside 'TestInterceptionWithNoAttributes' method.");
            throw new CoreException(); // Throw (and expect) an exception to be sure this method gets called
        }

        /// <summary>
        /// Tests <see cref="TestTransactionAttribute"/>.
        /// </summary>
        [TestMethod]
        [TestTransaction]
        public void UnitTestInterceptionWithTestTransaction()
        {
            Console.WriteLine("Inside 'TestInterceptionWithTestTransaction' method.");
        }

        /// <summary>
        /// Tests <see cref="TestTransactionAttribute"/>.
        /// </summary>
        [TestMethod]
        [TestTransaction(true)]
        public void UnitTestInterceptionWithTestTransactionCommitted()
        {
            Console.WriteLine("Inside 'TestInterceptionWithTestTransaction' method.");
        }

        /// <summary>
        /// Tests <see cref="TestTimerAttribute"/>.
        /// </summary>
        [TestMethod]
        [TestTimer(500)]
        public void UnitTestInterceptionWithTestTimerSuccess()
        {
        }

        /// <summary>
        /// Tests <see cref="TestTimerAttribute"/>.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CoreException))]
        [TestTimer(10)]
        public void UnitTestInterceptionWithTestTimerFail()
        {
            Thread.Sleep(100);
        }

        /// <summary>
        /// Tests <see cref="ExpectedExceptionMessageAttribute"/>.
        /// </summary>
        [TestMethod]
        [ExpectedExceptionMessage(typeof(CoreException), "My expected exception message.")]
        public void UnitTestInterceptionWithExpectedExceptionMessage()
        {
            throw new CoreException("My expected exception message.");
        }

        /// <summary>
        /// Tests <see cref="ExpectedExceptionMessageAttribute"/>.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CoreException))]
        [ExpectedExceptionMessage(typeof(CoreException), "My expected exception message.")]
        public void UnitTestInterceptionWithExpectedExceptionMessageFail()
        {
            throw new CoreException("My unexpected exception message.");
        }

        #endregion // Unit Tests
    }
}
