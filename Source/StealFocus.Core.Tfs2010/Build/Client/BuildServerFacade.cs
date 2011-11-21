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
    }
}
