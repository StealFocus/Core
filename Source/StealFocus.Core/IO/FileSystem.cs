// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileSystem.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   FileSystem Class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StealFocus.Core.IO
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Security.AccessControl;
    using System.Security.Cryptography;

    /// <summary>
    /// FileSystem Class.
    /// </summary>
    /// <remarks>
    /// Contains methods to help when working with the file system.
    /// </remarks>
    public static class FileSystem
    {
        #region Fields

        /// <summary>
        /// Used to indicate if a file is an assembly.
        /// </summary>
        private const int AssemblyExpected = -2146234344;

        #endregion // Fields

        #region Methods

        /// <summary>
        /// Gets a hash of a file at a given path.
        /// </summary>
        /// <param name="pathToFile">The path to the file.</param>
        /// <returns>The hash of the given file.</returns>
        public static string ComputeHash(string pathToFile)
        {
            CheckFilePathParameter("pathToFile", pathToFile);
            using (HashAlgorithm mdfiveHasher = MD5.Create())
            {
                byte[] hash;
                using (StreamReader streamReader = new StreamReader(pathToFile))
                {
                    hash = mdfiveHasher.ComputeHash(streamReader.BaseStream);
                }

                return BitConverter.ToString(hash);
            }
        }

        /// <summary>
        /// Compares two files and indicates if they are the same.
        /// </summary>
        /// <param name="pathToFile1">A path to file 1.</param>
        /// <param name="pathToFile2">A path to file 2.</param>
        /// <returns>Indicates if the files are the same.</returns>
        public static bool Compare(string pathToFile1, string pathToFile2)
        {
            CheckFilePathParameter("pathToFile1", pathToFile1);
            CheckFilePathParameter("pathToFile2", pathToFile2);
            string hashOfFile1 = ComputeHash(pathToFile1);
            string hashOfFile2 = ComputeHash(pathToFile2);
            if (hashOfFile1 == hashOfFile2)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines if a file is an Assembly.
        /// </summary>
        /// <param name="filePath">The path of the file.</param>
        /// <returns>Indicating if the file is an assembly.</returns>
        public static bool IsAssembly(string filePath)
        {
            try
            {
                AssemblyName.GetAssemblyName(filePath);
                return true;
            }
            catch
            {
                return false;
            }

            // TO DO: Sort this out.
            // catch (BadImageFormatException e)
            // {
            //   int result = Marshal.GetHRForException(e);
            //   return result != AssemblyExpected;
            // }
        }

        /// <summary>
        /// Copies the Access Control List (ACL) from one file to another.
        /// </summary>
        /// <param name="pathToSourceFile">The path to the source file.</param>
        /// <param name="pathToDestinationFile">The path to the destination file.</param>
        public static void CopyAccessControlList(string pathToSourceFile, string pathToDestinationFile)
        {
            CopyAccessControlList(pathToSourceFile, pathToDestinationFile, new FileSystemAccessRule[0]);
        }

        /// <summary>
        /// Copies the Access Control List (ACL) from one file to another and specify additional ACL rules on the destination file.
        /// </summary>
        /// <param name="pathToSourceFile">The path to the source file.</param>
        /// <param name="pathToDestinationFile">The path to the destination file.</param>
        /// <param name="additionalFileSystemAccessRules">An array of <see cref="FileSystemAccessRule"/>. The additional ACLs.</param>
        public static void CopyAccessControlList(string pathToSourceFile, string pathToDestinationFile, FileSystemAccessRule[] additionalFileSystemAccessRules)
        {
            if (additionalFileSystemAccessRules == null)
            {
                throw new ArgumentNullException("additionalFileSystemAccessRules");
            }

            CheckFilePathParameter("pathToSourceFile", pathToSourceFile);
            CheckFilePathParameter("pathToDestinationFile", pathToDestinationFile);
            FileSecurity sourceFileSecurity = File.GetAccessControl(pathToSourceFile);
            FileSecurity destinationFileSecurity = new FileSecurity();
            byte[] securityDescriptor = sourceFileSecurity.GetSecurityDescriptorBinaryForm();
            destinationFileSecurity.SetSecurityDescriptorBinaryForm(securityDescriptor);
            foreach (FileSystemAccessRule fileSystemAccessRule in additionalFileSystemAccessRules)
            {
                destinationFileSecurity.AddAccessRule(fileSystemAccessRule);
            }

            File.SetAccessControl(pathToDestinationFile, destinationFileSecurity);
        }

        /// <summary>
        /// Check if a file is at a given path when a path is given as a parameter.
        /// </summary>
        /// <param name="filePathParameterName">The name of the parameter.</param>
        /// <param name="filePathParameterValue">The path given in the parameter value.</param>
        private static void CheckFilePathParameter(string filePathParameterName, string filePathParameterValue)
        {
            if (filePathParameterValue == null)
            {
                throw new ArgumentNullException(filePathParameterName);
            }

            if (!File.Exists(filePathParameterValue))
            {
                string exceptionMessage = string.Format(CultureInfo.CurrentCulture, "The provided file path of '{0}' for parameter '{1}' was not valid.", filePathParameterValue, filePathParameterName);
                throw new CoreException(exceptionMessage);
            }
        }

        #endregion // Methods
    }
}
