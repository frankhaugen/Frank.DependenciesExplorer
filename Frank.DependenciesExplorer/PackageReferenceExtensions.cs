using Buildalyzer.Construction;

namespace Frank.DependenciesExplorer;

public static class PackageReferenceExtensions
{
    public static void PopulateTree(this IPackageReference packageReference, Nuget parentNuget, NugetLookup dependencyLookup)
    {
        var nuget = new Nuget
        {
            Name = packageReference.Name,
            Version = packageReference.Version
        };

        var childDependencies = dependencyLookup.GetValueOrDefault($"{parentNuget.Name}+{parentNuget.Version}") ?? [];
        foreach (var dep in childDependencies) 
            dep.PopulateTree(nuget, dependencyLookup);

        parentNuget.DependentNugets.Add(nuget);
    }
}