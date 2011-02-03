// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="SimpleXmlSerializerTest.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the SimpleXmlSerializerTest type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.Tests.Xml.Serialization
{
    using Core.Xml.Serialization;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for SimpleXmlSerializer.
    /// </summary>
    [TestClass]
    public class SimpleXmlSerializerTest
    {
        #region Unit Tests

        /// <summary>
        /// Tests serialize.
        /// </summary>
        [TestMethod]
        public void UnitTestSerialize()
        {
            SerializableWidget serializableWidget = new SerializableWidget();
            serializableWidget.MyProperty = "SomeValue";
            string xml = SimpleXmlSerializer<SerializableWidget>.Serialize(serializableWidget);
            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?><SerializableWidget xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"><MyProperty>SomeValue</MyProperty></SerializableWidget>", xml, "The XML produced from the serialization process was not as expected.");
        }

        /// <summary>
        /// Tests deserialize with namespace.
        /// </summary>
        [TestMethod]
        public void UnitTestSerializeWithXmlNamespace()
        {
            SerializableWidget serializableWidget = new SerializableWidget();
            serializableWidget.MyProperty = "SomeValue";
            string xml = SimpleXmlSerializer<SerializableWidget>.Serialize(serializableWidget, "http://www.MyCompany.com/Widgets");
            Assert.AreEqual("<?xml version=\"1.0\" encoding=\"utf-8\"?><SerializableWidget xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://www.MyCompany.com/Widgets\"><MyProperty>SomeValue</MyProperty></SerializableWidget>", xml, "The XML produced from the serialization process was not as expected.");
        }

        /// <summary>
        /// Tests deserialize.
        /// </summary>
        [TestMethod]
        public void UnitTestDeserialize()
        {
            string xml = Resource.GetXmlDocument("StealFocus.Core.Tests", ResouceNames.SerializableWidgetXml).OuterXml;
            SerializableWidget widget = SimpleXmlSerializer<SerializableWidget>.Deserialize(xml);
            Assert.IsNotNull(widget, "The object returned from the deserialization process was null when it was not expected to be.");
            Assert.AreEqual("MyValue", widget.MyProperty, "The value of the property after deserialization was not as expected.");
        }

        /// <summary>
        /// Tests deserialize with namespace.
        /// </summary>
        [TestMethod]
        public void UnitTestDeserializeWithXmlNamespace()
        {
            string xml = Resource.GetXmlDocument("StealFocus.Core.Tests", ResouceNames.SerializableWidgetWithXmlNamespaceXml).OuterXml;
            SerializableWidget widget = SimpleXmlSerializer<SerializableWidget>.Deserialize(xml, "http://www.MyCompany.com/Widgets");
            Assert.IsNotNull(widget, "The object returned from the deserialization process was null when it was not expected to be.");
            Assert.AreEqual("MyValue", widget.MyProperty, "The value of the property after deserialization was not as expected.");
        }

        #endregion // Unit Tests

        #region Structs

        /// <summary>
        /// Holds the resource names.
        /// </summary>
        private struct ResouceNames
        {
            /// <summary>
            /// Holds the SerializableWidget resource name.
            /// </summary>
            public const string SerializableWidgetXml = "StealFocus.Core.Tests.Xml.Serialization.Resources.SerializableWidget.xml";

            /// <summary>
            /// Holds the SerializableWidgetWithXmlNamespace resource name.
            /// </summary>
            public const string SerializableWidgetWithXmlNamespaceXml = "StealFocus.Core.Tests.Xml.Serialization.Resources.SerializableWidgetWithXmlNamespace.xml";
        }

        #endregion // Structs
    }
}