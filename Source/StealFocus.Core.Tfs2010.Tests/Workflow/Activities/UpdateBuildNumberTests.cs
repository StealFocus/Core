// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="UpdateBuildNumberTests.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the UpdateBuildNumberTests type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

namespace StealFocus.Core.Tfs2010.Tests.Workflow.Activities
{
    using System;
    using System.Activities;
    using System.Activities.Statements;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;

    using Microsoft.TeamFoundation.Build.Client;
    using Microsoft.TeamFoundation.Build.Workflow.Activities;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Rhino.Mocks;

    using UpdateBuildNumber = StealFocus.Core.Tfs2010.Workflow.Activities.UpdateBuildNumber;

    [TestClass]
    public class UpdateBuildNumberTests
    {
        [TestMethod]
        public void UnitTestUpdateBuildNumberInTestXamlActivityUsingWorkflowInvokerWithInvalidBuildNumberFormat()
        {
            MockRepository mockRepository = new MockRepository();

            // Arrange
            IBuildDetail mockBuildDetail = mockRepository.DynamicMock<IBuildDetail>();

            // Act
            mockRepository.ReplayAll();
            try
            {
                RunUpdateBuildNumberInTestXamlActivityUsingWorkflowInvoker(new UpdateBuildNumberTestActivityWithInvalidBuildNumberFormat(), mockBuildDetail, null, null);
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(ArgumentException), e.GetType());
                Assert.AreEqual("The 'BuildNumberFormat' value did not contain '{0}' as required (this will be replaced by the version number).", e.Message);
            }

            // Assert
            mockRepository.VerifyAll();
        }

        /// <summary>
        /// Tests the component initialising the version correctly.
        /// </summary>
        [TestMethod]
        public void UnitTestUpdateBuildNumberInTestXamlActivityUsingWorkflowInvokerWithNoPreviousBuilds()
        {
            MockRepository mockRepository = new MockRepository();
            Version expectedVersion = UpdateBuildNumber.GetVersionNumber(2, 2, DateTime.Now, 0);
            string expectedBuildNumber = GetBuildNumberForTest("Acme.PetShop-Trunk-Full", expectedVersion);
            
            // Arrange
            IBuildDetail mockBuildDetailForCurrentBuild = Arrange(mockRepository);
            
            // Act
            mockRepository.ReplayAll();
            RunUpdateBuildNumberInTestXamlActivityUsingWorkflowInvoker(new UpdateBuildNumberTestActivity(), mockBuildDetailForCurrentBuild, expectedBuildNumber, expectedVersion.ToString());

            // Assert
            mockRepository.VerifyAll();
        }

        /// <summary>
        /// Tests the component initialising the version correctly.
        /// </summary>
        [TestMethod]
        public void UnitTestUpdateBuildNumberInTestXamlActivityUsingWorkflowInvokerWithNoPreviousBuildsMatchingTheNamingConvention()
        {
            MockRepository mockRepository = new MockRepository();
            Version expectedVersion = UpdateBuildNumber.GetVersionNumber(2, 2, DateTime.Now, 0);
            string expectedBuildNumber = GetBuildNumberForTest("Acme.PetShop-Trunk-Full", expectedVersion);
            
            // Arrange
            IBuildDetail mockBuildDetailForCurrentBuild = Arrange(mockRepository, new[] { "Acme.PetShop-Trunk-Full" });
            
            // Act
            mockRepository.ReplayAll();
            RunUpdateBuildNumberInTestXamlActivityUsingWorkflowInvoker(new UpdateBuildNumberTestActivity(), mockBuildDetailForCurrentBuild, expectedBuildNumber, expectedVersion.ToString());

            // Assert
            mockRepository.VerifyAll();
        }

        /// <summary>
        /// Tests that the component continues incrementing the "Revision" component.
        /// </summary>
        [TestMethod]
        public void UnitTestUpdateBuildNumberInTestXamlActivityUsingWorkflowInvokerWithAPreviousBuildFromTheSameDay()
        {
            MockRepository mockRepository = new MockRepository();
            Version previousVersion = UpdateBuildNumber.GetVersionNumber(2, 2, DateTime.Now, 0);
            Version expectedVersion = UpdateBuildNumber.GetVersionNumber(2, 2, DateTime.Now, 1);
            string previousBuildNumber = GetBuildNumberForTest("Acme.PetShop-Trunk-Full", previousVersion);
            string expectedBuildNumber = GetBuildNumberForTest("Acme.PetShop-Trunk-Full", expectedVersion);
            
            // Arrange
            IBuildDetail mockBuildDetailForCurrentBuild = Arrange(mockRepository, new[] { previousBuildNumber });

            // Act
            mockRepository.ReplayAll();
            RunUpdateBuildNumberInTestXamlActivityUsingWorkflowInvoker(new UpdateBuildNumberTestActivity(), mockBuildDetailForCurrentBuild, expectedBuildNumber, expectedVersion.ToString());

            // Assert
            mockRepository.VerifyAll();
        }

        /// <summary>
        /// Tests that the component continues incrementing the "Revision" component.
        /// </summary>
        [TestMethod]
        public void UnitTestUpdateBuildNumberInTestXamlActivityUsingWorkflowApplicationWithAPreviousBuildFromTheSameDay()
        {
            MockRepository mockRepository = new MockRepository();
            Version previousVersion = UpdateBuildNumber.GetVersionNumber(0, 0, DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)), 3);
            string previousBuildNumber = GetBuildNumberForTest("Acme.PetShop-Trunk-Full", previousVersion);

            // Arrange
            IBuildDetail mockBuildDetailForCurrentBuild = Arrange(mockRepository, new[] { previousBuildNumber });

            // Act
            mockRepository.ReplayAll();
            RunUpdateBuildNumberInTestXamlActivityUsingWorkflowApplication(new UpdateBuildNumberTestActivity(), mockBuildDetailForCurrentBuild);

            // Assert
            mockRepository.VerifyAll();
        }

        /// <summary>
        /// Tests that the component continues incrementing the "Revision" component.
        /// </summary>
        [TestMethod]
        public void UnitTestUpdateBuildNumberInTestCodeActivityUsingWorkflowApplicationWithAPreviousBuildFromTheSameDay()
        {
            MockRepository mockRepository = new MockRepository();
            Version previousVersion = UpdateBuildNumber.GetVersionNumber(0, 0, DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)), 3);
            string previousBuildNumber = GetBuildNumberForTest("Acme.PetShop-Trunk-Full", previousVersion);

            // Arrange
            IBuildDetail mockBuildDetailForCurrentBuild = Arrange(mockRepository, new[] { previousBuildNumber });

            // Act
            mockRepository.ReplayAll();
            RunUpdateBuildNumberInTestCodeActivityUsingWorkflowApplication(mockBuildDetailForCurrentBuild);

            // Assert
            mockRepository.VerifyAll();
        }

        [TestMethod]
        public void UnitTestUpdateBuildNumberInTestXamlActivityUsingWorkflowInvokerWithADifferentPreviousBuildLabel()
        {
            MockRepository mockRepository = new MockRepository();
            Version previousVersion = UpdateBuildNumber.GetVersionNumber(2, 2, DateTime.Now, 0);
            Version expectedVersion = UpdateBuildNumber.GetVersionNumber(2, 2, DateTime.Now, 1);
            string previousBuildNumber = GetBuildNumberForTest("Acme.AnotherShop-Trunk-Full", previousVersion);
            string expectedBuildNumber = GetBuildNumberForTest("Acme.PetShop-Trunk-Full", expectedVersion);

            // Arrange
            IBuildDetail mockBuildDetailForCurrentBuild = Arrange(mockRepository, new[] { previousBuildNumber });

            // Act
            mockRepository.ReplayAll();
            RunUpdateBuildNumberInTestXamlActivityUsingWorkflowInvoker(new UpdateBuildNumberTestActivity(), mockBuildDetailForCurrentBuild, expectedBuildNumber, expectedVersion.ToString());

            // Assert
            mockRepository.VerifyAll();
        }

        [TestMethod]
        public void UnitTestUpdateBuildNumberInTestXamlActivityUsingWorkflowInvokerWithAPreviousBuildFromTheSameDayAndChangeMajorMinor()
        {
            MockRepository mockRepository = new MockRepository();
            Version previousVersion = UpdateBuildNumber.GetVersionNumber(1, 1, DateTime.Now, 0);
            Version expectedVersion = UpdateBuildNumber.GetVersionNumber(2, 2, DateTime.Now, 1);
            string previousBuildNumber = GetBuildNumberForTest("Acme.PetShop-Trunk-Full", previousVersion);
            string expectedBuildNumber = GetBuildNumberForTest("Acme.PetShop-Trunk-Full", expectedVersion);

            // Arrange
            IBuildDetail mockBuildDetailForCurrentBuild = Arrange(mockRepository, new[] { previousBuildNumber });

            // Act
            mockRepository.ReplayAll();
            RunUpdateBuildNumberInTestXamlActivityUsingWorkflowInvoker(new UpdateBuildNumberTestActivity(), mockBuildDetailForCurrentBuild, expectedBuildNumber, expectedVersion.ToString());

            // Assert
            mockRepository.VerifyAll();
        }

        /// <summary>
        /// Tests that the component resets the "Revision" component to zero.
        /// </summary>
        [TestMethod]
        public void UnitTestUpdateBuildNumberInTestXamlActivityUsingWorkflowInvokerWithAPreviousBuildFromTheDayBefore()
        {
            MockRepository mockRepository = new MockRepository();
            Version previousVersion = UpdateBuildNumber.GetVersionNumber(2, 2, DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)), 9);
            Version expectedVersion = UpdateBuildNumber.GetVersionNumber(2, 2, DateTime.Now, 0);
            string previousBuildNumber = GetBuildNumberForTest("Acme.PetShop-Trunk-Full", previousVersion);
            string expectedBuildNumber = GetBuildNumberForTest("Acme.PetShop-Trunk-Full", expectedVersion);

            // Arrange
            IBuildDetail mockBuildDetailForCurrentBuild = Arrange(mockRepository, new[] { previousBuildNumber });

            // Act
            mockRepository.ReplayAll();
            RunUpdateBuildNumberInTestXamlActivityUsingWorkflowInvoker(new UpdateBuildNumberTestActivity(), mockBuildDetailForCurrentBuild, expectedBuildNumber, expectedVersion.ToString());

            // Assert
            mockRepository.VerifyAll();
        }

        [TestMethod]
        public void UnitTestUpdateBuildNumberInTestXamlActivityUsingWorkflowInvokerWithAPreviousBuildFromTheDayBeforeAndChangeMajorMinor()
        {
            MockRepository mockRepository = new MockRepository();
            Version previousVersion = UpdateBuildNumber.GetVersionNumber(1, 1, DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)), 9);
            Version expectedVersion = UpdateBuildNumber.GetVersionNumber(2, 2, DateTime.Now, 0);
            string previousBuildNumber = GetBuildNumberForTest("Acme.PetShop-Trunk-Full", previousVersion);
            string expectedBuildNumber = GetBuildNumberForTest("Acme.PetShop-Trunk-Full", expectedVersion);

            // Arrange
            IBuildDetail mockBuildDetailForCurrentBuild = Arrange(mockRepository, new[] { previousBuildNumber });

            // Act
            mockRepository.ReplayAll();
            RunUpdateBuildNumberInTestXamlActivityUsingWorkflowInvoker(new UpdateBuildNumberTestActivity(), mockBuildDetailForCurrentBuild, expectedBuildNumber, expectedVersion.ToString());

            // Assert
            mockRepository.VerifyAll();
        }

        [TestMethod]
        public void UnitTestUpdateBuildNumberInTestXamlActivityUsingWorkflowInvokerWithMultiplePreviousBuilds()
        {
            MockRepository mockRepository = new MockRepository();
            Version previousVersion = UpdateBuildNumber.GetVersionNumber(1, 1, DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)), 9);
            Version expectedVersion = UpdateBuildNumber.GetVersionNumber(2, 2, DateTime.Now, 0);
            const string PreviousBuildNumber1 = "randomString";
            string previousBuildNumber2 = GetBuildNumberForTest("Acme.PetShop-Trunk-Full", previousVersion);
            string expectedBuildNumber = GetBuildNumberForTest("Acme.PetShop-Trunk-Full", expectedVersion);

            // Arrange
            IBuildDetail mockBuildDetailForCurrentBuild = Arrange(mockRepository, new[] { PreviousBuildNumber1, previousBuildNumber2 });

            // Act
            mockRepository.ReplayAll();
            RunUpdateBuildNumberInTestXamlActivityUsingWorkflowInvoker(new UpdateBuildNumberTestActivity(), mockBuildDetailForCurrentBuild, expectedBuildNumber, expectedVersion.ToString());

            // Assert
            mockRepository.VerifyAll();
        }

        private static string GetBuildNumberForTest(string prefix, Version version)
        {
            return string.Format(CultureInfo.CurrentCulture, "{0}-{1}", prefix, version);
        }

        private static IBuildDetail Arrange(MockRepository mockRepository, IEnumerable<string> existingBuildNumbers)
        {
            IBuildDetail mockBuildDetailForCurrentBuild = mockRepository.DynamicMock<IBuildDetail>();
            IBuildServer mockBuildServer = mockRepository.DynamicMock<IBuildServer>();
            IBuildDetailSpec mockBuildDetailSpec = mockRepository.DynamicMock<IBuildDetailSpec>();
            IBuildQueryResult mockBuildQueryResult = mockRepository.DynamicMock<IBuildQueryResult>();
            ArrayList previousBuildsList = new ArrayList();
            foreach (string existingBuildNumber in existingBuildNumbers)
            {
                IBuildDetail mockBuildDetailForPreviousBuild = mockRepository.DynamicMock<IBuildDetail>();
                previousBuildsList.Add(mockBuildDetailForPreviousBuild);
                Expect.Call(mockBuildDetailForPreviousBuild.BuildNumber).Return(existingBuildNumber);
            }
            
            IBuildDetail[] previousBuilds = (IBuildDetail[])previousBuildsList.ToArray(typeof(IBuildDetail));
            mockRepository.Record();
            Expect.Call(mockBuildDetailForCurrentBuild.BuildServer).Return(mockBuildServer);
            Expect.Call(mockBuildDetailForCurrentBuild.TeamProject).Return("TeamProjectName");
            Expect.Call(mockBuildServer.CreateBuildDetailSpec("TeamProjectName")).Return(mockBuildDetailSpec);
            Expect.Call(mockBuildServer.QueryBuilds(mockBuildDetailSpec)).Return(mockBuildQueryResult);
            Expect.Call(mockBuildQueryResult.Builds).Return(previousBuilds);
            
            return mockBuildDetailForCurrentBuild;
        }

        private static IBuildDetail Arrange(MockRepository mockRepository)
        {
            IBuildDetail mockBuildDetailForCurrentBuild = mockRepository.DynamicMock<IBuildDetail>();
            IBuildServer mockBuildServer = mockRepository.DynamicMock<IBuildServer>();
            IBuildDetailSpec mockBuildDetailSpec = mockRepository.DynamicMock<IBuildDetailSpec>();
            IBuildQueryResult mockBuildQueryResult = mockRepository.DynamicMock<IBuildQueryResult>();
            ArrayList previousBuildsList = new ArrayList();
            IBuildDetail[] previousBuilds = (IBuildDetail[])previousBuildsList.ToArray(typeof(IBuildDetail));
            mockRepository.Record();
            Expect.Call(mockBuildDetailForCurrentBuild.BuildServer).Return(mockBuildServer);
            Expect.Call(mockBuildDetailForCurrentBuild.TeamProject).Return("TeamProjectName");
            Expect.Call(mockBuildServer.CreateBuildDetailSpec("TeamProjectName")).Return(mockBuildDetailSpec);
            Expect.Call(mockBuildServer.QueryBuilds(mockBuildDetailSpec)).Return(mockBuildQueryResult);
            Expect.Call(mockBuildQueryResult.Builds).Return(previousBuilds);
            return mockBuildDetailForCurrentBuild;
        }

        private static void RunUpdateBuildNumberInTestXamlActivityUsingWorkflowInvoker(Activity activityToTest, IBuildDetail mockBuildDetail, string expectedBuildNumber, string expectedVersionNumber)
        {
            WorkflowInvoker workflowInvoker = new WorkflowInvoker(activityToTest);
            workflowInvoker.Extensions.Add(mockBuildDetail);
            IDictionary<string, object> dictionary = workflowInvoker.Invoke();
            bool buildNumberFound = false;
            bool versionNumberFound = false;
            foreach (KeyValuePair<string, object> keyValuePair in dictionary)
            {
                System.Diagnostics.Debug.WriteLine("Found Key '" + keyValuePair.Key + "' with Value '" + keyValuePair.Value + "'.");
                if (keyValuePair.Key == "BuildNumber")
                {
                    buildNumberFound = true;
                    Assert.AreEqual(expectedBuildNumber, keyValuePair.Value);
                }

                if (keyValuePair.Key == "VersionNumber")
                {
                    versionNumberFound = true;
                    Assert.AreEqual(expectedVersionNumber, keyValuePair.Value);
                }
            }

            Assert.IsTrue(buildNumberFound);
            Assert.IsTrue(versionNumberFound);
        }

        private static void RunUpdateBuildNumberInTestXamlActivityUsingWorkflowApplication(Activity activityToTest, IBuildDetail mockBuildDetail)
        {
            WorkflowApplication workflowApplication = new WorkflowApplication(activityToTest);
            workflowApplication.Extensions.Add(mockBuildDetail);
            AutoResetEvent idleEvent = new AutoResetEvent(false);
            workflowApplication.Completed = delegate(WorkflowApplicationCompletedEventArgs e)
                {
                    idleEvent.Set();
                };

            workflowApplication.Run();
            idleEvent.WaitOne();
        }

        private static void RunUpdateBuildNumberInTestCodeActivityUsingWorkflowApplication(IBuildDetail mockBuildDetail)
        {
            // Variables - in
            Variable<string> buildNumberFormat = new Variable<string>("BuildNumberFormat", "Acme.PetShop-Trunk-Full-{0}");
            Variable<int> majorVersion = new Variable<int>("MajorVersion", 1);
            Variable<int> minorVersion = new Variable<int>("MinorVersion", 0);

            // Variables - out
            Variable<string> buildNumber = new Variable<string>("BuildNumber");
            Variable<string> versionNumber = new Variable<string>("VersionNumber");

            // Activities
            GetBuildDetail getBuildDetail = new GetBuildDetail();
            UpdateBuildNumber updateBuildNumber = new UpdateBuildNumber
                {
                    BuildNumberFormat = buildNumberFormat,
                    MajorVersion = majorVersion, 
                    MinorVersion = minorVersion,
                    BuildNumber = buildNumber,
                    VersionNumber = versionNumber
                };

            // Sequence
            Sequence sequence = new Sequence();
            sequence.Variables.Add(buildNumberFormat);
            sequence.Variables.Add(majorVersion);
            sequence.Variables.Add(minorVersion);
            sequence.Variables.Add(buildNumber);
            sequence.Variables.Add(versionNumber);
            sequence.Activities.Add(getBuildDetail);
            sequence.Activities.Add(updateBuildNumber);

            // Run
            WorkflowApplication workflowApplication = new WorkflowApplication(sequence);
            workflowApplication.Extensions.Add(mockBuildDetail);
            AutoResetEvent idleEvent = new AutoResetEvent(false);
            workflowApplication.Completed = delegate(WorkflowApplicationCompletedEventArgs e)
                {
                    idleEvent.Set();
                };

            workflowApplication.Run();
            idleEvent.WaitOne();
        }
    }
}
