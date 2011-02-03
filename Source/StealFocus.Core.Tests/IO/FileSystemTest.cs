// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="FileSystemTest.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the FileSystemTest type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.Tests.IO
{
    using System;
    using System.Reflection;

    using Core.IO;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for <see cref="FileSystem"/>.
    /// </summary>
    [TestClass]
    public class FileSystemTest
    {
        #region Fields

        /// <summary>
        /// Holds the name of the resource assembly.
        /// </summary>
        private const string ResourceAssemblyName = "StealFocus.Core.Tests";

        #endregion // Fields

        #region Unit Tests

        /// <summary>
        /// Tests <see cref="FileSystem.ComputeHash(string)"/>.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UnitTestComputeHashWithNullFilePath()
        {
            FileSystem.ComputeHash(null);
        }

        /// <summary>
        /// Tests <see cref="FileSystem.ComputeHash(string)"/>.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CoreException), @"The provided file path of 'C:\Path\InvalidFile.txt' for parameter 'pathToFile' was not valid.")]
        public void IntegrationTestComputeHashWithInvalidFilePath()
        {
            FileSystem.ComputeHash(@"C:\Path\InvalidFile.txt");
        }

        /// <summary>
        /// Tests <see cref="FileSystem.Compare(string, string)"/>.
        /// </summary>
        [TestMethod]
        public void IntegrationTestCompareSameFiles()
        {
            Resource.GetFileAndWriteToPath(ResourceAssemblyName, ResourcePaths.SomeFile, FilePaths.SomeFilePath);
            Resource.GetFileAndWriteToPath(ResourceAssemblyName, ResourcePaths.FileTheSameAsSomeFile, FilePaths.FileTheSameAsSomeFilePath);
            Assert.IsTrue(FileSystem.Compare(FilePaths.SomeFilePath, FilePaths.FileTheSameAsSomeFilePath), "The files were reported as different when they were not expected to be.");
        }

        /// <summary>
        /// Tests <see cref="FileSystem.Compare(string, string)"/>.
        /// </summary>
        [TestMethod]
        public void IntegrationTestCompareDifferentFiles()
        {
            Resource.GetFileAndWriteToPath(ResourceAssemblyName, ResourcePaths.SomeFile, FilePaths.SomeFilePath);
            Resource.GetFileAndWriteToPath(ResourceAssemblyName, ResourcePaths.FileDifferentToSomeFile, FilePaths.FileDifferentToSomeFilePath);
            Assert.IsFalse(FileSystem.Compare(FilePaths.SomeFilePath, FilePaths.FileDifferentToSomeFilePath), "The files were reported as the same when they were not expected to be.");
        }

        /// <summary>
        /// Tests <see cref="FileSystem.IsAssembly(string)"/>.
        /// </summary>
        [TestMethod]
        public void IntegrationTestIsAssemblyValid()
        {
            string validAssemblyPath = Assembly.GetExecutingAssembly().Location;
            bool isAssembly = FileSystem.IsAssembly(validAssemblyPath);
            Assert.IsTrue(isAssembly, "The Assembly was reported as not being an assembly when it was expected to be.");
        }

        /// <summary>
        /// Tests <see cref="FileSystem.IsAssembly(string)"/>.
        /// </summary>
        [TestMethod]
        public void IntegrationTestIsAssemblyInvalid()
        {
            const string InvalidAssemblyPath = "StealFocus.Core.Tests.dll.config";
            bool isAssembly = FileSystem.IsAssembly(InvalidAssemblyPath);
            Assert.IsFalse(isAssembly, "The file was reported as being an assembly when it was not expected to be.");
        }

        /// <summary>
        /// Tests <see cref="FileSystem.CopyAccessControlList(string, string)"/>.
        /// </summary>
        [TestMethod]
        public void IntegrationTestCopyAcl()
        {
            FileSystem.CopyAccessControlList("StealFocus.Core.dll", "StealFocus.Core.Tests.dll.config");
        }

        /// <summary>
        /// Clean up after the tests.
        /// </summary>
        [TestCleanup]
        public void Cleanup()
        {
            System.IO.File.Delete("SomeFile.txt");
            System.IO.File.Delete("FileTheSameAsSomeFile.txt");
            System.IO.File.Delete("FileDifferentToSomeFile.txt");
        }

        #endregion // Unit Tests

        #region Structs

        /// <summary>
        /// Holds the file paths.
        /// </summary>
        private struct FilePaths
        {
            /// <summary>
            /// Path to SomeFile.txt.
            /// </summary>
            public const string SomeFilePath = @"SomeFile.txt";

            /// <summary>
            /// Path to FileTheSameAsSomeFile.txt.
            /// </summary>
            public const string FileTheSameAsSomeFilePath = @"FileTheSameAsSomeFile.txt";

            /// <summary>
            /// Path to FileDifferentToSomeFile.txt.
            /// </summary>
            public const string FileDifferentToSomeFilePath = @"FileDifferentToSomeFile.txt";
        }

        /// <summary>
        /// Holds the resource paths.
        /// </summary>
        private struct ResourcePaths
        {
            /// <summary>
            /// Path to SomeFile.txt.
            /// </summary>
            public const string SomeFile = @"StealFocus.Core.Tests.IO.Resources.SomeFile.txt";

            /// <summary>
            /// Path to FileTheSameAsSomeFile.txt.
            /// </summary>
            public const string FileTheSameAsSomeFile = @"StealFocus.Core.Tests.IO.Resources.FileTheSameAsSomeFile.txt";

            /// <summary>
            /// Path to FileDifferentToSomeFile.txt.
            /// </summary>
            public const string FileDifferentToSomeFile = @"StealFocus.Core.Tests.IO.Resources.FileDifferentToSomeFile.txt";
        }

        #endregion // Structs
    }
}