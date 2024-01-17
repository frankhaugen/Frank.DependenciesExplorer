using System.Text;

namespace Frank.DependenciesExplorer;

public class ProjectNode(string name, IList<PackageNode> dependencies)
{
    public string Name { get; } = name;

    public IList<PackageNode> Dependencies { get; } = dependencies;

    public string PrintHierarchy(string indent = "")
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"{indent}{Name}");
        foreach (var child in this.Dependencies)
        {
            sb.Append(child.PrintHierarchy(indent + "    "));
        }

        return sb.ToString();
    }
}