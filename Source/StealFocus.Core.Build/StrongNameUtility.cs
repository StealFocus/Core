// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StrongNameUtility.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the StrongNameUtility type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StealFocus.Core.Build
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Security.Cryptography;
    using System.Text;

    using Microsoft.Build.Utilities;

    /// <summary>
    /// StrongNameUtility Class.
    /// </summary>
    /// <remarks>
    /// Provides helpers to run "sn.exe" and manipulate Strong Name Keys.
    /// </remarks>
    public static class StrongNameUtility
    {
        /// <summary>
        /// Holds the name of "sn.exe".
        /// </summary>
        private const string SNDotExeName = "sn.exe";

        /// <summary>
        /// Generate an Strong Name Key pair.
        /// </summary>
        /// <param name="targetDotNetFrameworkVersion">The target .NET Framework version.</param>
        /// <param name="strongNameKeyPairPath">The path to the Strong Name Key pair.</param>
        public static void GenerateKeyPair(TargetDotNetFrameworkVersion targetDotNetFrameworkVersion, string strongNameKeyPairPath)
        {
            string args = string.Format(CultureInfo.CurrentCulture, "-k \"{0}\"", strongNameKeyPairPath);
            ExecuteSNDotExe(targetDotNetFrameworkVersion, args);
        }

        /// <summary>
        /// Generate a Public Key.
        /// </summary>
        /// <param name="targetDotNetFrameworkVersion">The target .NET Framework version.</param>
        /// <param name="strongNameKeyPairPath">The path to the Strong Name Key pair.</param>
        /// <param name="publicKeyFilePath">The path to write to.</param>
        public static void GeneratePublicKey(TargetDotNetFrameworkVersion targetDotNetFrameworkVersion, string strongNameKeyPairPath, string publicKeyFilePath)
        {
            string args = string.Format(CultureInfo.CurrentCulture, "-p \"{0}\" \"{1}\"", strongNameKeyPairPath, publicKeyFilePath);
            ExecuteSNDotExe(targetDotNetFrameworkVersion, args);
        }

        /// <summary>
        /// Extract the public key token from the given Strong Name Key Pair File.
        /// </summary>
        /// <param name="strongNameKeyPairFilePath">The path to the Strong Name Key Pair File.</param>
        /// <returns>The Public Key Token.</returns>
        public static string ExtractPublicKeyToken(string strongNameKeyPairFilePath)
        {
            StrongNameKeyPair strongNameKeyPair = GetStrongNameKey(strongNameKeyPairFilePath);
            byte[] publicKey = strongNameKeyPair.PublicKey;
            byte[] publicKeyToken = new byte[8];
            using (SHA1 sha1 = new SHA1Managed())
            {
                byte[] hash = sha1.ComputeHash(publicKey);
                Array.Copy(hash, hash.Length - publicKeyToken.Length, publicKeyToken, 0, publicKeyToken.Length);
            }
            
            Array.Reverse(publicKeyToken, 0, publicKeyToken.Length);
            StringBuilder stringBuilder = new StringBuilder(16);
            for (int i = 0; i < publicKeyToken.Length; i++)
            {
                stringBuilder.Append(publicKeyToken[i].ToString("x2", CultureInfo.CurrentCulture));
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Extract the public key from the given Strong Name Key Pair File.
        /// </summary>
        /// <param name="strongNameKeyPairFilePath">The path to the Strong Name Key Pair File.</param>
        /// <returns>The Public Key.</returns>
        public static string ExtractPublicKey(string strongNameKeyPairFilePath)
        {
            StrongNameKeyPair strongNameKeyPair = GetStrongNameKey(strongNameKeyPairFilePath);
            byte[] publicKey = strongNameKeyPair.PublicKey;
            StringBuilder stringBuilder = new StringBuilder(16);
            for (int i = 0; i < publicKey.Length; i++)
            {
                stringBuilder.Append(publicKey[i].ToString("x2", CultureInfo.CurrentCulture));
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Get the Strong Name Key Pair.
        /// </summary>
        /// <param name="strongNameKeyPairFilePath">The path to the Strong Name Key Pair File.</param>
        /// <returns>The Strong Name Key Pair.</returns>
        private static StrongNameKeyPair GetStrongNameKey(string strongNameKeyPairFilePath)
        {
            byte[] buffer;
            using (FileStream strongNameKeyPairFileStream = new FileStream(strongNameKeyPairFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                buffer = new byte[(int)strongNameKeyPairFileStream.Length];
                strongNameKeyPairFileStream.Read(buffer, 0, (int)strongNameKeyPairFileStream.Length);
            }

            return new StrongNameKeyPair(buffer);
        }

        /// <summary>
        /// Execute sn.exe.
        /// </summary>
        /// <param name="targetDotNetFrameworkVersion">The target .NET Framework version.</param>
        /// <param name="strongNameDotExePathArguments">The arguments to supply.</param>
        private static void ExecuteSNDotExe(TargetDotNetFrameworkVersion targetDotNetFrameworkVersion, string strongNameDotExePathArguments)
        {
            string strongNameDotExePath = ToolLocationHelper.GetPathToDotNetFrameworkSdkFile(SNDotExeName, targetDotNetFrameworkVersion);
            ProcessStartInfo processStartInfo = new ProcessStartInfo(strongNameDotExePath, strongNameDotExePathArguments);
            processStartInfo.CreateNoWindow = true;
            processStartInfo.RedirectStandardError = true;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.UseShellExecute = false;
            Process process = Process.Start(processStartInfo);
            string standardOutput = process.StandardOutput.ReadToEnd();
            string standardError = process.StandardError.ReadToEnd();
            process.WaitForExit();
            Console.WriteLine(standardOutput);
            if (process.ExitCode != 0)
            {
                Console.WriteLine(standardError);
            }
        }
    }
}
