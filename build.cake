#addin "Cake.Incubator"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var buildDir = Directory("./src/AlmVR.Server/AlmVR.Server/bin") + Directory(configuration);

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir);
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore("./src/AlmVR.Client/AlmVR.Client.sln");
});

Task("Build-Server")
	.Does(() =>
{
	var dotNetCoreSettings = new DotNetCoreBuildSettings
	{
		Configuration = "Release"
	};
	
	DotNetCoreBuild("./src/AlmVR.Server/AlmVR.Server.sln", dotNetCoreSettings);
});

Task("Build-Client")
    .IsDependentOn("Restore-NuGet-Packages")
	.Does(() =>
{
	// Build the client.
	MSBuild("./src/AlmVR.Client/AlmVR.Client.sln", settings =>
        settings.SetConfiguration(configuration));
});

Task("Build-Unity");

Task("Build")
	.IsDependentOn("Build-Server")
	.IsDependentOn("Build-Client")
	.IsDependentOn("Build-Unity");

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Build");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
