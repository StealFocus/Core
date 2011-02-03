// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="UseAppropriateNamespaces.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the UseAppropriateNamespaces type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.VisualStudio2008.FxCop.Rules
{
    using System;

    using Microsoft.FxCop.Sdk;

    /// <summary>
    /// Checks that Namespaces do not contain "Utils", "Utilities", "Helpers" or "Common".
    /// </summary>
    internal class UseAppropriateNamespaces : BaseRule
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UseAppropriateNamespaces"/> class. 
        /// </summary>
        public UseAppropriateNamespaces() : base("UseAppropriateNamespaces")
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// This member overrides <see cref="BaseIntrospectionRule.TargetVisibility"/>.
        /// </summary>
        /// <value>
        /// This property is <see cref="TargetVisibilities.All"/>.
        /// </value>
        public override TargetVisibilities TargetVisibility
        {
            get { return TargetVisibilities.All; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Check Method.
        /// </summary>
        /// <param name="namespaceName">The namespace.</param>
        /// <param name="types">The types.</param>
        /// <returns>Any problems.</returns>
        public override ProblemCollection Check(string namespaceName, TypeNodeCollection types)
        {
            if (namespaceName == null)
            {
                throw new ArgumentNullException("namespaceName");
            }

            if (namespaceName.Contains("Utilities"))
            {
                System.Diagnostics.Debug.WriteLine("UseAppropriateNamespaces: Utilities");
                Problems.Add(new Problem(GetNamedResolution("Utilities", namespaceName), namespaceName));
            }
            else if (namespaceName.Contains("Utils"))
            {
                System.Diagnostics.Debug.WriteLine("UseAppropriateNamespaces: Utils");
                Problems.Add(new Problem(GetNamedResolution("Utilities", namespaceName), namespaceName));
            }
            else if (namespaceName.Contains("Helpers"))
            {
                System.Diagnostics.Debug.WriteLine("UseAppropriateNamespaces: Helpers");
                Problems.Add(new Problem(GetNamedResolution("Helpers", namespaceName), namespaceName));
            }
            else if (namespaceName.Contains("Common"))
            {
                System.Diagnostics.Debug.WriteLine("UseAppropriateNamespaces: Common");
                Problems.Add(new Problem(GetNamedResolution("Common", namespaceName), namespaceName));
            }

            return Problems;
        }

        #endregion Methods
    }
}
