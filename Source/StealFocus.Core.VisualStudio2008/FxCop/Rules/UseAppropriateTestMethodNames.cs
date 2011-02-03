// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="UseAppropriateTestMethodNames.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the UseAppropriateTestMethodNames type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.VisualStudio2008.FxCop.Rules
{
    using Microsoft.FxCop.Sdk;

    /// <summary>
    /// UseAppropriateTestMethodNames Class.
    /// </summary>
    internal class UseAppropriateTestMethodNames : BaseRule
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UseAppropriateTestMethodNames"/> class.
        /// </summary>
        public UseAppropriateTestMethodNames() : base("UseAppropriateTestMethodNames")
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
        /// Check Member.
        /// </summary>
        /// <param name="member">The member to check.</param>
        /// <returns>Any problems.</returns>
        public override ProblemCollection Check(Member member)
        {
            // To do.
            return Problems;
        }

        #endregion Methods
    }
}
