using NuGet.Packaging.Core;
using NuGet.ProjectModel;

namespace Frank.DependenciesExplorer;

public static class LockFileExtensions
{
    public static IEnumerable<LockFileTargetLibrary> GetLibraries(this LockFile lockFile)
        => lockFile.Targets.SelectMany(target => target.Libraries);

    public static Dictionary<LockFileTargetLibrary, List<PackageDependency>> GetLookup(this LockFile lockFile) =>
        lockFile
            .GetLibraries()
            .ToDictionary(
                library => library,
                library => library
                    .Dependencies
                    .ToList());

    public static IList<NugetPackage> GetHierarchy(this LockFile lockFile)
    {
        var dependencyGroupLibraries = lockFile.ProjectFileDependencyGroups.SelectMany(group => group.Dependencies).Select(dependency => dependency.Split(" >= ")[0]).ToList();
        var lookup = GetLookup(lockFile);
        return lockFile.GetLibraries()
            .Where(library => library.Name != null && dependencyGroupLibraries.Contains(library.Name))
            .Select(library => CreateTree(library, lookup))
            .ToList();
    }

    private static NugetPackage CreateTree(LockFileTargetLibrary library, Dictionary<LockFileTargetLibrary, List<PackageDependency>> lookup)
    {
        var children = new List<NugetPackage>();
        if (lookup.TryGetValue(library, out var value))
            children.AddRange(value.Select(child => lookup.Keys.FirstOrDefault(lib => lib.Name == child.Id && lib.Version?.OriginalVersion == child.VersionRange?.MinVersion?.OriginalVersion))
                .OfType<LockFileTargetLibrary>()
                .ToList()
                .Select(childLibrary => CreateTree(childLibrary, lookup)));

        return new NugetPackage(library, children);
    }
}