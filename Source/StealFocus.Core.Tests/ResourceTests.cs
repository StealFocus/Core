// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ResourceTests.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the ResourceTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.Tests
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Xml;
    using System.Xml.Schema;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for <see cref="Resource"/>.
    /// </summary>
    [TestClass]
    public partial class ResourceTests
    {
        #region Fields

        /// <summary>
        /// Holds the name of the assembly containing the resources.
        /// </summary>
        private const string ResourceAssemblyName = "StealFocus.Core.Tests";

        #endregion // Fields

        #region Unit Tests

        /// <summary>
        /// Tests <see cref="Resource.GetString(Type, string)"/>.
        /// </summary>
        [TestMethod]
        public void UnitTestGetString()
        {
            string resourceString = Resource.GetString(typeof(ResourceTests), "GoodResourceName");
            Assert.AreEqual("My string.", resourceString, "The resource string was not as expected.");
        }

        /// <summary>
        /// Tests <see cref="Resource.GetString(Type, string)"/>.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CoreException), "No Resource String matching the key 'BadResourceName' could be found for type 'StealFocus.Core.Resource'.")]
        public void UnitTestGetStringWithBadResourceName()
        {
            Resource.GetString(typeof(ResourceTests), "BadResourceName");
        }

        /// <summary>
        /// Tests <see cref="Resource.GetString(Type, string)"/>.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Parameter may not be null.")]
        public void UnitTestGetStringWithNullRequester()
        {
            Resource.GetString(null, "SomeResourceName");
        }

        /// <summary>
        /// Tests <see cref="Resource.GetString(Type, string)"/>.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Parameter may not be null.")]
        public void UnitTestGetStringWithNullKey()
        {
            Resource.GetString(this, null);
        }

        /// <summary>
        /// Tests <see cref="Resource.GetXmlDocument(string, string)"/>.
        /// </summary>
        [TestMethod]
        public void UnitTestGetXmlDocument()
        {
            XmlDocument myXmlDocument = Resource.GetXmlDocument(ResourceAssemblyName, ResouceNames.MyXmlDocumentResource);
            Assert.IsNotNull(myXmlDocument, "The XML Document was null when it was not expected to be.");
        }

        /// <summary>
        /// Tests <see cref="Resource.GetXmlDocument(string, string)"/>.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CoreException), "The requested Assembly 'BadAssemblyName' holding the Resource could not be found.")]
        public void UnitTestGetXmlDocumentWithBadAssemblyName()
        {
            Resource.GetXmlDocument("BadAssemblyName", "ResourceNameIsIrrelevant");
        }

        /// <summary>
        /// Tests <see cref="Resource.GetXmlDocument(string, string)"/>.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CoreException), "The schema resource 'BadResourceName' was not found in the assembly 'StealFocus.Core'.")]
        public void UnitTestGetXmlDocumentWithBadResourceName()
        {
            Resource.GetXmlDocument(ResourceAssemblyName, "BadResourceName");
        }

        /// <summary>
        /// Tests <see cref="Resource.GetXmlSchema(string, string)"/>.
        /// </summary>
        [TestMethod]
        public void UnitTestGetXmlSchema()
        {
            XmlSchema schema = Resource.GetXmlSchema(ResourceAssemblyName, "StealFocus.Core.Tests.Resources.MyXsdResource.xsd");
            Assert.IsNotNull(schema, "The Schema was null where it was not expected to be.");
        }

        /// <summary>
        /// Tests <see cref="Resource.GetXmlSchema(string, string)"/>.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CoreException))]
        public void UnitTestGetXmlSchemaWithBadResourceName()
        {
            Resource.GetXmlSchema(ResourceAssemblyName, "BadResourceName");
        }

        /// <summary>
        /// Tests <see cref="Resource.GetXmlSchema(string, string)"/>.
        /// </summary>
        [TestMethod]
        public void UnitTestGetXmlSchemaWithBadlyFormedXml()
        {
            try
            {
                Resource.GetXmlSchema(ResourceAssemblyName, "StealFocus.Core.Tests.Resources.MyBadlyFormedXsdResource.xsd");
            }
            catch (CoreException e)
            {
                Assert.IsTrue(e.InnerException.GetType() == typeof(XmlException), "The inner exception was not as expected.");
            }
        }

        /// <summary>
        /// Tests <see cref="Resource.GetXmlSchema(string, string)"/>.
        /// </summary>
        [TestMethod]
        public void UnitTestGetXmlSchemaWithInvalidXml()
        {
            try
            {
                Resource.GetXmlSchema(ResourceAssemblyName, "StealFocus.Core.Tests.Resources.MyInvalidXsdResource.xsd");
            }
            catch (CoreException e)
            {
                Assert.IsTrue(e.InnerException.GetType() == typeof(XmlSchemaException), "The inner exception was not as expected.");
            }
        }

        /// <summary>
        /// Tests <see cref="Resource.GetFileAndWriteToPath(string, string, string)"/>.
        /// </summary>
        [TestMethod]
        public void UnitTestGetFileAndWriteToPath()
        {
            const string FileOutputPath = "SomeFile.txt";
            Resource.GetFileAndWriteToPath(ResourceAssemblyName, ResouceNames.SomeFileResource, FileOutputPath);
            Assert.IsTrue(File.Exists(FileOutputPath), "The file did not exist where it was expected to.");
        }

        /// <summary>
        /// Tests <see cref="Resource.GetImage(string, string)"/>.
        /// </summary>
        [TestMethod]
        public void UnitTestGetImage()
        {
            Bitmap image = Resource.GetImage(ResourceAssemblyName, ResouceNames.TestImage);
            Assert.IsNotNull(image, "The image returned was null where it was not expected to be.");
        }

        #endregion // Unit Tests

        #region Structs

        /// <summary>
        /// Holds the resource names.
        /// </summary>
        private struct ResouceNames
        {
            /// <summary>
            /// The MyXmlDocument resource name.
            /// </summary>
            public const string MyXmlDocumentResource = "StealFocus.Core.Tests.Resources.MyXmlDocumentResource.xml";

            /// <summary>
            /// The SomeFile resource name.
            /// </summary>
            public const string SomeFileResource = "StealFocus.Core.Tests.Resources.SomeFile.txt";

            /// <summary>
            /// The TestImage resource name.
            /// </summary>
            public const string TestImage = "StealFocus.Core.Tests.Resources.TestImage.bmp";
        }

        #endregion // Structs
    }
}