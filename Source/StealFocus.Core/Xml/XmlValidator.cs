// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="XmlValidator.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the XmlValidator type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.Xml
{
    using System;
    using System.Xml;
    using System.Xml.Schema;

    /// <summary>
    /// XmlValidator Class.
    /// </summary>
    public static class XmlValidator
    {
        #region Methods

        /// <summary>
        /// Validate the given XML against the schema.
        /// </summary>
        /// <param name="xmlDocument">An <see cref="XmlDocument"/>. The XML to validate.</param>
        /// <param name="schemas">An <see cref="XmlSchemaSet"/>. The schema to validate with.</param>
        public static void Validate(XmlDocument xmlDocument, XmlSchemaSet schemas)
        {
            if (xmlDocument == null)
            {
                throw new ArgumentNullException("xmlDocument");
            }

            xmlDocument.Schemas = schemas;
            xmlDocument.Validate(null);
        }

        #endregion // Methods
    }
}
