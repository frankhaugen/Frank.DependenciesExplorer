using System.Text;

namespace Frank.DependenciesExplorer;

public class NugetLookup() : Dictionary<string, List<NugetBase>>()
{
    public override string ToString()
    {
        var stringBuilder = new StringBuilder();
        foreach (var (key, value) in this)
        {
            stringBuilder.AppendLine($"{key}:");
            foreach (var nuget in value)
            {
                stringBuilder.AppendLine($"  {nuget.Name}+{nuget.Version}");
            }
        }
        
        return stringBuilder.ToString();
    }
}
