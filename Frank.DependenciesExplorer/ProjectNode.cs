using System.Text;

namespace Frank.DependenciesExplorer;

public class ProjectNode(string name, IList<NugetPackage> dependencies)
{
    public string Name { get; } = name;

    public IList<NugetPackage> NugetDependencies { get; } = dependencies;

    public string PrintHierarchy(string indent = "")
    {
        var sb = new StringBuilder();
        sb.AppendLine($"{indent}{Name}");
        foreach (var child in NugetDependencies) sb.Append(child.PrintHierarchy(indent + "    "));

        return sb.ToString();
    }
}