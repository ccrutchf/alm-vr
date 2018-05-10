#addin "Cake.Incubator"
#addin nuget:?package=Cake.Unity3D

using System.IO;

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

Task("Build-Unity")
	.IsDependentOn("Build-Client")
	.Does(() =>
{
	// Presuming the build.cake file is within the Unity3D project folder.
	var projectPath = System.IO.Path.GetFullPath("./src/AlmVR.Headset");
	
	// The location we want the build application to go
	var outputPath = System.IO.Path.Combine(projectPath, "_build", "x64", "alm-vr.exe");
	
	// Get the absolute path to the 2018.1.0f1 Unity3D editor.
	string unityEditorLocation = @"C:\Program Files\Unity\Editor\Unity.exe";
	
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
