@ECHO OFF
ECHO Please select a build type to run.
ECHO.
ECHO Full build (must be run the first time)
ECHO - Clean Solution
ECHO - Initialise build
ECHO    - Delete old Build directory
ECHO    - Delete old test results
ECHO - Initialise environment
ECHO    - Configure IIS
ECHO       - Add AppPool service accounts to correct Local Groups
ECHO       - Create AppPools (with specified service accounts)
ECHO       - Create Virtual Directories
ECHO       - Register correct version of ASP.NET
ECHO       - Configure additional script maps e.g. ".mvc"
ECHO    - Add Strong Name verification skipping entry (local build is delay signed)
ECHO    - Reset IIS for Strong Name verification skipping entry (local build only)
ECHO    - Create Event Log Sources
ECHO - Compile all configurations
ECHO - Run tests for all configurations
ECHO - Build Documentation
ECHO - Create MSIs for all configurations and all environments
ECHO.
ECHO Quick build (will only succeed once a Full build has completed)
ECHO - Initialise build
ECHO    - Delete old Build directory
ECHO    - Delete old test results
ECHO - Compile Debug configuration
ECHO - Run tests for Debug configuration
ECHO.
:question
SET /p buildChoice=Please enter F (for Full build) or Q (for Quick build): 
IF /i %buildChoice% == f (
ECHO.
%windir%\Microsoft.NET\Framework64\v4.0.30319\msbuild.exe ..\Scripts\StealFocus.Core.Build.proj /t:CleanSolution /consoleloggerparameters:Verbosity=minimal /fileLogger /fileLoggerParameters:LogFile=CleanSolution.msbuild.log;verbosity=diagnostic
ECHO.
%windir%\Microsoft.NET\Framework64\v4.0.30319\msbuild.exe ..\Scripts\StealFocus.Core.Build.proj /p:IsAllConfigurationsBuild=true;IsCommandLineBuild=true;IsDocumentationBuild=false;IsInitialiseEnvironmentBuild=true;IsPackagedBuild=true /consoleloggerparameters:Verbosity=minimal /fileLogger /fileLoggerParameters:LogFile=SolutionBuild.msbuild.log;verbosity=diagnostic
) ELSE (
	IF /i %buildChoice% == q (
	ECHO.
	%windir%\Microsoft.NET\Framework64\v4.0.30319\msbuild.exe ..\Scripts\StealFocus.Core.Build.proj /p:IsCommandLineBuild=true /consoleloggerparameters:Verbosity=minimal /fileLogger /fileLoggerParameters:LogFile=SolutionBuild.msbuild.log;verbosity=diagnostic
	) ELSE (
		ECHO.
		ECHO Invalid selection
		ECHO.
		goto :question
	)
)
ECHO.
pause
