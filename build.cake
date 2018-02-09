#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
#addin nuget:?package=Cake.Git

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Debug");
var buildDir = Directory("./TestAssemblyVersioning/bin") + Directory(configuration);
var sln = File("./TestAssemblyVersioning.sln");
var thisRepo = MakeAbsolute(Directory("./"));
var assemblyInfo = File("./TestAssemblyVersioning/Properties/AssemblyInfo.cs");

Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir);
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore(sln);
});

Task("Version")
	.Does(() => 
{
	var branch = GitBranchCurrent(thisRepo);
	Information(branch);
	// var latest = GitLogTip(thisRepo);
	// Information(latest);
	var sha = branch.Tip.Sha.Substring(0, 6); // Not right - should use LibGit2Sharp's ObjectDatabase.ShortenObjectId()
	//*
	CreateAssemblyInfo(assemblyInfo, new AssemblyInfoSettings {
		InformationalVersion = string.Format("1.0.{0}.{1}", sha, branch.FriendlyName)
	});
	//*/
});

Task("Build")
	.IsDependentOn("Version")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    if(IsRunningOnWindows())
    {
      // Use MSBuild
      MSBuild(sln, settings =>
        settings.SetConfiguration(configuration));
    }
    else
    {
      // Use XBuild
      XBuild(sln, settings =>
        settings.SetConfiguration(configuration));
    }
})
.Finally(() =>
{
	// restore assembly.cs files
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    NUnit3("./**/bin/" + configuration + "/*.Tests.dll", new NUnit3Settings {
        NoResults = true
        });
});


Task("Default")
    .IsDependentOn("Run-Unit-Tests");

RunTarget(target);