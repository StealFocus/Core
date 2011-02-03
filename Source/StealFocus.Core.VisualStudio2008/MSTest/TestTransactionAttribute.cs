// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="TestTransactionAttribute.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the TestTransactionAttribute type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace StealFocus.Core.VisualStudio2008.MSTest
{
    using System;
    using System.Transactions;

    /// <summary>
    /// TestTransactionAttribute Class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class TestTransactionAttribute : Attribute
    {
        #region Fields

        /// <summary>
        /// Holds whether to commit the transaction.
        /// </summary>
        private readonly bool commit;
        
        /// <summary>
        /// Holds the scope options.
        /// </summary>
        private readonly TransactionScopeOption transactionScopeOption;

        /// <summary>
        /// Holds the transaction options.
        /// </summary>
        private readonly TransactionOptions transactionOptions;
        
        #endregion // Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TestTransactionAttribute"/> class.
        /// </summary>
        public TestTransactionAttribute() : this(false, TransactionScopeOption.Required, new TransactionOptions())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestTransactionAttribute"/> class.
        /// </summary>
        /// <param name="commit">Whether to commit.</param>
        public TestTransactionAttribute(bool commit) : this(commit, TransactionScopeOption.Required, new TransactionOptions())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestTransactionAttribute"/> class.
        /// </summary>
        /// <param name="commit">Whether to commit.</param>
        /// <param name="transactionScopeOption">The Transaction Scope Option.</param>
        /// <param name="transactionOptions">The Transaction Options.</param>
        public TestTransactionAttribute(bool commit, TransactionScopeOption transactionScopeOption, TransactionOptions transactionOptions)
        {
            this.commit = commit;
            this.transactionScopeOption = transactionScopeOption;
            this.transactionOptions = transactionOptions;
        }

        #endregion // Constructors

        #region Properties

        /// <summary>
        /// Gets a value indicating whether Commit.
        /// </summary>
        public bool Commit
        {
            get { return this.commit; }
        }

        /// <summary>
        /// Gets TransactionScopeOption.
        /// </summary>
        public TransactionScopeOption TransactionScopeOption
        {
            get { return this.transactionScopeOption; }
        }

        /// <summary>
        /// Gets TransactionOptions.
        /// </summary>
        public TransactionOptions TransactionOptions
        {
            get { return this.transactionOptions; }
        }

        #endregion // Properties
    }
}
