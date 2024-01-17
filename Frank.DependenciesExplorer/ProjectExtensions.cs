using Buildalyzer;
using NuGet.ProjectModel;
using SlnParser.Contracts;

namespace Frank.DependenciesExplorer;

public static class ProjectExtensions
{
    public static FileInfo GetProjectFileInfo(this IProjectAnalyzer project) 
        => new(project.ProjectFile.Path);

    public static LockFile GetLockFile(this IProjectAnalyzer project) 
        => new LockFileFormat().Read(Path.Combine(project.GetProjectFileInfo().Directory?.FullName ?? "", "obj", "project.assets.json"));

    public static ProjectNode GetProjectNode(this IProject project)
    {
        var solutionProject = project as SolutionProject;
        var projectAnalyzer = new AnalyzerManager().GetProject(solutionProject?.File.FullName);
        return new ProjectNode(project.Name, projectAnalyzer.GetLockFile().GetHierarchy());
    }
}