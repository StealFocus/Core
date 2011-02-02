// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DateVersionLabellerTests.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the DateVersionLabellerTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StealFocus.Core.CCNet.Tests
{
    using System;
    using System.Globalization;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Rhino.Mocks;

    using ThoughtWorks.CruiseControl.Core;

    /// <summary>
    /// DateVersionLabellerTests Class.
    /// </summary>
    [TestClass]
    public class DateVersionLabellerTests
    {
        #region Fields

        /// <summary>
        /// Holds the date format.
        /// </summary>
        private const string TodayDateNumberFormat = "yMMdd";

        /// <summary>
        /// Holds the mocks.
        /// </summary>
        private MockRepository mocks;

        /// <summary>
        /// Holds the mock integration result.
        /// </summary>
        private IIntegrationResult mockIntegrationResult;

        #endregion // Fields

        #region Initialize & Cleanup

        /// <summary>
        /// Initialises the test.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.mocks = new MockRepository();
            this.mockIntegrationResult = this.mocks.DynamicMock<IIntegrationResult>();
        }

        #endregion Initialize & Cleanup

        #region Unit Tests

        /// <summary>
        /// Test <see cref="DateVersionLabeller.Run"/> with null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UnitTestRunWithArgumentNull()
        {
            DateVersionLabeller dateVersionLabeller = new DateVersionLabeller();
            dateVersionLabeller.MajorVersionNumber = 1;
            dateVersionLabeller.MinorVersionNumber = 0;
            dateVersionLabeller.Run(null);
        }

        /// <summary>
        /// Test <see cref="DateVersionLabeller.Generate"/> with null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UnitTestGenerateWithArgumentNull()
        {
            DateVersionLabeller dateVersionLabeller = new DateVersionLabeller();
            dateVersionLabeller.MajorVersionNumber = 1;
            dateVersionLabeller.MinorVersionNumber = 0;
            dateVersionLabeller.Generate(null);
        }

        /// <summary>
        /// Test <see cref="DateVersionLabeller.Generate"/> the first build of the day.
        /// </summary>
        [TestMethod]
        public void UnitTestGenerateForFirstBuildOfTheDay()
        {
            using (this.mocks.Record())
            {
                Expect.Call(this.mockIntegrationResult.Label).Return("1.0.0.0");
                LastCall.Repeat.Once();
                Expect.Call(this.mockIntegrationResult.Label).Return("1.0.0.0");
                LastCall.Repeat.Once();
            }

            DateVersionLabeller dateVersionLabeller = new DateVersionLabeller();
            dateVersionLabeller.MajorVersionNumber = 1;
            dateVersionLabeller.MinorVersionNumber = 0;
            string label;
            using (this.mocks.Playback())
            {
                label = dateVersionLabeller.Generate(this.mockIntegrationResult);
            }

            string expected = GetLabelForToday(1, 0, 0);
            Assert.AreEqual(expected, label, "The Label returned was not as expected.");
            this.mocks.VerifyAll();
        }

        /// <summary>
        /// Test <see cref="DateVersionLabeller.Generate"/> not the first build of the day.
        /// </summary>
        [TestMethod]
        public void UnitTestGenerateForNotFirstBuildOfTheDay()
        {
            string previousLabel = GetLabelForToday(1, 0, 4);
            using (this.mocks.Record())
            {
                Expect.Call(this.mockIntegrationResult.Label).Return(previousLabel);
                LastCall.Repeat.Once();
                Expect.Call(this.mockIntegrationResult.Label).Return(previousLabel);
                LastCall.Repeat.Once();
            }

            DateVersionLabeller dateVersionLabeller = new DateVersionLabeller();
            dateVersionLabeller.MajorVersionNumber = 1;
            dateVersionLabeller.MinorVersionNumber = 0;
            string label;
            using (this.mocks.Playback())
            {
                label = dateVersionLabeller.Generate(this.mockIntegrationResult);
            }

            string expected = GetLabelForToday(1, 0, 5);
            Assert.AreEqual(expected, label, "The Label returned was not as expected.");
            this.mocks.VerifyAll();
        }

        /// <summary>
        /// Test <see cref="DateVersionLabeller.Generate"/> with null previous label.
        /// </summary>
        [TestMethod]
        public void UnitTestGenerateWithNullPreviousLabel()
        {
            using (this.mocks.Record())
            {
                Expect.Call(this.mockIntegrationResult.Label).Return(null);
                LastCall.Repeat.Once();
            }

            DateVersionLabeller dateVersionLabeller = new DateVersionLabeller();
            dateVersionLabeller.MajorVersionNumber = 1;
            dateVersionLabeller.MinorVersionNumber = 0;
            string label;
            using (this.mocks.Playback())
            {
                label = dateVersionLabeller.Generate(this.mockIntegrationResult);
            }

            string expected = GetLabelForToday(1, 0, 0);
            Assert.AreEqual(expected, label, "The Label returned was not as expected.");
            this.mocks.VerifyAll();
        }

        /// <summary>
        /// Test <see cref="DateVersionLabeller.Generate"/> with invalid previous label.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void UnitTestGenerateWithInvalidPreviousLabel()
        {
            using (this.mocks.Record())
            {
                Expect.Call(this.mockIntegrationResult.Label).Return("1");
                LastCall.Repeat.Once();
                Expect.Call(this.mockIntegrationResult.Label).Return("1");
                LastCall.Repeat.Once();
            }

            DateVersionLabeller dateVersionLabeller = new DateVersionLabeller();
            dateVersionLabeller.MajorVersionNumber = 1;
            dateVersionLabeller.MinorVersionNumber = 0;
            using (this.mocks.Playback())
            {
                dateVersionLabeller.Generate(this.mockIntegrationResult);
            }

            this.mocks.VerifyAll();
        }

        /// <summary>
        /// Test <see cref="DateVersionLabeller.Generate"/> with invalid previous label.
        /// </summary>
        [TestMethod]
        public void UnitTestGenerateWithInvalidPreviousLabelAndInitialise()
        {
            DateVersionLabeller dateVersionLabeller = new DateVersionLabeller();
            dateVersionLabeller.MajorVersionNumber = 1;
            dateVersionLabeller.MinorVersionNumber = 0;
            dateVersionLabeller.Initialise = true;
            using (this.mocks.Record())
            {
                Expect.Call(this.mockIntegrationResult.Label).Return("1");
                LastCall.Repeat.Once();
                Expect.Call(this.mockIntegrationResult.Label).Return("1");
                LastCall.Repeat.Once();
            }

            string label;
            using (this.mocks.Playback())
            {
                label = dateVersionLabeller.Generate(this.mockIntegrationResult);
            }

            string expected = GetLabelForToday(1, 0, 0);
            Assert.AreEqual(expected, label, "The Label returned was not as expected.");
            this.mocks.VerifyAll();
        }

        /// <summary>
        /// Test <see cref="DateVersionLabeller.Run"/>.
        /// </summary>
        [TestMethod]
        public void UnitTestRun()
        {
            string expected = GetLabelForToday(1, 0, 0);
            using (this.mocks.Record())
            {
                Expect.Call(this.mockIntegrationResult.Label).Return(null);
                this.mockIntegrationResult.Label = expected;
            }

            DateVersionLabeller dateVersionLabeller = new DateVersionLabeller();
            dateVersionLabeller.MajorVersionNumber = 1;
            dateVersionLabeller.MinorVersionNumber = 0;
            using (this.mocks.Playback())
            {
                dateVersionLabeller.Run(this.mockIntegrationResult);
            }

            this.mocks.VerifyAll();
        }

        #endregion // Unit Tests

        #region Methods

        /// <summary>
        /// Gets a build label for today.
        /// </summary>
        /// <param name="major">The major version number.</param>
        /// <param name="minor">The minor version number.</param>
        /// <param name="build">The build number.</param>
        /// <returns>A build label.</returns>
        private static string GetLabelForToday(int major, int minor, int build)
        {
            int todayDateNumber = int.Parse(DateTime.Now.ToString(TodayDateNumberFormat, CultureInfo.CurrentCulture), CultureInfo.CurrentCulture);
            Version version = new Version(major, minor, todayDateNumber, build);
            return version.ToString();
        }

        #endregion // Methods
    }
}
