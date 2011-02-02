// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlElementExtensions.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the XmlElementExtensions type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StealFocus.Core.Xml
{
    using System;
    using System.Globalization;
    using System.Xml;

    /// <summary>
    /// XmlElementExtensions Class.
    /// </summary>
    /// <remarks>
    /// Extention methods for <see cref="XmlElement"/>.
    /// </remarks>
    public static class XmlElementExtensions
    {
        /// <summary>
        /// Adds an element to the document.
        /// </summary>
        /// <param name="parentElement">The parent.</param>
        /// <param name="elementName">The XML element name.</param>
        /// <param name="elementNamespace">The XML element namespace.</param>
        /// <returns>The XML element.</returns>
        public static XmlElement AddElement(this XmlElement parentElement, string elementName, string elementNamespace)
        {
            if (parentElement == null)
            {
                throw new ArgumentNullException("parentElement");
            }

            XmlElement xmlElement = parentElement.OwnerDocument.CreateElement(elementName, elementNamespace);
            parentElement.AppendChild(xmlElement);
            return xmlElement;
        }

        /// <summary>
        /// Adds an element to the document.
        /// </summary>
        /// <param name="parentElement">The parent.</param>
        /// <param name="elementName">The XML element name.</param>
        /// <returns>The XML element.</returns>
        public static XmlElement AddElement(this XmlElement parentElement, string elementName)
        {
            if (parentElement == null)
            {
                throw new ArgumentNullException("parentElement");
            }

            XmlElement xmlElement = parentElement.OwnerDocument.CreateElement(elementName, parentElement.NamespaceURI);
            parentElement.AppendChild(xmlElement);
            return xmlElement;
        }

        /// <summary>
        /// Adds an element to the document as a sibling.
        /// </summary>
        /// <param name="siblingElement">The sibling.</param>
        /// <param name="elementName">The XML element name.</param>
        /// <param name="elementNamespace">The XML element namespace.</param>
        /// <returns>The XML element.</returns>
        public static XmlElement AddElementBefore(this XmlElement siblingElement, string elementName, string elementNamespace)
        {
            if (siblingElement == null)
            {
                throw new ArgumentNullException("siblingElement");
            }

            XmlElement xmlElement = siblingElement.OwnerDocument.CreateElement(elementName, elementNamespace);
            siblingElement.ParentNode.InsertBefore(xmlElement, siblingElement);
            return xmlElement;
        }

        /// <summary>
        /// Adds an element to the document as a sibling.
        /// </summary>
        /// <param name="siblingElement">The sibling.</param>
        /// <param name="elementName">The XML element name.</param>
        /// <returns>The XML element.</returns>
        public static XmlElement AddElementBefore(this XmlElement siblingElement, string elementName)
        {
            if (siblingElement == null)
            {
                throw new ArgumentNullException("siblingElement");
            }

            XmlElement xmlElement = siblingElement.OwnerDocument.CreateElement(elementName, siblingElement.ParentNode.NamespaceURI);
            siblingElement.ParentNode.InsertBefore(xmlElement, siblingElement);
            return xmlElement;
        }

        /// <summary>
        /// Adds an element to the document as a sibling.
        /// </summary>
        /// <param name="siblingElement">The sibling.</param>
        /// <param name="elementName">The XML element name.</param>
        /// <param name="elementNamespace">The XML element namespace.</param>
        /// <returns>The XML element.</returns>
        public static XmlElement AddElementAfter(this XmlElement siblingElement, string elementName, string elementNamespace)
        {
            if (siblingElement == null)
            {
                throw new ArgumentNullException("siblingElement");
            }

            XmlElement xmlElement = siblingElement.OwnerDocument.CreateElement(elementName, elementNamespace);
            siblingElement.ParentNode.InsertAfter(xmlElement, siblingElement);
            return xmlElement;
        }

        /// <summary>
        /// Adds an element to the document as a sibling.
        /// </summary>
        /// <param name="siblingElement">The sibling.</param>
        /// <param name="elementName">The XML element name.</param>
        /// <returns>The XML element.</returns>
        public static XmlElement AddElementAfter(this XmlElement siblingElement, string elementName)
        {
            if (siblingElement == null)
            {
                throw new ArgumentNullException("siblingElement");
            }

            XmlElement xmlElement = siblingElement.OwnerDocument.CreateElement(elementName, siblingElement.ParentNode.NamespaceURI);
            siblingElement.ParentNode.InsertAfter(xmlElement, siblingElement);
            return xmlElement;
        }

        /// <summary>
        /// Add a value to the element.
        /// </summary>
        /// <param name="xmlElement">The XML element to hold the value.</param>
        /// <param name="value">The value to add.</param>
        /// <returns>The XML element.</returns>
        public static XmlElement AddValue(this XmlElement xmlElement, string value)
        {
            if (xmlElement == null)
            {
                throw new ArgumentNullException("xmlElement");
            }

            xmlElement.InnerText = value;
            return xmlElement;
        }

        /// <summary>
        /// Adds an attribute to the element.
        /// </summary>
        /// <param name="xmlElement">The XML element to contain the attribute.</param>
        /// <param name="attributeName">The name of the attribute.</param>
        /// <param name="attributeValue">The value for the attribute.</param>
        /// <returns>The original XML element.</returns>
        public static XmlElement AddAttribute(this XmlElement xmlElement, string attributeName, string attributeValue)
        {
            if (xmlElement == null)
            {
                throw new ArgumentNullException("xmlElement");
            }

            XmlAttribute xmlAttribute = xmlElement.OwnerDocument.CreateAttribute(attributeName);
            xmlAttribute.InnerText = attributeValue;
            xmlElement.Attributes.Append(xmlAttribute);
            return xmlElement;
        }

        /// <summary>
        /// Gets the parent.
        /// </summary>
        /// <param name="xmlElement">The current XML element.</param>
        /// <returns>The parent.</returns>
        public static XmlElement Parent(this XmlElement xmlElement)
        {
            if (xmlElement == null)
            {
                throw new ArgumentNullException("xmlElement");
            }

            XmlElement parent = xmlElement.ParentNode as XmlElement;
            if (parent == null)
            {
                throw new CoreException("The parent of the provided element was not an XmlElement.");
            }

            return parent;
        }

        /// <summary>
        /// Selects a child element of the provided name.
        /// </summary>
        /// <param name="xmlElement">The XML element.</param>
        /// <param name="childName">The child name.</param>
        /// <returns>The child element.</returns>
        /// <remarks>
        /// Ignores any XML namespaces, looks for the first match only.
        /// </remarks>
        public static XmlElement Child(this XmlElement xmlElement, string childName)
        {
            return Child(xmlElement, childName, 0);
        }

        /// <summary>
        /// Selects a child element of the provided name.
        /// </summary>
        /// <param name="xmlElement">The XML element.</param>
        /// <param name="childName">The child name.</param>
        /// <param name="childNameInstanceMatch">The instance to match, zero based e.g. 1 matches the second element matching the given name.</param>
        /// <returns>The child element.</returns>
        /// <remarks>
        /// Ignores any XML namespaces.
        /// </remarks>
        public static XmlElement Child(this XmlElement xmlElement, string childName, int childNameInstanceMatch)
        {
            if (xmlElement == null)
            {
                throw new ArgumentNullException("xmlElement");
            }

            int count = 0;
            foreach (XmlNode childNode in xmlElement.ChildNodes)
            {
                XmlElement childElement = childNode as XmlElement;
                if (childElement != null && childElement.Name == childName)
                {
                    if (count == childNameInstanceMatch)
                    {
                        return childElement;
                    }

                    count++;
                }
            }

            string exceptionMessage = string.Format(CultureInfo.CurrentCulture, "Could not find a child element with name of '{0}' (instance number was '{1}').", childName, childNameInstanceMatch);
            throw new CoreException(exceptionMessage);
        }
    }
}
