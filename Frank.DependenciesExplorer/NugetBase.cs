namespace Frank.DependenciesExplorer;

public class NugetBase
{
    public string? Name { get; set; }
    public string? Version { get; set; }

    public override bool Equals(object? obj) =>
        obj is NugetBase other &&
        Name == other.Name &&
        Version == other.Version;

    public override int GetHashCode() => HashCode.Combine(Name, Version);
}