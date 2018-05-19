#addin "Cake.Incubator"
#addin nuget:?package=Cake.Unity3D

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
    .IsDependentOn("Clean")
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

Task("Build-Unity")
	.IsDependentOn("Build-Client")
	.Does(() =>
{
	var unityEditorLocation = EnvironmentVariable("UNITY_EDITOR_LOCATION") ?? @"C:\Program Files\Unity\Editor\Unity.exe";
	
	Information($"Unity Editor Location: {unityEditorLocation}");
	
	// Presuming the build.cake file is within the Unity3D project folder.
	var projectPath = System.IO.Path.GetFullPath("./src/AlmVR.Headset");
	
	// The location we want the build application to go
	var outputPath = System.IO.Path.Combine(projectPath, "Build", "x64", "alm-vr.exe");
	
	// Create our build options.
	var options = new Unity3DBuildOptions()
	{
		Platform = Unity3DBuildPlatform.StandaloneWindows64,
		OutputPath = outputPath,
		UnityEditorLocation = unityEditorLocation,
		ForceScriptInstall = true,
		BuildVersion = "1.0.0"
	};
	
	// Perform the Unity3d build.
	BuildUnity3DProject(projectPath, options);
});

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
