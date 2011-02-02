// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="BizTalkCatalogExplorer.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the BizTalkCatalogExplorer type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.BizTalk2009
{
    using System;
    using System.Collections;
    using System.Globalization;

    using Microsoft.BizTalk.ApplicationDeployment;
    using Microsoft.BizTalk.ExplorerOM;

    /// <summary>
    /// BizTalkCatalogExplorer Class.
    /// </summary>
    public class BizTalkCatalogExplorer : IBizTalkCatalogExplorer
    {
        #region Fields

        /// <summary>
        /// Holds the connection string.
        /// </summary>
        private readonly string managementDatabaseConnectionString;

        /// <summary>
        /// Holds the <c>BizTalk</c> catalog explorer.
        /// </summary>
        private readonly IBtsCatalogExplorer2 btsCatalogExplorer;

        /// <summary>
        /// Holds the database server name.
        /// </summary>
        private readonly string managementDatabaseServerName;

        /// <summary>
        /// Holds the database name.
        /// </summary>
        private readonly string managementDatabaseName;

        #endregion // Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BizTalkCatalogExplorer"/> class. 
        /// </summary>
        /// <param name="managementDatabaseConnectionString">The connection string.</param>
        public BizTalkCatalogExplorer(string managementDatabaseConnectionString) : this(new BtsCatalogExplorer(), managementDatabaseConnectionString)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BizTalkCatalogExplorer"/> class. 
        /// </summary>
        /// <param name="btsCatalogExplorer">The <c>BizTalk</c> catalog explorer.</param>
        /// <param name="managementDatabaseConnectionString">The connection string.</param>
        public BizTalkCatalogExplorer(IBtsCatalogExplorer2 btsCatalogExplorer, string managementDatabaseConnectionString)
        {
            if (btsCatalogExplorer == null)
            {
                throw new ArgumentNullException("btsCatalogExplorer");
            }

            if (string.IsNullOrEmpty(managementDatabaseConnectionString))
            {
                throw new ArgumentException("The 'managementDatabaseConnectionString' must not be null or empty.");
            }

            this.managementDatabaseConnectionString = managementDatabaseConnectionString;
            string[] connStringComponents = this.managementDatabaseConnectionString.Split(';');
            foreach (string component in connStringComponents)
            {
                if (component.StartsWith("server", StringComparison.OrdinalIgnoreCase))
                {
                    this.managementDatabaseServerName = component.Split('=')[1];
                }
                else if (component.StartsWith("database", StringComparison.OrdinalIgnoreCase))
                {
                    this.managementDatabaseName = component.Split('=')[1];
                }
            }

            this.btsCatalogExplorer = btsCatalogExplorer;
            this.btsCatalogExplorer.ConnectionString = this.managementDatabaseConnectionString;
        }

        #endregion // Constructors

        #region IBizTalkCatalogExplorer Members

        /// <summary>
        /// Change database.
        /// </summary>
        /// <param name="newManagementDatabaseConnectionString">The new connection string.</param>
        public void ChangeManagementDatabase(string newManagementDatabaseConnectionString)
        {
            this.btsCatalogExplorer.ConnectionString = newManagementDatabaseConnectionString;
        }

        /// <summary>
        /// Check if the <c>BizTalk</c> application exists.
        /// </summary>
        /// <param name="applicationName">The application name.</param>
        /// <returns>Indicating if the application exists.</returns>
        public bool ApplicationExists(string applicationName)
        {
            if (string.IsNullOrEmpty(applicationName))
            {
                throw new ArgumentException("The applicationName should not be null or empty.", "applicationName");
            }

            return this.GetApplication(applicationName) != null;
        }

        /// <summary>
        /// Create a new <c>BizTalk</c> application.
        /// </summary>
        /// <param name="applicationName">The <c>BizTalk</c> application name.</param>
        /// <returns>An <see cref="Microsoft.BizTalk.ExplorerOM.IBizTalkApplication"/>. The new application.</returns>
        public Microsoft.BizTalk.ExplorerOM.IBizTalkApplication CreateApplication(string applicationName)
        {
            if (string.IsNullOrEmpty(applicationName))
            {
                throw new ArgumentException("The applicationName should not be null or empty.", "applicationName");
            }

            Microsoft.BizTalk.ExplorerOM.IBizTalkApplication bizTalkApplication = this.btsCatalogExplorer.AddNewApplication();
            bizTalkApplication.Name = applicationName;
            this.btsCatalogExplorer.SaveChanges();
            this.btsCatalogExplorer.Refresh();
            return bizTalkApplication;
        }

        /// <summary>
        /// Get the <c>BizTalk</c> application.
        /// </summary>
        /// <param name="applicationName">The <c>BizTalk</c> application name.</param>
        /// <returns>An <see cref="Microsoft.BizTalk.ExplorerOM.IBizTalkApplication"/>. The application.</returns>
        public Microsoft.BizTalk.ExplorerOM.IBizTalkApplication GetApplication(string applicationName)
        {
            if (string.IsNullOrEmpty(applicationName))
            {
                throw new ArgumentException("The applicationName should not be null or empty.", "applicationName");
            }

            foreach (Microsoft.BizTalk.ExplorerOM.IBizTalkApplication application in this.btsCatalogExplorer.Applications)
            {
                if (application.Name == applicationName)
                {
                    return application;
                }
            }

            return null;
        }

        /// <summary>
        /// Get application names.
        /// </summary>
        /// <returns>The application names.</returns>
        public string[] GetApplicationNames()
        {
            ArrayList applicationNameList = new ArrayList();
            foreach (Microsoft.BizTalk.ExplorerOM.IBizTalkApplication bizTalkApplication in this.btsCatalogExplorer.Applications)
            {
                applicationNameList.Add(bizTalkApplication.Name);
            }

            return (string[])applicationNameList.ToArray(typeof(string));
        }

        /// <summary>
        /// Remove the <c>BizTalk</c> application.
        /// </summary>
        /// <param name="applicationName">The application name.</param>
        public void RemoveApplication(string applicationName)
        {
            if (string.IsNullOrEmpty(applicationName))
            {
                throw new ArgumentException("The applicationName should not be null or empty.", "applicationName");
            }

            if (!this.ApplicationExists(applicationName))
            {
                string exceptionMessage = string.Format(CultureInfo.CurrentCulture, "The BizTalk application with the name '{0}' does not exist and could not be removed.", applicationName);
                throw new CoreException(exceptionMessage);
            }

            using (Group group = new Group())
            {
                // Bring application to a stop (we can't removed an app with running Orchestrations)
                BizTalkApplication bizTalkApplication1 = new BizTalkApplication(this.managementDatabaseConnectionString, applicationName);
                bizTalkApplication1.StopAll();
                BizTalkApplication bizTalkApplication2 = new BizTalkApplication(this.managementDatabaseConnectionString, applicationName);
                bizTalkApplication2.TerminateAllOrchestrations();

                // Now remove the app
                group.DBName = this.managementDatabaseName;
                group.DBServer = this.managementDatabaseServerName;
                Microsoft.BizTalk.ApplicationDeployment.ApplicationCollection applications = group.Applications;
                applications.UiLevel = 2;
                Microsoft.BizTalk.ApplicationDeployment.Application applicationToRemove = applications[applicationName];
                applications.Remove(applicationToRemove);
            }
        }

        #endregion // IBizTalkCatalogExplorer Members
    }
}