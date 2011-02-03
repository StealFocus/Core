// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="Resource.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the Resource type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core
{
    using System;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Resources;
    using System.Threading;
    using System.Xml;
    using System.Xml.Schema;

    /// <summary>
    /// Resource Class.
    /// </summary>
    /// <remarks />
    public static partial class Resource
    {
        /// <summary>
        /// Gets a resource string for the <c>requester</c> matching the <c>key</c>.
        /// </summary>
        /// <param name="requester">An <see cref="object"/>. The object requesting the resource.</param>
        /// <param name="key">The key identifying the resource..</param>
        /// <returns>The string retrieved from the Resource File.</returns>
        /// <remarks>
        /// None.
        /// </remarks>
        public static string GetString(object requester, string key)
        {
            if (requester == null)
            {
                throw new ArgumentNullException("requester");
            }

            return GetString(requester.GetType(), key);
        }

        /// <summary>
        /// Gets a resource string for the <c>requesterType</c> matching the <c>key</c>.
        /// </summary>
        /// <param name="requesterType">An <see cref="object"/>. The <see cref="Type"/> of the requester of the resource.</param>
        /// <param name="key">The key identifying the resource..</param>
        /// <returns>The string retrieved from the Resource File.</returns>
        /// <remarks>
        /// None.
        /// </remarks>
        /// <exception cref="CoreException">Thrown when the <see cref="ResourceManager"/> returns a null resource string for the provided key.</exception>
        public static string GetString(Type requesterType, string key)
        {
            if (requesterType == null)
            {
                throw new ArgumentNullException("requesterType");
            }

            ResourceManager resourceManager = new ResourceManager(requesterType);
            string resourceString = resourceManager.GetString(key, GetCurrentCulture());
            if (resourceString == null)
            {
                throw new CoreException(string.Format(CultureInfo.CurrentCulture, "No Resource String matching the key '{0}' could be found for type '{1}'.", key, requesterType.FullName));
            }

            return resourceString;
        }

        /// <summary>
        /// Loads an XML Document from a resource.
        /// </summary>
        /// <param name="assemblyName">The assembly name.</param>
        /// <param name="xmlDocumentResourceName">The XML Document Resource Name.</param>
        /// <returns>
        /// An <see cref="XmlDocument"/>, the embedded resource.
        /// </returns>
        /// <remarks>
        /// The <c>assemblyName</c> should be the assembly file name without the ".dll" extension. The <c>xmlDocumentResourceName</c> should be the fully qualified name of the resource e.g. "Namespace.DocumentName.xml".
        /// </remarks>
        public static XmlDocument GetXmlDocument(string assemblyName, string xmlDocumentResourceName)
        {
            return GetXmlDocument(GetAssemblyContainingResource(assemblyName), xmlDocumentResourceName);
        }

        /// <summary>
        /// Gets an XML Document from an embedded resource.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="xmlDocumentResourceName">The XML Document Resource Name.</param>
        /// <returns>
        /// An <see cref="XmlDocument"/>, the embedded resource.
        /// </returns>
        /// <remarks />
        public static XmlDocument GetXmlDocument(Assembly assembly, string xmlDocumentResourceName)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }

            XmlDocument document = new XmlDocument();
            using (Stream resourceStream = assembly.GetManifestResourceStream(xmlDocumentResourceName))
            {
                if (resourceStream == null)
                {
                    string exceptionMessage = string.Format(CultureInfo.CurrentCulture, "The resource '{0}' was not found in the assembly '{1}'.", xmlDocumentResourceName, assembly.FullName);
                    throw new CoreException(exceptionMessage);
                }

                document.Load(resourceStream);
            }

            return document;
        }

        /// <summary>
        /// Loads an XSD Schema from a resource.
        /// </summary>
        /// <param name="assemblyName">The assembly name.</param>
        /// <param name="xsdDocumentResourceName">The XSD Document Resource Name.</param>
        /// <returns>
        /// An <see cref="XmlSchema"/>, the embedded resource.
        /// </returns>
        /// <remarks>
        /// The <c>assemblyName</c> should be the assembly file name without the ".dll" extension. The <c>xsdDocumentResourceName</c> should be the fully qualified name of the resource e.g. "Namespace.DocumentName.xml".
        /// </remarks>
        public static XmlSchema GetXmlSchema(string assemblyName, string xsdDocumentResourceName)
        {
            return GetXmlSchema(GetAssemblyContainingResource(assemblyName), xsdDocumentResourceName);
        }

        /// <summary>
        /// Loads an XSD Schema from a resource.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="xsdDocumentResourceName">The XSD Document Resource Name.</param>
        /// <returns>
        /// An <see cref="XmlSchema"/>, the embedded resource.
        /// </returns>
        /// <remarks>
        /// The <c>assemblyName</c> should be the assembly file name without the ".dll" extension. The <c>xsdDocumentResourceName</c> should be the fully qualified name of the resource e.g. "Namespace.DocumentName.xml".
        /// </remarks>
        public static XmlSchema GetXmlSchema(Assembly assembly, string xsdDocumentResourceName)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }

            using (Stream resourceStream = assembly.GetManifestResourceStream(xsdDocumentResourceName))
            {
                if (resourceStream == null)
                {
                    string exceptionMessage = string.Format(CultureInfo.CurrentCulture, "The resource '{0}' was not found in the assembly '{1}'.", xsdDocumentResourceName, assembly.FullName);
                    throw new CoreException(exceptionMessage);
                }

                try
                {
                    return XmlSchema.Read(resourceStream, null);
                }
                catch (XmlException e)
                {
                    string exceptionMessage = string.Format(CultureInfo.CurrentCulture, "The XSD Schema in resource '{0}' from assembly '{1}' was not valid XML. Please see the stack trace for more information.", xsdDocumentResourceName, assembly.FullName);
                    throw new CoreException(exceptionMessage, e);
                }
                catch (XmlSchemaException e)
                {
                    string exceptionMessage = string.Format(CultureInfo.CurrentCulture, "The XSD Schema in resource '{0}' from assembly '{1}' was invalid. Please see the stack trace for more information.", xsdDocumentResourceName, assembly.FullName);
                    throw new CoreException(exceptionMessage, e);
                }
            }
        }

        /// <summary>
        /// Get a file from an embedded resource.
        /// </summary>
        /// <param name="assemblyName">The assembly name.</param>
        /// <param name="fileResourceName">The name of the resource.</param>
        /// <returns>
        /// A Stream containing the file.
        /// </returns>
        public static Stream GetFile(string assemblyName, string fileResourceName)
        {
            return GetFile(GetAssemblyContainingResource(assemblyName), fileResourceName);
        }

        /// <summary>
        /// Get a file from an embedded resource.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="fileResourceName">The name of the resource.</param>
        /// <returns>
        /// A Stream containing the file.
        /// </returns>
        public static Stream GetFile(Assembly assembly, string fileResourceName)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }

            return assembly.GetManifestResourceStream(fileResourceName);
        }

        /// <summary>
        /// Gets a file from an embedded resource and writes it to a path.
        /// </summary>
        /// <param name="assemblyName">The assembly name.</param>
        /// <param name="fileResourceName">The name of the resource.</param>
        /// <param name="filePathToBeWrittenTo">The path to write the file to.</param>
        public static void GetFileAndWriteToPath(string assemblyName, string fileResourceName, string filePathToBeWrittenTo)
        {
            GetFileAndWriteToPath(GetAssemblyContainingResource(assemblyName), fileResourceName, filePathToBeWrittenTo);
        }

        /// <summary>
        /// Gets a file from an embedded resource and writes it to a path.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="fileResourceName">The name of the resource.</param>
        /// <param name="filePathToBeWrittenTo">The path to write the file to.</param>
        public static void GetFileAndWriteToPath(Assembly assembly, string fileResourceName, string filePathToBeWrittenTo)
        {
            using (StreamReader sr = new StreamReader(GetFile(assembly, fileResourceName)))
            {
                using (StreamWriter sw = new StreamWriter(filePathToBeWrittenTo))
                {
                    while (sr.Peek() >= 0)
                    {
                        sw.WriteLine(sr.ReadLine());
                    }

                    sw.Flush();
                }
            }
        }

        /// <summary>
        /// Gets a binary file from an embedded resource and writes it to a path.
        /// </summary>
        /// <param name="assemblyName">The assembly name.</param>
        /// <param name="fileResourceName">The name of the resource.</param>
        /// <param name="filePathToBeWrittenTo">The path to write the file to.</param>
        public static void GetBinaryFileAndWriteToPath(string assemblyName, string fileResourceName, string filePathToBeWrittenTo)
        {
            GetBinaryFileAndWriteToPath(GetAssemblyContainingResource(assemblyName), fileResourceName, filePathToBeWrittenTo);
        }

        /// <summary>
        /// Gets a binary file from an embedded resource and writes it to a path.
        /// </summary>
        /// <param name="fileResourceName">The name of the resource.</param>
        /// <param name="filePathToBeWrittenTo">The path to write the file to.</param>
        public static void GetBinaryFileAndWriteToPath(string fileResourceName, string filePathToBeWrittenTo)
        {
            GetBinaryFileAndWriteToPath(Assembly.GetExecutingAssembly(), fileResourceName, filePathToBeWrittenTo);
        }

        /// <summary>
        /// Gets a binary file from an embedded resource and writes it to a path.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="fileResourceName">The name of the resource.</param>
        /// <param name="filePathToBeWrittenTo">The path to write the file to.</param>
        public static void GetBinaryFileAndWriteToPath(Assembly assembly, string fileResourceName, string filePathToBeWrittenTo)
        {
            using (FileStream fs = new FileStream(filePathToBeWrittenTo, FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    using (Stream stream = GetFile(assembly, fileResourceName))
                    {
                        byte[] buffer = new byte[(int)stream.Length];
                        stream.Read(buffer, 0, (int)stream.Length);
                        bw.Write(buffer);
                    }
                }
            }
        }

        /// <summary>
        /// Gets an embedded image resource.
        /// </summary>
        /// <param name="assemblyName">The assembly name.</param>
        /// <param name="imageResourceName">The image resource name.</param>
        /// <returns>
        /// An image from the embedded resource.
        /// </returns>
        public static Bitmap GetImage(string assemblyName, string imageResourceName)
        {
            return GetImage(GetAssemblyContainingResource(assemblyName), imageResourceName);
        }

        /// <summary>
        /// Gets an embedded image resource.
        /// </summary>
        /// <param name="imageResourceName">The image resource name.</param>
        /// <returns>
        /// An image from the embedded resource.
        /// </returns>
        public static Bitmap GetImage(string imageResourceName)
        {
            return GetImage(Assembly.GetExecutingAssembly(), imageResourceName);
        }

        /// <summary>
        /// Gets an embedded image resource.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="imageResourceName">The image resource name.</param>
        /// <returns>
        /// An image from the embedded resource.
        /// </returns>
        public static Bitmap GetImage(Assembly assembly, string imageResourceName)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }

            Stream stream = assembly.GetManifestResourceStream(imageResourceName);
            if (stream == null)
            {
                string exceptionMessage = string.Format(CultureInfo.CurrentCulture, "The assembly '{0}' did not contain the resource '{1}'.", assembly.FullName, imageResourceName);
                throw new CoreException(exceptionMessage);
            }

            return new Bitmap(stream);
        }

        /// <summary>
        /// Gets an assembly resource.
        /// </summary>
        /// <param name="assemblyName">The assembly name.</param>
        /// <param name="assemblyResourceName">The assembly resource name.</param>
        /// <returns>
        /// An assembly from the embedded resource.
        /// </returns>
        public static byte[] GetAssemblyBytes(string assemblyName, string assemblyResourceName)
        {
            return GetAssemblyBytes(GetAssemblyContainingResource(assemblyName), assemblyResourceName);
        }

        /// <summary>
        /// Gets an assembly resource.
        /// </summary>
        /// <param name="assemblyResourceName">The assembly resource name.</param>
        /// <returns>
        /// An assembly from the embedded resource.
        /// </returns>
        public static byte[] GetAssemblyBytes(string assemblyResourceName)
        {
            return GetAssemblyBytes(Assembly.GetExecutingAssembly(), assemblyResourceName);
        }

        /// <summary>
        /// Gets an assembly resource.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="assemblyResourceName">The assembly resource name.</param>
        /// <returns>
        /// An assembly from the embedded resource.
        /// </returns>
        public static byte[] GetAssemblyBytes(Assembly assembly, string assemblyResourceName)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }

            Stream stream = assembly.GetManifestResourceStream(assemblyResourceName);
            if (stream == null)
            {
                string exceptionMessage = string.Format(CultureInfo.CurrentCulture, "The assembly '{0}' did not contain the resource '{1}'.", assembly.FullName, assemblyResourceName);
                throw new CoreException(exceptionMessage);
            }

            byte[] buffer = new byte[(int)stream.Length];
            stream.Read(buffer, 0, (int)stream.Length);
            return buffer;
        }

        /// <summary>
        /// Gets an assembly resource.
        /// </summary>
        /// <param name="assemblyName">The assembly name.</param>
        /// <param name="assemblyResourceName">The assembly resource name.</param>
        /// <returns>
        /// An assembly from the embedded resource.
        /// </returns>
        public static Assembly GetAssembly(string assemblyName, string assemblyResourceName)
        {
            return GetAssembly(GetAssemblyContainingResource(assemblyName), assemblyResourceName);
        }

        /// <summary>
        /// Gets an assembly resource.
        /// </summary>
        /// <param name="assemblyResourceName">The assembly resource name.</param>
        /// <returns>
        /// An assembly from the embedded resource.
        /// </returns>
        public static Assembly GetAssembly(string assemblyResourceName)
        {
            return GetAssembly(Assembly.GetExecutingAssembly(), assemblyResourceName);
        }

        /// <summary>
        /// Gets an assembly resource.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="assemblyResourceName">The assembly resource name.</param>
        /// <returns>
        /// An assembly from the embedded resource.
        /// </returns>
        public static Assembly GetAssembly(Assembly assembly, string assemblyResourceName)
        {
            return Assembly.Load(GetAssemblyBytes(assembly, assemblyResourceName));
        }

        /// <summary>
        /// Gets the assembly containing the resource.
        /// </summary>
        /// <param name="assemblyName">The assembly Name.</param>
        /// <returns>
        /// An assembly.
        /// </returns>
        private static Assembly GetAssemblyContainingResource(string assemblyName)
        {
            try
            {
                return Assembly.Load(assemblyName);
            }
            catch (FileNotFoundException e)
            {
                string exceptionMessage = string.Format(CultureInfo.CurrentCulture, "The requested Assembly '{0}' holding the Resource could not be found.", assemblyName);
                throw new CoreException(exceptionMessage, e);
            }
        }

        /// <summary>
        /// Gets the current <see cref="CultureInfo">culture</see>.
        /// </summary>
        /// <returns>The culture information for the current thread.</returns>
        /// <remarks>
        /// None.
        /// </remarks>
        private static CultureInfo GetCurrentCulture()
        {
            return Thread.CurrentThread.CurrentCulture;
        }
    }
}