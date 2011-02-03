// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="TestSelector.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the TestSelector type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.VisualStudio2008
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.IO;
    using System.Reflection;

    using IO;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// TestSelector Class.
    /// </summary>
    public static class TestSelector
    {
        /// <summary>
        /// Relative from the folder holding the test results, typically the name of this folder is of the form factor "username_machineName YYYY-MM-DD HH_mm_SS".
        /// </summary>
        private const string ArtifactDirectoryRelativePath = "Out";

        /// <summary>
        /// Holds a *.* pattern.
        /// </summary>
        private const string UnrestrictedSearchPatter = "*.*";

        /// <summary>
        /// Get All Test Names.
        /// </summary>
        /// <param name="assemblyDirectory">The directory containing the assemblies.</param>
        /// <returns>A list of test names.</returns>
        /// <remarks />
        public static StringCollection GetAllTestNames(string assemblyDirectory)
        {
            return GetAllTestNames(assemblyDirectory, UnrestrictedSearchPatter);
        }

        /// <summary>
        /// Get All Test Names.
        /// </summary>
        /// <param name="assemblyDirectory">The directory containing the assemblies.</param>
        /// <param name="searchPattern">The search pattern to use when looking for assemblies e.g. MyCompany.*.dll.</param>
        /// <returns>A list of test names.</returns>
        /// <remarks />
        public static StringCollection GetAllTestNames(string assemblyDirectory, string searchPattern)
        {
            string[] filesInAssemblyDirectory = Directory.GetFiles(assemblyDirectory, searchPattern, SearchOption.TopDirectoryOnly);
            ArrayList assembliesInAssemblyDirectoryList = new ArrayList();
            foreach (string file in filesInAssemblyDirectory)
            {
                if (FileSystem.IsAssembly(file))
                {
                    assembliesInAssemblyDirectoryList.Add(file);
                }
            }

            string[] assembliesInAssemblyDirectory = (string[])assembliesInAssemblyDirectoryList.ToArray(typeof(string));
            StringCollection fullyQualifiedMethodNames = new StringCollection();

            // Load all the assemblies before loading the types in each. If we do not load all the 
            // assemblies before we start iterating the types contained within, we wil get type load 
            // exceptions because of the dependencies.
            ArrayList assemblyList = new ArrayList(assembliesInAssemblyDirectory.Length);
            foreach (string assemblyPath in assembliesInAssemblyDirectory)
            {
                assemblyList.Add(Assembly.LoadFrom(assemblyPath));
            }

            Assembly[] assemblies = (Assembly[])assemblyList.ToArray(typeof(Assembly));
            foreach (Assembly assembly in assemblies)
            {
                string assemblyName = Path.GetFileName(assembly.ManifestModule.ScopeName);
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.IsClass && type.IsPublic)
                    {
                        string fullyQualifiedClassName = type.FullName;

                        // TODO: Investigate whether we need to interrogate the inheritence chain i.e. set the boolean flag to true.
                        object[] customClassAttributes = type.GetCustomAttributes(typeof(TestClassAttribute), false);
                        if (customClassAttributes.Length > 0)
                        {
                            MethodInfo[] methods = type.GetMethods();
                            foreach (MethodInfo methodInfo in methods)
                            {
                                object[] customMethodAttributes = methodInfo.GetCustomAttributes(typeof(TestMethodAttribute), false);
                                if (customMethodAttributes.Length > 0)
                                {
                                    // This is a "TestMethod" in a "TestClass".
                                    // Get the fully qualified name of the method:
                                    // "MyCompany.dllMyCompany.Namespace.ClassName.MethodName()"
                                    // Please note: Bit of a hack here - all Test Methods will have a "()" signature.
                                    fullyQualifiedMethodNames.Add(assemblyName + fullyQualifiedClassName + "." + methodInfo.Name + "()");
                                }
                            }
                        }
                    }
                }
            }

            return fullyQualifiedMethodNames;
        }

        /// <summary>
        /// Get All Test Names.
        /// </summary>
        /// <param name="workspaceLocalFoldersToSearch">The directories to search.</param>
        /// <returns>A list of test names.</returns>
        /// <remarks />
        public static StringCollection GetAllTestNames(string[] workspaceLocalFoldersToSearch)
        {
            return GetAllTestNames(workspaceLocalFoldersToSearch, UnrestrictedSearchPatter);
        }

        /// <summary>
        /// Get All Test Names.
        /// </summary>
        /// <param name="workspaceLocalFoldersToSearch">The directories to search.</param>
        /// <param name="searchPattern">The search pattern to use when looking for assemblies e.g. MyCompany.*.dll.</param>
        /// <returns>A list of test names.</returns>
        /// <remarks />
        public static StringCollection GetAllTestNames(string[] workspaceLocalFoldersToSearch, string searchPattern)
        {
            // Get the folder holding the last test run, this is the newest folder in the "TestResults" folder.
            string mostRecentTestResultFolder = TestResult.GetMostRecentTestResultFolder(workspaceLocalFoldersToSearch);

            // All the assemblies for the current test run will be held in the "Out" folder under the 
            // "mostRecentTestResultFolder" directory - get the path for this folder.
            // So, "assemblyDirectory" = "<path>\TestResults\username_machineName YYYY-MM-DD HH_mm_SS\Out".
            string assemblyDirectory = Path.Combine(mostRecentTestResultFolder, ArtifactDirectoryRelativePath);
            return GetAllTestNames(assemblyDirectory, searchPattern);
        }
    }
}
