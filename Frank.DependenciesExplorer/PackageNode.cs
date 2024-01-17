using System.Text;
using NuGet.ProjectModel;

namespace Frank.DependenciesExplorer;

public class PackageNode(LockFileTargetLibrary package, IList<PackageNode> dependencies)
{
    public LockFileTargetLibrary Package { get; } = package;

    public IList<PackageNode> Dependencies { get; } = dependencies;

    public string PrintHierarchy(string indent = "")
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"{indent}{Package.Name} ({Package.Version?.OriginalVersion})");
        foreach (var child in this.Dependencies)
        {
            sb.Append(child.PrintHierarchy(indent + "    "));
        }

        return sb.ToString();
    }
}