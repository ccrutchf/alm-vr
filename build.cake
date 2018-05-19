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
	var projectPath = System.IO.Path.GetFullPath("./src/AlmVR.Headset");
	var outputPath = System.IO.Path.Combine(projectPath, "Build", "x64", "alm-vr.exe");
	
	Information($"Unity Editor Location: {unityEditorLocation}");
	Information($"Project Path {projectPath}");
	Information($"Output Path {outputPath}");
	
	//StartProcess(unityEditorLocation, 
	//	$"-batchmode -quit -projectPath \"{ProjectPath}\" -executeMethod Cake.Unity3D.AutomatedBuild.Build --output-path=C:\projects\alm-vr\src\AlmVR.Headset\Build\x64\alm-vr.exe --platform=StandaloneWindows64 --version=1.0.0"
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
