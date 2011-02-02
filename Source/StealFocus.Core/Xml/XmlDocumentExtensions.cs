// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlDocumentExtensions.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the XmlDocumentExtensions type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StealFocus.Core.Xml
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    /// <summary>
    /// XmlDocumentExtensions Class.
    /// </summary>
    /// <remarks>
    /// Extention methods for <see cref="XmlDocument"/>.
    /// </remarks>
    public static class XmlDocumentExtensions
    {
        /// <summary>
        /// Adds an element to the document.
        /// </summary>
        /// <param name="xmlDocument">The XML document.</param>
        /// <param name="elementName">The XML element name.</param>
        /// <param name="elementNamespace">The XML element namespace.</param>
        /// <returns>The XML element.</returns>
        public static XmlElement AddElement(this XmlDocument xmlDocument, string elementName, string elementNamespace)
        {
            if (xmlDocument == null)
            {
                throw new ArgumentNullException("xmlDocument");
            }

            XmlElement xmlElement = xmlDocument.CreateElement(elementName, elementNamespace);
            xmlDocument.AppendChild(xmlElement);
            return xmlElement;
        }

        /// <summary>
        /// Adds an element to the document.
        /// </summary>
        /// <param name="xmlDocument">The XML document.</param>
        /// <param name="elementName">The XML element name.</param>
        /// <returns>The XML element.</returns>
        public static XmlElement AddElement(this XmlDocument xmlDocument, string elementName)
        {
            if (xmlDocument == null)
            {
                throw new ArgumentNullException("xmlDocument");
            }

            XmlElement xmlElement = xmlDocument.CreateElement(elementName);
            xmlDocument.AppendChild(xmlElement);
            return xmlElement;
        }

        /// <summary>
        /// Selects an XML element.
        /// </summary>
        /// <param name="xmlDocument">The XML document.</param>
        /// <param name="xpath">The XPath e.g. "/def:MyDocumentElement".</param>
        /// <param name="namespaces">The namespaces e.g. the key/value pair of "def" and "http://schemas.acme.com/PetShop".</param>
        /// <returns>The selected XML element.</returns>
        public static XmlElement SelectElement(this XmlDocument xmlDocument, string xpath, IDictionary<string, string> namespaces)
        {
            if (xmlDocument == null)
            {
                throw new ArgumentNullException("xmlDocument");
            }

            if (namespaces == null)
            {
                throw new ArgumentNullException("namespaces");
            }

            XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
            foreach (KeyValuePair<string, string> keyValuePair in namespaces)
            {
                xmlNamespaceManager.AddNamespace(keyValuePair.Key, keyValuePair.Value);
            }

            XmlNode node = xmlDocument.SelectSingleNode(xpath, xmlNamespaceManager);
            XmlElement selectedElement = node as XmlElement;
            if (selectedElement == null)
            {
                throw new CoreException("The selected node was not an XmlElement.");
            }

            return selectedElement;
        }
    }
}
