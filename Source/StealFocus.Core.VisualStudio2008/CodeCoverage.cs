// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="CodeCoverage.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the CodeCoverage type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.VisualStudio2008
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.IO;
    using System.Xml;

    using Microsoft.VisualStudio.CodeCoverage;

    /// <summary>
    /// CodeCoverage Class.
    /// </summary>
    /// <remarks/>
    public class CodeCoverage
    {
        #region Fields

        /// <summary>
        /// Artefact directory.
        /// </summary>
        private const string ArtifactDirectoryRelativePath = @"..\..\Out";

        /// <summary>
        /// Name of the file containing the coverage data.
        /// </summary>
        private const string DataCoverageFileName = "data.coverage";

        /// <summary>
        /// Holds the code coverage information.
        /// </summary>
        private readonly CoverageInfo coverageInfo;

        /// <summary>
        /// Holds the code coverage XML.
        /// </summary>
        private readonly XmlDocument coverageXml;

        #endregion // Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeCoverage"/> class. 
        /// </summary>
        /// <param name="pathToCoverageFile">
        /// The path to the coverage file (the file is typically called <c>data.coverage</c>). This value should be the full path to that file.
        /// </param>
        /// <remarks>
        /// </remarks>
        public CodeCoverage(string pathToCoverageFile)
        {
            string directory = Path.GetDirectoryName(pathToCoverageFile);
            string artifactDirectory = Path.Combine(directory, ArtifactDirectoryRelativePath);
            CoverageInfoManager.ExePath = artifactDirectory;
            CoverageInfoManager.SymPath = artifactDirectory;
            this.coverageInfo = CoverageInfoManager.CreateInfoFromFile(pathToCoverageFile);
            this.coverageXml = this.GetCoverageXml();
        }

        #endregion // Constructors

        #region Methods

        /// <summary>
        /// Gets the coverage file path.
        /// </summary>
        /// <param name="workspaceLocalFoldersToSearch">The work space local folders to search.</param>
        /// <returns>The path to the code coverage file.</returns>
        /// <remarks/>
        public static string GetCoverageFilePath(string[] workspaceLocalFoldersToSearch)
        {
            string mostRecentTestResultFolder = TestResult.GetMostRecentTestResultFolder(workspaceLocalFoldersToSearch);
            string[] coverageDataFiles = Directory.GetFiles(mostRecentTestResultFolder, DataCoverageFileName, SearchOption.AllDirectories);
            if (coverageDataFiles.Length < 1)
            {
                throw new CoreException("The Visual Studio Code Coverage file could not be found from the current Workspace mappings. Has a test run been complete to produce the code coverage data?");
            }

            if (coverageDataFiles.Length > 1)
            {
                throw new CoreException("More than one Visual Studio Code Coverage file was found. Could not determine which to use. Please delete your 'TestResults' folder and perform a test run to produce new code coverage data.");
            }

            return coverageDataFiles[0];
        }

        /// <summary>
        /// Writes the XML to file.
        /// </summary>
        /// <param name="outputPath">The output path.</param>
        /// <remarks/>
        public void WriteXmlToFile(string outputPath)
        {
            CoverageDS coverageDS = this.coverageInfo.BuildDataSet(null);
            coverageDS.WriteXml(outputPath);
        }

        /// <summary>
        /// Writes the XML to a stream.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <remarks/>
        public void WriteXml(Stream stream)
        {
            CoverageDS coverageDS = this.coverageInfo.BuildDataSet(null);
            coverageDS.WriteXml(stream);
        }

        /// <summary>
        /// Gets all the Namespaces from the Code Coverage data.
        /// </summary>
        /// <returns>An array of Namespace Key Names e.g. <c>MyAssembly.dllMyNamespace.MySubNamespace</c>. This is the concatenation of the Assembly name (including file extension) and the namespace.</returns>
        /// <remarks />
        public string[] GetAllNamespaces()
        {
            XmlNodeList namespaceKeyNameNodes = this.coverageXml.SelectNodes(XPaths.NamespaceKeyNames);
            if (namespaceKeyNameNodes == null)
            {
                throw new CoreException("No Namespace Key Nodes found in the Code Coverage file.");
            }

            ArrayList namespaceKeyNameList = new ArrayList(namespaceKeyNameNodes.Count);
            foreach (XmlNode namespaceKeyNameNode in namespaceKeyNameNodes)
            {
                namespaceKeyNameList.Add(namespaceKeyNameNode.InnerXml);
            }

            return (string[])namespaceKeyNameList.ToArray(typeof(string));
        }

        /// <summary>
        /// Gets the coverage for assembly.
        /// </summary>
        /// <param name="assemblyName">The name of the assembly e.g. "MyCompany.Application.dll".</param>
        /// <returns>An <see cref="int"/>. The code coverage.</returns>
        /// <remarks/>
        public int GetCoverageForAssembly(string assemblyName)
        {
            string xpath = string.Format(CultureInfo.CurrentCulture, XPaths.Assembly, assemblyName);
            XmlNode moduleNode = this.coverageXml.SelectSingleNode(xpath);
            if (moduleNode == null)
            {
                return -1;
            }

            decimal blocksCovered = decimal.Parse(moduleNode.SelectSingleNode(XmlElementNames.BlocksCovered).InnerXml, CultureInfo.CurrentCulture);
            decimal blocksNotCovered = decimal.Parse(moduleNode.SelectSingleNode(XmlElementNames.BlocksNotCovered).InnerXml, CultureInfo.CurrentCulture);
            decimal totalBlocks = blocksCovered + blocksNotCovered;
            if (blocksNotCovered == 0)
            {
                return 100;
            }

            return Convert.ToInt32((blocksCovered / totalBlocks) * 100);
        }

        /// <summary>
        /// Gets the coverage for namespace.
        /// </summary>
        /// <param name="namespaceKeyName">The Namespace Key Name e.g. <c>MyAssembly.dllMyNamespace.MySubNamespace</c>. This is the concatenation of the Assembly name (including file extension) and the namespace.</param>
        /// <returns>The coverage according to the lines covered/not covered. If the <c>namespaceKeyName</c> is not found, <c>-1</c> is returned.</returns>
        /// <remarks>
        /// The assembly name and namespace name are provided because it is possible for the same namespace to exist in two different assemblies (though this is bad practice).
        /// </remarks>
        public int GetCoverageForNamespace(string namespaceKeyName)
        {
            string xpath = string.Format(CultureInfo.CurrentCulture, XPaths.NamespaceTableByNamespaceKeyName, namespaceKeyName);
            XmlNode namespaceTableNode = this.coverageXml.SelectSingleNode(xpath);
            if (namespaceTableNode == null)
            {
                return -1;
            }

            decimal blocksCovered = decimal.Parse(namespaceTableNode.SelectSingleNode(XmlElementNames.BlocksCovered).InnerXml, CultureInfo.CurrentCulture);
            decimal blocksNotCovered = decimal.Parse(namespaceTableNode.SelectSingleNode(XmlElementNames.BlocksNotCovered).InnerXml, CultureInfo.CurrentCulture);
            decimal totalBlocks = blocksCovered + blocksNotCovered;
            if (blocksNotCovered == 0)
            {
                return 100;
            }

            return Convert.ToInt32((blocksCovered / totalBlocks) * 100);
        }

        /// <summary>
        /// Gets the coverage for namespace.
        /// </summary>
        /// <param name="assemblyName">The assembly name e.g. <c>MyAssembly.dll</c>. It must include the extension (.dll or .exe).</param>
        /// <param name="namespaceName">The namespace.</param>
        /// <returns>The coverage according to the lines covered/not covered. If the <c>namespaceKeyName</c> is not found, <c>-1</c> is returned.</returns>
        /// <remarks>
        /// The assembly name and namespace name are provided because it is possible for the same namespace to exist in two different assemblies (though this is bad practice).
        /// </remarks>
        public int GetCoverageForNamespace(string assemblyName, string namespaceName)
        {
            return this.GetCoverageForNamespace(assemblyName + namespaceName);
        }

        /// <summary>
        /// Gets the coverage for method.
        /// </summary>
        /// <param name="namespaceKeyName">The Namespace Key Name e.g. <c>MyAssembly.dllMyNamespace.MySubNamespace</c>. This is the concatenation of the Assembly name (including file extension) and the namespace.</param>
        /// <param name="className">The (unqualified) class name.</param>
        /// <param name="methodName">The Method name plus signature e.g. "MyMethod(string,string)".</param>
        /// <returns>An <see cref="int"/>. The code coverage.</returns>
        /// <remarks />
        public int GetCoverageForMethod(string namespaceKeyName, string className, string methodName)
        {
            return this.GetCoverageForMethod(namespaceKeyName + "." + className, methodName);
        }

        /// <summary>
        /// Gets the coverage for method.
        /// </summary>
        /// <param name="classKeyName">The full class name (missing the last period) e.g. "StealFocus.Core.VisualStudio2008.CodeCoverage" = "StealFocus.Core.VisualStudio2008CodeCoverage".</param>
        /// <param name="methodName">The Method name plus signature e.g. "MyMethod(string,string)".</param>
        /// <returns>An <see cref="int"/>. The code coverage.</returns>
        /// <remarks />
        public int GetCoverageForMethod(string classKeyName, string methodName)
        {
            if (classKeyName == null)
            {
                throw new ArgumentNullException("classKeyName");
            }

            if (methodName == null)
            {
                throw new ArgumentNullException("methodName");
            }

            // For some reason the "ClassKeyName" in the coverage XML is missing the "." betweeen the last 
            // namespace component and the class name
            // e.g. "StealFocus.Core.VisualStudio2008.CodeCoverage" = "StealFocus.Core.VisualStudio2008CodeCoverage"
            // We need to replicate this.
            string modifiedClassKeyName = classKeyName.Remove(classKeyName.LastIndexOf('.'), 1);
            string xpath = string.Format(CultureInfo.CurrentCulture, XPaths.Method, modifiedClassKeyName, methodName);
            XmlNode methodNode = this.coverageXml.SelectSingleNode(xpath);
            if (methodNode == null)
            {
                return -1;
            }

            decimal blocksCovered = decimal.Parse(methodNode.SelectSingleNode(XmlElementNames.BlocksCovered).InnerXml, CultureInfo.CurrentCulture);
            decimal blocksNotCovered = decimal.Parse(methodNode.SelectSingleNode(XmlElementNames.BlocksNotCovered).InnerXml, CultureInfo.CurrentCulture);
            decimal totalBlocks = blocksCovered + blocksNotCovered;
            if (blocksNotCovered == 0)
            {
                return 100;
            }

            return Convert.ToInt32((blocksCovered / totalBlocks) * 100);
        }

        /// <summary>
        /// Gets the coverage XML.
        /// </summary>
        /// <returns>An <see cref="XmlDocument"/>. The coverage XML.</returns>
        private XmlDocument GetCoverageXml()
        {
            XmlDocument tempCoverageXml;
            using (MemoryStream ms = new MemoryStream())
            {
                CoverageDS coverageDS = this.coverageInfo.BuildDataSet(null);
                coverageDS.WriteXml(ms);
                ms.Seek(0, SeekOrigin.Begin);
                tempCoverageXml = new XmlDocument();
                tempCoverageXml.Load(ms);
            }

            return tempCoverageXml;
        }

        #endregion // Methods

        #region Structs

        /// <summary>
        /// Represents the XML element names.
        /// </summary>
        private struct XmlElementNames
        {
            /// <summary>
            /// BlocksCovered Element name.
            /// </summary>
            public const string BlocksCovered = "BlocksCovered";

            /// <summary>
            /// BlocksNotCovered Element name.
            /// </summary>
            public const string BlocksNotCovered = "BlocksNotCovered";
        }

        /// <summary>
        /// Represents the XPaths.
        /// </summary>
        private struct XPaths
        {
            /// <summary>
            /// Namespace Table By Namespace Key Name XPath.
            /// </summary>
            public const string NamespaceTableByNamespaceKeyName = "/CoverageDSPriv/Module/NamespaceTable[NamespaceKeyName = '{0}']";

            /// <summary>
            /// Namespace Key Names XPath.
            /// </summary>
            public const string NamespaceKeyNames = "/CoverageDSPriv/Module/NamespaceTable/NamespaceKeyName";

            /// <summary>
            /// Method XPath.
            /// </summary>
            public const string Method = "/CoverageDSPriv/Module/NamespaceTable/Class[ClassKeyName = '{0}']/Method[MethodName = '{1}']";

            /// <summary>
            /// Assembly XPath.
            /// </summary>
            public const string Assembly = "/CoverageDSPriv/Module[ModuleName = '{0}']";
        }

        #endregion // Structs
    }
}
