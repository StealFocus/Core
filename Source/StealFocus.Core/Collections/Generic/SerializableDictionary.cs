// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="SerializableDictionary.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the SerializableDictionary type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.Collections.Generic
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml;
    using System.Xml.Schema;
    using System.Xml.Serialization;

    /// <summary>
    /// Dictionary Class.
    /// </summary>
    /// <remarks>
    /// Can be serialized.
    /// </remarks>
    /// <typeparam name="TKey">Type representing the key type.</typeparam>
    /// <typeparam name="TValue">Type representing the value type.</typeparam>
    [Serializable]
    [XmlRoot("Dictionary")]
    public class SerializableDictionary<TKey, TValue> : System.Collections.Generic.Dictionary<TKey, TValue>, IXmlSerializable
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the SerializableDictionary class.
        /// </summary>
        public SerializableDictionary()
        {
        }

        /// <summary>
        /// Initializes a new instance of the SerializableDictionary class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        protected SerializableDictionary(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        #endregion // Constructors

        #region IXmlSerializable Members

        /// <summary>
        /// Returns the Schema.
        /// </summary>
        /// <returns>The schema.</returns>
        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Reads the XML.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public void ReadXml(XmlReader reader)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));
            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();
            if (wasEmpty)
            {
                return;
            }

            while (reader.NodeType != XmlNodeType.EndElement)
            {
                reader.ReadStartElement("item");
                reader.ReadStartElement("key");
                TKey key = (TKey)keySerializer.Deserialize(reader);
                reader.ReadEndElement();
                reader.ReadStartElement("value");
                TValue value = (TValue)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();
                Add(key, value);
                reader.ReadEndElement();
                reader.MoveToContent();
            }

            reader.ReadEndElement();
        }

        /// <summary>
        /// Writes the XML.
        /// </summary>
        /// <param name="writer">An <see cref="XmlWriter"/>. The writer.</param>
        public void WriteXml(XmlWriter writer)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));
            foreach (TKey key in Keys)
            {
                writer.WriteStartElement("item");
                writer.WriteStartElement("key");
                keySerializer.Serialize(writer, key);
                writer.WriteEndElement();
                writer.WriteStartElement("value");
                TValue value = this[key];
                valueSerializer.Serialize(writer, value);
                writer.WriteEndElement();
                writer.WriteEndElement();
            }
        }

        #endregion // IXmlSerializable Members
    }
}
