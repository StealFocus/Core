﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StrongNameUtilityTests.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the StrongNameUtilityTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StealFocus.Core.Tests
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Contains tests for <see cref="StrongNameUtility"/>.
    /// </summary>
    [TestClass]
    public class StrongNameUtilityTests
    {
        /// <summary>
        /// Tests <see cref="StrongNameUtility.ExtractPublicKeyToken(string)"/>.
        /// </summary>
        [TestMethod]
        public void IntegrationTestExtractPublicKeyToken()
        {
            string publicKeyToken = StrongNameUtility.ExtractPublicKeyToken(@"D:\Workspaces\Temp\StealFocus.Core\Trunk\Source\StealFocus.Core.StrongNameKeyPair.snk");
            Assert.IsTrue(!string.IsNullOrEmpty(publicKeyToken));
            Console.WriteLine("Public Key Token is: " + publicKeyToken);
        }

        /// <summary>
        /// Tests <see cref="StrongNameUtility.ExtractPublicKey(string)"/>.
        /// </summary>
        [TestMethod]
        public void IntegrationTestExtractPublicKey()
        {
            string publicKey = StrongNameUtility.ExtractPublicKey(@"D:\Workspaces\Temp\StealFocus.Core\Trunk\Source\StealFocus.Core.StrongNameKeyPair.snk");
            Assert.IsTrue(!string.IsNullOrEmpty(publicKey));
            Console.WriteLine("Public Key is: " + publicKey);
        }
    }
}
