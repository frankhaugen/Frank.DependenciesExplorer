using NuGet.Packaging.Core;

namespace Frank.DependenciesExplorer;

public static class NugetBaseExtensions
{
    public static NugetBase ToNugetBase(this PackageDependency packageDependency) => new NugetBase { Name = packageDependency.Id, Version = packageDependency.VersionRange.ToNormalizedString() };
    
    public static void PopulateTree(this NugetBase dep, Nuget parentNuget, NugetLookup lookup)
    {
        var nuget = new Nuget
        {
            Name = dep.Name,
            Version = dep.Version
        };

        var childDependencies = lookup.GetValueOrDefault($"{nuget.Name}+{nuget.Version}") ?? [];
        foreach (var pack in childDependencies) PopulateTree(pack, nuget, lookup);

        parentNuget.DependentNugets.Add(nuget);
    }
}