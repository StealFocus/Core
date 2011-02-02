// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateVersionLabeller.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the DateVersionLabeller type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StealFocus.Core.CCNet
{
    using System;
    using System.Globalization;

    using Exortech.NetReflector;

    using ThoughtWorks.CruiseControl.Core;

    /// <summary>
    /// DateVersionLabeller Class.
    /// </summary>
    [ReflectorType("dateVersionLabeller")]
    public class DateVersionLabeller : ILabeller
    {
        #region Fields

        /// <summary>
        /// Holds the separator for version numbers.
        /// </summary>
        private const char Separator = '.';

        /// <summary>
        /// Holds the format for today's date.
        /// </summary>
        private const string TodayDateNumberFormat = "yMMdd";

        /// <summary>
        /// Holds the version.
        /// </summary>
        private Version version = new Version(0, 0, 0, 0);

        #endregion // Fields

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether to initialise the DateVersion.
        /// </summary>
        /// <remarks>
        /// When set to true the previous build label will be ignored if not a valid version number.
        /// </remarks>
        [ReflectorProperty("initialise", Required = true, InstanceType = typeof(bool))]
        public bool Initialise
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the major version number.
        /// </summary>
        [ReflectorProperty("majorVersionNumber", Required = true, InstanceType = typeof(int))]
        public int MajorVersionNumber
        {
            get { return this.version.Major; }
            set { this.version = new Version(value, this.version.Minor, this.version.Build, this.version.Revision); }
        }

        /// <summary>
        /// Gets or sets the minor version number.
        /// </summary>
        [ReflectorProperty("minorVersionNumber", Required = true, InstanceType = typeof(int))]
        public int MinorVersionNumber
        {
            get { return this.version.Minor; }
            set { this.version = new Version(this.version.Major, value, this.version.Build, this.version.Revision); }
        }

        /// <summary>
        /// Gets or sets the build number.
        /// </summary>
        [ReflectorProperty("buildNumber", Required = false, InstanceType = typeof(int))]
        public int BuildNumber
        {
            get { return this.version.Build; }
            set { this.version = new Version(this.version.Major, this.version.Minor, value, this.version.Revision); }
        }

        /// <summary>
        /// Gets or sets the revision number.
        /// </summary>
        [ReflectorProperty("revisionNumber", Required = false, InstanceType = typeof(int))]
        public int RevisionNumber
        {
            get { return this.version.Revision; }
            set { this.version = new Version(this.version.Major, this.version.Minor, this.version.Build, value); }
        }

        #endregion // Properties

        #region ILabeller Members

        /// <summary>
        /// Generate the label.
        /// </summary>
        /// <param name="integrationResult">The result from last build.</param>
        /// <returns>The label.</returns>
        [CLSCompliant(false)]
        public string Generate(IIntegrationResult integrationResult)
        {
            if (integrationResult == null)
            {
                throw new ArgumentNullException("integrationResult");
            }

            int todayDateNumber = int.Parse(DateTime.Now.ToString(TodayDateNumberFormat, CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);
            if (integrationResult.Label == null)
            {
                this.MajorVersionNumber = 1;
                this.MinorVersionNumber = 0;
                this.BuildNumber = todayDateNumber;
                this.RevisionNumber = 0;
                return this.version.ToString();
            }

            string[] buildNumberComponents = integrationResult.Label.Split(Separator);
            if (buildNumberComponents.Length != 4 && !this.Initialise)
            {
                throw new ArgumentException("The Label from the previous result was not a valid assembly version number.");
            }

            if (buildNumberComponents.Length != 4 && this.Initialise)
            {
                this.MajorVersionNumber = this.MajorVersionNumber;
                this.MinorVersionNumber = this.MinorVersionNumber;
                this.BuildNumber = 0;
                this.RevisionNumber = 0;
            }

            if (buildNumberComponents.Length == 4)
            {
                this.MajorVersionNumber = int.Parse(buildNumberComponents[0], CultureInfo.CurrentCulture);
                this.MinorVersionNumber = int.Parse(buildNumberComponents[1], CultureInfo.CurrentCulture);
                this.BuildNumber = int.Parse(buildNumberComponents[2], CultureInfo.CurrentCulture);
                this.RevisionNumber = int.Parse(buildNumberComponents[3], CultureInfo.CurrentCulture);
            }

            if (this.BuildNumber == todayDateNumber)
            {
                this.RevisionNumber = this.RevisionNumber + 1;
            }
            else
            {
                this.BuildNumber = todayDateNumber;
                this.RevisionNumber = 0;
            }

            return this.version.ToString();
        }

        #region ITask Members

        /// <summary>
        /// Runs the task.
        /// </summary>
        /// <param name="result">The result.</param>
        [CLSCompliant(false)]
        public void Run(IIntegrationResult result)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            result.Label = this.Generate(result);
        }

        #endregion // ITask Members

        #endregion // ILabeller Members
    }
}