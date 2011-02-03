// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="TestResult.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the TestResult type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.VisualStudio2008
{
    using System;
    using System.IO;

    /// <summary>
    /// TestResult Class.
    /// </summary>
    public static class TestResult
    {
        #region Fields

        /// <summary>
        /// Holds the TestResults folder name.
        /// </summary>
        private const string TestResultsFolderName = "TestResults";

        #endregion // Fields

        #region Methods

        /// <summary>
        /// Gets the most recent folder under the "TestResults" folder found in the solution.
        /// </summary>
        /// <param name="workspaceLocalFoldersToSearch">The workspace folders to search.</param>
        /// <returns>The path to the most recent test results.</returns>
        public static string GetMostRecentTestResultFolder(string[] workspaceLocalFoldersToSearch)
        {
            if (workspaceLocalFoldersToSearch == null)
            {
                throw new ArgumentNullException("workspaceLocalFoldersToSearch");
            }

            string mostRecentTestResultFolder = null;
            foreach (string folder in workspaceLocalFoldersToSearch)
            {
                string[] testResultsFolders = Directory.GetDirectories(folder, TestResultsFolderName, SearchOption.AllDirectories);
                foreach (string testResultsFolder in testResultsFolders)
                {
                    string[] testResultFolders = Directory.GetDirectories(testResultsFolder);
                    DateTime mostRecentModDate = DateTime.MinValue;
                    foreach (string testResultFolder in testResultFolders)
                    {
                        if (Directory.GetCreationTimeUtc(testResultFolder) > mostRecentModDate)
                        {
                            mostRecentTestResultFolder = testResultFolder;
                        }
                    }
                }
            }

            if (mostRecentTestResultFolder == null)
            {
                throw new CoreException("The folder holding the Visual Studio Code Coverage file could not be found from the current Workspace mappings. Have any tests been run to produce test data?");
            }

            return mostRecentTestResultFolder;
        }

        #endregion // Methods
    }
}
