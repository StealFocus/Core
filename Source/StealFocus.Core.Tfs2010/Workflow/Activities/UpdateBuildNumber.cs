// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="UpdateBuildNumber.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the UpdateBuildNumber type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

namespace StealFocus.Core.Tfs2010.Workflow.Activities
{
    using System;
    using System.Activities;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text.RegularExpressions;

    using Microsoft.TeamFoundation.Build.Client;

    using StealFocus.Core.Tfs2010.Build.Client;

    [BuildActivity(HostEnvironmentOption.All)]
    public sealed class UpdateBuildNumber : CodeActivity
    {
        private const string VersionReplaceToken = "{0}";

        /// <remarks>
        /// E.g. "Acme.PetShop-Trunk-Full-{0}" where "{0}" is the version number.
        /// </remarks>
        [RequiredArgument]
        public InArgument<string> BuildNumberFormat { get; set; }

        [RequiredArgument]
        public InArgument<int> MajorVersion { get; set; }

        [RequiredArgument]
        public InArgument<int> MinorVersion { get; set; }

        /// <remarks>
        /// E.g. "Acme.PetShop-Trunk-Full-1.0.11205.1".
        /// </remarks>
        public OutArgument<string> BuildNumber { get; set; }

        /// <remarks>
        /// E.g. "1.0.11205.1".
        /// </remarks>
        public OutArgument<string> VersionNumber { get; set; }

        public static Version GetVersionNumber(int major, int minor, DateTime dateOfBuild, int revision)
        {
            int buildComponent = GetBuildComponentOfVersionNumberForDate(dateOfBuild);
            return new Version(major, minor, buildComponent, revision);
        }

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            base.CacheMetadata(metadata);
            metadata.RequireExtension(typeof(IBuildDetail));
        }

        protected override void Execute(CodeActivityContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (!this.BuildNumberFormat.Get(context).Contains(VersionReplaceToken))
            {
                string message = string.Format(CultureInfo.CurrentCulture, "The 'BuildNumberFormat' value did not contain '{0}' as required (this will be replaced by the version number).", VersionReplaceToken);
                throw new ArgumentException(message);
            }

            IBuildDetail buildDetail = context.GetExtension<IBuildDetail>();
            BuildServerFacade buildServerFacade = new BuildServerFacade(buildDetail.BuildServer);
            DateTime dateOfBuild = DateTime.Now;

            // e.g. latestBuildNumberFromAllBuildDefinitions = "Acme.PetShop-Trunk-Full-0.0.11114.3"
            string[] latestBuildNumbersFromAllBuildDefinitions = buildServerFacade.GetLatestBuildNumbersFromAllBuildDefinitions(buildDetail.TeamProject, 10);
            Version nextVersionNumber;
            if (latestBuildNumbersFromAllBuildDefinitions.Length < 1)
            {
                // No previous builds at all so just create a new version from scratch.
                nextVersionNumber = GetFreshVersionNumber(this.MajorVersion.Get(context), this.MinorVersion.Get(context), dateOfBuild);
            }
            else
            {
                Version latestVersionNumber = GetLatestVersionNumberFromPreviousBuildNumbers(latestBuildNumbersFromAllBuildDefinitions);
                if (latestVersionNumber == null)
                {
                    // The previous build number was not found to have a version number contained in it.
                    nextVersionNumber = GetFreshVersionNumber(this.MajorVersion.Get(context), this.MinorVersion.Get(context), dateOfBuild);
                }
                else
                {
                    // We found a version number in the previous build number so increment it.
                    nextVersionNumber = GetNextVersionNumber(latestVersionNumber, this.MajorVersion.Get(context), this.MinorVersion.Get(context), dateOfBuild);
                }
            }

            string nextBuildNumber = string.Format(CultureInfo.CurrentCulture, this.BuildNumberFormat.Get(context), nextVersionNumber);
            this.BuildNumber.Set(context, nextBuildNumber);
            this.VersionNumber.Set(context, nextVersionNumber.ToString());
            buildDetail.BuildNumber = nextBuildNumber;
            buildDetail.Save();
        }

        private static Version GetLatestVersionNumberFromPreviousBuildNumbers(IEnumerable<string> previousBuildNumbers)
        {
            foreach (string buildNumber in previousBuildNumbers)
            {
                Match match = Regex.Match(buildNumber, @"\d+.\d+.\d+.\d+");
                if (match.Success)
                {
                    string[] strings = match.Value.Split('.');
                    int major = int.Parse(strings[0], CultureInfo.CurrentCulture);
                    int minor = int.Parse(strings[1], CultureInfo.CurrentCulture);
                    int build = int.Parse(strings[2], CultureInfo.CurrentCulture);
                    int revision = int.Parse(strings[3], CultureInfo.CurrentCulture);
                    return new Version(major, minor, build, revision);
                }
            }

            return null;
        }

        private static Version GetFreshVersionNumber(int majorVersion, int minorVersion, DateTime dateOfBuild)
        {
            return GetVersionNumber(majorVersion, minorVersion, dateOfBuild, 0);
        }

        private static Version GetNextVersionNumber(Version existingVersion, int configuredMajorVersion, int configuredMinorVersion, DateTime dateOfBuild)
        {
            int buildComponent = GetBuildComponentOfVersionNumberForDate(dateOfBuild);
            int revision = 0;
            if (existingVersion.Build == buildComponent)
            {
                revision = existingVersion.Revision + 1;
            }

            return GetVersionNumber(configuredMajorVersion, configuredMinorVersion, dateOfBuild, revision);
        }

        private static int GetBuildComponentOfVersionNumberForDate(DateTime dateOfBuild)
        {
            string year = dateOfBuild.Year.ToString("0", CultureInfo.CurrentCulture).Substring(3);
            string month = dateOfBuild.Month.ToString("00", CultureInfo.CurrentCulture);
            string day = dateOfBuild.Day.ToString("00", CultureInfo.CurrentCulture);
            string build = year + month + day;
            return int.Parse(build, CultureInfo.CurrentCulture);
        }
    }
}
