using System.Text;
using NuGet.ProjectModel;

namespace Frank.DependenciesExplorer;

public class NugetPackage(LockFileTargetLibrary package, IList<NugetPackage> dependencies)
{
    public LockFileTargetLibrary Package { get; } = package;

    public IList<NugetPackage> Dependencies { get; } = dependencies;

    public string PrintHierarchy(string indent = "")
    {
        var sb = new StringBuilder();
        sb.AppendLine($"{indent}{Package.Name} ({Package.Version?.OriginalVersion})");
        foreach (var child in Dependencies) sb.Append(child.PrintHierarchy(indent + "    "));

        return sb.ToString();
    }
}