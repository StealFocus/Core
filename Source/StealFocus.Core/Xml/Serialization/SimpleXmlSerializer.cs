// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleXmlSerializer.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the SimpleXmlSerializer type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StealFocus.Core.Xml.Serialization
{
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary>
    /// SimpleXmlSerializer Class.
    /// </summary>
    /// <remarks>
    /// None.
    /// </remarks>
    /// <typeparam name="T">The type to be serialized.</typeparam>
    public static class SimpleXmlSerializer<T>
    {
        #region Methods

        /// <summary>
        /// Serializes the given object to XML.
        /// </summary>
        /// <param name="value">An <see cref="object"/>. To be serialized.</param>
        /// <returns>The XML representing the object.</returns>
        /// <remarks>
        /// None.
        /// </remarks>
        public static string Serialize(T value)
        {
            return Serialize(value, new XmlSerializer(value.GetType()));
        }

        /// <summary>
        /// Serializes the given object to XML.
        /// </summary>
        /// <param name="value">An <see cref="object"/>. To be serialized.</param>
        /// <param name="defaultNamespace">The default namespace.</param>
        /// <returns>The XML representing the object.</returns>
        /// <remarks>
        /// None.
        /// </remarks>
        public static string Serialize(T value, string defaultNamespace)
        {
            return Serialize(value, new XmlSerializer(value.GetType(), defaultNamespace));
        }

        /// <summary>
        /// Deserializes the XML to the given Type.
        /// </summary>
        /// <param name="xml">The XML representing the Object.</param>
        /// <returns>An <see cref="object"/>.</returns>
        /// <remarks>
        /// None.
        /// </remarks>
        public static T Deserialize(string xml)
        {
            return Deserialize(xml, new XmlSerializer(typeof(T)));
        }

        /// <summary>
        /// Deserializes the XML to the given Type.
        /// </summary>
        /// <param name="xml">The XML representing the Object.</param>
        /// <param name="defaultNamespace">The default namespace.</param>
        /// <returns>An <see cref="object"/>.</returns>
        /// <remarks>
        /// None.
        /// </remarks>
        public static T Deserialize(string xml, string defaultNamespace)
        {
            return Deserialize(xml, new XmlSerializer(typeof(T), defaultNamespace));
        }

        /// <summary>
        /// Serializes the given object to XML.
        /// </summary>
        /// <param name="value">The object.</param>
        /// <param name="xmlSerializer">An <see cref="XmlSerializer"/>. The serializer.</param>
        /// <returns>The XML containing the serialized object.</returns>
        private static string Serialize(T value, XmlSerializer xmlSerializer)
        {
            string xml;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8))
                {
                    using (StreamReader streamReader = new StreamReader(memoryStream, Encoding.UTF8))
                    {
                        xmlSerializer.Serialize(xmlTextWriter, value);
                        memoryStream.Position = 0;
                        xml = streamReader.ReadToEnd();
                    }
                }
            }

            return xml;
        }

        /// <summary>
        /// Deserializes the XML to the given Type.
        /// </summary>
        /// <param name="xml">The XML representing the Object.</param>
        /// <param name="xmlSerializer">An <see cref="XmlSerializer"/>. The serializer.</param>
        /// <returns>The deserialized object.</returns>
        private static T Deserialize(string xml, XmlSerializer xmlSerializer)
        {
            using (StringReader stringReader = new StringReader(xml))
            {
                T deserializedObject = (T)xmlSerializer.Deserialize(stringReader);
                return deserializedObject;
            }
        }

        #endregion // Methods
    }
}
