// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="SerializableDictionaryTests.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the DictionaryTest type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.Tests.Collections.Generic
{
    using System.IO;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    using Core.Collections.Generic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// SerializableDictionaryTests Class.
    /// </summary>
    [TestClass]
    public class SerializableDictionaryTests
    {
        #region Unit Tests

        /// <summary>
        /// Tests serialization.
        /// </summary>
        [TestMethod]
        public void UnitTestSerialize()
        {
            SerializableDictionary<string, string> dictionary = new SerializableDictionary<string, string>();
            dictionary.Add("myKey", "myValue");
            XmlSerializer xmlSerializer = new XmlSerializer(dictionary.GetType());
            string xml;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8))
                {
                    using (StreamReader streamReader = new StreamReader(memoryStream, Encoding.UTF8))
                    {
                        xmlSerializer.Serialize(xmlTextWriter, dictionary);
                        memoryStream.Position = 0;
                        xml = streamReader.ReadToEnd();
                        streamReader.Close();
                    }
                }
            }

            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?><Dictionary><item><key><string>myKey</string></key><value><string>myValue</string></value></item></Dictionary>", xml, "The XML representing the serialized dictionary was not as expected.");
        }

        /// <summary>
        /// Tests deserialization.
        /// </summary>
        [TestMethod]
        public void UnitTestDeserialize()
        {
            const string Xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Dictionary><item><key><string>myKey</string></key><value><string>myValue</string></value></item></Dictionary>";
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SerializableDictionary<string, string>));
            SerializableDictionary<string, string> deserializedDictionary;
            using (StringReader stringReader = new StringReader(Xml))
            {
                deserializedDictionary = (SerializableDictionary<string, string>)xmlSerializer.Deserialize(stringReader);
            }

            Assert.IsNotNull(deserializedDictionary, "The deserialized dictionary was null where it was not expected to be.");
            Assert.IsNotNull(deserializedDictionary["myKey"], "The expected value in the dictionary was not found.");
            Assert.AreEqual("myValue", deserializedDictionary["myKey"], "The value was not as expected.");
        }

        #endregion // Unit Tests
    }
}
