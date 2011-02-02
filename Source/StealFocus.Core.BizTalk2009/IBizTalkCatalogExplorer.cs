// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="IBizTalkCatalogExplorer.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the IBizTalkCatalogExplorer type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.BizTalk2009
{
    /// <summary>
    /// IBizTalkCatalogExplorer Interface.
    /// </summary>
    public interface IBizTalkCatalogExplorer
    {
        /// <summary>
        /// Change database.
        /// </summary>
        /// <param name="newManagementDatabaseConnectionString">The new connection string.</param>
        void ChangeManagementDatabase(string newManagementDatabaseConnectionString);

        /// <summary>
        /// Check if the <c>BizTalk</c> application exists.
        /// </summary>
        /// <param name="applicationName">The application name.</param>
        /// <returns>Indicating if the application exists.</returns>
        bool ApplicationExists(string applicationName);

        /// <summary>
        /// Create a new <c>BizTalk</c> application.
        /// </summary>
        /// <param name="applicationName">The <c>BizTalk</c> application name.</param>
        /// <returns>An <see cref="Microsoft.BizTalk.ExplorerOM.IBizTalkApplication"/>. The new application.</returns>
        Microsoft.BizTalk.ExplorerOM.IBizTalkApplication CreateApplication(string applicationName);

        /// <summary>
        /// Get the <c>BizTalk</c> application.
        /// </summary>
        /// <param name="applicationName">The <c>BizTalk</c> application name.</param>
        /// <returns>An <see cref="Microsoft.BizTalk.ExplorerOM.IBizTalkApplication"/>. The application.</returns>
        Microsoft.BizTalk.ExplorerOM.IBizTalkApplication GetApplication(string applicationName);

        /// <summary>
        /// Get application names.
        /// </summary>
        /// <returns>The application names.</returns>
        string[] GetApplicationNames();

        /// <summary>
        /// Remove the <c>BizTalk</c> application.
        /// </summary>
        /// <param name="applicationName">The application name.</param>
        void RemoveApplication(string applicationName);
    }
}