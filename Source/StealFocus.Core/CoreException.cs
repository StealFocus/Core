// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="CoreException.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the CoreException type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// CoreException Class.
    /// </summary>
    /// <remarks>
    /// Base exception class for the framework.
    /// </remarks>
    [Serializable]
    public class CoreException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CoreException"/> class.
        /// </summary>
        public CoreException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <remarks>
        /// None.
        /// </remarks>
        public CoreException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">An <see cref="Exception"/>. The exception that is the cause of the current exception. If the <paramref name="innerException"/> parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
        /// <remarks>
        /// None.
        /// </remarks>
        public CoreException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreException"/> class.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.</param>
        /// <remarks>
        /// None.
        /// </remarks>
        protected CoreException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
