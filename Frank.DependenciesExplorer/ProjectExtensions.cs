using Buildalyzer;
using NuGet.ProjectModel;

namespace Frank.DependenciesExplorer;

public static class ProjectExtensions
{
    public static FileInfo GetProjectFileInfo(this IProjectAnalyzer project) => new(project.ProjectFile.Path);

    public static LockFile GetLockFile(this IProjectAnalyzer project) 
        => new LockFileFormat().Read(Path.Combine(project.GetProjectFileInfo().Directory?.FullName ?? "", "obj", "project.assets.json"));

    public static NugetLookup GetNugetLookup(this IProjectAnalyzer project) 
        => project.GetLockFile().GetNugetLookup();
}