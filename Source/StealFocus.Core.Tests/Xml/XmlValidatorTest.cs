// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="XmlValidatorTest.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the XmlValidatorTest type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.Tests.Xml
{
    using System;
    using System.Xml;
    using System.Xml.Schema;

    using Core.Xml;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for <see cref="XmlValidator"/>.
    /// </summary>
    [TestClass]
    public class XmlValidatorTest
    {
        #region Unit Tests

        /// <summary>
        /// Tests <see cref="XmlValidator.Validate(XmlDocument, XmlSchemaSet)"/>.
        /// </summary>
        [TestMethod]
        public void UnitTestValidate()
        {
            XmlSchemaSet schemas = new XmlSchemaSet();
            schemas.Add(Resource.GetXmlSchema("StealFocus.Core.Tests", "StealFocus.Core.Tests.Xml.Resources.Schema.xsd"));
            XmlDocument xmlDocument = Resource.GetXmlDocument("StealFocus.Core.Tests", "StealFocus.Core.Tests.Xml.Resources.Document.xml");
            XmlValidator.Validate(xmlDocument, schemas);
        }

        /// <summary>
        /// Tests <see cref="XmlValidator.Validate(XmlDocument, XmlSchemaSet)"/>.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UnitTestValidateWithNullParameter()
        {
            XmlValidator.Validate(null, null);
        }

        #endregion // Unit Tests
    }
}