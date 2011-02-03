// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ExpectedExceptionMessageAttribute.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the ExpectedExceptionMessageAttribute type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.VisualStudio2008.MSTest
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// ExpectedExceptionMessageAttribute Class.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Use to declare an expected Exception with a specific message.
    /// </para>
    /// <para>
    /// The MSTest attribute of <see cref="ExpectedExceptionAttribute"/> has the 
    /// property <see cref="ExpectedExceptionAttribute.Message"/> but this does not
    /// specify the expected Exception message but the message to be logged to the 
    /// test output.
    /// </para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ExpectedExceptionMessageAttribute : Attribute
    {
        #region Fields

        /// <summary>
        /// Holds the exception type.
        /// </summary>
        private readonly Type exceptionType;

        /// <summary>
        /// Holds the exception message.
        /// </summary>
        private readonly string exceptionMessage;

        #endregion // Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpectedExceptionMessageAttribute"/> class.
        /// </summary>
        /// <param name="exceptionType">The exception type.</param>
        /// <param name="exceptionMessage">The exception message.</param>
        public ExpectedExceptionMessageAttribute(Type exceptionType, string exceptionMessage)
        {
            this.exceptionType = exceptionType;
            this.exceptionMessage = exceptionMessage;
        }

        #endregion // Constructors

        #region Properties

        /// <summary>
        /// Gets ExceptionType.
        /// </summary>
        /// <value>
        /// The exception type.
        /// </value>
        public Type ExceptionType
        {
            get { return this.exceptionType; }
        }

        /// <summary>
        /// Gets ExceptionMessage.
        /// </summary>
        /// <value>
        /// The exception message.
        /// </value>
        public string ExceptionMessage
        {
            get { return this.exceptionMessage; }
        }

        #endregion // Properties
    }
}