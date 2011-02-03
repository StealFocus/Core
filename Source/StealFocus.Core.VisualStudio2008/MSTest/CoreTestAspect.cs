// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="CoreTestAspect.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the CoreTestAspect type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.VisualStudio2008.MSTest
{
    using System;
    using System.Reflection;
    using System.Runtime.Remoting.Messaging;

    /// <summary>
    /// CoreTestAspect Class.
    /// </summary>
    /// <typeparam name="TAttribute">The <see cref="Attribute"/> associated with the Aspect.</typeparam>
    /// <remarks />
    public abstract class CoreTestAspect<TAttribute> where TAttribute : Attribute
    {
        #region Methods

        /// <summary>
        /// GetAttribute Method.
        /// </summary>
        /// <param name="message">An <see cref="IMessage"/>. The message.</param>
        /// <returns>The attribute.</returns>
        protected TAttribute GetAttribute(IMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            string typeName = (string)message.Properties[MessageKeys.TypeName];
            string methodName = (string)message.Properties[MessageKeys.MethodName];
            Type callingType = Type.GetType(typeName);
            MethodInfo methodInfo = callingType.GetMethod(methodName);
            object[] attributes = methodInfo.GetCustomAttributes(typeof(TAttribute), true);
            TAttribute attribute = null;
            if (attributes.Length > 0)
            {
                attribute = attributes[0] as TAttribute;
            }

            return attribute;
        }

        /// <summary>
        /// FakeTargetResponse Method.
        /// </summary>
        /// <param name="message">An <see cref="IMessage"/>. The message.</param>
        /// <returns>An <see cref="IMessage"/>. The reply.</returns>
        protected IMessage FakeTargetResponse(IMessage message)
        {
            IMethodCallMessage methodCallMessage = new MethodCall(message);
            return new MethodResponse(new Header[0], methodCallMessage);
        }
        
        #endregion // Methods

        #region Structs

        /// <summary>
        /// Holds the keys from the message.
        /// </summary>
        private struct MessageKeys
        {
            /// <summary>
            /// TypeName key.
            /// </summary>
            public const string TypeName = "__TypeName";

            /// <summary>
            /// MethodName key.
            /// </summary>
            public const string MethodName = "__MethodName";
        }

        #endregion // Structs
    }
}