// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="CommandLineToolTests.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the CommandLineToolTest type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.Tests
{
    using System;
    using System.Diagnostics;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// CommandLineToolTests Class.
    /// </summary>
    public abstract class CommandLineToolTests
    {
        /// <summary>
        /// Run a command line tool.
        /// </summary>
        /// <param name="fileName">The executable file.</param>
        /// <param name="arguments">The arguments.</param>
        /// <param name="workingDirectory">The working directory.</param>
        protected static void RunCommandLineTool(string fileName, string arguments, string workingDirectory)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo(fileName, arguments);
            processStartInfo.CreateNoWindow = true;
            processStartInfo.RedirectStandardError = true;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.UseShellExecute = false;
            processStartInfo.WorkingDirectory = workingDirectory;
            Process process = Process.Start(processStartInfo);
            Console.WriteLine(process.StandardOutput.ReadToEnd());
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                Console.WriteLine(process.StandardError.ReadToEnd());
                Assert.Fail("The command line tool failed.");
            }
        }
    }
}
