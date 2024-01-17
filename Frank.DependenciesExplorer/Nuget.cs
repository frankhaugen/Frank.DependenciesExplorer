using System.Text;

namespace Frank.DependenciesExplorer;

public class Nuget : NugetBase
{
    public List<Nuget> DependentNugets { get; } = new();

    public override bool Equals(object? obj) =>
        obj is Nuget nuget &&
        base.Equals(obj) &&
        DependentNugets == nuget.DependentNugets;

    public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), DependentNugets);

    public override string ToString()
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($"{Name} {Version}");
        foreach (var dependentNuget in DependentNugets)
        {
            stringBuilder.AppendLine($"  {dependentNuget}");
        }
        
        return stringBuilder.ToString();
    }
}