// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="UseAppropriateClassNames.cs" company="StealFocus">
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
    /// Checks that Classes are not called "Utils", "Utilities" or "Helper".
    /// </summary>
    internal class UseAppropriateClassNames : BaseRule
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UseAppropriateClassNames"/> class.
        /// </summary>
        public UseAppropriateClassNames() : base("UseAppropriateClassNames")
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
        /// <param name="type">The type to check.</param>
        /// <returns>Any problems.</returns>
        public override ProblemCollection Check(TypeNode type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            string name = type.Name.Name;
            if (name.EndsWith("Utilities", StringComparison.OrdinalIgnoreCase) || name.EndsWith("Utils", StringComparison.OrdinalIgnoreCase))
            {
                Problems.Add(new Problem(GetNamedResolution("Utilities", name), name));
            }
            else if (name.EndsWith("Helper", StringComparison.OrdinalIgnoreCase))
            {
                Problems.Add(new Problem(GetNamedResolution("Helper", name), name));
            }

            return Problems;
        }

        #endregion Methods
    }
}
