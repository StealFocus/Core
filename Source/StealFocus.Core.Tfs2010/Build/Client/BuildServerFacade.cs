// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="BuildServerFacade.cs" company="StealFocus">
//   Copyright StealFocus. All rights reserved.
// </copyright>
// <summary>
//   Defines the BuildServerFacade type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------

namespace StealFocus.Core.Tfs2010.Build.Client
{
    using System.Collections;

    using Microsoft.TeamFoundation.Build.Client;

    public class BuildServerFacade
    {
        private readonly IBuildServer buildServer;

        public BuildServerFacade(IBuildServer buildServer)
        {
            this.buildServer = buildServer;
        }

        public string GetLatestBuildNumberFromAllBuildDefinitions(string teamProjectName)
        {
            IBuildDetailSpec buildDetailSpec = this.buildServer.CreateBuildDetailSpec(teamProjectName);
            buildDetailSpec.Reason = BuildReason.All;
            buildDetailSpec.QueryOrder = BuildQueryOrder.FinishTimeDescending;
            buildDetailSpec.QueryDeletedOption = QueryDeletedOption.IncludeDeleted;
            buildDetailSpec.MaxBuildsPerDefinition = 1;
            IBuildQueryResult buildQueryResult = this.buildServer.QueryBuilds(buildDetailSpec);
            if (buildQueryResult.Builds.Length < 1)
            {
                return null;
            }

            return buildQueryResult.Builds[0].BuildNumber;
        }

        public string[] GetLatestBuildNumbersFromAllBuildDefinitions(string teamProjectName, int numberOfBuildNumbersToRetrieve)
        {
            IBuildDetailSpec buildDetailSpec = this.buildServer.CreateBuildDetailSpec(teamProjectName);
            buildDetailSpec.Reason = BuildReason.All;
            buildDetailSpec.QueryOrder = BuildQueryOrder.FinishTimeDescending;
            buildDetailSpec.QueryDeletedOption = QueryDeletedOption.IncludeDeleted;
            buildDetailSpec.MaxBuildsPerDefinition = numberOfBuildNumbersToRetrieve;
            IBuildQueryResult buildQueryResult = this.buildServer.QueryBuilds(buildDetailSpec);
            ArrayList buildNumberList = new ArrayList(numberOfBuildNumbersToRetrieve);
            for (int i = 0; i < numberOfBuildNumbersToRetrieve && i < buildQueryResult.Builds.Length; i++)
            {
                buildNumberList.Add(buildQueryResult.Builds[i].BuildNumber);
            }

            return (string[])buildNumberList.ToArray(typeof(string));
        }
    }
}
