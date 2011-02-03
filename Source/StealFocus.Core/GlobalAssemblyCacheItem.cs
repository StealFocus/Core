// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="GlobalAssemblyCacheItem.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the GlobalAssemblyCacheItem type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core
{
    /// <summary>
    /// GlobalAssemblyCacheItem Class.
    /// </summary>
    public class GlobalAssemblyCacheItem
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalAssemblyCacheItem"/> class. 
        /// </summary>
        /// <param name="name">The name of the item.</param>
        /// <param name="version">The version.</param>
        /// <param name="locale">The locale.</param>
        /// <param name="publicKeyToken">The public key token.</param>
        public GlobalAssemblyCacheItem(string name, string version, string locale, string publicKeyToken)
        {
            this.Name = name;
            this.Version = version;
            this.Locale = locale;
            this.PublicKeyToken = publicKeyToken;
        }

        #endregion // Constructors

        #region Properties

        /// <summary>
        /// Gets the Locale.
        /// </summary>
        public string Locale
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the Name.
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the PublicKeyToken.
        /// </summary>
        public string PublicKeyToken
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the Version.
        /// </summary>
        public string Version
        {
            get;
            private set;
        }

        #endregion // Properties
    }
}
