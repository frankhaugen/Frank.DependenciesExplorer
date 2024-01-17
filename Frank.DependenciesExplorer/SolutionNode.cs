using System.Text;

namespace Frank.DependenciesExplorer;

public class SolutionNode(string name, IList<ProjectNode> projects)
{
    public string Name { get; } = name;

    public IList<ProjectNode> Projects { get; } = projects;

    public string PrintHierarchy(string indent = "")
    {
        var sb = new StringBuilder();
        sb.AppendLine($"{indent}{Name}");
        foreach (var child in Projects) sb.Append(child.PrintHierarchy(indent + "    "));

        return sb.ToString();
    }
}